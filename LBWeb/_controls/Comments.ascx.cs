using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Controls
{
    public partial class Comments : UserControl
    {
        #region members
        private int _counter;
        #endregion

        #region accessors
        /// <summary>
        /// Required: The comments collection for the domain object showing comments for.
        /// </summary>
        public Apollo.Models.Comments CommentsCollection { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.EnableViewState = false;
            if (CommentsCollection == null)
                return;

            if (!Page.IsPostBack)
                Functions.RenderScreenMessage(_responseBox);

            // needs to be rebound here so that the report control works.
            RenderComments();
        }

        #region event handlers
        /// <summary>
        /// Handles the rendering of each comment in the comments list.
        /// </summary>
        protected void CommentItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var comment = e.Item.DataItem as Comment;
            var avatarImage = e.Item.FindControl("_avatarImage") as Image;
            var avatarLink = e.Item.FindControl("_avatarLink") as HyperLink;
            var authorTextLink = e.Item.FindControl("_authorTextLink") as HyperLink;
            var posted = e.Item.FindControl("_posted") as Literal;
            var commentText = e.Item.FindControl("_commentText") as Literal;
            var reportLink = e.Item.FindControl("_reportLink") as LinkButton;
            var reportArea = e.Item.FindControl("_reportLinkArea") as PlaceHolder;
            var hr = e.Item.FindControl("_hr") as PlaceHolder;

            avatarLink.NavigateUrl = string.Format("/forums/UserInfo{0}.aspx", comment.Author.ForumUserId);
            avatarImage.Width = Unit.Pixel(60);
            if (comment.Author.AvatarUri != null)
            {
                if (comment.Author.AvatarUri.AbsoluteUri.StartsWith(ConfigurationManager.AppSettings["Global.SiteURL"]))
                {
                    var mode = (comment.Author.AvatarUri.AbsoluteUri.ToLower().Contains("/forums/uploads/avatars")) ? "fa" : "fla";
                    avatarImage.ImageUrl = string.Format("/img.ashx?s={0}&w=60&id={1}", mode, WebUtils.PageNameFromUrl(comment.Author.AvatarUri.AbsoluteUri));
                }
                else
                {
                    avatarImage.ImageUrl = comment.Author.AvatarUri.AbsoluteUri;
                }
            }
            else
            {
                avatarImage.ImageUrl = "/_images/user60.gif";
            }

            authorTextLink.Text = comment.Author.Username;
            authorTextLink.NavigateUrl = avatarLink.NavigateUrl;
            posted.Text = Apollo.Utilities.Helpers.ToRelativeDateString(comment.Created);
            commentText.Text = WebUtils.PlainTextToHtml(comment.Text);

            if (!Functions.IsUserLoggedIn())
            {
                reportArea.Visible = false;
            }
            else
            {
                reportLink.CommandName = "Comment";
                reportLink.CommandArgument = comment.Id.ToString();
            }

            if (CommentsCollection.Items.Count > 1 && _counter < CommentsCollection.Items.Count - 1)
                hr.Visible = true;

            _counter++;
        }

        /// <summary>
        /// Handles the posting of a new member comment.
        /// </summary>
        protected void PostCommentHandler(object sender, EventArgs ea)
        {
            var user = Functions.GetCurrentUser();
            if (user.Status != UserStatus.Active)
            {
                _responseBox.InnerText = "Inactive user!";
                _responseBox.Visible = true;
                RenderComments();
                return;
            }

            if (_comment.Text.Trim() == String.Empty)
            {
                _responseBox.InnerText = "Please supply a comment first!";
                _responseBox.Visible = true;
                RenderComments();
                return;
            }

            // todo: strip out profanity.
            var comment = CommentsCollection.New();
            comment.Text = _comment.Text.Trim();
            comment.Author = Functions.GetCurrentUser();

            // add to the collection and persist.
            CommentsCollection.Add(comment);

            var userSession = Functions.GetCurrentUserSession();
            userSession.PageActions.Add(new PageAction("<b>Comment Added!</b> You can view your comment below."));
            Response.Redirect(Request.Url + "#comments");
        }

        /// <summary>
        /// Handles the reporting of a comment to the moderation team.
        /// </summary>
        protected void ReportCommentHandler(object sender, CommandEventArgs cma)
        {
            var commentToReport = CommentsCollection.GetComment(long.Parse((string)cma.CommandArgument));
            if (commentToReport == null)
                return;

            // can this comment be reported?
            if (commentToReport.ReportStatus == CommentReportStatus.NoReport)
            {
                commentToReport.ReportStatus = CommentReportStatus.Reported;
                Apollo.Server.Instance.MemberCommentsServer.UpdateComment(commentToReport);

                var userSession = Functions.GetCurrentUserSession();
                userSession.PageActions.Add(new PageAction("<b>Comment Reported!</b> A moderator will be made aware of this comment."));
                Response.Redirect(Request.Url + "#comments");
            }
            else
            {
                var userSession = Functions.GetCurrentUserSession();
                userSession.PageActions.Add(new PageAction("<b>Cannot Report!</b> This comment has already been reported."));
                Response.Redirect(Request.Url + "#comments");
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Writes the comments and stats out to the control ui.
        /// </summary>
        private void RenderComments()
        {
            if (Functions.IsUserLoggedIn())
            {
                _anonView.Visible = false;
                _memberView.Visible = true;
            }
            else
            {
                _anonView.Visible = true;
                _memberView.Visible = false;
            }

            if (CommentsCollection.Items.Count > 0)
            {
                _commentsTitle.Text = string.Format("<b>{0}</b> Comment", CommentsCollection.Items.Count);
                if (CommentsCollection.Items.Count > 1)
                    _commentsTitle.Text += "s";

                _commentsTable.DataSource = CommentsCollection.Items;
                _commentsTable.DataBind();
            }
            else
            {
                _commentsTitle.Text = "Comments";
            }
        }
        #endregion
    }
}