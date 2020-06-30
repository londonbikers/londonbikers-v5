using System;
using System.Web.UI;
using Tetron.Logic;

namespace Tetron
{
    public partial class SiteMaster : MasterPage
    {
        #region members
        private bool _showRhsColumn = true;
        #endregion

        #region accessors
        public Constants.NavigationArea NavigationArea { get; set; }
        /// <summary>
        /// Gets or sets the url to a cover image for the page's content. Used by Facebook and Digg for preview images.
        /// </summary>
        public string MetaImageUrl { get; set; }
        public string PageTitle { get; set;  }
        public string PageDescription { get; set; }
        /// <summary>
        /// Determines whether or not the right-hand-side column is rendered.
        /// </summary>
        public bool ShowRhsColumn
        {
            get { return _showRhsColumn; }
            set { _showRhsColumn = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cmd"] == "logoff")
                Functions.LogUserOff();

            _mainNav.NavigationArea = NavigationArea;
            if (string.IsNullOrEmpty(PageTitle))
                PageTitle = "londonbikers.com";

            Page.Title = PageTitle;
            Page.MetaDescription = PageDescription;

            if (!ShowRhsColumn)
                _rhs.Visible = false;

            // Facebook OpenGraph.
            _openGraphMetaData.OpenGraphImage = MetaImageUrl;
            _openGraphMetaData.OpenGraphUrl = Request.Url.AbsoluteUri.ToLower().Replace("default.aspx", string.Empty);
            _openGraphMetaData.OpenGraphTitle = PageTitle;
            _openGraphMetaData.OpenGraphDescription = PageDescription;

            // user controls
            if (Functions.IsUserLoggedIn())
            {
                var member = Functions.GetCurrentUser();
                _memberUserControls.Visible = true;
                _userLink.Text = member.Username;

                if (member.HasRole(Apollo.Server.Instance.UserServer.Security.GetRole("staff")))
                    _toolsLink.Text = " | <a href=\"/tools\" class=\"usrSub\">tools</a>";
            }
            else
            {
                _anonUserControls.Visible = true;
            }
        }
    }
}