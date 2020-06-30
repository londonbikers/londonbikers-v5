using System;
using System.Configuration;

namespace Tetron.Tools.Content
{
	public partial class ImagePreview : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			_image.Src = ConfigurationManager.AppSettings["Global.MediaLibraryURL"] + Request.QueryString["f"];
			_image.Width = int.Parse(Request.QueryString["w"]);
		}
	}
}