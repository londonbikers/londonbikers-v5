using System;
using System.Web.UI.WebControls;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Tetron.Tools.Gallery
{
	public partial class DefaultPage : System.Web.UI.Page
	{
		#region members
		protected Apollo.Server _server;
		private ISite _site;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
            _header.PageType = "gallery";
            _header.PageBackgroundType = "none";
            _server = Apollo.Server.Instance;

			if (Request.QueryString["a"] == "delcat" && Helpers.IsNumeric(Request.QueryString["id"]))
				DeleteCategory(long.Parse(Request.QueryString["id"]));

		    if (Page.IsPostBack) return;
		    _siteList.DataSource = _server.GetSites();
		    _siteList.DataTextField = "Name";
		    _siteList.DataValueField = "ID";
		    _siteList.DataBind();

		    _site = _server.GetSite(int.Parse(_siteList.SelectedValue));
		    _newCategoryLink.NavigateUrl = "category.aspx?s=" + _siteList.SelectedValue;
		    BuildTree();
		}
        
		#region public methods
		/// <summary>
		/// Handles the changing of the current site.
		/// </summary>
		public void SiteChangedHandler(object sender, EventArgs ea) 
		{
            _site = _server.GetSite(int.Parse(_siteList.SelectedValue));
            _newCategoryLink.NavigateUrl = "category.aspx?s=" + _siteList.SelectedValue;
			BuildTree();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Initiates the rendering of the Gallery tree structure.
		/// </summary>
		private void BuildTree() 
		{
            var root = new TreeNode("<b>Galleries</b>");
            _treeView.Nodes.Add(root);

            foreach (var category in _site.GalleryCategories)
				RenderTreeNode(category, root, false);
		}

		/// <summary>
		/// Recursive function to map out the current branch, and any others below the current.
		/// </summary>
		private static void RenderTreeNode(IGalleryCategory galleryCategory, TreeNode currentNode, bool expandNode) 
		{
            var newNode = new TreeNode("&nbsp;" + galleryCategory.Name, string.Empty, "/_images/silk/folder.png")
            {
                Expanded = expandNode,
                SelectAction = TreeNodeSelectAction.Expand
            };
		    currentNode.ChildNodes.Add(newNode);

            var editNode = new TreeNode("&nbsp;<u>Edit Category</u>", string.Empty, "/_images/silk/folder_edit.png", string.Format("category.aspx?id={0}", galleryCategory.Id), "_top");
            var deleteNode = new TreeNode("&nbsp;<u>Delete Category</u>", string.Empty, "/_images/silk/folder_delete.png", string.Format("default.aspx?a=delcat&id={0}", galleryCategory.Id), "_top");
            var newGalleryNode = new TreeNode("&nbsp;<u>New Gallery</u>", string.Empty, "/_images/silk/picture_add.png", string.Format("gallery.aspx?category={0}", galleryCategory.Id), "_top");

			newNode.ChildNodes.Add(editNode);
			newNode.ChildNodes.Add(deleteNode);
			newNode.ChildNodes.Add(newGalleryNode);

			// add the galleries.
			foreach (var gallery in galleryCategory.AllGalleries)
			{
				var gNode = new TreeNode
                {
                    NavigateUrl = string.Format("gallery.aspx?category={0}&id={1}", galleryCategory.Id, gallery.Id),
                    Text = "&nbsp;" + gallery.Title,
                    ImageUrl = "/_images/silk/pictures.png"
                };

			    newNode.ChildNodes.Add(gNode);
			}
		}

		/// <summary>
		/// Deletes a specific Gallery Category.
		/// </summary>
		private void DeleteCategory(long id)
		{
			// delete the category.
			var galleryCategory = _server.GalleryServer.GetCategory(id);
			_server.GalleryServer.DeleteCategory(galleryCategory);
			Response.Redirect("./");
		}
		#endregion
	}
}