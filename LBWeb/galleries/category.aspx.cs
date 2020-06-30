using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Galleries
{
    public partial class Category : Page
    {
        #region members
        private int _counter;
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

            var page = 1;
            if (Request.QueryString["p"] != null)
                page = int.Parse(Request.QueryString["p"]);
            
            var category = Apollo.Server.Instance.GalleryServer.GetCategory(Convert.ToInt64(RouteData.Values["id"]));
            if (category == null)
            {
                Logger.LogWarning("null category! - id: " + RouteData.Values["id"]);
                Response.Redirect("/galleries/");
            }

            if (Master != null) ((SiteMaster) Master).PageTitle = "Photos: " + category.Name;
            _title.Text = category.Name;
            _description.Text = category.Description;

            var pageGalleries = category.Galleries.GetPage(page);
            _galleries.DataSource = pageGalleries;
            _galleries.DataBind();

            _topPager.TotalItemCount = category.Galleries.Count;
            _bottomPager.TotalItemCount = category.Galleries.Count;
        }

        #region event handlers
        /// <summary>
        /// Applies formatting to the favourite tag gallery repeater control items.
        /// </summary>
        protected void GalleryHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var gallery = e.Item.DataItem as IGallery;
            var image = e.Item.FindControl("_image") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var numOfPhotos = e.Item.FindControl("_numOfPhotos") as Literal;

            var imageId = gallery.Photos[0].GalleryImages.Thumbnail.ToLower();
            var imageUrl = Functions.DynamicImage(imageId, new Size { Width = 160, Height = 100 }, DynamicImageType.GalleryImage, DynamicImageResizeType.SetSize);
            var tooltip = WebUtils.ToSafeHtmlParameter(gallery.Title);
            var href = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
            var sep = e.Item.FindControl("_rowSep") as Literal;

            image.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", href, imageUrl, tooltip);
            title.Text = gallery.Title;
            numOfPhotos.Text = gallery.Photos.Count.ToString("#,###");

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