using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Apollo.Models;
using Apollo;

namespace Tetron.Tools.Content
{
	public partial class IncomingPage : Page
	{
		#region members
		protected Button ChangeStatuses;
		private	Server _server;
	    #endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
			_header.PageType = "editorial";
			_header.PageBackgroundType = "none";
			_server = Apollo.Server.Instance;

		    ExecuteSearch();
		}

		#region event handlers
		/// <summary>
		/// Applies formatting to the latest-items repeater control items.
		/// </summary>
		protected void ItemCreatedHandler(object sender, RepeaterItemEventArgs e)
		{
		    switch (e.Item.ItemType)
		    {
		        case ListItemType.AlternatingItem:
		        case ListItemType.Item:
	            {
	                if (e.Item.DataItem == null)
	                    return;

	                var doc = _server.ContentServer.GetDocument((long)e.Item.DataItem);
	                var selector = e.Item.FindControl("_selector") as CheckBox;
	                var imagesIcon = e.Item.FindControl("_imagesIcon") as Image;
	                var titleLink = e.Item.FindControl("_titleLink") as HyperLink;
	                var created = e.Item.FindControl("_created") as Literal;
	                var site = e.Item.FindControl("_site") as Literal;

	                imagesIcon.Visible = doc.EditorialImages.Count > 0;
	                selector.ID = string.Format("sel_{0}", doc.Id);
	                titleLink.Text = doc.Title;
	                titleLink.NavigateUrl = "document.aspx?id=" + doc.Id;
			
	                if (doc.Type == DocumentType.Generic)
	                {
	                    // you know, this isn't strictly how the rule should be, generic docs should be
	                    // able to go against channels as well, so we may have to revise this later.

	                    site.Text = doc.Sections[0].ParentSite.Name;
	                }
	                else
	                {
	                    foreach (var section in doc.Sections)
	                        site.Text += section.ParentChannel.ParentSite.Name + "<br />\n";
	                }
			
	                created.Text = doc.Created.ToShortDateString() +  " - " + doc.Created.ToShortTimeString();
	            }
	            break;
		        case ListItemType.Footer:
	            {
	                var footer = e.Item.FindControl("_footer") as Literal;
	                footer.Text = "Showing " + _grid.Items.Count + " document";
	                footer.Text += (_grid.Items.Count > 1) ?  "s." : ".";
	                footer.Text += " Max 100.";
	            }
	            break;
		    }
		}

        /// <summary>
        /// Handles the deletion of any documents marked via checkboxes in the form.
        /// </summary>
        protected void DeleteDocumentHandler(object sender, ImageClickEventArgs ea)
        {
            foreach (var doc in Request.Form.AllKeys.Where(q => q.Contains("sel_")).Select(key => Apollo.Server.Instance.ContentServer.GetDocument(long.Parse(key.Substring(key.IndexOf("sel_") + 4)))))
                Apollo.Server.Instance.ContentServer.DeleteDocument(doc);

            Response.Redirect(Request.Url.ToString());
        }
	    #endregion

        #region private methods
        /// <summary>
        /// Performs the document search.
        /// </summary>
        private void ExecuteSearch()
        {
            var finder = _server.ContentServer.NewDocumentFinder();
            _grid.DataSource = finder.GetDocumentsByCriteria(string.Empty, string.Empty, string.Empty, new List<string> { "Incoming" }, string.Empty, string.Empty, DateTime.Parse("01 Jan 2000"), DateTime.Now.AddDays(1), 200);
            _grid.DataBind();
            _grid.Visible = true;
        }
        #endregion
	}
}