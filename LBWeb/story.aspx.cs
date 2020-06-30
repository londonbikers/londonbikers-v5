using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron
{
    public partial class Story : Page
    {
        #region members
        private IDocument _document;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // url validation.
            if (!Functions.IsUrlIdValid(this))
            {
                Logger.LogWarning("Invalid url: " + Request.Url.AbsoluteUri);
                Response.Redirect("/news/");
            }

            _document = Apollo.Server.Instance.ContentServer.GetDocument(Convert.ToInt64(RouteData.Values["id"]));
            if (_document == null)
            {
                Logger.LogWarning("null document! - id: " + RouteData.Values["id"]);
                Response.Redirect("/news/");
            }

            if (Master != null)
                ((SiteMaster) Master).NavigationArea = _document.Type == DocumentType.News
                                                           ? Constants.NavigationArea.News
                                                           : Constants.NavigationArea.Features;

            // bounce from here if this isn't published and the users not a staffer.
            var user = Functions.GetCurrentUser();
            if (_document.Status != "Published" && (user == null || !user.HasRole(Apollo.Server.Instance.UserServer.Security.GetRole("staff"))))
            {
                Logger.LogWarning(string.Format("non published doc, non staffer: _document.Status: {0} username: {1}", _document.Status, user.Username));
                Response.Redirect("/news");
            }

            _document.MarkViewed(user);

            // sort out the crappy formatting from tinymce editor!
            var body = _document.Body;
            if (body.Contains("<b>Press Release:</b>"))
                _prPrefix.Visible = true;
            body = FormatBody(body);
            body = body.Replace("<b>Press Release:</b>", string.Empty);
            body = FormatBody(body);
            var firstParagraph = Helpers.GetFirstParagraph(body, out body);
            firstParagraph = WebUtils.FilterOutHtml(firstParagraph);
            body = body.Replace("align=\"justify\"", string.Empty);
            body = FormatBody(body);
            body = body.Replace(" style=\"text-align: left\" ", string.Empty);
            body = body.Replace(" style=\"text-align: left\"", string.Empty);

            // views requires admin user..
            _views.Text = string.Format("| Views: {0}", _document.Views.ToString("###,###"));
            if (Master != null)
            {
                ((SiteMaster) Master).PageTitle = _document.Title;
                ((SiteMaster) Master).PageDescription = !string.IsNullOrEmpty(_document.Abstract)
                                                            ? WebUtils.ToAttributeString(_document.Abstract)
                                                            : WebUtils.ToAttributeString(firstParagraph);
            }

            _title.Text = Server.HtmlEncode(_document.Title);
            _pubDate.Text = Helpers.ToRelativeDateString(_document.Published);
            _author.Text = GetAuthor(_document);
            _comments.CommentsCollection = _document.Comments;

            foreach (ITag tag in _document.Tags)
                _tags.Text += string.Format("<a href=\"/tags/{0}\">{1}</a>, ", WebUtils.SimpleUrlEncode(tag.Name), tag.Name);
            _tags.Text = _tags.Text.Substring(0, _tags.Text.Length - 2);

            _firstPara.Text = firstParagraph;
            _body.Text = body;

            var imageUrl = GetIntroImageUrl(_document);
            if (!string.IsNullOrEmpty(imageUrl))
                _introImage.Text = string.Format("<img src=\"{0}\" id=\"introImg\" />", imageUrl);

            var slideshowPhotos = _document.EditorialImages.FilterImages(ContentImageType.SlideShow);
            if (slideshowPhotos != null && slideshowPhotos.Count() > 0)
            {
                _photosArea.Visible = true;
                _photos.DataSource = slideshowPhotos;
                _photos.DataBind();
            }

            RenderRelatedContent(_document);

            // if there's no intro image, we should try and get a smaller cover image.
            if (!string.IsNullOrEmpty(_introImage.Text)) return;
            var coverImageUrl = GetCoverImageUrl(_document);
            if (!string.IsNullOrEmpty(coverImageUrl))
                _coverImage.Text = string.Format("<img src=\"{0}\" alt=\"\" id=\"coverImg\" />", coverImageUrl);
        }

        #region event handlers
        /// <summary>
        /// Handles the rendering of each photo in the high-res images list.
        /// </summary>
        protected void PhotoItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var image = e.Item.DataItem as EditorialImage;
            var imageHtml = e.Item.FindControl("_image") as Literal;
            if (image == null) return;
            var imageUrl = Functions.DynamicImage(image.Filename, 92, DynamicImageType.Editorial, DynamicImageResizeType.Square);
            var bigImageUrl = Functions.DynamicImage(image.Filename, 945);
            var fullSizeImageUrl = Functions.GetApolloObjectUrl(_document, image);

            if (imageHtml == null) return;
            var innerLink = Server.HtmlEncode(string.Format("<a href=\"{0}\" target=\"_blank\" class=\"ppInnerLink\">See full-size photo</a>", fullSizeImageUrl));
            imageHtml.Text = string.Format("<a href=\"{0}\" rel=\"prettyPhoto[g1]\" title=\"{1}\"><img src=\"{2}\" alt=\"{3}\" /></a>", bigImageUrl, innerLink, imageUrl, image.Name);
        }

        /// <summary>
        /// Applies formatting to the related documents repeater control items.
        /// </summary>
        protected void RelatedDocumentItemCreatedHandler(object sender, RepeaterItemEventArgs e)
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

            // not all stories have a cover image (most won't with V5).
            var editorialImageId = string.Empty;
            var editorialImage = doc.EditorialImages.GetImage(ContentImageType.Cover);
            if (editorialImage != null)
            {
                editorialImageId = editorialImage.Filename.ToLower();
            }
            else
            {
                editorialImage = doc.EditorialImages.GetImage(ContentImageType.SlideShow);
                if (editorialImage != null)
                {
                    editorialImageId = editorialImage.Filename.ToLower();
                }
                else
                {
                    editorialImage = doc.EditorialImages.GetImage(ContentImageType.Intro);
                    if (editorialImage != null)
                        editorialImageId = editorialImage.Filename.ToLower();
                }
            }

            if (!string.IsNullOrEmpty(editorialImageId))
            {
                image.ImageUrl = Functions.DynamicImage(editorialImageId, 63, DynamicImageType.Editorial, DynamicImageResizeType.Square);
                image.NavigateUrl = link.NavigateUrl;
            }
        }
        #endregion

        #region private methods
        private static string FormatBody(string body)
        {
            // remove all leading nbsp's
            while (body.StartsWith("&nbsp;"))
                body = body.Substring(6);

            // remove all leading br's.)
            while (body.StartsWith("<br />"))
                body = body.Substring(6);

            body = body.Replace("<div align=\"justify\"><br /></div>", string.Empty);

            // remove crappy justification wrapper
            if (body.StartsWith("<p align=\"justify\">") && body.EndsWith("</p>"))
            {
                body = body.Remove(0, "<p align=\"justify\">".Length);
                body = body.Remove(body.LastIndexOf("</p>"));
            }
            if (body.StartsWith("<div align=\"justify\">") && body.EndsWith("</div>"))
            {
                body = body.Remove(0, "<div align=\"justify\">".Length);
                body = body.Remove(body.LastIndexOf("</div>"));
            }
            if (body.StartsWith("<p>") && body.EndsWith("</p>"))
            {
                body = body.Remove(0, "<p>".Length);
                body = body.Remove(body.LastIndexOf("</p>"));
            }
            
            if (body.IndexOf("<br />") == -1 && body.IndexOf("<p>") == -1)
                body = body.Replace("\r\n\r\n", "<br /><br />");

            // remove weird Word crap.
            body = body.Replace("<p>&nbsp;</p>", string.Empty);
            body = body.Replace("<p class=\"MsoNormal\">&nbsp;</p>", string.Empty);
            body = body.Replace("<p class=\"MsoNormal\"", "<p");

            // remove weird misc crap.
            body = body.Replace("<br />&nbsp;<br /><br />", "<br /><br />");
            body = body.Replace("<br /><br /><br />", "<br /><br />");
            body = body.Replace("<p align=\"justify\">&nbsp;</p>", string.Empty);

            body = body.Replace("<strong>", "<b>");
            body = body.Replace("</strong>", "</b>");

            return body.Trim();
        }

        private string GetIntroImageUrl(IDocument doc)
        {
            // try to find an image that is wide enough and isn't too tall.
            var slides = doc.EditorialImages.FilterImages(ContentImageType.SlideShow).Where(q => q.Width >= 680 && q.Height <= 500).ToList();
            if (slides.Count > 0)
            {
                var r = new Random(DateTime.Now.Millisecond);
                var image = slides[r.Next(slides.Count)];

                // meta-data image.
                if (Master != null)
                    ((SiteMaster) Master).MetaImageUrl = ConfigurationManager.AppSettings["Global.SiteURL"] + Functions.DynamicImage(image.Filename, 130);

                var imageUrl = Functions.DynamicImage(image.Filename, 680);
                return imageUrl;
            }

            slides = doc.EditorialImages.FilterImages(ContentImageType.SlideShow).Where(q => q.Width >= 680).ToList();
            if (slides.Count > 0)
            {
                var r = new Random(DateTime.Now.Millisecond);
                var image = slides[r.Next(slides.Count)];

                // meta-data image.
                if (Master != null)
                    ((SiteMaster)Master).MetaImageUrl = ConfigurationManager.AppSettings["Global.SiteURL"] + Functions.DynamicImage(image.Filename, 130);

                var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 680, Height = 400 });
                return imageUrl;
            }

            // try and use an intro image.
            if (doc.Type == DocumentType.Article)
            {
                var image = doc.EditorialImages.GetImage(ContentImageType.Intro);
                // meta-data image.
                if (Master != null)
                    ((SiteMaster)Master).MetaImageUrl = ConfigurationManager.AppSettings["Global.SiteURL"] + Functions.DynamicImage(image.Filename, 130);

                var imageUrl = Functions.DynamicImage(image.Filename, new Size { Width = 680, Height = 400 });
                return imageUrl;
            }

            // failing that, return nothing.
            return string.Empty;
        }

        private static string GetCoverImageUrl(IDocument doc)
        {
            if (doc.EditorialImages.CoverImage > -1)
                return string.Format("{0}editorial/{1}", ConfigurationManager.AppSettings["Global.MediaLibraryURL"], doc.EditorialImages[doc.EditorialImages.CoverImage].Filename);

            // could add in another search for an outsized image from the slideshows but cba right now.
            return string.Empty;
        }

        private static string GetAuthor(IDocument doc)
        {
            if (!string.IsNullOrEmpty(doc.Author.Firstname) && !string.IsNullOrEmpty(doc.Author.Lastname))
                return doc.Author.Firstname + " " + doc.Author.Lastname;
            return doc.Author.Username;
        }

        private void RenderRelatedContent(IDocument doc)
        {
            var desiredStories = _body.Text.Length > 700 ? 3 : 2;

            // popular stories.
            var count = 0;
            var popularStories = new List<IDocument>();
            var newsStories = Apollo.Server.Instance.ContentServer.PopularDocuments.Single(q => q.Type == DocumentType.News);
            if (newsStories.Items.Count > 0)
            {
                while ((popularStories.Count < desiredStories) && count < newsStories.Items.Count)
                //while (popularStories.Count < desiredStories)
                {
                    var story = newsStories.Items[count];
                    if (story.Id != doc.Id)
                        popularStories.Add(story);

                    count++;
                }

                _popularStories.DataSource = popularStories;
                _popularStories.DataBind();
            }

            if (popularStories.Count == 0)
                _popularDocsArea.Visible = false;

            // -- similar stories.
            // loop all tags, add one of each
            // if not desired number, loop again.
            
            var similarStories = new List<IDocument>();
            while (similarStories.Count < desiredStories)
            {
                var c = similarStories.Count;
                foreach (var tag in doc.Tags.Cast<ITag>().TakeWhile(tag => similarStories.Count != desiredStories))
                {
                    var dtg = tag.PopularDocuments.SingleOrDefault(q => q.Type == DocumentType.News);
                    if (dtg == null || dtg.Items.Count <= 0) continue;

                    IDocument d = null;
                    foreach (var d2 in dtg.Items)
                    {
                        var d3 = d2;
                        if (similarStories.Where(q => q.Id == d3.Id).Count() != 0 || d2.Id == doc.Id) continue;
                        d = d2;
                        break;
                    }

                    if (d == null)
                        continue;

                    var isCurrentDoc = d.Id == doc.Id;
                    var isInPopDocs = popularStories.Count(p => p.Id == d.Id) > 0;
                    var isInSimDocs = similarStories.Where(q => q.Id == d.Id).Count() > 0;

                    if (!isCurrentDoc && !isInPopDocs && !isInSimDocs)
                        similarStories.Add(d);
                }

                // no tags added, so we must have done all we can.
                if (similarStories.Count == c)
                    break;
            }

            // no related stories in the popular content, so use latest tag docs instead.
            // this should be refactored to reduce code duplication.
            if (similarStories.Count == 0)
            {
                while (similarStories.Count < desiredStories)
                {
                    var c = similarStories.Count;
                    foreach (var tag in doc.Tags.Cast<ITag>().TakeWhile(tag => similarStories.Count != desiredStories))
                    {
                        var dtg = tag.LatestDocuments.SingleOrDefault(q => q.Type == DocumentType.News);
                        if (dtg == null || dtg.Items.Count <= 0) continue;

                        IDocument d = null;
                        foreach (var d2 in dtg.Items)
                        {
                            var d3 = d2;
                            if (similarStories.Where(q => q.Id == d3.Id).Count() != 0 || d2.Id == doc.Id) continue;
                            d = d2;
                            break;
                        }

                        if (d == null)
                            continue;

                        var isCurrentDoc = d.Id == doc.Id;
                        var isInPopDocs = popularStories.Count(p => p.Id == d.Id) > 0;
                        var isInSimDocs = similarStories.Where(q => q.Id == d.Id).Count() > 0;

                        if (!isCurrentDoc && !isInPopDocs && !isInSimDocs)
                            similarStories.Add(d);
                    }

                    // no tags added, so we must have done all we can.
                    if (similarStories.Count == c)
                        break;
                }
            }
            
            
            if (similarStories.Count > 0)
            {
                _relatedDocs.DataSource = similarStories.OrderByDescending(q => q.Published);
                _relatedDocs.DataBind();
            }
            else
            {
                _relatedDocsArea.Visible = false;
            }

            if (popularStories.Count == 0 && similarStories.Count == 0)
                _otherContentArea.Visible = false;
        }
        #endregion
    }
}