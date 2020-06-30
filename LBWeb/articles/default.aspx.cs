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

namespace Tetron.Articles
{
    public partial class Default : Page
    {
        #region members
        private int _featuresItemCounter;
        private int _featuresTotalCounter;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).NavigationArea = Constants.NavigationArea.Features;
            if (Master != null) ((SiteMaster) Master).PageTitle = "Reviews & Features";

            // latest articles
            var latestArticles = Functions.GetConfiguredSite().Channels[0].Articles.LatestDocuments.RetrieveDocuments(4);
            _features.DataSource = latestArticles;
            _features.DataBind();

            // popular articles.
            var popularArticles = Apollo.Server.Instance.ContentServer.PopularDocuments.Single(q => q.Type == DocumentType.Article);
            var popularArticlesList = new List<IDocument>();
            var popularArticlesLotSize = popularArticles.Items.Count > 20 ? 20 : popularArticles.Items.Count;

            foreach (var doc in popularArticles.Items)
            {
                var doc1 = doc;
                if (!latestArticles.Exists(q => q.Id == doc1.Id))
                    popularArticlesList.Add(doc);

                if (popularArticlesList.Count == popularArticlesLotSize)
                    break;
            }

            if (popularArticlesList.Count > 0)
            {
                _popularArticlesList.DataSource = popularArticlesList;
                _popularArticlesList.DataBind();
            }
            else
            {
                // use the latest articles instead.
                var moreLatestArticles = Functions.GetConfiguredSite().Channels[0].Articles.LatestDocuments.RetrieveDocuments(24);
                
                // remove the first four as they're in the opening part of the page already.
                moreLatestArticles.RemoveRange(0, 4);

                _popularArticlesList.DataSource = moreLatestArticles;
                _popularArticlesList.DataBind();
                _popularFeaturesHeading.Text = "more";
            }

            // favourite tags.
            var tagNames = new[] {"bike reviews", "clothing", "products", "interviews", "motogp", "bsb", "mx"};
            var tags = tagNames.Select(tagName => Apollo.Server.Instance.GetTag(tagName)).ToList();
            _favouriteTagListInstance.DataSource = tags;
            _favouriteTagListInstance.DataBind();
        }

        #region event handlers
         /// <summary>
        /// Applies formatting to the popular articles repeater control items.
        /// </summary>
        protected void PopularArticlesListCreatedHandler(object sender, RepeaterItemEventArgs e)
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

            var url = string.Format("/articles/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, doc.Title);
            published.Text = Helpers.ToRelativeDateString(doc.Published); 

            var imageUrl = string.Empty;
            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true) ??
                                 doc.EditorialImages.GetImage(ContentImageType.Intro);

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
        /// Applies formatting to the latest features repeater control items.
        /// </summary>
        protected void FeaturesCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var imageLink = e.Item.FindControl("_imageLink") as Literal;
            var title = e.Item.FindControl("_title") as Literal;
            var tag = e.Item.FindControl("_tag") as Literal;
            var rowSep = e.Item.FindControl("_rowSep") as Literal;
            var url = string.Format("/articles/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));
            var image = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true) ??
                        doc.EditorialImages.GetImage(ContentImageType.Intro);

            var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 323, Height = 170 }, DynamicImageType.Editorial, DynamicImageResizeType.SetSize);
            imageLink.Text = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", url, imageUrl, WebUtils.ToSafeHtmlParameter(doc.Title));
            title.Text = string.Format("<a href=\"{0}\">{1}</a>", url, doc.Title);
            tag.Text = Functions.FormatTagNameForTitle(doc.Tags[0].Name);

            if (_featuresItemCounter == 1 && _featuresTotalCounter < 3)
            {
                rowSep.Visible = true;
                _featuresItemCounter = 0;
            }
            else
            {
                _featuresItemCounter++;
            }

            _featuresTotalCounter++;
        }

        protected void FavouriteTagInstanceHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var tag = e.Item.DataItem as ITag;
            var tagName = e.Item.FindControl("_tagName") as Literal;
            var docList = e.Item.FindControl("_docList") as Repeater;
            var docs = tag.LatestDocuments.Single(q => q.Type == DocumentType.Article).Items;

            tagName.Text = tag.Name;
            var docSelection = new List<IDocument>();
            var ceiling = docs.Count > 3 ? 3 : docs.Count;
            for (var i = 0; i < ceiling; i++)
                docSelection.Add(docs[i]);
            
            docList.DataSource = docSelection;
            docList.DataBind();
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void FavouriteTagDocsHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var doc = e.Item.DataItem as IDocument;
            var link = e.Item.FindControl("_link") as HyperLink;
            var image = e.Item.FindControl("_img") as HyperLink;

            link.Text = doc.Title;
            link.NavigateUrl = string.Format("/articles/{0}/{1}", doc.Id, WebUtils.ToUrlString(doc.Title));

            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow, true) ??
                                 doc.EditorialImages.GetImage(ContentImageType.Intro);

            if (editorialImage != null)
            {
                var imageId = editorialImage.Filename.ToLower();
                image.ImageUrl = Functions.DynamicImage(imageId, 50, DynamicImageType.Editorial, DynamicImageResizeType.Square);
                image.NavigateUrl = link.NavigateUrl;
            }
        }
        #endregion
    }
}