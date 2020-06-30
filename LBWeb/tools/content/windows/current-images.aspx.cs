using System;
using System.Linq;
using System.Web.UI;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;

namespace Tetron.Tools.Content.Windows
{
	public partial class current_images : Page
	{
		#region members
		private Logic.Functions	_functions;
		private Server _server;
		private int	_position;
		private Document _document;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			// collect essential objects.
            _functions = new Logic.Functions();
            _server = Apollo.Server.Instance;
            _document = _functions.GetContainer(Request.QueryString["container"]).ApolloDocument;

			// are we to select the cover or intro image?
			if (IsPostBack && Request.Form["_coverImageIndex"] != String.Empty)
				SelectCoverImage();
			if (IsPostBack && Request.Form["_introImageIndex"] != String.Empty)
				SelectIntroImage();

			// bind the grid.
            if (_document.EditorialImages.Count > 0)
			{
                _currentImages.DataSource = _document.EditorialImages.List;
                _currentImages.DataBind();
                _prompt.Text = String.Empty;
			}
			else
			{
                _prompt.Text = "<br />&nbsp;&nbsp<b>none.</b>";
			}
		}

		#region public methods
		protected void RemoveImages(object sender, ImageClickEventArgs ea) 
		{
			foreach (var image in from string imageId in Request.Form
			                      where Helpers.IsNumeric(imageId)
			                      select _document.EditorialImages.GetImage(long.Parse(imageId)))
			{
			    _document.EditorialImages.Remove(image.Id);
			    if (image.Type == ContentImageType.SlideShow)
			    {
			        try
			        {
			            _server.ContentServer.DeleteImage(image, true);
			        }
			        catch
			        {
			        }
			    }
			}

            if (_document.EditorialImages.Count == 0)
			{
                _document.EditorialImages.CoverImage = -1;
                _document.EditorialImages.IntroImage = -1;
			}

			Response.Redirect("current-images.aspx?container=" + Request.QueryString["container"]);
		}
        
		protected string Property(string field) 
		{
			var property = String.Empty;
            var image = _document.EditorialImages[_position];

			switch (field)
			{
				case "Image":
					property = "<a href=\"javascript:preview('../image-preview.aspx?f=editorial/" + image.Filename + "&w=" + image.Width + "&h=" + image.Height + "', " + (image.Width + 40) + ", " + (image.Height + 25) + ")\">";
					property += "<img src=\"/img.ashx?id=" + image.Filename + "&dl=1&w=60&nw=1\" style=\"border-width:2;border-color:#6E7A67;border-style:solid\" width=\"60\" title=\"size: " + image.Width + "x" + image.Height + "\" />";
					property += "</a>";
					break;
				case "Name":
					property = image.Name;
					break;
				case "Index":
                    property = _position.ToString();
					break;
				case "CoverChecked":
                    if (_document.EditorialImages.CoverImage == _position)
						property = "checked=\"checked\"";
					break;
				case "IntroChecked":
                    if (_document.EditorialImages.IntroImage == _position)
						property = "checked=\"checked\"";
					break;
				case "Type":
					property = image.Type.ToString();
					break;
				case "ID":
					property = image.Id.ToString();
                    _position++;
					break;
			}

			return property;
		}
		#endregion

		#region private methods
		private void SelectCoverImage() 
		{
			// sets the given image as the documents cover image.
			var newIndex = int.Parse(Request.Form["_coverImageIndex"]);
			if (_document.EditorialImages.CoverImage != newIndex)
                _document.EditorialImages.CoverImage = newIndex;
			else
                _document.EditorialImages.CoverImage = -1;
		}

		private void SelectIntroImage() 
		{
			// sets the given image as the documents intro image.
			var newIndex = int.Parse(Request.Form["_introImageIndex"]);
			if (_document.EditorialImages.IntroImage != newIndex)
                _document.EditorialImages.IntroImage = newIndex;
			else
                _document.EditorialImages.IntroImage = -1;
		}
		#endregion
	}
}