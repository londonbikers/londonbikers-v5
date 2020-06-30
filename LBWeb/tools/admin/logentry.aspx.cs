using System;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Admin
{
	public partial class LogEntryPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "admin";
			_header.PageBackgroundType = "none";

			ShowDetails();
		}
		
		#region private methods
		/// <summary>
		/// Binds the log entries to the list.
		/// </summary>
		private void ShowDetails() 
		{
			var log = Logger.GetLog(int.Parse(Request.QueryString["id"]));

			_id.Text = log.Id.ToString();
			_when.Text = log.When.ToString();
			_context.Text = log.Context;
			_message.Text = log.Message;
            _stackTrace.Text = WebUtils.PlainTextToHtml(log.StackTrace);
		}
		#endregion
	}
}