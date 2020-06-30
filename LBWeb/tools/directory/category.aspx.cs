using System;
using System.Web.UI;
using Apollo.Models;
using Apollo.Utilities;

namespace Tetron.Tools.Directory
{
	public partial class CategoryPage : Page
	{
		#region members
		protected Apollo.Server _server;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
			_header.PageType = "directory";
			_header.PageBackgroundType = "none";
			_server = Apollo.Server.Instance;

			if (!IsPostBack)
				RenderForm();

			if (Request.QueryString["c"] == "persisted")
				_prompt.Text = "* Category updated!";
		}

		#region public methods
		/// <summary>
		/// Handles the persistence of any changes made to the Category object on this page.
		/// </summary>
		protected void CategoryUpdateHandler(object sender, ImageClickEventArgs ea) 
		{
			if (_name.Text == string.Empty)
			{
				_prompt.Text = "* Please supply a name for this category.";
				return;
			}

		    var directoryCategory = Helpers.IsNumeric(Request.QueryString["cid"]) ? _server.DirectoryServer.GetCategory(long.Parse(Request.QueryString["cid"])) : _server.DirectoryServer.NewCategory();
			directoryCategory.Name = _name.Text;
			directoryCategory.Description = _description.Text;
			directoryCategory.Keywords.Clear();
			Helpers.DelimitedStringToCollection(directoryCategory.Keywords, _keywords.Text);
			directoryCategory.RequiresMembership = _membershipRequired.Checked;

			// is this category to be a sub-category of another?
			if (Helpers.IsNumeric(Request.QueryString["parent"]))
				directoryCategory.ParentDirectoryCategory = _server.DirectoryServer.GetCategory(long.Parse(Request.QueryString["parent"]));

			// persist all changes.
			if (!_server.DirectoryServer.UpdateCategory(directoryCategory))
				_prompt.Text = "* Unable to update this category, a problem occured.";

			Response.Redirect("category.aspx?cid=" + directoryCategory.Id.ToString() + "&c=persisted");
		}
		#endregion

		#region private methods
		/// <summary>
		/// Populates the form with any relevant object data.
		/// </summary>
		private void RenderForm() 
		{
			// show the existing category's details if that's what we're view, else show nothing as it's new.
		    if (!Helpers.IsNumeric(Request.QueryString["cid"])) return;
		    var directoryCategory = _server.DirectoryServer.GetCategory(long.Parse(Request.QueryString["cid"]));
		    _name.Text = directoryCategory.Name;
		    _description.Text = directoryCategory.Description;
		    _keywords.Text = Helpers.CollectionToDelimitedString(directoryCategory.Keywords);
		    _membershipRequired.Checked = directoryCategory.RequiresMembership;
		}
		#endregion
	}
}