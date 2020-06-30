using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using User = Apollo.Models.User;

namespace Tetron.Logic
{
	public partial class Functions
    {
        #region v5 methods
        public static bool IsUrlIdValid(Page page)
        {
            if (page.AppRelativeVirtualPath == "~/galleries/photo.aspx")
                return page.RouteData.Values.ContainsKey("gallery") && Helpers.IsNumeric(page.RouteData.Values["photo"] as string);

            return page.RouteData.Values.ContainsKey("id") && Helpers.IsNumeric(page.RouteData.Values["id"] as string);
        }
	    #endregion

        #region public methods
	    /// <summary>
	    /// Generates the correct URL for an Apollo object.
	    /// </summary>
	    /// <param name="content">The Apollo object to generate a URL for.</param>
	    public static string GetApolloObjectUrl(ICommonBase content)
		{
		    if (content is Document)
			{
				var doc = content as Document;
                return string.Format("/{0}/{1}/{2}", doc.Sections[0].UrlIdentifier, doc.Id, WebUtils.ToUrlString(doc.Title));
			}
    
            if (content is Gallery)
		    {
		        return string.Format("/galleries/{0}/{1}", content.Id, WebUtils.ToUrlString(((IGallery) content).Title));
		    }

            if (content is GalleryCategory)
            {
                return string.Format("/galleries/category/{0}/{1}", content.Id, WebUtils.ToUrlString(((IGalleryCategory)content).Name));
            }
		    
            if (content is GalleryImage)
		    {
		        var image = content as GalleryImage;
		        return string.Format("/galleries/image/{0}/{1}/{2}", image.ParentGalleryId, image.Id, WebUtils.ToUrlString(image.Name));
		    }
		    
            if (content is DirectoryItem)
                return string.Format("/directory/{0}/{1}", content.Id, WebUtils.ToUrlString(((DirectoryItem)content).Title));

            if (content is DirectoryCategory)
                return string.Format("/directory/category/{0}/{1}", content.Id, WebUtils.ToUrlString(((DirectoryCategory)content).Name));

		    throw new ArgumentException("Non-supported object provided.");
		}

        /// <summary>
        /// Generates the correct URL for an EditorialImage object.
        /// </summary>
        public static string GetApolloObjectUrl(IDocument document, IEditorialImage image)
        {
            var section = document.Sections[0].UrlIdentifier;
            return string.Format("/{0}/image/{1}/{2}", section, document.Id, image.Id);
        }

	    /// <summary>
        /// Provides the users default or specified gallery image size, depending on their status and/or preference.
        /// </summary>
        /// <returns>A string representing the image size, i.e. "1024"; for 1024 pixels primary dimension.</returns>
        public static int GetUsersGalleryImageSizePreference()
        {
            var resolution = 1024;
            if (IsUserLoggedIn())
                resolution = (int)HttpContext.Current.Session["GalleryImageResolutionPreference"];

            return resolution;
        }

        /// <summary>
        /// Attempts to retrieve the users formal name, i.e. firstname and lastname. If no possible, retrieves their username.
        /// </summary>
        public static string GetFormalUsername(User user)
        {
            if (user.Firstname != String.Empty && user.Lastname != String.Empty)
                return Helpers.ToCapitalised(user.Firstname) + " " + Helpers.ToCapitalised(user.Lastname);
            
            return user.Username;
        }

	    /// <summary>
		/// Performs custom formatting (capitalisation, punctuation, etc) on Tag names.
		/// </summary>
		public static string FormatTagNameForTitle(string tag)
		{
			tag = tag.ToLower();
			if (tag == String.Empty)
				return tag;

			var formats = HttpContext.Current.Application["CustomTagFormats"] as string[];
            if (formats != null)
                foreach (var t in formats.Where(t => t.ToLower() == tag))
                    return t;

	        return Helpers.ToCapitalised(tag);
		}

		/// <summary>
		/// Splits documents into collections of tags with documents.
		/// </summary>
		public static ITagCollection FilterDocumentsIntoTagGroups(IEnumerable<IDocument> documents) 
		{
			var tags = Server.Instance.GetTagCollection();
			foreach (var document in documents.Where(document => document.Tags.Count > 0))
			{
			    if (tags.Contains(document.Tags[0]))
			    {
			        tags.GetTag(document.Tags[0].Name).Documents.Add(document);
			    }
			    else
			    {
			        var tag = Server.Instance.GetTag(document.Tags[0].Name);
			        tag.Documents.Add(document);
			        tags.Add(tag);
			    }
			}

		    return tags;
		}

	    /// <summary>
		/// Converts the underlying raw url into the public rewritten url.
		/// </summary>
		public static string GetRewrittenUrl(string url, bool absoluteUrl)
		{
			url = url.ToLower();
			if (url.IndexOf("story.aspx") > -1 || url.IndexOf("article.aspx") > 1)
			{
				var section = Server.Instance.ContentServer.GetSection(int.Parse(HttpContext.Current.Request.QueryString["section"]));
				if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["id"]))
				{
					// item page
                    var doc = Server.Instance.ContentServer.GetDocument(long.Parse(HttpContext.Current.Request.QueryString["id"]));
                    url = string.Format("{0}/{1}/{2}", section.UrlIdentifier, doc.Id, WebUtils.ToUrlString(doc.Title));
				}
				else
				{
					// index page
					url = string.Format("{0}", section.UrlIdentifier);
				}
			}
			else if (url.IndexOf("galleries/gallery.aspx") > -1)
			{
                var gallery = Server.Instance.GalleryServer.GetGallery(long.Parse(HttpContext.Current.Request.QueryString["id"]));
                url = string.Format("galleries/{0}/{1}", gallery.Id, WebUtils.ToUrlString(gallery.Title));
			}
			else if (url.IndexOf("galleries/category.aspx") > -1)
			{
                var cat = Server.Instance.GalleryServer.GetCategory(long.Parse(HttpContext.Current.Request.QueryString["id"]));
                url = string.Format("galleries/category/{0}/{1}", cat.Id, WebUtils.ToUrlString(cat.Name));
			}

	        url = url.Replace("/default.aspx", string.Empty);
            if (absoluteUrl && !url.StartsWith("http://"))
                url = ConfigurationManager.AppSettings["Global.SiteURL"] + "/"+ url;

			return url;
		}
        
		/// <summary>
		/// Retrieves the Site that this application is configured for.
		/// </summary>
		public static ISite GetConfiguredSite() 
		{
			return Server.Instance.GetSite(int.Parse(ConfigurationManager.AppSettings["Apollo.SiteID"]));
		}
        
		/// <summary>
		/// Converts the names in an ContentImageType enum to an ArrayList, for databinding purposes.
		/// </summary>
		public static List<string> ContentImageTypeToArrayList()
		{
			var names = Enum.GetNames(typeof(ContentImageType));
		    return names.ToList();
		}
        
		/// <summary>
		/// Fills a DropDownList control with a representation of the Directory hirachy.
		/// </summary>
		public static void DirectoryStructureToDropDownList(DropDownList list) 
		{
			foreach (DirectoryCategory cat in Server.Instance.DirectoryServer.CommonQueries.RootCategories())
				EnumerateDirectoryCategoryDropDownList(list, cat, 0);
		}

	    /// <summary>
	    /// Returns a HTML fragment that represents a breadcrumb-navigation method.
	    /// </summary>
	    /// <param name="directoryCategory">The Category to build the fragment from.</param>
	    /// <param name="style">The type of breakcrumbs to use.</param>
	    public static string DirectoryCategoryToBreadcrumb(DirectoryCategory directoryCategory, Constants.BreadcrumbStyle style)
	    {
	        switch (style)
	        {
	            case Constants.BreadcrumbStyle.Navigational:
	                return DirectoryCategoryToNavigationalBreadcrumb(directoryCategory);
	            case Constants.BreadcrumbStyle.Listing:
	                return DirectoryCategoryToListingBreadcrumb(directoryCategory);
	            default:
	                return DirectoryCategoryToSearchResultBreadcrumb(directoryCategory);
	        }
	    }

	    /// <summary>
		/// Returns a new Container object.
		/// </summary>
		public Container NewContainer() 
		{
			return new Container();
		}
        
		/// <summary>
		/// Updates a Container in the transport mechanism.
		/// </summary>
		public void UpdateContainer(Container container) 
		{
			HttpContext.Current.Session[container.Uid.ToString()] = container;
		}

	    /// <summary>
		/// Retrieves a specific Container for the transport mechanism.
		/// </summary>
		/// <param name="uid">The Container UID.</param>
		public Container GetContainer(string uid)
		{
		    if (HttpContext.Current.Session[uid] != null)
                return HttpContext.Current.Session[uid] as Container;

		    return null;
		}

	    /// <summary>
		/// Returns a useful set of date-ranges for use in queries.  
		/// </summary>
		/// <param name="period">Acceptable periods include; Today, This week, This month, All</param>
		public DateRange Range(string period) 
		{
			var range = new DateRange();
			switch (period)
			{
				case "Today":
					range.From = DateTime.Now.AddDays(-1);
					range.To = DateTime.Now.AddDays(1);
					break;

				case "This week":
					range.From = DateTime.Now.AddDays(-7);
					range.To = DateTime.Now.AddDays(1);
					break;

				case "This month":
					range.From = DateTime.Now.AddMonths(-1);
					range.To = DateTime.Now.AddDays(1);
					break;

				case "All":
					range.From = DateTime.Parse("1/1/1970");
					range.To = DateTime.Now.AddDays(1);
					break;
			}
		
			return range;
		}
        
		/// <summary>
		/// Allows the user to be remembered each time they visit the site so no login is required.
		/// </summary>
		public static void WritePersistantAuthCookie() 
		{
			// clear any existingcookie.
			DeletePersistantAuthCookie();

			var cookie = new HttpCookie("TetronPersistantAuth")
            {
                Value = GetCurrentUser().Username,
                Expires = DateTime.Now.AddYears(1)
            };

		    HttpContext.Current.Response.Cookies.Add(cookie);
		}
    
		/// <summary>
		/// Retrieves the current logged-in User object.
		/// </summary>
		public static User GetCurrentUser()
		{
		    if (!IsUserLoggedIn())
				return null;

		    return ((UserSession) HttpContext.Current.Session["User"]).User as User;
		}

	    /// <summary>
        /// Retrieves the current logged-in UserSession object, used for tracking sessions and actions.
        /// </summary>
        public static UserSession GetCurrentUserSession()
	    {
	        if (HttpContext.Current == null || HttpContext.Current.Session == null)
				return null;

	        return HttpContext.Current.Session["User"] as UserSession;
	    }

	    /// <summary>
		/// Draws out any ScreenMessage to a HtmlGenericControl (DIV?).
		/// </summary>
		public static void RenderScreenMessage(HtmlGenericControl messageBox)
		{
			var userSession = GetCurrentUserSession();
	        if (userSession == null || userSession.PageActions.Count <= 0 || userSession.PageActions[0].ScreenMessage == String.Empty) return;
	        messageBox.Visible = true;
	        messageBox.InnerHtml = userSession.PageActions[0].ScreenMessage;
	        userSession.PageActions.Clear();
		}
        
		/// <summary>
		/// Ensures an Apollo use is correctly logged in and tracked.
		/// </summary>
		public static void LogUserIn(User user, bool autoLoginInFuture) 
		{
            // put the user into a UserSession container.
            var userSession = GetCurrentUserSession();
            userSession.User = user;
			LoginToForums(user);

            if (autoLoginInFuture)
                WritePersistantAuthCookie();
		}
        
		/// <summary>
		/// Determines whether or not the current vistor is a logged-in member.
		/// </summary>
		public static bool IsUserLoggedIn()
		{
		    var userSession = GetCurrentUserSession();
		    return userSession != null && userSession.User != null;
		}

	    /// <summary>
		/// If a user is using persistant authentication, then we can log them straight in.
		/// </summary>
		public static void AutoAuthentication() 
		{
	        if (HttpContext.Current.Request.Cookies["TetronPersistantAuth"] == null) return;
	        var username = HttpContext.Current.Request.Cookies["TetronPersistantAuth"].Value;
	        if (string.IsNullOrEmpty(username))
	        {
	            // invalid cookie, delete it.
	            HttpContext.Current.Response.Cookies.Remove("TetronPersistantAuth");
	            return;
	        }

	        var user = Server.Instance.UserServer.GetUser(username);

            #if (!DEBUG)
	        HttpContext.Current.Request.Cookies["TetronPersistantAuth"].Domain = ConfigurationManager.AppSettings["Global.Domain"];
            #endif

	        if (user != null)
	        {
	            // check the user is allowed to login.
                //if (BannedIPAddresses.IsIPBanned(null))
                //    HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["Global.SiteURL"] + "/oops.aspx?pid=3");

	            if (user.Status != UserStatus.Active)
	                HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["Global.SiteURL"] + "/oops.aspx?pid=4");

	            LogUserIn(user, true);

	            // we can drop default document names for a cleaner url.
	            var url = HttpContext.Current.Request.Url.ToString().ToLower().Replace("default.aspx", string.Empty);
	            HttpContext.Current.Response.Redirect(url);
	        }
	        else
	        {
	            // invalid cookie, delete it. most likely caused by a change of username.
	            HttpContext.Current.Response.Cookies.Remove("TetronPersistantAuth");
	        }
		}

        /// <summary>
        /// Should the user wish to log-off, this method will end the session and remove any persistant-auth cookies.
        /// </summary>
        public static void LogUserOff()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                InstantASP.Common.Authentication.Authentication.Logout();
                DeletePersistantAuthCookie();
                HttpContext.Current.Session.Abandon();
            }

            if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/forums"))
            {
                // redirect the user to the homepage.
                HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["Global.SiteURL"]);
            }
            else
            {
                // reload the current page without the command argument.
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Replace("&cmd=logoff", String.Empty).Replace("?cmd=logoff", String.Empty).Replace("default.aspx", String.Empty));
            }
        }

        /// <summary>
        /// Registers another guest user with the application.
        /// </summary>
        public static void AddGuestVisitor()
        {
            var guestCount = (int)HttpContext.Current.Application["GuestUserCount"];
            guestCount++;
            HttpContext.Current.Application["GuestUserCount"] = guestCount;
        }

        /// <summary>
        /// Registers another guest user with the application.
        /// </summary>
        public static void RemoveGuestVisitor()
        {
			if (HttpContext.Current == null)
				return;

            var guestCount = (int)HttpContext.Current.Application["GuestUserCount"];
            guestCount--;
            HttpContext.Current.Application["GuestUserCount"] = guestCount;
        }
		#endregion

		#region private methods
        /// <summary>
        /// Removes the cookie which enables the site to remember a user between sessions.
        /// </summary>
        private static void DeletePersistantAuthCookie()
        {
            HttpContext.Current.Response.Cookies["TetronPersistantAuth"].Expires = DateTime.Now.AddDays(-1);
        }

	    /// <summary>
		/// Forums use a different UMS, so we need to log-in seperately.
		/// </summary>
		private static void LoginToForums(IUser user) 
		{
            if (user.ForumUserId > 0)
            {
                var forumUser = new InstantASP.InstantForum.Components.User(user.ForumUserId);
                forumUser.Authenticate(true);
            }
            else
            {
                // forum id is zero, not good.
                Logger.LogWarning(string.Format("User '{0}' has no forum id, cannot log in to the forums.", user.Username));
                HttpContext.Current.Response.Redirect(string.Format("{0}/oops.aspx?pid=5", ConfigurationManager.AppSettings["Global.SiteURL"]));
            }
		}
        
		/// <summary>
		/// Renders a 'branch' of the Directory structure into a DropDownList control.
		/// </summary>
        private static void EnumerateDirectoryCategoryDropDownList(ListControl list, DirectoryCategory directoryCategory, int level) 
		{
		    if (list == null) throw new ArgumentNullException("list");
		    if (directoryCategory == null)
				return;

			var item = new ListItem {Value = directoryCategory.Id.ToString()};
		    var text = Helpers.MultiplyString("....", level + 1) + "|_ " + directoryCategory.Name;
			item.Text = text;
			list.Items.Add(item);

			foreach (DirectoryCategory subCat in directoryCategory.SubDirectoryCategories)
				EnumerateDirectoryCategoryDropDownList(list, subCat, level + 1);
		}
        
		/// <summary>
		/// Returns a HTML fragment that represents a breadcrumb-navigation method.
		/// </summary>
        private static string DirectoryCategoryToNavigationalBreadcrumb(IDirectoryCategory directoryCategory) 
		{
			if (directoryCategory == null)
				return string.Empty;

			var trail = new ArrayList();
			var builder = new StringBuilder();
			
			if (directoryCategory.ParentDirectoryCategory != null)
			{
                var parent = directoryCategory.ParentDirectoryCategory;
				while (parent != null)
				{
					trail.Add(parent);
					parent = parent.ParentDirectoryCategory;
				}
			}

			trail.Reverse();
			builder.Append("<a href=\"/directory\" class=\"darker\">Home</a> / ");

            foreach (DirectoryCategory cat in trail)
                builder.AppendFormat("<a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a> / ", cat.Id, WebUtils.ToUrlString(cat.Name), cat.Name);

			builder.Append(directoryCategory.Name);
			return builder.ToString();
		}
        
		/// <summary>
		/// Returns a HTML fragment that represents a breadcrumb-navigation method.
		/// </summary>
		private static string DirectoryCategoryToListingBreadcrumb(DirectoryCategory directoryCategory) 
		{
			if (directoryCategory == null)
				return String.Empty;

			var trail = new ArrayList();
			var builder = new StringBuilder();
			
			if (directoryCategory.ParentDirectoryCategory != null)
			{
				var parent = directoryCategory.ParentDirectoryCategory;
				while (parent != null)
				{
					trail.Add(parent);
					parent = parent.ParentDirectoryCategory;
				}
			}

			trail.Reverse();
            builder.Append("<a href=\"/directory\" class=\"darker\">Home</a> / ");

            foreach (DirectoryCategory cat in trail)
                builder.AppendFormat("<a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a> / ", cat.Id, WebUtils.ToUrlString(cat.Name), cat.Name);

            builder.AppendFormat("<a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a>", directoryCategory.Id, WebUtils.ToUrlString(directoryCategory.Name), directoryCategory.Name);
			return builder.ToString();
		}
		
		/// <summary>
		/// Returns a HTML fragment that represents a breadcrumb-navigation method.
		/// </summary>
		private static string DirectoryCategoryToSearchResultBreadcrumb(DirectoryCategory directoryCategory) 
		{
			if (directoryCategory == null)
				return String.Empty;

			var trail = new ArrayList();
			var builder = new StringBuilder();
		
			if (directoryCategory.ParentDirectoryCategory != null)
			{
				var parent = directoryCategory.ParentDirectoryCategory;
				while (parent != null)
				{
					trail.Add(parent);
					parent = parent.ParentDirectoryCategory;
				}
			}

			trail.Reverse();

            foreach (DirectoryCategory cat in trail)
                builder.AppendFormat("<a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a> / ", cat.Id, WebUtils.ToUrlString(cat.Name), cat.Name);

            builder.AppendFormat("<a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a>", directoryCategory.Id, WebUtils.ToUrlString(directoryCategory.Name), directoryCategory.Name);
			return builder.ToString();
		}
	    #endregion
	}
}