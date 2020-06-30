using System;
using System.Configuration;
using System.Drawing;
using System.Web.UI;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron
{
    public partial class EditorialImagePage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RouteData.Values["did"] == null || RouteData.Values["iid"] == null)
                Response.Redirect("/");

            var server = Apollo.Server.Instance;
            var document = server.ContentServer.GetDocument(long.Parse(RouteData.Values["did"].ToString()));
            var image = document.EditorialImages.GetImage(long.Parse(RouteData.Values["iid"].ToString()));

            if (document == null)
            {
                Logger.LogWarning("null document! - id: " + RouteData.Values["did"]);
                Response.Redirect("/");
            }

            if (image == null)
            {
                Logger.LogWarning("null image! - id: " + RouteData.Values["iid"]);
                Response.Redirect("/");
            }

            var imageUrl = Functions.DynamicImage(image.Filename, 680, DynamicImageType.Editorial, DynamicImageResizeType.ByWidth, false, true);
            _image.ImageUrl = imageUrl;
            _image.NavigateUrl = "/img.ashx?id=" + image.Filename.ToLower();

            string remainder;
            var firstParagraph = Helpers.GetFirstParagraph(document.Body, out remainder);
            var documentType = String.Empty;
            switch (document.Type)
            {
                case DocumentType.News:
                    documentType = "Story";
                    break;
                case DocumentType.Article:
                    documentType = "Article";
                    break;
            }

            ((SiteMaster) Master).PageTitle = string.Format("Photo for: {0}", document.Title);
            ((SiteMaster) Master).PageDescription = WebUtils.FilterOutHtml(firstParagraph);
            ((SiteMaster) Master).MetaImageUrl = ConfigurationManager.AppSettings["Global.SiteURL"] + Functions.DynamicImage(image.Filename, 130);

            _description.Text = firstParagraph;
            _title.Text = document.Title;

            _documentLink.NavigateUrl = Functions.GetApolloObjectUrl(document);
            _documentLink.Text = string.Format("Read the full {0}", documentType.ToLower());
        }
    }
}