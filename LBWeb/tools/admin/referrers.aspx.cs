using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Apollo.Models;

namespace Tetron.Tools.Admin
{
	public partial class ReferrersPage : Page
	{
		#region members
		private BlacklistedReferrersCollection _referrers;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "admin";
			_header.PageBackgroundType = "none";

			if (Request.QueryString["rr"] != null)
				RemoveBlacklistedReferrer(Request.QueryString["rr"]);

			ShowDetails();
		}

		#region public methods
		/// <summary>
		/// Handles the process of adding a URL to the blacklisted referrers list.
		/// </summary>
		protected void AddBlacklistedReferrerHandler(object sender, ImageClickEventArgs ea) 
		{
		    if (_url.Text.Trim() == String.Empty) return;
		    Apollo.Server.Instance.AssetServer.BlacklistedReferrers.Add(_url.Text.Trim());
		    ShowDetails();
		}

		/// <summary>
		/// Handles the rendering of each item in the table.
		/// </summary>
		protected void ItemDataBoundHandler(object sender, RepeaterItemEventArgs ea) 
		{
			if (ea.Item.ItemType == ListItemType.Header)
			{
				var noResults = (HtmlTableRow)ea.Item.FindControl("_noResultsRow");
				if (_referrers.Count > 0)
					noResults.Visible = false;
			}

		    if (ea.Item.ItemType != ListItemType.AlternatingItem && ea.Item.ItemType != ListItemType.Item) return;
		    var referrer = (string)ea.Item.DataItem;
		    var url = (Literal)ea.Item.FindControl("_url");	
		    var link = (HyperLink)ea.Item.FindControl("_removeLink");

		    url.Text = referrer;
		    link.NavigateUrl = "referrers.aspx?rr=" + Server.UrlEncode(referrer);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Binds the referrers to the list.
		/// </summary>
		private void ShowDetails() 
		{
			_referrers = Apollo.Server.Instance.AssetServer.BlacklistedReferrers;
			_grid.DataSource = _referrers;
			_grid.DataBind();
		}

		/// <summary>
		/// Removes an existing blacklisted referrer from the system.
		/// </summary>
		private void RemoveBlacklistedReferrer(string url)
		{
			Apollo.Server.Instance.AssetServer.BlacklistedReferrers.Remove(url);
			Response.Redirect("referrers.aspx");
		}
		#endregion
	}
}