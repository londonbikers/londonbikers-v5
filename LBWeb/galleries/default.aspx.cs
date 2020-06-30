using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Galleries
{
    public partial class Default : Page
    {
        #region members
        private const int MaxLatestGalleries = 6;
        private int _latestGalleriesItemCounter;
        private int _latestGalleriesTotalCounter;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).NavigationArea = Constants.NavigationArea.Photos;
            if (Master != null) ((SiteMaster)Master).PageTitle = "Photo Galleries";

            var site = Functions.GetConfiguredSite();

            // get the latest galleries...
            _latestGalleries.DataSource = site.RecentGalleries.RetrieveGalleries(MaxLatestGalleries);
            _latestGalleries.DataBind();

            _categoriesList.DataSource = site.GalleryCategories;
            _categoriesList.DataBind();

            // stats...
            _numOfGalleries.Text = Apollo.Server.Instance.GalleryServer.Statistics.GalleryCount.ToString("###,###");
            _numOfPhotos.Text = Apollo.Server.Instance.GalleryServer.Statistics.ImageCount.ToString("###,###");

            // favourite tags...
            _tag1.DataSource = Apollo.Server.Instance.GetTag("bsb").LatestGalleries.Retrieve(5);
            _tag1.DataBind();
            _tag2.DataSource = Apollo.Server.Instance.GetTag("wsb").LatestGalleries.Retrieve(5);
            _tag2.DataBind();
            _tag3.DataSource = Apollo.Server.Instance.GetTag("shows").LatestGalleries.Retrieve(5);
            _tag3.DataBind();
            _tag4.DataSource = Apollo.Server.Instance.GetTag("products").LatestGalleries.Retrieve(5);
            _tag4.DataBind();
        }

        #region event handlers
        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void LatestGalleriesHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var gallery = e.Item.DataItem as IGallery;
            var imageLink = e.Item.FindControl("_imageLink") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var tag = e.Item.FindControl("_tag") as Literal;
            var rowSep = e.Item.FindControl("_rowSep") as Literal;
            var url = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
            var imageId = ((IGalleryImage)gallery.Photos[0]).GalleryImages.Thumbnail.ToLower();
            var imageUrl = Functions.DynamicImage(imageId, new Size { Width = 323, Height = 170 }, DynamicImageType.GalleryImage, DynamicImageResizeType.SetSize);
            imageLink.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", url, imageUrl, WebUtils.ToSafeHtmlParameter(gallery.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, gallery.Title);
            tag.Text = gallery.Categories[0].Name;// +" | " + gallery.Photos.Count.ToString("#,###") + " photos.";

            if (_latestGalleriesItemCounter == 1 && _latestGalleriesTotalCounter < (MaxLatestGalleries - 1))
            {
                rowSep.Visible = true;
                _latestGalleriesItemCounter = 0;
            }
            else
            {
                _latestGalleriesItemCounter++;
            }

            _latestGalleriesTotalCounter++;
        }

        /// <summary>
        /// Applies formatting to the gallery categories repeater control items.
        /// </summary>
        protected void CategoriesHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var category = e.Item.DataItem as GalleryCategory;
            var link = e.Item.FindControl("_link") as Literal;
            var url = Functions.GetApolloObjectUrl(category);
            link.Text = string.Format("<a href=\"{0}\" class=\"light\">{1}</a>", url, category.Name);
        }

        /// <summary>
        /// Applies formatting to the favourite tag gallery repeater control items.
        /// </summary>
        protected void FavouriteColumnHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var gallery = e.Item.DataItem as IGallery;
            var image = e.Item.FindControl("_image") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var numOfPhotos = e.Item.FindControl("_numOfPhotos") as Literal;

            var imageId = ((IGalleryImage)gallery.Photos[0]).GalleryImages.Thumbnail.ToLower();
            var imageUrl = Functions.DynamicImage(imageId, new Size { Width = 157, Height = 60 }, DynamicImageType.GalleryImage, DynamicImageResizeType.SetSize);
            var tooltip = WebUtils.ToSafeHtmlParameter(gallery.Title);
            var href = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));

            image.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", href, imageUrl, tooltip);
            title.Text = gallery.Title;
            numOfPhotos.Text = gallery.Photos.Count.ToString("#,###");
        }
        #endregion
    }
}