using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;

namespace Tetron.Tools.Content.Windows
{
	public partial class FindImages : Page
	{
		#region members
		protected string Prompt;
		private Server _server;
	    private Logic.Functions _functions;
		private List<long> _idList;
		private bool _enumerateImage = true;
		private int _position;
		private EditorialImage _editorialImage;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_server = Apollo.Server.Instance;
		    _functions = new Logic.Functions();

			if (Request.QueryString["search"] != null)
				ExecuteSearch();

			if (Request.QueryString["add"] == "true")
				_body.Attributes.Add("onload", "parent._currentImages.location.replace('current-images.aspx?container=" + Request.QueryString["container"] + "');");
		}

		#region public methods
		protected string Property(string field) 
		{
			var property = string.Empty;
            if (_enumerateImage)
			{
                _editorialImage = _server.ContentServer.GetImage(_idList[_position]);
                _enumerateImage = false;
			}

			switch (field)
			{
				case "Image":
                    property = "<a href=\"javascript:preview('../image-preview.aspx?f=editorial/" + _editorialImage.Filename + "&w=" + _editorialImage.Width + "&h=" + _editorialImage.Height + "', " + (_editorialImage.Width + 40) + ", " + (_editorialImage.Height + 25) + ")\">";
                    property += "<img src=\"/img.ashx?id=" + _editorialImage.Filename + "&dl=1&w=60\" style=\"border-width: 2px; border-color: #6E7A67; border-style: solid;\" width=\"60\" title=\"size: " + _editorialImage.Width + "x" + _editorialImage.Height + "\" />";
					property += "</a>";
					break;
				case "Name":
                    property = _editorialImage.Name;
					break;
				case "Width":
                    property = _editorialImage.Width.ToString();
					break;
				case "Height":
                    property = _editorialImage.Height.ToString();
					break;
				case "Type":
                    property = _editorialImage.Type.ToString();
					break;
				case "ID":
					property = _editorialImage.Id.ToString();
					_position++;
                    _enumerateImage = true;
					break;
			}

			return property;
		}

		protected void AddImages(object sender, ImageClickEventArgs ea) 
		{
            var document = _functions.GetContainer(Request.QueryString["container"]).ApolloDocument;
			foreach (var element in Request.Form.Cast<string>().Where(Helpers.IsNumeric))
			    document.EditorialImages.Add(long.Parse(element));

			Response.Redirect("find-images.aspx?container=" + Request.QueryString["container"] + "&search=1&keyword=" + Request.QueryString["keyword"] + "&add=true");
		}
		#endregion

		#region private methods
		private void ExecuteSearch() 
		{
			var finder = _server.ContentServer.NewImageFinder();
			if (!string.IsNullOrEmpty(Request.QueryString["keyword"]))
				finder.FindLike(Request.QueryString["keyword"].Trim(), "Name");

			finder.FindValue(((int)ContentImageType.Normal).ToString(), "Type");
			finder.FindValue(((int)ContentImageType.Cover).ToString(), "Type");
			finder.FindValue(((int)ContentImageType.Intro).ToString(), "Type");
            finder.OrderBy("Created", FinderOrder.Desc);
			
			_idList = finder.Find(100);
			BindGrid();
		}

		private void BindGrid() 
		{
			if (_idList.Count > 0)
			{
                _results.DataSource = _idList;
                _results.DataBind();
                _results.Visible = true;
                Prompt = "Found " + _idList.Count + " images (100 max).";
			}
			else
			{
                _results.Visible = false;
                Prompt = "No images found.";
			}
		}
		#endregion
	}
}