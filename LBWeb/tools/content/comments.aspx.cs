using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Tools.Content
{
	public partial class Comments : System.Web.UI.Page
	{
		#region members
		private const int MaxCommentsToShow = 100;
		private List<Comment> _comments;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
            _header.PageType = "editorial";
            _header.PageBackgroundType = "none";
			Page.EnableViewState = false;

		    if (Page.IsPostBack) return;
		    Functions.RenderScreenMessage(_screenMessage);
		    LatestCommentsViewHandler(null, null);
		}

		#region event handlers
		/// <summary>
		/// Applies formatting to the latest-comments repeater control items.
		/// </summary>
		protected void ItemCreatedHandler(object sender, RepeaterItemEventArgs e)
		{
		    switch (e.Item.ItemType)
		    {
		        case ListItemType.AlternatingItem:
		        case ListItemType.Item:
	            {
	                if (e.Item.DataItem == null)
	                    return;

	                var comment = e.Item.DataItem as Comment;
	                var mediaLink = e.Item.FindControl("_mediaLink") as HyperLink;
	                var authorLink = e.Item.FindControl("_authorLink") as HyperLink;
	                var editLink = e.Item.FindControl("_editLink") as HyperLink;
	                var created = e.Item.FindControl("_created") as Literal;
	                var status = e.Item.FindControl("_status") as Literal;
	                var commentText = e.Item.FindControl("_commentText") as Literal;
	                var icon = e.Item.FindControl("_icon") as Image;

	                if (comment != null)
	                {
	                    switch (comment.OwnerType)
	                    {
	                        case CommentOwnerType.Editorial:
	                            var doc = comment.Owner as Document;
	                            mediaLink.Text = doc.Title;
	                            switch (doc.Type)
	                            {
	                                case DocumentType.News:
	                                    icon.ImageUrl = "/_images/silk/newspaper.png";
	                                    break;
	                                case DocumentType.Article:
	                                    icon.ImageUrl = "/_images/silk/layout.png";
	                                    break;
	                            }
	                            break;

	                        case CommentOwnerType.Galleries:
	                            mediaLink.Text = ((Apollo.Models.Gallery)comment.Owner).Title;
	                            icon.ImageUrl = "/_images/silk/images.png";
	                            break;

	                        case CommentOwnerType.GalleryImages:
	                            mediaLink.Text = ((GalleryImage)comment.Owner).Name;
	                            icon.ImageUrl = "/_images/silk/image.png";
	                            break;

	                        case CommentOwnerType.Directory:
	                            mediaLink.Text = ((DirectoryItem)comment.Owner).Title;
	                            icon.ImageUrl = "/_images/silk/report.png";
	                            break;
	                    }

	                    mediaLink.NavigateUrl = Functions.GetApolloObjectUrl(comment.Owner) + "#comments";
	                    authorLink.Text = comment.Author.Username;
	                    authorLink.NavigateUrl = "../users/user.aspx?uid=" + comment.Author.Uid;
	                    created.Text = Helpers.ToRelativeDateString(comment.Created, false);
	                    status.Text = comment.Status.ToString();
	                    editLink.NavigateUrl = "comment.aspx?id=" + comment.Id;

	                    commentText.Text = WebUtils.PlainTextToHtml(Helpers.ToShortString(comment.Text, 500));
	                    if (comment.ReportStatus == CommentReportStatus.Reported)
	                        commentText.Text = "<img src=\"/_images/silk/bullet_error.png\" align=\"absmiddle\" /> <span style=\"color: red;\">" + commentText.Text + "</span>";
	                }
	            }
	            break;
		        case ListItemType.Footer:
	            {
	                var footer = e.Item.FindControl("_footer") as Literal;
	                footer.Text = "Showing " + _grid.Items.Count + " comment";
	                footer.Text += (_grid.Items.Count > 1) ? "s." : ".";
	                footer.Text += " Max " + MaxCommentsToShow + ".";
	            }
	            break;
		    }
		}

	    /// <summary>
		/// Handles the click action of the view Latest Comments button.
		/// </summary>
		protected void LatestCommentsViewHandler(object sender, EventArgs ea)
		{
			_comments = Apollo.Server.Instance.MemberCommentsServer.GetLatestComments(MaxCommentsToShow);
			_grid.DataSource = _comments;
			_grid.DataBind();

			_latestCommentsButton.Style["font-weight"] = "bold";
			_reportedCommentsButton.Style.Remove("font-weight");
		}

		/// <summary>
		/// Handles the click action of the view Reported Comments button.
		/// </summary>
		protected void ReportedCommentsViewHandler(object sender, EventArgs ea)
		{
			_comments = Apollo.Server.Instance.MemberCommentsServer.GetLatestReportedComments(MaxCommentsToShow);
			_grid.DataSource = _comments;
			_grid.DataBind();

			_reportedCommentsButton.Style["font-weight"] = "bold";
			_latestCommentsButton.Style.Remove("font-weight");
		}
		#endregion
	}
}