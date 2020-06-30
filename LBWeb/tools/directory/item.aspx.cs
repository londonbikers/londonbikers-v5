using System;
using System.Linq;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Directory
{
	public partial class ItemPage : System.Web.UI.Page
	{
		#region members
		private Apollo.Server _server;
		private DirectoryItem _directoryItem;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
            _header.PageTitle = "Tools";
            _header.PageType = "directory";
            _header.PageBackgroundType = "none";
            _server = Apollo.Server.Instance;
            _prompt.EnableViewState = false;

			if (!Helpers.IsNumeric(Request.QueryString["id"]))
				Response.Redirect("./");

			// has an item already been placed into session?
			if (Request.UrlReferrer.LocalPath == Request.Url.LocalPath)
			{
                _directoryItem = Session["WorkingDirectoryItem"] as DirectoryItem;
			}
			else
			{
				// collect item.
				_directoryItem = _server.DirectoryServer.GetItem(long.Parse(Request.QueryString["id"]));
				Session["WorkingDirectoryItem"] = _directoryItem;
			}

			switch (Request.QueryString["a"])
			{
				case "rl":
					RemoveLinkHandler();
					break;
				case "ri":
					RemoveImageHandler();
					break;
				case "rc":
					RemoveCategoryHandler();
					break;
			}

			if (!Page.IsPostBack)
				ShowDetails();
		}
        
		#region public methods
		/// <summary>
		/// Handles the deletion of the current item.
		/// </summary>
		protected void DeleteItemHandler(object sender, EventArgs ea) 
		{
			_server.DirectoryServer.DeleteItem(_directoryItem);
			Response.Redirect("./");
		}

		/// <summary>
		/// Handles the processing of the item submission.
		/// </summary>
		protected void UpdateItemHandler(object sender, EventArgs ea) 
		{
			if (!IsValidForm())
				return;

            _directoryItem.Title = _title.Text.Trim();
            _directoryItem.Description = _description.Text.Trim();
            _directoryItem.TelephoneNumber = _telephoneNumber.Text.Trim();
            _directoryItem.Postcode = _postcode.Text;

            _directoryItem.Keywords.Clear();
            Helpers.DelimitedStringToCollection(_directoryItem.Keywords, _keyword.Text.Trim().ToLower());

            _server.DirectoryServer.UpdateItem(_directoryItem);
            _prompt.Text = "* Item updated!<br /><br />";
			ShowDetails();
		}
        
		/// <summary>
		/// Handles the adding of links to the links list.
		/// </summary>
		protected void AddLinkHandler(object sender, EventArgs ea) 
		{
            var link = _link.Text.Trim();
			if (link == String.Empty)
				return;

			if (!link.StartsWith("http://"))
				link = "http://" + link;

			// ensure this wouldn't be a duplicate link.
            if (_directoryItem.Links.Any(itemLink => link == itemLink))
                return;

            _link.Text = string.Empty;
            _directoryItem.Links.Add(link);
			ShowDetails();
		}

		/// <summary>
		/// Handles the adding of links to the links list.
		/// </summary>
		protected void AddCategoryHandler(object sender, EventArgs ea) 
		{
            _directoryItem.DirectoryCategories.Add(_server.DirectoryServer.GetCategory(long.Parse(_categorySelector.SelectedValue)));
			ShowDetails();
		}

		/// <summary>
		/// Handles the adding of images to the image list.
		/// </summary>
		protected void AddImageHandler(object sender, EventArgs ea) 
		{
			var image = _imageURL.Text.Trim();
			if (image == String.Empty)
				return;

			// ensure this wouldn't be a duplicate image.
			if (_directoryItem.Images.Any(itemImage => image == itemImage))
			    return;

			_imageURL.Text = string.Empty;
            _directoryItem.Images.Add(image);
			ShowDetails();
		}

		/// <summary>
		/// Handles the rendering of link list items.
		/// </summary>
		protected void LinkItemCreatedHandler(object sender, RepeaterItemEventArgs ea) 
		{
		    if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
		    var removeLink = ea.Item.FindControl("_removeLinkLink") as HyperLink;
		    var link = ea.Item.FindControl("_listLink") as HyperLink;
		    var source = ea.Item.DataItem as string;
		    if (source == null)
		        return;

		    removeLink.NavigateUrl = "item.aspx?id=" + Request.QueryString["id"] + "&a=rl&rid=" + Server.UrlEncode(source);
		    link.Text = source.Replace("http://", string.Empty);
		    link.NavigateUrl = source;
		    link.Target = "_blank";
		}

		/// <summary>
		/// Handles the rendering of Category list items.
		/// </summary>
		protected void CategoryItemCreatedHandler(object sender, RepeaterItemEventArgs ea) 
		{
		    if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
		    var removeLink = ea.Item.FindControl("_removeCategoryLink") as HyperLink;
		    var link = ea.Item.FindControl("_listLink") as HyperLink;
		    var cat = ea.Item.DataItem as DirectoryCategory;

		    if (cat == null)
		        return;

		    removeLink.NavigateUrl = "item.aspx?id=" + Request.QueryString["id"] + "&a=rc&rid=" + cat.Id.ToString();
		    link.Text = cat.Name;
		    link.NavigateUrl = string.Format("~/directory/category/{0}/{1}/", cat.Id, WebUtils.ToUrlString(cat.Name));
		    link.Target = "_blank";
		}

		/// <summary>
		/// Handles the rendering of image list items.
		/// </summary>
		protected void ImageItemCreatedHandler(object sender, RepeaterItemEventArgs ea) 
		{
		    if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
		    var removeLink = ea.Item.FindControl("_removeImageLink") as HyperLink;
		    var image = ea.Item.FindControl("_listImage") as Image;
		    var source = ea.Item.DataItem as string;

		    if (source == null)
		        return;

		    removeLink.NavigateUrl = "item.aspx?id=" + Request.QueryString["id"] + "&a=ri&rid=" + Server.UrlEncode(source);
		    image.ImageUrl = source;
		    image.ToolTip = WebUtils.ToSafeHtmlParameter(source);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Binds the correct item source to the results-table.
		/// </summary>
		private void ShowDetails() 
		{
            _title.Text = _directoryItem.Title;
            _description.Text = _directoryItem.Description;
            _telephoneNumber.Text = _directoryItem.TelephoneNumber;
            _keyword.Text = Helpers.CollectionToDelimitedString(_directoryItem.Keywords);
            _submitterLink.Text = _directoryItem.Submiter.Username;
            _submitterLink.NavigateUrl = "../users/user.aspx?uid=" + _directoryItem.Submiter.Uid.ToString();
            _postcode.Text = _directoryItem.Postcode;
            _locationRecognised.Text = (_directoryItem.Latitude != 0) ? "yes" : "no";

            _categorySelector.Items.Clear();
            Logic.Functions.DirectoryStructureToDropDownList(_categorySelector);

            _categoriesList.DataSource = _directoryItem.DirectoryCategories;
            _categoriesList.DataBind();

            _linkList.DataSource = _directoryItem.Links;
            _linkList.DataBind();

            _imageList.DataSource = _directoryItem.Images;
            _imageList.DataBind();
		}
        
		/// <summary>
		/// Handles the user-action for removing a link.
		/// </summary>
		private void RemoveLinkHandler() 
		{
			if (string.IsNullOrEmpty(Request.QueryString["rid"]))
				return;

            _directoryItem.Links.Remove(Server.UrlDecode(Request.QueryString["rid"]));
			ReloadItem();
		}
        
		/// <summary>
		/// Handles the user-action for removing an image.
		/// </summary>
		private void RemoveImageHandler() 
		{
			if (string.IsNullOrEmpty(Request.QueryString["rid"]))
				return;

			_directoryItem.Images.Remove(Server.UrlDecode(Request.QueryString["rid"]));
			ReloadItem();
		}
        
		/// <summary>
		/// Handles the user-action for removing a category.
		/// </summary>
		private void RemoveCategoryHandler() 
		{
			if (string.IsNullOrEmpty(Request.QueryString["rid"]))
				return;

			if (_directoryItem.DirectoryCategories.Count == 1)
			{
				_prompt.Text = "* Items require at least one category, cannot remove.<br /><br />";
				return;
			}

			_directoryItem.DirectoryCategories.Remove(long.Parse(Request.QueryString["rid"]));
			ReloadItem();
		}
        
		/// <summary>
		/// Ensures that the user-supplied data from the form is valid.
		/// </summary>
		private bool IsValidForm() 
		{
			var isValid = true;
            if (_title.Text.Trim() == String.Empty)
			{
				isValid = false;
                _prompt.Text += "&nbsp;&nbsp;&nbsp;&nbsp;- No title supplied.<br />\n";
			}

            if (_description.Text.Trim() == String.Empty)
			{
				isValid = false;
                _prompt.Text += "&nbsp;&nbsp;&nbsp;&nbsp;- No description supplied.<br />\n";
			}

            if (_directoryItem.Links.Count == 0 && _telephoneNumber.Text.Trim() == String.Empty)
			{
				isValid = false;
                _prompt.Text += "&nbsp;&nbsp;&nbsp;&nbsp;- No contact details supplied, please supply a telephone number, or a link.<br />\n";
			}

            if (_postcode.Text.Trim() != String.Empty && !Helpers.IsPostCode(_postcode.Text))
			{
				isValid = false;
                _prompt.Text += "&nbsp;&nbsp;&nbsp;&nbsp;- That doesn't appear to be a valid UK postcode, please correct it, or remove it.<br />\n";
			}

			if (!isValid)
                _prompt.Text = "* Oops, please fix the following before continuing:<br />\n" + _prompt.Text + "<br />\n";

			return isValid;
		}
        
		/// <summary>
		/// Reloads the page, with the basic querystring information.
		/// </summary>
		private void ReloadItem() 
		{
			Response.Redirect("item.aspx?id=" + _directoryItem.Id.ToString());
		}
		#endregion
	}
}