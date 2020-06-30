using System;

namespace Tetron.Tools.Admin
{
	public partial class DefaultPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "admin";
			_header.PageBackgroundType = "none";
		}
	}
}