using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Configuration;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron.Tools.Content
{
	public partial class images : Page
	{
		#region members
		private	Server _server;
		private	List<long> _results;
	    private	int _currentRecord;
		private int _maxRecords;
		private	bool _advanceImage = true;
		private	EditorialImage _propertyEditorialImage;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
            _header.PageTitle = "Tools";
            _header.PageType = "editorial";
            _header.PageBackgroundType = "none";

            _server = Apollo.Server.Instance;
            Functions.GetCurrentUser();
		    _maxRecords = 50;

		    if (Page.IsPostBack) return;
		    _type.DataSource = Functions.ContentImageTypeToArrayList();
		    _type.DataBind();

		    if (Request.QueryString["a"] == "delimg" && Helpers.IsNumeric(Request.QueryString["id"]))
		    {
		        _server.ContentServer.DeleteImage(_server.ContentServer.GetImage(long.Parse(Request.QueryString["id"])), false);
		        ExecuteSearch(true);
		    }
		    else
		    {
		        ExecuteSearch(false);
		    }
		}

		#region public methods
		/// <summary>
		/// Handles the users serch form action.
		/// </summary>
		protected void FindImagesEvnt(object sender, ImageClickEventArgs ea) 
		{
			ExecuteSearch(false);
		}

		protected string Property(string name) 
		{
			var property = string.Empty;
		    if (_advanceImage)
			{
				try
				{
                    _propertyEditorialImage = _server.ContentServer.GetImage(_results[_currentRecord]);
                    _advanceImage = false;
				}
				catch
				{
					// the image does not exist.
					return string.Empty;
				}
			}

			switch (name)
			{
				case "Image":
                    var adjustedWidth = _propertyEditorialImage.Width < 200 ? _propertyEditorialImage.Width : 200;
					string url;
                    if (_propertyEditorialImage.Type == ContentImageType.SlideShow)
                        url = "../../img.ashx?id=" + _propertyEditorialImage.Filename + "&dl=1&w=" + adjustedWidth + "&nw=1";
					else
                        url = ConfigurationManager.AppSettings["Global.MediaLibraryURL"] + "editorial/" + _propertyEditorialImage.Filename;

                    property = "<a href=\"javascript:preview('image-preview.aspx?f=editorial/" + _propertyEditorialImage.Filename + "&w=" + _propertyEditorialImage.Width + "&h=" + _propertyEditorialImage.Height + "', " + (_propertyEditorialImage.Width + 40) + ", " + (_propertyEditorialImage.Height + 25) + ")\">";
					property += "<img style=\"border-style:solid; margin-bottom: 2px; border-color:#7A7A7A; border-width:1px;\" src=\"" + url + "\" border=\"0\" width=\"" + adjustedWidth + "\" />";
					property += "</a>";
					break;
				case "Name":
                    property = _propertyEditorialImage.Name;
					break;
				case "Type":
                    property = _propertyEditorialImage.Type.ToString();
					break;
				case "Dimensions":
                    property = _propertyEditorialImage.Width + " x " + _propertyEditorialImage.Height;
					break;
				case "DeleteLink":
                    if (_propertyEditorialImage.AssociationCount == 0)
                        property = "<a href=\"images.aspx?a=delimg&id=" + _propertyEditorialImage.Id + "\" title=\"delete this image\">(delete)</a>";
					else
						property = "(delete)";

                    _currentRecord++;
                    _advanceImage = true;
					break;
			}

			return property;
		}
		#endregion

		#region private methods
		private void ExecuteSearch(bool runPreviousSearch) 
		{
            var finder = _server.ContentServer.NewImageFinder();
			var keyword = string.Empty;
			var type = string.Empty;

			if (runPreviousSearch)
			{
				// repeat last query.
                if (Session["ImageSearchKeyword"] != null)
                    keyword = Session["ImageSearchKeyword"] as string;

				if (Session["ImageSearchType"] != null)
					type = Session["ImageSearchType"] as string;
			}
			else
			{
				// new search.
				keyword = _keyword.Text;
                if (_byType.Checked)
                    type = _type.SelectedItem.Value;
			}

			if (keyword != String.Empty)
				finder.FindLike(keyword, "Name");

			if (!string.IsNullOrEmpty(type))
			{
                finder.FindValue(((int)(ContentImageType)Enum.Parse(typeof(ContentImageType), type)).ToString(), "Type");
			}
			else
			{
				// show default types.
				finder.FindValue(((int)ContentImageType.Normal).ToString(), "Type");
				finder.FindValue(((int)ContentImageType.Cover).ToString(), "Type");
				finder.FindValue(((int)ContentImageType.Intro).ToString(), "Type");
			}

			finder.OrderBy("Created", FinderOrder.Desc);
            _results = finder.Find(_maxRecords);

			// ensure the search criteria is cached for re-use later if need be.
			Session["ImageSearchKeyword"] = keyword;
			Session["ImageSearchType"] = type;

			BindGrid();
		}
        
		private void BindGrid() 
		{
			// create a dummy datasource.
            var dummySource = new string[Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_results.Count / 5)) + 1)];

            _grid.DataSource = dummySource;
            _grid.DataBind();
            _grid.Visible = true;
            _currentRecord = 0;

            if (_grid.Items.Count > 0)
			{
                _prompt.Text = "Found " + _results.Count + " image";
                if (_grid.Items.Count > 1)
                    _prompt.Text += "s.";
				else
                    _prompt.Text += ".";
                _prompt.Text += " (" + _maxRecords + " max)";
			}
			else
			{
                _prompt.Text = "No images found.";
                _grid.Visible = false;
			}
		}
		#endregion
	}
}