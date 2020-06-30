using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Apollo;
using Apollo.Directory;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Directory
{
	public partial class DefaultPage : Page
	{
		#region members
		protected Server _server;
		protected ImageButton _changeStatuses;
		private int _itemCount;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
            _header.PageType = "directory";
            _header.PageBackgroundType = "none";
            _server = Apollo.Server.Instance;

			ShowDetails();
		}

		#region event handlers
		/// <summary>
		/// Handles the rendering of each item in the table.
		/// </summary>
		protected void ItemDataBoundHandler(object sender, RepeaterItemEventArgs ea) 
		{
			if (ea.Item.ItemType == ListItemType.Header)
			{
                var noResults = ea.Item.FindControl("_noResultsRow") as HtmlTableRow;
				if (_itemCount > 0)
					noResults.Visible = false;
			}
		    if (ea.Item.ItemType != ListItemType.AlternatingItem && ea.Item.ItemType != ListItemType.Item) return;
		    var item = _server.DirectoryServer.GetItem((long)ea.Item.DataItem);
		    var title = ea.Item.FindControl("_title") as Literal;
		    var when = ea.Item.FindControl("_when") as Literal;
		    var status = ea.Item.FindControl("_status") as DropDownList;
		    status.ID = "dis__" + item.Id.ToString();
		    var submitterLink = ea.Item.FindControl("_submitterLink") as HyperLink;
		    var categoryLink = ea.Item.FindControl("_categoryLink") as HyperLink;
		    var editLink = ea.Item.FindControl("_editLink") as HyperLink;

		    title.Text = item.Title;
		    if (item.DirectoryCategories.Count > 0)
		    {
		        categoryLink.Text = item.DirectoryCategories[0].Name;
		        categoryLink.NavigateUrl = string.Format("~/directory/category/{0}/{1}", item.DirectoryCategories[0].Id, WebUtils.ToUrlString(item.DirectoryCategories[0].Name));
		    }

		    submitterLink.Text = item.Submiter.Username;
		    submitterLink.NavigateUrl = "../users/user.aspx?uid=" + item.Submiter.Uid;
		    when.Text = item.Created.ToLongDateString() + " - " + item.Created.ToShortTimeString();
		    editLink.NavigateUrl = "item.aspx?id=" + item.Id;

		    WebUtils.PopulateDropDownFromEnum(status, item.Status, false);
		    status.SelectedIndex = status.Items.IndexOf(status.Items.FindByValue(item.Status.ToString()));
		}

		/// <summary>
		/// Handles the changing of item status'.
		/// </summary>
		protected void ChangeStatusHandler(object sender, ImageClickEventArgs ea) 
		{
			for (var i = 0; i < Request.Form.Count; i++)
			{
				var key	= Request.Form.Keys[i];
				var status = Request.Form[key];

                if (key.IndexOf("dis__") > -1)
                    key = key.Substring(key.IndexOf("dis__") + 5);

			    if (!Helpers.IsNumeric(key)) continue;
			    var directoryItem = _server.DirectoryServer.GetItem(long.Parse(key));
			    if (directoryItem.Status == (DirectoryStatus) Enum.Parse(typeof (DirectoryStatus), status)) continue;
			    directoryItem.Status = (DirectoryStatus)Enum.Parse(typeof(DirectoryStatus), status);

			    // is this item being published? if so, mail the submitter.
			    if (directoryItem.Status == DirectoryStatus.Active)
			    {
			        var arguments = new string[4];
			        arguments[0] = directoryItem.Submiter.Username;
			        arguments[1] = directoryItem.Title;
			        arguments[2] = directoryItem.Id.ToString();
			        arguments[3] = WebUtils.SimpleUrlEncode(directoryItem.Title);

			        _server.CommunicationServer.SendMail(EmailType.DirectoryItemPublished, true, directoryItem.Submiter.Email, arguments);
			    }

			    _server.DirectoryServer.UpdateItem(directoryItem);
			}
		}
        
		/// <summary>
		/// Handles the searching of Directory Items.
		/// </summary>
		protected void SearchHandler(object sender, ImageClickEventArgs ea) 
		{
			if (_keyword.Text.Trim() == string.Empty)
				return;

			var finder = _server.DirectoryServer.NewItemFinder();
			finder.FindLike(_keyword.Text.Trim(), "Title");
			finder.OrderBy("Created", FinderOrder.Desc);
			
			_items.DataSource = finder.Find(50);
			_items.DataBind();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Binds the correct item source to the results-table.
		/// </summary>
		private void ShowDetails() 
		{
			var finder = new ItemFinder();
			finder.OrderBy("Created", FinderOrder.Desc);
			var ids = finder.Find(50);

			_itemCount = ids.Count;
			_items.DataSource = ids;
			_items.DataBind();			
		}
		#endregion
	}
}
