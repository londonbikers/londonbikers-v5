using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Linq;
using System.Configuration;
using Apollo;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron
{
	/// <summary>
	/// Generates an RSS 2.0 feed for a variety of content sources.
	/// </summary>
	public class FeedGenerator : IHttpHandler 
	{
		/// <summary>
		/// Main Function.
		/// </summary>
		public void ProcessRequest(HttpContext context) 
		{
			if (!Helpers.IsNumeric(context.Request.QueryString["section"]))
			{
				context.Response.Write("No section supplied!");
				return;
			}

			var section = Server.Instance.ContentServer.GetSection(int.Parse(context.Request.QueryString["section"]));
            var tag = (context.Request.QueryString["tag"] != null) ? Server.Instance.GetTag(WebUtils.SimpleUrlDecode(context.Request.QueryString["tag"])) : null;

			context.Response.Buffer = true;
			context.Response.ContentType = "text/xml";
			var writer = new XmlTextWriter(context.Response.OutputStream, System.Text.Encoding.UTF8);

			writer.WriteStartDocument();
			writer.WriteStartElement("rss");
			writer.WriteAttributeString("version", "2.0");
			writer.WriteStartElement("channel");

			writer.WriteElementString("language", "en-Gb");
			writer.WriteElementString("lastBuildDate", Helpers.DateTimeToRfc822String(DateTime.Now));
			writer.WriteElementString("link", ConfigurationManager.AppSettings["Global.SiteURL"]);
			writer.WriteElementString("description", ConfigurationManager.AppSettings["Global.SiteDescription"]);

			writer.WriteStartElement("image");
			writer.WriteElementString("title", ConfigurationManager.AppSettings["Global.Domain"]);
			writer.WriteElementString("url", "/_images/sml-lbox-logo.gif");
			writer.WriteElementString("link", ConfigurationManager.AppSettings["Global.SiteURL"]);
			writer.WriteEndElement();

			// build the feed items.
			switch (section.ContentType)
			{
			    case ContentType.Article:
			    case ContentType.News:
			        {
			            var type = (section.ContentType == ContentType.News) ? "News" : "Articles";
			            var title = "The Latest Motorcycle " + type;
			            if (tag != null)
			                title += string.Format(" (Tag: {0})", Functions.FormatTagNameForTitle(tag.Name));

			            writer.WriteElementString("title", title);
			            WriteEditorialFeedItems(writer, section, tag);
			        }
			        break;
			    case ContentType.Galleries:
			        writer.WriteElementString("title", "Latest Galleries");
			        WriteGalleryFeedItems(writer);
			        break;
			}

			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}
        
		/// <summary>
		/// Determines is subsequent requests can reuse this handler.
		/// </summary>
		public bool IsReusable { get { return false; } }
        
		#region private methods
		/// <summary>
		/// Adds feed items to an xml file for news items.
		/// </summary>
		private static void WriteEditorialFeedItems(XmlWriter writer, ISection section, ITag tag) 
		{
			var typeUrlPart = (section.ContentType == ContentType.News) ? "news" : "articles";
		    //var source = tag != null ? section.GetDocumentsByTag(tag).LatestDocuments.RetrieveDocuments(20) : section.LatestDocuments.RetrieveDocuments(20);

		    IEnumerable source;
            if (tag != null)
            {
                var i = 0;
                var docs = new List<IDocument>();
                switch (section.ContentType)
                {
                    case ContentType.News:
                        foreach (var doc in tag.LatestDocuments.Single(q => q.Type == DocumentType.News).Items)
                        {
                            docs.Add(doc);
                            if (i == 19) break;
                            i++;
                        }
                        break;
                    case ContentType.Article:
                        foreach (var doc in tag.LatestDocuments.Single(q => q.Type == DocumentType.Article).Items)
                        {
                            docs.Add(doc);
                            if (i == 19) break;
                            i++;
                        }
                        break;
                }
                source = docs;
            }
            else
            {
                source = section.LatestDocuments.RetrieveDocuments(20);
            }

			foreach (IDocument document in source)
			{
                var link = string.Format("{0}/{1}/{2}/{3}", ConfigurationManager.AppSettings["Global.SiteURL"], typeUrlPart, document.Id, WebUtils.ToUrlString(document.Title));
				writer.WriteStartElement("item");
				writer.WriteElementString("guid", link);
				writer.WriteElementString("title", document.Title);

				// tags/categories.
                writer.WriteElementString("category", Functions.FormatTagNameForTitle(document.Tags[0].Name));

				// -- disabled as FeedBurner seems only to support the last tag!
                //foreach (Tag docTag in document.Tags)
                //    writer.WriteElementString("category", docTag.Name);

				if (section.ContentType == ContentType.News || section.ContentType == ContentType.Article)
				{
					var imageFragment = String.Empty;
					if (section.ContentType == ContentType.News && document.EditorialImages.CoverImage > -1)
					{
						var cover = document.EditorialImages[document.EditorialImages.CoverImage];
                        imageFragment = string.Format("<a href=\"{0}\"><img border=\"0\" src=\"{1}/img.ashx?id={2}&nw=1&w=100&dl=1&wr=1\" style=\"float: left; padding-right: 10px;\" /></a>", link, ConfigurationManager.AppSettings["Global.SiteURL"], cover.Filename.ToLower());
					}
					else if (section.ContentType == ContentType.Article && document.EditorialImages.IntroImage > -1)
					{
						var intro = document.EditorialImages[document.EditorialImages.IntroImage];
						var mediaPath = ConfigurationManager.AppSettings["Global.MediaLibraryURL"] + "editorial/";
						imageFragment = string.Format("<a href=\"{0}\"><img border=\"0\" src=\"{1}{2}\" /></a><br /><br />", link, mediaPath, intro.Filename);
					}

					writer.WriteElementString("description", imageFragment + document.Abstract);
				}
				else
				{
					writer.WriteElementString("description", document.Abstract);
				}

				writer.WriteElementString("link", link);
				writer.WriteEndElement();
			}
		}
        
		/// <summary>
		/// Adds feed items to an xml file for galleries.
		/// </summary>
		private static void WriteGalleryFeedItems(XmlWriter writer) 
		{
		    foreach (var gallery in Functions.GetConfiguredSite().RecentGalleries.RetrieveGalleries(20))
			{
                var link = string.Format("{0}/galleries/{1}/{2}", ConfigurationManager.AppSettings["Global.SiteURL"], gallery.Id, WebUtils.ToUrlString(gallery.Title));
                var filename = ((GalleryImage) gallery.Photos[0]).GalleryImages.Thumbnail.ToLower();

				writer.WriteStartElement("item");
                writer.WriteElementString("guid", link);
				writer.WriteElementString("title", gallery.Title);

				var imageFragment = string.Format("<a href=\"{0}\"><img border=\"0\" src=\"{1}/img.ashx?s=gi&w=640&dl=1&id={2}\" style=\"float: left; padding-right: 10px;\" /></a>", link, ConfigurationManager.AppSettings["Global.SiteURL"], filename);

			    string body;
                writer.WriteElementString("description", imageFragment + Helpers.GetFirstParagraph(gallery.Description, out body));
				
				writer.WriteElementString("link", link);
				writer.WriteEndElement();
			}
		}
		#endregion
	}
}