using System;
using System.Text;
using Apollo.Models;
using Apollo.Utilities.Web;

namespace Tetron.Directory
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Database;
            if (Master != null) ((SiteMaster)Master).PageTitle = "The Directory";

            RenderCategoryList();
        }

        #region private methods
        /// <summary>
        /// Draws out a select version of the Directory categories structure.
        /// </summary>
        private void RenderCategoryList()
        {
            var list = new StringBuilder();
            var categories = Apollo.Server.Instance.DirectoryServer.CommonQueries.RootCategories();
            const int columns = 2;
            var itemsPerColumn = (categories.Count / columns) + 1;
            var count = 0;

            list.Append("<table align=\"center\" cellpadding=\"10\">");
            list.Append("<tr>");
            list.Append("<td valign=\"top\">");
            list.Append("<div style=\"line-height: 19px;\">");

            foreach (DirectoryCategory category in categories)
            {
                if (count == itemsPerColumn)
                {
                    count = 0;
                    list.Append("</td>");
                    list.Append("<td valign=\"top\">");
                    list.Append("<div style=\"line-height: 19px;\">");
                }

                list.AppendFormat("<b><a href=\"category/{0}/{1}\" class=\"big\">{2}</a></b><br />", category.Id, WebUtils.ToUrlString(category.Name), category.Name);

                if (category.SubDirectoryCategories.Count > 0)
                {
                    var subCount = 1;

                    foreach (DirectoryCategory subCategory in category.SubDirectoryCategories)
                    {
                        // we don't want to show more than three sub-categories really.
                        if (subCount == 5)
                        {
                            list.Append(" &hellip;");
                            break;
                        }

                        list.AppendFormat("<a href=\"category/{0}/{1}\" class=\"darker\">{2}</a>", subCategory.Id, WebUtils.ToUrlString(subCategory.Name), subCategory.Name);

                        if (subCount < category.SubDirectoryCategories.Count && subCount < 4)
                            list.Append(", ");

                        subCount++;
                    }

                    list.Append("<br />");
                }

                list.Append("<br />");
                count++;
            }

            list.Append("</div>");
            list.Append("</td>");
            list.Append("</tr>");
            list.Append("</table>");

            _categoryList.Text = list.ToString();
        }
        #endregion
    }
}