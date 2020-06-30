using System;
using System.Web.UI;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Tools.Content
{
	public partial class CommentPage : Page
	{
		#region members
		private Comment _comment;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "editorial";
			_header.PageBackgroundType = "none";

			if (!Helpers.IsNumeric(Request.QueryString["id"]))
				Response.Redirect("comments.aspx");

			_comment = Apollo.Server.Instance.MemberCommentsServer.GetComment(long.Parse(Request.QueryString["id"]));
			if (_comment == null)
				Response.Redirect("comments.aspx");

		    if (Page.IsPostBack) return;

		    // draw the comment details.
		    _id.Text = _comment.Id.ToString();
		    _commentText.Text = _comment.Text;
		    _created.Text = _comment.Created.ToLongDateString() + " - " + _comment.Created.ToShortTimeString();
		    _authorLink.Text = _comment.Author.Username;
		    _authorLink.NavigateUrl = "../users/user.aspx?uid=" + _comment.Author.Uid;
		    _deleteBtn.Attributes.Add("onclick", "if (!confirm('Do you really wish to delete this comment?')){return false;}");

		    switch (_comment.OwnerType)
		    {
		        case CommentOwnerType.Editorial:
		            var doc = _comment.Owner as Document;
		            _mediaLink.Text = doc.Title;
		            switch (doc.Type)
		            {
		                case DocumentType.News:
		                    _icon.ImageUrl = "/_images/silk/newspaper.png";
		                    break;
		                case DocumentType.Article:
		                    _icon.ImageUrl = "/_images/silk/layout.png";
		                    break;
		            }
		            break;

		        case CommentOwnerType.Galleries:
		            _mediaLink.Text = ((Apollo.Models.Gallery)_comment.Owner).Title;
		            _icon.ImageUrl = "/_images/silk/images.png";
		            break;

		        case CommentOwnerType.GalleryImages:
		            _mediaLink.Text = ((GalleryImage)_comment.Owner).Name;
		            _icon.ImageUrl = "/_images/silk/image.png";
		            break;

		        case CommentOwnerType.Directory:
		            _mediaLink.Text = ((DirectoryItem)_comment.Owner).Title;
		            _icon.ImageUrl = "/_images/silk/report.png";
		            break;
		    }

		    _mediaLink.NavigateUrl = Functions.GetApolloObjectUrl(_comment.Owner) + "#comments";
		    WebUtils.PopulateDropDownFromEnum(_reportStatus, _comment.ReportStatus, true);
		    _reportStatus.SelectedIndex = _reportStatus.Items.IndexOf(_reportStatus.Items.FindByValue(((int)_comment.ReportStatus).ToString()));
		}

		#region event handlers
		/// <summary>
		/// Handles the persistence of changes to a Comment object.
		/// </summary>
		protected void UpdateCommentHandler(object sender, ImageClickEventArgs ea)
		{
			if (_commentText.Text.Trim() == String.Empty)
			{
				_screenMessage.InnerHtml = "<b>Cannot Update!</b> Please supply some comment text.";
				_screenMessage.Visible = true;
				return;
			}

			_comment.Text = _commentText.Text.Trim();
			_comment.ReportStatus = (CommentReportStatus)Enum.Parse(typeof(CommentReportStatus), _reportStatus.SelectedValue);

			Apollo.Server.Instance.MemberCommentsServer.UpdateComment(_comment);

			_screenMessage.InnerHtml = "<b>Comment Updated!</b> Your changes are now visible on the site.";
			_screenMessage.Visible = true;
		}

		/// <summary>
		/// Handles the deletion of a Comment object.
		/// </summary>
		protected void DeleteCommentHandler(object sender, ImageClickEventArgs ea)
		{
			Apollo.Server.Instance.MemberCommentsServer.DeleteComment(_comment);
			Functions.GetCurrentUserSession().PageActions.Add(new PageAction("<b>Comment Deleted!</b> The comment has been removed from the site."));
			Response.Redirect("comments.aspx");
		}
		#endregion
	}
}