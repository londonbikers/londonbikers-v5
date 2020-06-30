using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using Apollo.Utilities;

namespace Tetron.Tools.Admin
{
	public partial class LogsPage : System.Web.UI.Page
	{
		#region members
		protected ArrayList Entries;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "admin";
			_header.PageBackgroundType = "none";

			ShowDetails();
		}

		#region public methods
		/// <summary>
		/// Clears the site logs database.
		/// </summary>
		protected void FlushLogsHandler(object sender, EventArgs ea) 
		{
			// do something!
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
				if (Entries.Count > 0)
					noResults.Visible = false;
			}

		    if (ea.Item.ItemType != ListItemType.AlternatingItem && ea.Item.ItemType != ListItemType.Item) return;
		    var entry = ea.Item.DataItem as Logger.LogEntry;
		    var type = ea.Item.FindControl("_type") as Literal;
		    var message = ea.Item.FindControl("_message") as Literal;
		    var when = ea.Item.FindControl("_when") as Literal;
		    var link = ea.Item.FindControl("_link") as HyperLink;

		    if (entry == null)
		        return;

		    link.NavigateUrl = "logentry.aspx?id=" + entry.Id;
		    type.Text = (entry.Type == Logger.LogEntryType.Error) ? "<b>" + entry.Type + "</b>" : entry.Type.ToString();
		    message.Text = entry.Message;
		    when.Text = entry.When.ToLongDateString() + " - " + entry.When.ToLongTimeString();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Binds the log entries to the list.
		/// </summary>
		private void ShowDetails() 
		{
			Entries = Logger.GetLogs(200);
			_grid.DataSource = Entries;
			_grid.DataBind();
		}
		#endregion
	}
}