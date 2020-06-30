using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Directory
{
    public partial class Submit : Page
    {
        #region members
        private Server _server;
        private DirectoryItem _directoryItem;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.IsNumeric(Request.QueryString["id"]))
                Response.Redirect("~/directory");

            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Database;
            if (Master != null) ((SiteMaster)Master).PageTitle = "Directory submission";

            _server = Apollo.Server.Instance;
            var id = long.Parse(Request.QueryString["id"]);
            var category = _server.DirectoryServer.GetCategory(id);
            var categoryUrl = string.Format("~/directory/category/{0}/{1}", category.Id, WebUtils.ToUrlString(category.Name));

            if (!Functions.IsUserLoggedIn())
                Response.Redirect(categoryUrl);

            // has an item already been placed into session?
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath == Request.Url.LocalPath)
            {
                if (Session["DirectoryItemSubmission"] == null)
                    Response.Redirect(categoryUrl);

                _directoryItem = Session["DirectoryItemSubmission"] as DirectoryItem;
            }
            else
            {
                _directoryItem = _server.DirectoryServer.NewItem();
                _directoryItem.DirectoryCategories.Add(category);
                Session["DirectoryItemSubmission"] = _directoryItem;
            }

            ShowDetails();
        }

        #region Event Handlers
        /// <summary>
        /// Removes the current item from session, allowing a new one to be made.
        /// </summary>
        protected void CancelItem(object sender, EventArgs ea)
        {
            Session.Remove("DirectoryItemSubmission");
            var category = _server.DirectoryServer.GetCategory(long.Parse(Request.QueryString["id"]));
            Response.Redirect(string.Format("~/directory/category/{0}/{1}", category.Id, WebUtils.ToUrlString(category.Name)));
        }

        /// <summary>
        /// Handles the processing of the item submission.
        /// </summary>
        protected void SubmitItemHandler(object sender, EventArgs ea)
        {
            if (!IsFormValid())
                return;

            SyncItemWithForm();
            _server.DirectoryServer.UpdateItem(_directoryItem);

            // show the success panel.
            Session.Remove("DirectoryItemSubmission");
            _successPanel.Visible = true;
            _form.Visible = false;
            _cancelLink.Visible = false;
            _cancelArea.Visible = false;
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
                link = string.Format("http://{0}", link);

            // ensure this wouldn't be a duplicate link.
            if (_directoryItem.Links.Any(itemLink => link == itemLink))
                return;

            _link.Text = String.Empty;
            _directoryItem.Links.Add(link);
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

            _imageURL.Text = String.Empty;
            _directoryItem.Images.Add(image);
            ShowDetails();
        }

        /// <summary>
        /// Handles the removal of Images and Links from the submission.
        /// </summary>
        protected void RemoveObjectHandler(object sender, CommandEventArgs ea)
        {
            switch (ea.CommandName)
            {
                case "image":
                    _directoryItem.Images.Remove(ea.CommandArgument as string);
                    break;
                case "link":
                    _directoryItem.Links.Remove(ea.CommandArgument as string);
                    break;
            }

            ShowDetails();
        }

        /// <summary>
        /// Handles the rendering of link list items.
        /// </summary>
        protected void LinkItemCreatedHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var removeLink = ea.Item.FindControl("_removeLinkLink") as LinkButton;
            var link = ea.Item.FindControl("_listLink") as HyperLink;
            var source = ea.Item.DataItem as string;

            if (source == null)
                return;

            removeLink.CommandName = "link";
            removeLink.CommandArgument = source;

            link.Text = source.Replace("http://", String.Empty);
            link.NavigateUrl = source;
            link.Target = "_blank";
        }

        /// <summary>
        /// Handles the rendering of image list items.
        /// </summary>
        protected void ImageItemCreatedHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var removeLink = ea.Item.FindControl("_removeImageLink") as LinkButton;
            var image = ea.Item.FindControl("_listImage") as Image;
            var source = ea.Item.DataItem as string;

            if (source == null)
                return;

            removeLink.CommandName = "image";
            removeLink.CommandArgument = source;

            image.ImageUrl = source;
            image.ToolTip = WebUtils.ToSafeHtmlParameter(source);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Sets up page controls and performs any binding.
        /// </summary>
        private void ShowDetails()
        {
            _successPanel.Visible = false;
            _categoryName.Text = _directoryItem.DirectoryCategories[0].Name;

            _linkList.DataSource = _directoryItem.Links;
            _linkList.DataBind();

            _imageList.DataSource = _directoryItem.Images;
            _imageList.DataBind();
        }

        /// <summary>
        /// Ensures that the user-supplied data from the form is valid.
        /// </summary>
        private bool IsFormValid()
        {
            _prompt.InnerHtml = String.Empty;
            var isValid = true;

            if (_title.Text.Trim() == String.Empty)
            {
                isValid = false;
                _prompt.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;- No title supplied.<br />\n";
            }

            if (_description.Text.Trim() == String.Empty)
            {
                isValid = false;
                _prompt.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;- No description supplied.<br />\n";
            }

            if (_directoryItem.Links.Count == 0 && _telephoneNumber.Text.Trim() == String.Empty)
            {
                isValid = false;
                _prompt.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;- No contact details supplied, please supply a telephone number, or a link.<br />\n";
            }

            if (_postcode.Text.Trim() != String.Empty)
            {
                if (!Helpers.IsPostCode(_postcode.Text))
                {
                    isValid = false;
                    _prompt.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;- That doesn't appear to be a valid UK postcode, please correct it, or remove it.<br />\n";
                }
            }

            if (!isValid)
            {
                _prompt.InnerHtml = string.Format("* Oops, please fix the following before continuing:<br />\n{0}<br />\n", _prompt.InnerHtml);
                _prompt.Visible = true;
            }

            return isValid;
        }

        /// <summary>
        /// Ensures that the Item has the latest details from the form.
        /// </summary>
        private void SyncItemWithForm()
        {
            _directoryItem.Title = _title.Text.Trim();
            _directoryItem.Description = _description.Text.Trim();
            _directoryItem.TelephoneNumber = _telephoneNumber.Text.Trim();
            _directoryItem.Submiter = Functions.GetCurrentUser();
            _directoryItem.Postcode = _postcode.Text;

            _directoryItem.Keywords.Clear();
            Helpers.DelimitedStringToCollection(_directoryItem.Keywords, _keyword.Text.Trim().ToLower());
        }
        #endregion
    }
}