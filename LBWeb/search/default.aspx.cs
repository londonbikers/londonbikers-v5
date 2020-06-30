using System;
using System.Web.UI;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Search
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var colon = RouteData.Values.ContainsKey("term") ? ":" : string.Empty;
            _title.Text = string.Format("<span class=\"lightText\">Search Results{0}</span>", colon);
            if (RouteData.Values.ContainsKey("term"))
            {
                var term = WebUtils.FromUrlString(RouteData.Values["term"].ToString());
                _title.Text += " " + Helpers.ToCapitalised(term);
                _searchTerm.Text = string.Format("customSearchControl.execute('{0}');", term);
            }
        }
    }
}