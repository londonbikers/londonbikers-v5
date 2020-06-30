using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Galleries
{
    public partial class Gallery : Page
    {
        #region members
        private int _counter;
        private string _mediaRoot;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).NavigationArea = Constants.NavigationArea.Photos;

            // url validation.
            if (!Functions.IsUrlIdValid(this))
            {
                Logger.LogWarning("Invalid url: " + Request.Url.AbsoluteUri);
                Response.Redirect("/galleries/");
            }

            var gallery = Apollo.Server.Instance.GalleryServer.GetGallery(Convert.ToInt64(RouteData.Values["id"]));
            if (gallery == null)
            {
                Logger.LogWarning("null gallery! - id: " + RouteData.Values["id"]);
                Response.Redirect("/galleries/");
            }

            if (Master != null) ((SiteMaster) Master).PageTitle = gallery.Title;
            _mediaRoot = ConfigurationManager.AppSettings["Global.MediaLibraryURL"];
            _title.Text = gallery.Title;
            Page.Title = gallery.Title;
            _description.Text = WebUtils.PlainTextToHtml(gallery.Description);
            _photoCount.Text = gallery.Photos.Count.ToString("###,###");
            _comments.CommentsCollection = gallery.Comments;

            var category = gallery.Categories[0];
            _category.Text = category.Name;
            _category.NavigateUrl = Functions.GetApolloObjectUrl(category);

            var firstImageUrl = Functions.GetApolloObjectUrl(gallery.Photos[0]);
            _preRenderLink.Text = string.Format("<link rel=\"prerender\" href=\"{0}\">", firstImageUrl);

            _photos.DataSource = gallery.Photos;
            _photos.DataBind();
        }

        #region event handlers
        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void PhotosHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var image = e.Item.DataItem as IGalleryImage;
            var imageLink = e.Item.FindControl("_imageLink") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var sep = e.Item.FindControl("_rowSep") as Literal;
            var comments = e.Item.FindControl("_comments") as Literal;
            var url = Functions.GetApolloObjectUrl(image);
            // 01/01/2019 - used to be galleries/thumbs/
            var imageUrl = string.Format("{0}galleries/1024/{1}", _mediaRoot, image.GalleryImages.Thumbnail);
            imageLink.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" width=\"154\" /></a>", url, imageUrl, WebUtils.ToSafeHtmlParameter(image.Name));
            title.Text = image.Name;

            if (image.Comments.Items.Count > 0)
            {
                var pluraliser = image.Comments.Items.Count > 1 ? "s" : string.Empty;
                comments.Text = string.Format("<span class=\"cmt\">{0} comment{1} -</span>", image.Comments.Items.Count, pluraliser);
            }

            if (_counter == 3)
            {
                sep.Visible = true;
                _counter = 0;
            }
            else
            {
                _counter++;
            }
        }
        #endregion
    }
}