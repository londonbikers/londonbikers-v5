using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Utilities;

namespace Tetron.Controls
{
    public partial class RoadWatchList : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var connection = new SqlConnection(ConfigurationManager.AppSettings["InstantASP_ConnectionString"]);
            var command = new SqlCommand("SELECT TOP 10 PostID, Title, TitleEncoded FROM InstantForum_Topics WHERE ForumID = 38 AND ParentID = 0 ORDER BY DateStamp DESC", connection);
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                _latestRoadWatchItems.DataSource = reader;
                _latestRoadWatchItems.DataBind();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "(User Control)RoadWatchList.Page_Load()");
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }
        }

        /// <summary>
        /// Applies formatting to the latest galleries repeater control items.
        /// </summary>
        protected void RoadWatchItemCreatedHandler(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var link = e.Item.FindControl("_link") as HyperLink;
            var linkTitle = (string) DataBinder.Eval(e.Item.DataItem, "TitleEncoded");
            link.NavigateUrl = string.Format("/forums/{0}/{1}", DataBinder.Eval(e.Item.DataItem, "PostID"), linkTitle);
            link.Text = DataBinder.Eval(e.Item.DataItem, "Title") as string;
        }
    }
}