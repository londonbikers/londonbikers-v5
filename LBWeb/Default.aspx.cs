using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron
{
    public partial class Default : System.Web.UI.Page
    {
        #region members
        private ISection _newsSection;
        private IChannel _channel;
        protected const int MaxGalleries = 3;
        protected int ThumbCounter;
        protected int TopContentImageCounter = 1;
        protected int CaptionCounter = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var site = Functions.GetConfiguredSite();
            if (Helpers.IsNumeric(Request.QueryString["cid"]))
            {
                // retrieve a specific channel.
                var channelId = int.Parse(Request.QueryString["cid"]);
                foreach (var siteChannel in site.Channels.Where(siteChannel => siteChannel.Id == channelId))
                {
                    _channel = siteChannel;
                    break;
                }
            }
            else
            {
                // retrieve the first (and probably only) channel for this site.
                _channel = site.Channels[0];
            }

            _newsSection = _channel.News;

            // galleries aren't channel specific for now.
            _latestGalleries.DataSource = site.RecentGalleries.RetrieveGalleries(MaxGalleries);
            _latestGalleries.DataBind();

            // top stories content slider.
            var tsCount = 0;
            var popularStories = new List<IDocument>();
            var newsStories = Apollo.Server.Instance.ContentServer.PopularDocuments.Single(q => q.Type == DocumentType.News);
            while (popularStories.Count < 4 && tsCount < newsStories.Items.Count)
            {
                var acceptableImages = newsStories.Items[tsCount].EditorialImages.List.Count(q => q.Type == ContentImageType.SlideShow && q.Width >= 386 && q.Height >= 222);
                if (acceptableImages > 0)
                    popularStories.Add(newsStories.Items[tsCount]);

                tsCount++;
            }
            
            _topDocsImageList.DataSource = popularStories;
            _topDocsImageList.DataBind();

            // features
            _features.DataSource = _channel.Articles.LatestDocuments.RetrieveDocuments(5);
            _features.DataBind();

            // editors choice stories.
            var docs = new List<IDocument>();
            var ceiling = _newsSection.FeaturedDocuments.Count < 4 ? _newsSection.FeaturedDocuments.Count : 4;
            for (var i = 0; i < ceiling; i++)
                docs.Add(_newsSection.FeaturedDocuments[i]);
            _editorsChoice.DataSource = docs;
            _editorsChoice.DataBind();

            RenderPopularStories();

            // upcoming events.
            _latestEvents.DataSource = Apollo.Server.Instance.CommunicationServer.GetUpcomingEvents(10);
            _latestEvents.DataBind();
        }

        #region event handlers
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

            var imageUrl = Functions.DynamicImage(image.Filename, new Size{Width = 125, Height = 170}, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
            imageLink.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", url, imageUrl, WebUtils.ToSafeHtmlParameter(doc.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, doc.Title);
            tag.Text = Functions.FormatTagNameForTitle(doc.Tags[0].Name);
        }

        protected void TopDocsImageCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var image = e.Item.FindControl("_image") as Literal;
            var url = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            var imageUrl = GetTopContentImageUrl(doc);
            var cssClass = TopContentImageCounter == 1 ? " class=\"show\"" : string.Empty;
            image.Text = string.Format("<li{0}><a href=\"{1}\"><img src=\"{2}\" width=\"446\" height=\"222\" title=\"\" alt=\"{3}\"/></a></li>", cssClass, url, imageUrl, WebUtils.ToSafeHtmlParameter(doc.Title));
            TopContentImageCounter++;
        }

        /// <summary>
        /// Applies formatting to the latest events repeater control items.
        /// </summary>
        protected void EventsItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var eventLink = e.Item.FindControl("_eventLink") as HyperLink;
            var entry = e.Item.DataItem as Google.GData.Calendar.EventEntry;

            eventLink.Text = Helpers.ToShortString(entry.Title.Text, 52);
            eventLink.NavigateUrl = entry.Links[0].AbsoluteUri;
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

            var imageId = gallery.Photos[0].GalleryImages.Thumbnail.ToLower();
            imageLink.ImageUrl = Functions.DynamicImage(imageId, new Size { Width = 219, Height = 170 }, DynamicImageType.GalleryImage, DynamicImageResizeType.SetSize);
            imageLink.ToolTip = WebUtils.ToSafeHtmlParameter(gallery.Title);
            imageLink.NavigateUrl = string.Format("/galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
            title.Text = gallery.Title;
            category.Text = gallery.Categories[0].Name;
            numOfPhotos.Text = gallery.Photos.Count.ToString("#,###");
            
            if (ThumbCounter < MaxGalleries -1)
                marginCss.Text = " mb5";

            ThumbCounter++;
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void EditorsChoiceItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_ecLink") as HyperLink;
            var comments = e.Item.FindControl("_ecComments") as Literal;
            var commentsPanel = e.Item.FindControl("_commentsPanel") as PlaceHolder;
            var image = e.Item.FindControl("_ecImg") as HyperLink;

            link.Text = doc.Title;
            link.NavigateUrl = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));

            if (doc.Comments.Items.Count > 0)
                comments.Text = doc.Comments.Items.Count.ToString();
            else
                commentsPanel.Visible = false;

            var eimage = doc.EditorialImages.GetImage(ContentImageType.Cover) ??
                         doc.EditorialImages.GetImage(ContentImageType.SlideShow);
            if (eimage == null) return;
            var imageId = eimage.Filename.ToLower();
            image.ImageUrl = Functions.DynamicImage(imageId, 63, DynamicImageType.Editorial, DynamicImageResizeType.Square);
            image.NavigateUrl = link.NavigateUrl;
        }

        /// <summary>
        /// Applies formatting to the tagged documents repeater control items.
        /// </summary>
        protected void DocumentItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_link") as HyperLink;
            var comments = e.Item.FindControl("_comments") as Literal;
            var commentsPanel = e.Item.FindControl("_commentsPanel") as PlaceHolder;
            var image = e.Item.FindControl("_img") as HyperLink;

            link.Text = doc.Title;
            link.NavigateUrl = string.Format("/news/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));

            if (doc.Comments.Items.Count > 0)
                comments.Text = doc.Comments.Items.Count.ToString();
            else
                commentsPanel.Visible = false;

            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.Cover) ??
                                 doc.EditorialImages.GetImage(ContentImageType.SlideShow);

            if (editorialImage == null) return;
            var imageId = editorialImage.Filename.ToLower();
            image.ImageUrl = Functions.DynamicImage(imageId, 63, DynamicImageType.Editorial, DynamicImageResizeType.Square);
            image.NavigateUrl = link.NavigateUrl;
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
                var imageUrl = Functions.DynamicImage(image.Filename, new Size {Width = 446, Height = 222}, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
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

        public void RenderPopularStories()
        {
            var docs = new List<IDocument>();
            const int quota = 3;
            int ceiling;
            int remainder;

            // motorcycle stories
            docs.Clear();
            var tag = Apollo.Server.Instance.GetTag("motorcycles");
            var docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q=>q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _motorcyclesDocs.DataSource = docs;
            _motorcyclesDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // products stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("products");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _productsDocs.DataSource = docs;
            _productsDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // products stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("clothing");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _clothingDocs.DataSource = docs;
            _clothingDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // london stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("london");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _londonDocs.DataSource = docs;
            _londonDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // bsb stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("bsb");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _bsbDocs.DataSource = docs;
            _bsbDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // wsb stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("wsb");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _wsbDocs.DataSource = docs;
            _wsbDocs.DataBind();

            // -------------------------------------------------------------------------------------------------------------

            // motogp stories
            docs.Clear();
            tag = Apollo.Server.Instance.GetTag("motogp");
            docSource = tag.PopularDocuments.Single(q => q.Type == DocumentType.News);
            ceiling = docSource.Items.Count < quota ? docSource.Items.Count : quota;
            for (var i = 0; i < ceiling; i++)
                docs.Add(docSource.Items[i]);

            remainder = quota - ceiling;
            if (remainder > 0)
            {
                var latestStories = tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items;
                foreach (var s in latestStories)
                {
                    if (!docs.Exists(q => q.Id == s.Id))
                        docs.Add(s);

                    if (docs.Count == quota)
                        break;
                }
            }

            _motogpDocs.DataSource = docs;
            _motogpDocs.DataBind();
        }
        #endregion
    }
}
