using System;

namespace Tetron.Tools
{
	public partial class DefaultPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			_header.PageTitle = "Tools";
			_header.PageType = "home";
			_header.PageBackgroundType = "none";

			// are we to welcome or bid farewell to the user?
			if (!Logic.Functions.IsUserLoggedIn())
				_salutation.Text = "Sorry but you are either not signed in, or you do not have authorisation to be here, please return from whence you came.";
			else
				_salutation.Text = string.Format("Welcome back {0}.", Logic.Functions.GetCurrentUser().Firstname);
		}
	}
}