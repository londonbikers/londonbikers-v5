using System;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities;

namespace Tetron.Tools.Directory
{
	public partial class StructurePage : System.Web.UI.Page
	{
		#region members
		protected Apollo.Server _server;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
			_header.PageTitle = "Tools";
            _header.PageType = "directory";
            _header.PageBackgroundType = "none";
            _server = Apollo.Server.Instance;

			if (Request.QueryString["c"] == "delcat")
				DeleteCategory();

			BuildTree();
		}
        
		#region private methods
		/// <summary>
		/// Handles the deletion of a specific category.
		/// </summary>
		private void DeleteCategory() 
		{
		    if (!Helpers.IsNumeric(Request.QueryString["cid"])) return;
		    var directoryCategory = _server.DirectoryServer.GetCategory(long.Parse(Request.QueryString["cid"]));
		    if (directoryCategory.DirectoryItems.Count > 0 || directoryCategory.SubDirectoryCategories.Count > 0)
		    {
		        _prompt.Text = "Selected category still has items, or sub-categories, cannot delete!<br /><br />";
		    }
		    else
		    {
		        _server.DirectoryServer.DeleteCategory(directoryCategory);
		        Response.Redirect("structure.aspx");
		    }
		}
        
		/// <summary>
		/// Initiates the rendering of the Directory tree structure.
		/// </summary>
		private void BuildTree() 
		{
            var root = new TreeNode("<b>Categories</b>");
			_treeView.Nodes.Add(root);

			foreach (DirectoryCategory category in _server.DirectoryServer.CommonQueries.RootCategories())
                RenderTreeNode(category, root, false);
		}

		/// <summary>
		/// Recursive function to map out the current branch, and any others below the current.
		/// </summary>
        private void RenderTreeNode(DirectoryCategory directoryCategory, TreeNode currentNode, bool expandNode) 
		{
            var url = string.Format("category.aspx?id={0}", directoryCategory.Id);

            var newNode = new TreeNode("&nbsp;" + directoryCategory.Name, string.Empty, "/_images/silk/folder.png")
            {
                Expanded = expandNode,
                SelectAction = TreeNodeSelectAction.Expand
            };
		    currentNode.ChildNodes.Add(newNode);

            var editNode = new TreeNode("&nbsp;<u>Edit Category</u>", string.Empty, "/_images/silk/folder_edit.png", string.Format("category.aspx?cid={0}", directoryCategory.Id), "_top");
            var deleteNode = new TreeNode("&nbsp;<u>Delete Category</u>", string.Empty, "/_images/silk/folder_delete.png", string.Format("structure.aspx?c=delcat&cid={0}", directoryCategory.Id), "_top");
            var newCategoryNode = new TreeNode("&nbsp;<u>New Category</u>", string.Empty, "/_images/silk/folder_add.png", string.Format("category.aspx?parent={0}", directoryCategory.Id), "_top");

			newNode.ChildNodes.Add(editNode);
			newNode.ChildNodes.Add(deleteNode);
			newNode.ChildNodes.Add(newCategoryNode);

			foreach (DirectoryCategory childCategory in directoryCategory.SubDirectoryCategories)
				RenderTreeNode(childCategory, newNode, expandNode);
		}
		#endregion
	}
}