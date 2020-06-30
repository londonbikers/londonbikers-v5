using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models.Interfaces;
using Tetron.Logic;

namespace Tetron.Controls
{
    public partial class NewsIndexComments : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var comments = Apollo.Server.Instance.MemberCommentsServer.GetLatestComments(20);
            _comments.DataSource = comments;
            _comments.DataBind();
        }

        #region event handlers
        /// <summary>
        /// Applies formatting to the latest-commented-documents list.
        /// </summary>
        protected void CommentsHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var comment = (IComment)e.Item.DataItem;

            // work out the link text.
            var linkText = string.Empty;
            switch (comment.OwnerType)
            {
                case Apollo.Models.CommentOwnerType.Directory:
                    linkText = ((IDirectoryItem) comment.Owner).Title;
                    break;
                case Apollo.Models.CommentOwnerType.Editorial:
                    linkText = ((IDocument)comment.Owner).Title;
                    break;
                case Apollo.Models.CommentOwnerType.Galleries:
                    linkText = ((IGallery)comment.Owner).Title;
                    break;
                case Apollo.Models.CommentOwnerType.GalleryImages:
                    linkText = ((IGalleryImage)comment.Owner).Name;
                    break;
            }

            var link = e.Item.FindControl("_link") as Literal;
            var url = Functions.GetApolloObjectUrl(comment.Owner);
            link.Text = string.Format("<a href=\"{0}\" class=\"darkLined\">{1}</a>", url, linkText);
        }
        #endregion
    }
}