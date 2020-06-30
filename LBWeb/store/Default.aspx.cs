using System;

namespace Tetron.Store
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Shop;
            if (Master != null) ((SiteMaster)Master).PageTitle = "The LB Store";
        }
    }
}