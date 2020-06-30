using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Directory
{
    public partial class Results : Page
    {
        #region members
        private Constants.SearchType _searchType;
        private IdPaginator _paginator;
        private Server _server;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["s"] == null)
                Response.Redirect("/directory");

            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Database;
            if (Master != null) ((SiteMaster)Master).PageTitle = "The Directory";
            _server = Apollo.Server.Instance;

            // determine what type of search this is.
            if (Request.QueryString["m"] != null)
            {
                switch (Request.QueryString["m"])
                {
                    case "t":
                        _searchType = Constants.SearchType.Title;
                        break;
                    default:
                        _searchType = Constants.SearchType.Meta;
                        break;
                }
            }
            else
            {
                _searchType = Constants.SearchType.Meta;
            }

            ExecuteSearch();
        }

        #region event handlers
        /// <summary>
        /// Handles the formatting of the search result items in the list.
        /// </summary>
        protected void ItemDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            switch (ea.Item.ItemType)
            {
                case ListItemType.Header:
                    {
                        var noResultsText = ea.Item.FindControl("_noResultsText") as Literal;
                        if (_paginator.DataSource.Count > 0)
                            noResultsText.Visible = false;
                        else
                            noResultsText.Text = string.Format("<i>* There were no results found for '{0}'</i>. Try again!", Request.QueryString["s"]);
                    }
                    break;
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                    {
                        var itemId = (long)ea.Item.DataItem;
                        var itemLink = ea.Item.FindControl("_itemLink") as HyperLink;
                        var categoryPath = ea.Item.FindControl("_itemCategoryPath") as Literal;
                        var itemDescription = ea.Item.FindControl("_itemDescription") as Literal;
                        var item = _server.DirectoryServer.GetItem(itemId);

                        itemLink.NavigateUrl = string.Format("/directory/{0}/{1}", item.Id, WebUtils.ToUrlString(item.Title));
                        itemLink.Text = item.Title;

                        string remainder;
                        itemDescription.Text = WebUtils.PlainTextToHtml(Helpers.GetFirstParagraph(item.Description, out remainder));

                        categoryPath.Text = "Shown in";
                        if (item.DirectoryCategories.Count > 1)
                            categoryPath.Text += " (amongst others)";
                        categoryPath.Text += ":<br />";

                        if (item.DirectoryCategories.Count > 0)
                            categoryPath.Text += Functions.DirectoryCategoryToBreadcrumb(item.DirectoryCategories[0], Constants.BreadcrumbStyle.SearchResult);
                    }
                    break;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Performs the user search.
        /// </summary>
        private void ExecuteSearch()
        {
            var criteria = Request.QueryString["s"];
            criteria = criteria.Trim();
            if (Master != null) ((SiteMaster)Master).PageTitle = "Search: " + criteria;

            if (criteria == string.Empty)
            {
                _promptDiv.InnerHtml = "<i>* Nothing to search for! Please try again.</i>";
                _promptDiv.Visible = true;
                _categoriesDiv.Visible = false;
                return;
            }

            _searchCriteria.Text = string.Format("You searched for '<i>{0}</i>'", criteria);

            // find matching items.
            var itemFinder = _server.DirectoryServer.NewItemFinder();
            var pageNumber = (Helpers.IsNumeric(Request.QueryString["p"])) ? int.Parse(Request.QueryString["p"]) : 1;
            _paginator = new IdPaginator { PageSize = 15 };

            itemFinder.FindLike(criteria, _searchType == Constants.SearchType.Title ? "Title" : "Keywords");
            itemFinder.FindValue("1", "Status");
            itemFinder.OrderBy("Rating", FinderOrder.Desc);
            itemFinder.OrderBy("Title", FinderOrder.Asc);
            _paginator.DataSource = itemFinder.Find(200);

            // if there's only one item found, just take the user directly there.
            if (_paginator.DataSource.Count == 1)
            {
                var item = Apollo.Server.Instance.DirectoryServer.GetItem(_paginator.DataSource[0]);
                Response.Redirect(string.Format("/directory/{0}/{1}", item.Id, WebUtils.ToUrlString(item.Title)));
            }

            // find matching categories.
            _categoryGrid.Text = BuildCategoriesList(criteria);

            _items.DataSource = _paginator.GetPage(pageNumber);
            _items.DataBind();

            if (_paginator.DataSource.Count <= 0) return;
            _paginationStats.Text = string.Format("{0} items, showing page {1} of {2}.", _paginator.DataSource.Count, _paginator.CurrentPage, _paginator.TotalPages);
            _topPaginationControls.Text = _paginator.PaginationControls;
            _bottomPaginationControls.Text = _topPaginationControls.Text;
        }

        /// <summary>
        /// Builds a HTML fragment representing the inner table structure for the matching-categories list.
        /// </summary>
        private string BuildCategoriesList(string criteria)
        {
            // perform a category search as well.
            var catFinder = _server.DirectoryServer.NewCategoryFinder();
            catFinder.FindLike(criteria, _searchType == Constants.SearchType.Title ? "Name" : "Keywords");

            catFinder.OrderBy("Name", FinderOrder.Asc);
            var catIDs = catFinder.Find(20);
            _catResultStats.Text = catIDs.Count + " categories found.";

            // are there any to show?
            if (catIDs.Count == 0)
                _categoriesDiv.Visible = false;

            var counter = 0;
            var cellClosed = false;
            var builder = new StringBuilder();

            foreach (var category in (from long id in catIDs select _server.DirectoryServer.GetCategory(id)).TakeWhile(category => category != null))
            {
                if (counter == 0)
                {
                    builder.Append("<td valign=\"top\" class=\"pr15\"><div style=\"line-height: 22px;\">\n");
                    cellClosed = false;
                }

                builder.Append("• " + Functions.DirectoryCategoryToBreadcrumb(category, Constants.BreadcrumbStyle.Listing) + "<br />\n");

                if (counter == 3)
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