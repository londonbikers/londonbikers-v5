using System;

namespace Tetron.Tools
{
	public partial class NotAuthorised : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			_header.PageTitle = "Tools";
			_header.PageType = "home";
			_header.PageBackgroundType = "none";
		}
	}
}