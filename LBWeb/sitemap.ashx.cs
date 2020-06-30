using System;
using System.Configuration;
using System.Web;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron
{
    /// <summary>
    /// Summary description for sitemap
    /// </summary>
    public class SiteMap : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/XML";
            context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            if (context.Request.QueryString["googlenews"] == null)
                RenderSiteMap(context);
            else
                RenderGoogleNewsSiteMap(context);
        }

        private static void RenderGoogleNewsSiteMap(HttpContext context)
        {
            context.Response.Write("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\n");
            var items = Apollo.Server.Instance.ContentServer.GetGoogleNewsSiteMapItems();
            var domain = ConfigurationManager.AppSettings["Global.SiteURL"];

            foreach (var item in items)
            {
                var url = String.Empty;
                switch (item.ContentType)
                {
                    case SiteMapContentType.News:
                        url = string.Format("{0}news/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.Article:
                        url = string.Format("{0}articles/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.Gallery:
                        url = string.Format("{0}galleries/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.DirectoryItem:
                        url = string.Format("{0}directory/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                }

                context.Response.Write("<url>\n");
                context.Response.Write(string.Format("<loc>{0}</loc>\n", url));
                context.Response.Write(string.Format("<lastmod>{0}</lastmod>\n", Helpers.DateTimeToIso8601String(item.LastModified)));

                context.Response.Write("<news:news>\n");
                context.Response.Write(string.Format("<news:publication_date>{0}</news:publication_date>", Helpers.DateTimeToIso8601String(item.LastModified)));
                context.Response.Write(string.Format("<news:keywords>{0}</news:keywords>", context.Server.HtmlEncode(item.Keywords)));
                context.Response.Write("</news:news>\n");

                context.Response.Write("</url>\n");
            }

            context.Response.Write("</urlset>");
        }

        private static void RenderSiteMap(HttpContext context)
        {
            context.Response.Write("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
            var items = Apollo.Server.Instance.ContentServer.GetSiteMapItems();
            var domain = ConfigurationManager.AppSettings["Global.SiteURL"];

            foreach (var item in items)
            {
                var url = String.Empty;
                switch (item.ContentType)
                {
                    case SiteMapContentType.News:
                        url = string.Format("{0}news/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.Article:
                        url = string.Format("{0}articles/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.Gallery:
                        url = string.Format("{0}galleries/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                    case SiteMapContentType.DirectoryItem:
                        url = string.Format("{0}directory/{1}/{2}", domain, item.ItemId, WebUtils.ToUrlString(item.Title));
                        break;
                }

                context.Response.Write("<url>\n");
                context.Response.Write(string.Format("<loc>{0}</loc>\n", url));
                context.Response.Write(string.Format("<lastmod>{0}</lastmod>\n", Helpers.DateTimeToIso8601String(item.LastModified)));
                context.Response.Write("</url>\n");
            }

            context.Response.Write("</urlset>");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}