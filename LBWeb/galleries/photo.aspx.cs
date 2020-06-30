using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Galleries
{
    public partial class Photo : Page
    {
        #region members
        protected string FrameId;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).NavigationArea = Constants.NavigationArea.Photos;
            if (Master != null) ((SiteMaster)Master).ShowRhsColumn = false;

            // url validation.
            if (!Functions.IsUrlIdValid(this))
            {
                Logger.LogWarning("Invalid url: " + Request.Url.AbsoluteUri);
                Response.Redirect("/galleries/");
            }

            var gallery = Apollo.Server.Instance.GalleryServer.GetGallery(Convert.ToInt64(RouteData.Values["gallery"]));
            if (gallery == null)
            {
                Logger.LogWarning("null gallery! - id: " + RouteData.Values["gallery"]);
                Response.Redirect("/galleries/");
            }

            var photoId = Convert.ToInt64(RouteData.Values["photo"]);
            var photo = gallery.Photos.SingleOrDefault(q => q.Id == photoId);
            if (photo == null)
            {
                Logger.LogWarning("null photo! - id: " + RouteData.Values["photo"]);
                Response.Redirect(Functions.GetApolloObjectUrl(gallery));
            }

            if (Master != null)
            {
                ((SiteMaster) Master).PageTitle = photo.Name;
                ((SiteMaster) Master).PageDescription = photo.Comment;
                ((SiteMaster) Master).MetaImageUrl = ConfigurationManager.AppSettings["Global.SiteURL"] + Functions.DynamicImage(photo.GalleryImages.OneThousandAndTwentyFour, 130, DynamicImageType.GalleryImage);
            }

            Page.Title = photo.Name;
            _title.Text = photo.Name;
            _views.Text = photo.Views.ToString("###,###");
            _photographer.Text = photo.Credit;
            var res = (int)Session["GalleryImageResolutionPreference"];
            FrameId = res == 1600 ? "frame1600" : "frame";

            _caption.Text = WebUtils.PlainTextToHtml(photo.Comment);
            if (string.IsNullOrEmpty(_caption.Text))
                _commentHolder.Visible = false;

            var user = Functions.GetCurrentUser();
            if (user != null && user.ForumUserId == 2)
                _viewsHolder.Visible = true;

            _comments.CommentsCollection = photo.Comments;

            _galleryLink.Text = gallery.Title;
            _galleryLink.NavigateUrl = Functions.GetApolloObjectUrl(gallery);

            var category = gallery.Categories[0];
            _category.Text = category.Name;
            _category.NavigateUrl = Functions.GetApolloObjectUrl(category);
            string imageHref;

            #region navigation left/right
            var previousImage = gallery.PreviousImage(photo);
            var nextImage = gallery.NextImage(photo);

            if (previousImage == null)
            {
                _previousThumbnail.Visible = false;
                _prevThumbGhost.Visible = true;
                _prevHolder.Visible = false;
            }
            else
            {
                _previousThumbnail.ImageUrl = string.Format("/img.ashx?id={0}&nw=1&w=100&s=gi&c=1", previousImage.GalleryImages.Thumbnail.ToLower());
                _previousThumbnail.NavigateUrl = string.Format("/galleries/image/{0}/{1}/{2}", gallery.Id, previousImage.Id, WebUtils.ToUrlString(previousImage.Name));
            }

            if (nextImage == null)
            {
                imageHref = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
                _nextThumbnail.Visible = false;
                _nextThumbGhost.Visible = true;
                _nextHolder.Visible = false;
            }
            else
            {
                imageHref = string.Format("/galleries/image/{0}/{1}/{2}", gallery.Id, nextImage.Id, WebUtils.ToUrlString(nextImage.Name));
                _preRenderLink.Text = string.Format("<link rel=\"prerender\" href=\"{0}\">", imageHref);
                _nextThumbnail.ImageUrl = string.Format("/img.ashx?id={0}&nw=1&w=100&s=gi&c=1", nextImage.GalleryImages.Thumbnail.ToLower());
                _nextThumbnail.NavigateUrl = imageHref;
            }
            #endregion

            var imageSrc = string.Format("/galleries/img.ashx?g={0}&i={1}", gallery.Id, photo.Id);
            _photo.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", imageHref, imageSrc, WebUtils.ToAttributeString(photo.Name));

        }
    }
}