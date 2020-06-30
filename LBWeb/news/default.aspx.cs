using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.News
{
    public partial class Default : Page
    {
        #region members
        protected int TopContentImageCounter = 1;
        protected int CaptionCounter = 1;
        protected const int MaxGalleries = 5;
        protected int ThumbCounter;
        private int _topStoriesCounter;
        private int _topStoriesCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.News;
            if (Master != null) ((SiteMaster) Master).PageTitle = "The Motorcycle News";
            var popularNews = Apollo.Server.Instance.ContentServer.PopularDocuments.Single(q => q.Type == DocumentType.News);
            var showingTopStories = RenderTopStories(popularNews);
            _topStoriesHolder.Visible = showingTopStories;

            // latest galleries
            var galleries = Apollo.Server.Instance.GalleryServer.CommonQueries.LatestPublicGalleries(MaxGalleries);
            if (galleries.Count() > 0)
            {
                _galleriesPlaceholder.Visible = true;
                _latestGalleries.DataSource = galleries;
                _latestGalleries.DataBind();
            }

            // popular stories
            // -- these need to be a fixed number, less the top-stories ones already being shown.
            var startingPoint = showingTopStories ? 4 : 0;
            var popularStoriesLotSize = (popularNews.Items.Count - startingPoint) > 20 ? 20 : popularNews.Items.Count - startingPoint;
            var popularStories = new List<IDocument>();
            for (var i = startingPoint; i < popularStoriesLotSize + startingPoint; i++)
                popularStories.Add(popularNews.Items[i]);

            var showLatestStoriesRhs = true;
            if (popularStories.Count > 0)
            {
                _popularStoriesList.DataSource = popularStories;
                _popularStoriesList.DataBind();    
            }
            else
            {
                // use latest stories instead.
                showLatestStoriesRhs = false;
                _latestStoriesRhsHolder.Visible = false;
                _popularStoriesTitle.Text = "Latest";
                var latestStories = Functions.GetConfiguredSite().Channels[0].News.LatestDocuments.RetrieveDocuments(20);
                _popularStoriesList.DataSource = latestStories;
                _popularStoriesList.DataBind();  
            }

            if (showLatestStoriesRhs)
            {
                var latestStories = Functions.GetConfiguredSite().Channels[0].News.LatestDocuments.RetrieveDocuments(40);
                _latestStoriesRhsRepeater.DataSource = latestStories;
                _latestStoriesRhsRepeater.DataBind();
            }

            // features
            _features.DataSource = Functions.GetConfiguredSite().Channels[0].Articles.LatestDocuments.RetrieveDocuments(5);
            _features.DataBind();
        }

        #region event handlers
        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void FeaturesCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var imageLink = e.Item.FindControl("_imageLink") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var tag = e.Item.FindControl("_tag") as Literal;
            var url = string.Format("/articles/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            var image = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true) ??
                        doc.EditorialImages.GetImage(ContentImageType.Intro);

            var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 125, Height = 170 }, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
            imageLink.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", url, imageUrl, WebUtils.ToSafeHtmlParameter(doc.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, doc.Title);
            tag.Text = Functions.FormatTagNameForTitle(doc.Tags[0].Name);
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void LatestStoriesRhsCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_link") as HyperLink;
            var image = e.Item.FindControl("_img") as HyperLink;

            link.Text = doc.Title;
            link.NavigateUrl = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));

            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true);
            if (editorialImage != null)
            {
                var imageId = editorialImage.Filename.ToLower();
                image.ImageUrl = Functions.DynamicImage(imageId, 50, DynamicImageType.Editorial, DynamicImageResizeType.Square);
                image.NavigateUrl = link.NavigateUrl;
            }
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void PopularStoriesListCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_image") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var docAbstract = e.Item.FindControl("_abstract") as Literal;
            var comments = e.Item.FindControl("_ecComments") as Literal;
            var commentsPanel = e.Item.FindControl("_commentsPanel") as PlaceHolder;
            var photosPanel = e.Item.FindControl("_photosPanel") as PlaceHolder;
            var numPhotos = e.Item.FindControl("_numPhotos") as Literal;
            var published = e.Item.FindControl("_published") as Literal;

            var url = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, doc.Title);
            published.Text = Helpers.ToRelativeDateString(doc.Published); 

            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true);
            var imageUrl = string.Empty;

            if (editorialImage != null)
            {
                var imageId = editorialImage.Filename.ToLower();
                imageUrl = Functions.DynamicImage(imageId, new Size{Width = 441, Height = 175}, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
            }

            link.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" class=\"storyImg\" /></a>", url, imageUrl, WebUtils.ToAttributeString(doc.Title));
            docAbstract.Text = doc.Abstract;

            if (doc.Comments.Items.Count > 0)
                comments.Text = doc.Comments.Items.Count.ToString();
            else
                commentsPanel.Visible = false;

            var photoCount = doc.EditorialImages.ImageTypeCount(ContentImageType.SlideShow);
            if (photoCount > 0)
                numPhotos.Text = photoCount.ToString();
            else
                photosPanel.Visible = false;

        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void TopStoriesListingCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_ecLink") as HyperLink;
            var image = e.Item.FindControl("_ecImg") as HyperLink;
            var hr = e.Item.FindControl("_hr") as Literal;

            link.Text = doc.Title;
            link.NavigateUrl = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            hr.Text = "<hr />";
            if (_topStoriesCounter < _topStoriesCount - 1)
                hr.Visible = true;

            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true);
            if (editorialImage != null)
            {
                var imageId = editorialImage.Filename.ToLower();
                image.ImageUrl = Functions.DynamicImage(imageId, 44, DynamicImageType.Editorial, DynamicImageResizeType.Square);
                image.NavigateUrl = link.NavigateUrl;    
            }

            _topStoriesCounter++;
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void TopDocsCaptionCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var caption = e.Item.FindControl("_caption") as Literal;
            var url = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            caption.Text = string.Format("<div id=\"caption{0}\" class=\"nivo-html-caption\">{1} <a href=\"{2}\">read more</a>.</div>", CaptionCounter, doc.Title, url);
            CaptionCounter++;
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void TopDocsImageCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var image = e.Item.FindControl("_image") as Literal;
            var url = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            var imageUrl = GetTopContentImageUrl(doc);
            image.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"\" title=\"{2}\" /></a>", url, imageUrl, WebUtils.ToAttributeString(doc.Title));
            TopContentImageCounter++;
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void LatestGalleriesItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var gallery = e.Item.DataItem as IGallery;
            var imageLink = e.Item.FindControl("_imageLink") as HyperLink;
            var title = e.Item.FindControl("_title") as Literal;
            var marginCss = e.Item.FindControl("_marginCss") as Literal;
            var category = e.Item.FindControl("_category") as Literal;
            var numOfPhotos = e.Item.FindControl("_numOfPhotos") as Literal;

            var imageId = ((IGalleryImage)gallery.Photos[0]).GalleryImages.Thumbnail.ToLower();
            imageLink.ImageUrl = Functions.DynamicImage(imageId, new Size { Width = 219, Height = 170 }, DynamicImageType.GalleryImage, DynamicImageResizeType.SetSize);
            imageLink.ToolTip = WebUtils.ToSafeHtmlParameter(gallery.Title);
            imageLink.NavigateUrl = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
            title.Text = gallery.Title;
            category.Text = gallery.Categories[0].Name;
            numOfPhotos.Text = gallery.Photos.Count.ToString("#,###");

            if (ThumbCounter < MaxGalleries - 1)
                marginCss.Text = " mb5";

            ThumbCounter++;
        }
        #endregion

        #region private methods
        private static string GetTopContentImageUrl(IDocument doc)
        {
            // try to find an image that is wide enough and isn't too tall.
            var slides = doc.EditorialImages.FilterImages(ContentImageType.SlideShow).Where(q => q.Width >= 446 && q.Height >= 222).ToList();
            if (slides.Count > 0)
            {
                var r = new Random(DateTime.Now.Millisecond);
                var image = slides[r.Next(slides.Count)];
                var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 446, Height = 222 }, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
                return imageUrl;
            }

            slides = doc.EditorialImages.FilterImages(ContentImageType.SlideShow).Where(q => q.Width >= 446).ToList();
            if (slides.Count > 0)
            {
                var r = new Random(DateTime.Now.Millisecond);
                var image = slides[r.Next(slides.Count)];
                var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 446, Height = 222 }, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
                return imageUrl;
            }

            // failing that, return nothing.
            return string.Empty;
        }

        private bool RenderTopStories(IDocumentTypeGroup newsStories)
        {
            // top stories
            var tsCount = 0;
            var topStories = new List<IDocument>();
            
            while (topStories.Count < 4 && tsCount < newsStories.Items.Count)
            {
                var acceptableImages = newsStories.Items[tsCount].EditorialImages.List.Count(q => q.Type == ContentImageType.SlideShow && q.Width >= 386 && q.Height >= 222);
                if (acceptableImages > 0)
                    topStories.Add(newsStories.Items[tsCount]);

                tsCount++;
            }

            if (topStories.Count < 4)
                return false;

            _topDocsImageList.DataSource = topStories;
            _topDocsImageList.DataBind();
            _topDocsCaptionList.DataSource = topStories;
            _topDocsCaptionList.DataBind();

            _topStoriesCount = topStories.Count;
            _popularStoriesListing.DataSource = topStories;
            _popularStoriesListing.DataBind();

            return true;
        }
        #endregion
    }
}