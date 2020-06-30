using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Apollo.Models;

namespace Tetron.Tools.Admin
{
	public partial class CachePage : System.Web.UI.Page
	{
		#region members
		protected List<CacheItem> _items;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "admin";
			_header.PageBackgroundType = "none";

			ShowDetails();
		}

		#region public methods
		/// <summary>
		/// Clears the cache of all items, forcing a fresh build fo each object.
		/// </summary>
		protected void FlushCacheHandler(object sender, EventArgs ea) 
		{
			Apollo.Server.Instance.CacheServer.FlushCache();
			ShowDetails();
		}

		/// <summary>
		/// Handles the rendering of each item in the table.
		/// </summary>
		protected void ItemDataBoundHandler(object sender, RepeaterItemEventArgs ea) 
		{
			if (ea.Item.ItemType == ListItemType.Header)
			{
				var noResults = (HtmlTableRow)ea.Item.FindControl("_noResultsRow");
				if (_items.Count > 0)
					noResults.Visible = false;
			}
			if (ea.Item.ItemType == ListItemType.AlternatingItem || ea.Item.ItemType == ListItemType.Item)
			{
				var item = (CacheItem)ea.Item.DataItem;
				var name = (Literal)ea.Item.FindControl("_name");
				var type = (Literal)ea.Item.FindControl("_type");
				var hits = (Literal)ea.Item.FindControl("_hits");

				if (item == null)
					return;

				if (item.Item is Document)
				{
					name.Text = ((Document)item.Item).Title;
					type.Text = ((Document)item.Item).Type.ToString();
				}
				else if (item.Item is Apollo.Models.Gallery)
				{
					name.Text = ((Apollo.Models.Gallery)item.Item).Title;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is GalleryImage)
				{
					name.Text = ((GalleryImage)item.Item).Name;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is GalleryCategory)
				{
					name.Text = ((GalleryCategory)item.Item).Name;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is User)
				{
					name.Text = ((User)item.Item).Username;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is DirectoryCategory)
				{
					name.Text = ((DirectoryCategory)item.Item).Name;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is DirectoryItem)
				{
					name.Text = ((DirectoryItem)item.Item).Title;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is Site)
				{
					name.Text = ((Site)item.Item).Name;
					type.Text = item.Item.ToString();
				}
				else if (item.Item is Channel)
				{
					var channel = item.Item as Channel;
					name.Text = channel.Name + " (" + channel.ParentSite.Name + ")";
					type.Text = channel.ToString();
				}
				else if (item.Item is Section)
				{
					var section = item.Item as Section;
					if (section.ParentChannel == null)
						name.Text = section.Name;
					else
						name.Text = section.Name + " (" + section.ParentChannel.Name + ", " + section.ParentChannel.ParentSite.Name + ")";
					
					type.Text = item.Item.ToString();
				}
				else
				{
					name.Text = "<i>type not handled</i>";
					type.Text = "<i>" + item.Item + "</i>";
				}

				hits.Text = item.RequestCount.ToString();
			}
		}
		#endregion

		#region private methods
		/// <summary>
		/// Binds the cache items to the list.
		/// </summary>
		private void ShowDetails() 
		{
			_cacheCount.Text = Apollo.Server.Instance.CacheServer.ItemsCached.ToString();
			_items = Apollo.Server.Instance.CacheServer.RetrieveMostPopularItems(50);
			_grid.DataSource = _items;
			_grid.DataBind();
		}
		#endregion
	}
}