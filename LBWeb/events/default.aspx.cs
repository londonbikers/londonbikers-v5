using System;
using System.Web.UI;

namespace Tetron.Events
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).NavigationArea = Constants.NavigationArea.Events;
            if (Master != null) ((SiteMaster) Master).PageTitle = "Events Calendar";
        }
    }
}