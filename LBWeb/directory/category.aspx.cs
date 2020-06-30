using System;
using System.Text;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Directory
{
    public partial class Category : System.Web.UI.Page
    {
        #region members
        private Server _server;
        private DirectoryCategory _directoryCategory;
        private DirectoryItemCollection _catDirectoryItems;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // url validation.
            if (!Functions.IsUrlIdValid(this))
            {
                Logger.LogWarning("Invalid url: " + Request.Url.AbsoluteUri);
                Response.Redirect("/directory/");
            }

            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Database;
            if (Master != null) ((SiteMaster)Master).PageTitle = "The Directory";
            _server = Apollo.Server.Instance;
            ShowDetails();
        }

        #region event handlers
        /// <summary>
        /// Handles the formatting of the Category Items in the list.
        /// </summary>
        protected void ItemDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            switch (ea.Item.ItemType)
            {
                case ListItemType.Header:
                    {
                        var noResultsText = ea.Item.FindControl("_noResultsText") as Literal;
                        if (_catDirectoryItems.Count > 0)
                        {
                            noResultsText.Visible = false;
                        }
                        else
                        {
                            if (Functions.IsUserLoggedIn())
                                noResultsText.Text = string.Format("<i>* There are no items in this category yet. Why not <a href=\"/directory/submit.aspx?id={0}\">submit</a> one?</i>", _directoryCategory.Id);
                            else
                                noResultsText.Text = "<i>* There are no items in this category yet. <a href=\"/register/\">Register</a> to submit an item to this category";
                        }
                    }
                    break;
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                    {
                        var item = ea.Item.DataItem as DirectoryItem;
                        var itemLink = ea.Item.FindControl("_itemLink") as HyperLink;
                        var itemDescription = ea.Item.FindControl("_itemDescription") as Literal;

                        itemLink.NavigateUrl = string.Format("/directory/{0}/{1}", item.Id, WebUtils.ToUrlString(item.Title));
                        itemLink.Text = item.Title;

                        string remainder;
                        var description = Helpers.GetFirstParagraph(item.Description, out remainder);
                        itemDescription.Text = WebUtils.PlainTextToHtml(description);
                    }
                    break;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Renders the Category data on the form.
        /// </summary>
        private void ShowDetails()
        {
            var id = Convert.ToInt64(RouteData.Values["id"]);
            _directoryCategory = _server.DirectoryServer.GetCategory(id);
            _categoryTitle.Text = _directoryCategory.Name;
            if (Master != null) ((SiteMaster)Master).PageTitle = "Category: " + _directoryCategory.Name;

            if (_directoryCategory == null)
            {
                Logger.LogWarning("category.aspx, category not found - no: " + Request.QueryString["id"]);
                Response.Redirect("~/directory");
            }

            _breadcrumbTrail.Text = Functions.DirectoryCategoryToBreadcrumb(_directoryCategory, Constants.BreadcrumbStyle.Navigational);

            if (_directoryCategory.SubDirectoryCategories.Count == 0)
                _subCategoriesDiv.Visible = false;
            else
                _subCatsGrid.Text = BuildSubCategoriesList();

            if (Functions.IsUserLoggedIn())
            {
                _submitLink.NavigateUrl = "/directory/submit.aspx?id=" + _directoryCategory.Id;
            }
            else
            {
                _submitLink.Visible = false;
                _registerSpan.Visible = true;
                _registerSpan.InnerHtml = "<a href=\"/register/\" class=\"BlueLink\">register</a> to submit an item.";
            }

            var pageNumber = (Helpers.IsNumeric(Request.QueryString["p"])) ? int.Parse(Request.QueryString["p"]) : 1;
            var paginator = new DirectoryItemPaginator();

            _catDirectoryItems = _directoryCategory.FilteredItems(DirectoryStatus.Active);
            paginator.PageSize = 15;
            paginator.DataSource = _catDirectoryItems;

            _items.DataSource = paginator.GetPage(pageNumber);
            _items.DataBind();

            if (paginator.DataSource.Count <= 0) return;
            _paginationStats.Text = string.Format("{0} items, showing page {1} of {2}.", paginator.DataSource.Count, paginator.CurrentPage, paginator.TotalPages);
            _topPaginationControls.Text = paginator.PaginationControls;
            _bottomPaginationControls.Text = _topPaginationControls.Text;
        }


        /// <summary>
        /// Builds a HTML fragment representing the inner table structure for the sub-cats list.
        /// </summary>
        private string BuildSubCategoriesList()
        {
            // are there any to show?
            if (_directoryCategory.SubDirectoryCategories.Count == 0)
                return "<td><i>none.</i></td>";

            var counter = 0;
            var cellClosed = false;
            var builder = new StringBuilder();

            foreach (DirectoryCategory category in _directoryCategory.SubDirectoryCategories)
            {
                if (counter == 0)
                {
                    builder.Append("<td valign=\"top\" class=\"pr15\"><div style=\"line-height: 20px;\">\n");
                    cellClosed = false;
                }

                builder.AppendFormat("• <a href=\"/directory/category/{0}/{1}\" class=\"darker\">{2}</a><br />\n", category.Id, WebUtils.ToUrlString(category.Name), category.Name);

                if (counter == 2)
                {
                    builder.Append("</div></td>\n");
                    counter = 0;
                    cellClosed = true;
                }
                else
                {
                    counter++;
                }
            }

            // ensure it ends with a closing td element.
            if (!cellClosed)
                builder.Append("</div></td>");

            return builder.ToString();
        }
        #endregion
    }
}