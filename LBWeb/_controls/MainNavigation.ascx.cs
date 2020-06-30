using System;

namespace Tetron.Controls
{
    public partial class MainNavigation : System.Web.UI.UserControl
    {
        #region members
        protected string HomeSelected;
        protected string NewsSelected;
        protected string FeaturesSelected;
        protected string PhotosSelected;
        protected string EventsSelected;
        protected string DatabaseSelected;
        protected string ShopSelected;
        protected string CommunitySelected;
        #endregion

        #region accessors
        public Constants.NavigationArea NavigationArea { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            const string seleced = " nselected";
            switch (NavigationArea)
            {
                case Constants.NavigationArea.Home:
                    HomeSelected = seleced;
                    break;
                case Constants.NavigationArea.News:
                    NewsSelected = seleced;
                    break;
                case Constants.NavigationArea.Features:
                    FeaturesSelected = seleced;
                    break;
                case Constants.NavigationArea.Photos:
                    PhotosSelected = seleced;
                    break;
                case Constants.NavigationArea.Events:
                    EventsSelected = seleced;
                    break;
                case Constants.NavigationArea.Database:
                    DatabaseSelected = seleced;
                    break;
                case Constants.NavigationArea.Shop:
                    ShopSelected = seleced;
                    break;
                case Constants.NavigationArea.Community:
                    CommunitySelected = seleced;
                    break;
            }
        }
    }
}