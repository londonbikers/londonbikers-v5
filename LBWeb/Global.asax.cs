using System;
using System.Configuration;
using System.Web;
using System.Web.Routing;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron
{
    public class Global : HttpApplication
    {
        #region event handlers
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);


            // keep a cached list of the custom tag formats.
            if (ConfigurationManager.AppSettings["Tetron.CustomTagFormats"] != null)
                Application.Add("CustomTagFormats", ConfigurationManager.AppSettings["Tetron.CustomTagFormats"].Split(char.Parse(",")));

            // keep a cached list of all the hidden forum administrators.
            //if (ConfigurationManager.AppSettings["InstantASP_HiddenAdminIDs"] != null)
            //    Application.Add("InstantASP_HiddenAdminIDs", ConfigurationManager.AppSettings["InstantASP_HiddenAdminIDs"].Split(char.Parse(",")));

            Application.Add("GuestUserCount", 0);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }

        void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session["User"] = new UserSession();
            Session.Add("GalleryImageResolutionPreference", 1024);

            // default Tetron user session data.
            var cookie = Request.Cookies["TetronGenericCookie"];
            if (cookie != null && cookie["GalleryImageResolutionPreference"] != null)
            {
                // dumping 800 preferences now we don't support the size anymore.
                var size = Convert.ToInt32(cookie["GalleryImageResolutionPreference"]);
                if (size == 800)
                {
                    cookie["GalleryImageResolutionPreference"] = "1024";
                    size = 1024;
                }

                Session["GalleryImageResolutionPreference"] = size;
            }

            // log the user into the system if they're recognised.
            Functions.AutoAuthentication();

            // if the user isn't logged in, we need to increment the guest user count.
            if (!Functions.IsUserLoggedIn())
                Functions.AddGuestVisitor();
        }

        void Application_BeginRequest(Object sender, EventArgs e)
        {
            var path = Request.Path.ToLower();
            var url = Request.Url.AbsoluteUri.ToLower();

            // check to see if the visitor is entering the www. domain, if so, get them onto the base domain.
            // this is so that there's no trouble with cookies and sessions across domains.

            if (url.IndexOf("http://www.") > -1)
                Response.Redirect(url.Replace("http://www.", "http://").Replace("default.aspx", String.Empty));

            // ID = current url's.
            // UID = legacy url's.

            var newUrl = string.Empty;
            if (path.StartsWith("/story.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "~/urlconverter.ashx?type=document&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/news.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "~/news/";
            }
            else if (path.StartsWith("/editorial-image.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=documentimage&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/article.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=document&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/galleries/category.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=gallerycategory&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/galleries/gallery.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=gallery&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/galleries/slideshow.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=galleryslideshow&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/galleries/image.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=galleryimage&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/directory/category.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=directorycategory&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }
            else if (path.StartsWith("/directory/item.aspx") && Request.QueryString["rewrite"] == null)
            {
                newUrl = "/urlconverter.ashx?type=directoryitem&id=" + Request.QueryString["id"] + "&uid=" + Request.QueryString["uid"];
            }

            if (!string.IsNullOrEmpty(newUrl))
                Response.Redirect(newUrl, true);
        }

        /// <summary>
        /// Session objects are not available at this point.
        /// </summary>
        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            LegacyUrlConversion();

            #region tag page redirects
            // bounce news/feature specific tag requests to the universal tag page.
            if (!Request.Url.AbsoluteUri.Contains("/news/tags/") && !Request.Url.AbsoluteUri.Contains("/articles/tags/"))
                return;
            var tag = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
            Response.Redirect("/tags/" + tag);
            #endregion
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

            if (Functions.IsUserLoggedIn())
            {
                //InstantASP.Common.Authentication.Authentication.Logout();
            }
            else
            {
                Functions.RemoveGuestVisitor();
            }
        }
        #endregion

        #region private methods
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("news", "news/{id}/{title}", "~/story.aspx");
            routes.MapPageRoute("articles", "articles/{id}/{title}", "~/story.aspx");
            routes.MapPageRoute("search", "search/{term}", "~/search/default.aspx");
            routes.MapPageRoute("gallery resolution selector", "galleries/res/{res}", "~/galleries/resselect.aspx");
            routes.MapPageRoute("gallery", "galleries/{id}/{title}", "~/galleries/gallery.aspx");
            routes.MapPageRoute("gallery photo", "galleries/image/{gallery}/{photo}/{title}", "~/galleries/photo.aspx");
            routes.MapPageRoute("gallery category", "galleries/category/{id}/{title}", "~/galleries/category.aspx");
            routes.MapPageRoute("directory category", "directory/category/{id}/{title}", "~/directory/category.aspx");
            routes.MapPageRoute("directory item", "directory/{id}/{title}", "~/directory/item.aspx");
            routes.MapPageRoute("tag", "tags/{term}", "~/Tag.aspx");
            routes.MapPageRoute("news image", "news/image/{did}/{iid}", "~/image.aspx");
            routes.MapPageRoute("feature image", "articles/image/{did}/{iid}", "~/image.aspx");

            // dynamic document routes.
            routes.MapPageRoute("competitions", "competitions", "~/section.aspx", false, new RouteValueDictionary { {"sid", 3 } });
            routes.MapPageRoute("competitions document", "competitions/{did}/{title}", "~/section.aspx");
            routes.MapPageRoute("partners", "partners", "~/section.aspx", false, new RouteValueDictionary { { "sid", 4 } });
            routes.MapPageRoute("partners document", "partners/{did}/{title}", "~/section.aspx");
            routes.MapPageRoute("discounts", "discounts", "~/section.aspx", false, new RouteValueDictionary { { "sid", 5 } });
            routes.MapPageRoute("discounts document", "discounts", "~/section.aspx");
            routes.MapPageRoute("rss", "rss", "~/section.aspx", false, new RouteValueDictionary { { "sid", 7 } });
            routes.MapPageRoute("rss document", "rss", "~/section.aspx");
            routes.MapPageRoute("charity", "charity", "~/section.aspx", false, new RouteValueDictionary { { "sid", 11 } });
            routes.MapPageRoute("charity document", "charity", "~/section.aspx");
            routes.MapPageRoute("events", "events/{did}", "~/section.aspx");
            routes.MapPageRoute("insurance", "insurance", "~/section.aspx", false, new RouteValueDictionary { { "sid", 15 } });
        }

        private void LegacyUrlConversion()
        {
            var isLegacy = false;
            var identifier = string.Empty;
            var type = string.Empty;

            if (Request.Url.AbsoluteUri.Contains("/galleries/gallery/"))
            {
                isLegacy = true;
                identifier = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
                type = "gallery";
            }
            else if (Request.Url.AbsoluteUri.Contains("/directory/item/"))
            {
                isLegacy = true;
                identifier = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
                type = "directoryitem";
            }
            else if (Request.Url.AbsoluteUri.Contains("/directory/category/"))
            {
                identifier = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf("/") + 1);
                if (Helpers.IsGuid(identifier))
                {
                    isLegacy = true;
                    type = "directorycategory";
                }
            }

            if (isLegacy)
            {
                var converter = new Code.UrlConverter();
                converter.Convert(type, identifier);
            }
            
            if (Request.Url.AbsoluteUri.Contains("/story.aspx?section=1&id=") && Request.Url.AbsoluteUri.EndsWith("&rewrite=1"))
            {
                // don't forget to convert these url's!
                // http://londonbikers.com/story.aspx?section=1&id=3131&rewrite=1

                var id = long.Parse(Request.QueryString["id"]);
                var doc = Apollo.Server.Instance.ContentServer.GetDocument(id);
                var url = Functions.GetApolloObjectUrl(doc);

                Response.StatusCode = 301;
                Response.RedirectLocation = url;
                Response.End();
            }
        }
        #endregion
    }
}
