using System.Configuration;

namespace Tetron.Tools.Resources.Controls
{
	public partial  class Header : System.Web.UI.UserControl
	{
		#region members
		protected string PageBackgroundSource = string.Empty;
	    #endregion

		#region accessors
	    public string PageType { get; set; }
	    public string PageTitle { get; set; }
	    public string PageBackgroundType { get; set; }
	    #endregion

        #region constructors
        public Header()
        {
            PageBackgroundType = string.Empty;
            PageTitle = string.Empty;
            PageType = string.Empty;
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
		{
			var server = Apollo.Server.Instance;
			var user = Logic.Functions.GetCurrentUser();

			if (PageTitle != string.Empty)
				PageTitle = " - " + PageTitle;
			
			_title.InnerText = ConfigurationManager.AppSettings["Global.DefaultPageTitle"] + PageTitle;
			
			// define tabs and their status.
			_blankTab.Visible = false;
			_editorialTab.Attributes["class"] = "tab-inactive";
			_editorialTab.Attributes["background"] = "/tools/resources/images/tab-inactive.gif";
			_editorialTab.InnerHtml = "<a href=\"" + "/tools/content/\" class=\"inactivetab\" title=\"administer the editorial system\">editorial</a>";
			
			_galleryTab.Attributes["class"] = "tab-inactive";
			_galleryTab.Attributes["background"] = "/tools/resources/images/tab-inactive.gif";
			_galleryTab.InnerHtml = "<a href=\"/tools/gallery/\" class=\"inactivetab\" title=\"administer the gallery system\">gallery</a>";
			
			_usersTab.Attributes["class"] = "tab-inactive";
			_usersTab.Attributes["background"] = "/tools/resources/images/tab-inactive.gif";
			_usersTab.InnerHtml = "<a href=\"/tools/users/\" class=\"inactivetab\" title=\"manage the users\">users</a>";

			_directoryTab.Attributes["class"] = "tab-inactive";
			_directoryTab.Attributes["background"] = "/tools/resources/images/tab-inactive.gif";
			_directoryTab.InnerHtml = "<a href=\"/tools/directory/\" class=\"inactivetab\" title=\"manage the directory\">directory</a>";

			_adminTab.Attributes["class"] = "tab-inactive";
			_adminTab.Attributes["background"] = "/tools/resources/images/tab-inactive.gif";
			_adminTab.InnerHtml = "<a href=\"/tools/admin/\" class=\"inactivetab\" title=\"administer the site\">admin</a>";

			switch (PageType)
			{
			    case "editorial":
			        _editorialTab.Attributes["class"] = "tab-active";
			        _editorialTab.Attributes["background"] = "/tools/resources/images/tab-active.gif";
			        _editorialTab.InnerHtml = "<a href=\"/tools/content/\" class=\"activetab\" title=\"use the editorial system\">editorial</a>";
			        _pageLabel.Src = "/tools/resources/images/label-editorial.gif";
			        break;
			    case "gallery":
			        _galleryTab.Attributes["class"] = "tab-active";
			        _galleryTab.Attributes["background"] = "/tools/resources/images/tab-active.gif";
			        _galleryTab.InnerHtml = "<a href=\"/tools/gallery/\" class=\"activetab\" title=\"administer the gallery system\">gallery</a>";
			        _pageLabel.Src = "/tools/resources/images/label-administration.gif";
			        break;
			    case "users":
			        _usersTab.Attributes["class"] = "tab-active";
			        _usersTab.Attributes["background"] = "/tools/resources/images/tab-active.gif";
			        _usersTab.InnerHtml = "<a href=\"/tools/users/\" class=\"activetab\" title=\"manage the users\">users</a>";
			        _pageLabel.Src = "/tools/resources/images/label-administration.gif";
			        break;
			    case "directory":
			        _directoryTab.Attributes["class"] = "tab-active";
			        _directoryTab.Attributes["background"] = "/tools/resources/images/tab-active.gif";
			        _directoryTab.InnerHtml = "<a href=\"/tools/directory/\" class=\"activetab\" title=\"manage the directory\">directory</a>";
			        _pageLabel.Src = "/tools/resources/images/label-administration.gif";
			        break;
			    case "admin":
			        _adminTab.Attributes["class"] = "tab-active";
			        _adminTab.Attributes["background"] = "/tools/resources/images/tab-active.gif";
			        _adminTab.InnerHtml = "<a href=\"/tools/admin/\" class=\"activetab\" title=\"administer the site\">admin</a>";
			        _pageLabel.Src = "/tools/resources/images/label-administration.gif";
			        break;
			    default:
			        _pageLabel.Src = "/tools/resources/images/intro-welcome.gif";
			        break;
			}

			// decide which systems the user has access to.
			if (user != null)
			{
                _userName.Text = user.Firstname + " " + user.Lastname;
				var blankTabCount = 0;
				if (!user.HasRole(server.UserServer.Security.GetRole("journalist")) && 
					!user.HasRole(server.UserServer.Security.GetRole("editor")))
				{
					_editorialTab.Visible = false;
					blankTabCount++;
				}
				if (!user.HasRole(server.UserServer.Security.GetRole("gallery admin")))
				{
					_galleryTab.Visible = false;
					blankTabCount++;
				}
				if (!user.HasRole(server.UserServer.Security.GetRole("moderator")))
				{
					_usersTab.Visible = false;
					blankTabCount++;
				}
				if (!user.HasRole(server.UserServer.Security.GetRole("directory admin")))
				{
					_directoryTab.Visible = false;
					blankTabCount++;
				}
				if (!user.HasRole(server.UserServer.Security.GetRole("admin")))
				{
					_adminTab.Visible = false;
					blankTabCount++;
				}
				if (blankTabCount > 0)
				{
					_blankTab.Visible = true;
					_blankTab.ColSpan = 5 - blankTabCount;
				}
			}
			
			// decide on the page background.
			switch (PageBackgroundType)
			{
				case "pillar":
					PageBackgroundSource = "/tools/resources/images/page-bg-default.gif";
					break;
				case "none":
					PageBackgroundSource = "/tools/resources/images/page-editorial-bg.gif";
					break;
				case "document":
					PageBackgroundSource = "/tools/resources/images/document-bg.gif";
					break;
			}

			// deny access to the following controls if the user is not logged in.
			if (user == null)
			{
				_editorialTab.Visible = false;
				_galleryTab.Visible = false;
				_usersTab.Visible = false;
				_directoryTab.Visible = false;
				_adminTab.Visible = false;
			}

			// user authentication management.
			if (user != null)
				_signinHref.Visible	= false;	
			else
				_signinHref.HRef = "/";

			// define the section menu.
			switch (PageType)
			{
				case "directory":
					_sectionMenu.Text = "&#187; <a href=\"default.aspx\" title=\"manage the directory items\">manage items</a><br />\n";
					_sectionMenu.Text += "&#187; <a href=\"structure.aspx\" title=\"manage the diretory structure\">edit structure</a><br />\n";
					break;
				case "editorial":
					_sectionMenu.Text = "&#187; <a href=\"document.aspx\" title=\"author a new document\">new document</a><br />\n";
                    _sectionMenu.Text += "&#187; <a href=\"incoming.aspx\" title=\"incoming documents\">incoming news</a><br />\n";
					_sectionMenu.Text += "&#187; <a href=\"featured-documents.aspx\" title=\"review the featured documents\">featured docs</a><br />\n";
					_sectionMenu.Text += "&#187; <a href=\"images.aspx\" title=\"view the image library\">image library</a><br />\n";
                    _sectionMenu.Text += "&#187; <a href=\"comments.aspx\" title=\"moderate the member comments\">comments</a>\n";
					break;
				case "users":
					_sectionMenu.Text = "User stats:<br /><br />\n";
					_sectionMenu.Text += "<b>" + server.UserServer.Statistics.UsersCount + "</b> Total<br />\n";
					_sectionMenu.Text += "<b>" + server.UserServer.Statistics.ActiveUsersCount + "</b> Active<br />\n";
					_sectionMenu.Text += "<b>" + server.UserServer.Statistics.SuspendedUsers + "</b> Suspended<br />\n";
					_sectionMenu.Text += "<b>" + server.UserServer.Statistics.DeletedUsers + "</b> Deleted";
					break;
				case "admin":
					_sectionMenu.Text = "&#187; <a href=\"referrers.aspx\" title=\"manage the site referrers\">referrers</a><br />\n";
					_sectionMenu.Text += "&#187; <a href=\"cache.aspx\" title=\"manage the application cache\">cache</a><br />\n";
					_sectionMenu.Text += "&#187; <a href=\"logs.aspx\" title=\"manage the application logs\">logs</a><br />\n";
					break;
			}
		}
	}
}