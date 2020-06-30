using System;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron.Tools.Gallery
{
	public partial class Category : System.Web.UI.Page
	{
		#region members
		protected Apollo.Server _server;
		private bool _isCreateMode;
		private ISite _site;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
            _header.PageType = "gallery";
            _header.PageBackgroundType = "none";

			// reference the application.
            _server = Apollo.Server.Instance;
			
			if (!string.IsNullOrEmpty(Request.QueryString["id"]))
			{
				_isCreateMode = false;

				// pre-pop the form.
				if (!Page.IsPostBack)
				{
					var galleryCategory = _server.GalleryServer.GetCategory(long.Parse(Request.QueryString["id"]));
					_name.Text = galleryCategory.Name;
                    _description.Text = galleryCategory.Description;
                    _site = galleryCategory.ParentSite;
                    _pageMode.Text = "Edit an existing " + _site.Name;
				}
			}
			else
			{
				// require a site id.
				if (!Helpers.IsNumeric(Request.QueryString["s"]))
					Response.Redirect("./");

                _site = _server.GetSite(int.Parse(Request.QueryString["s"]));
                _pageMode.Text = "Create a new " + _site.Name;
                _isCreateMode = true;
			}
		}

		#region public methods
		/// <summary>
		/// Handles the persistence of any Category object changes.
		/// </summary>
		protected void PersistCategory(object sender, EventArgs ea) 
		{
			if (_name.Text == String.Empty)
			{
                _prompt.Text = "* Please supply a name for this category.";
				return;
			}

			GalleryCategory galleryCategory;
            if (_isCreateMode)
			{
                galleryCategory = _server.GalleryServer.NewCategory();
                galleryCategory.Owner = Functions.GetCurrentUser();
                galleryCategory.ParentSite = _site;
			}
			else
			{
                galleryCategory = _server.GalleryServer.GetCategory(long.Parse(Request.QueryString["id"]));
			}

            galleryCategory.Name = _name.Text;
            galleryCategory.Description = _description.Text;

            _server.GalleryServer.UpdateCategory(galleryCategory);
            _prompt.Text = "Category Updated.";

		}
		#endregion
	}
}