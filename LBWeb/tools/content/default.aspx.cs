using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Tetron.Logic;
using Apollo;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Content
{
	public partial class DefaultPage : Page
	{
		#region members
		protected Button ChangeStatuses;
		private	Server _server;
		private	Functions _functions;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
			_header.PageType = "editorial";
			_header.PageBackgroundType = "none";
			_server = Apollo.Server.Instance;
			_functions = new Functions();

			if (!IsPostBack)
			{
				// populate the criteria-bar elements.
				foreach (var staffer in from Guid uid in _server.UserServer.NewUserFinder().GetStaff() select _server.UserServer.GetUser(uid))
				    _author.Items.Add(new ListItem(staffer.Username, staffer.Uid.ToString()));

                _status.DataSource = _server.Types.DocumentStatus.Where(s => s != "Incoming");
				_status.DataBind();

                WebUtils.PopulateDropDownFromEnum(_type, new DocumentType(), true);

				_site.DataSource = _server.GetSites();
				_site.DataTextField = "Name";
				_site.DataValueField = "ID";
				_site.DataBind();
			}

			if (!IsPostBack && Request.QueryString["pcmd"] == null)
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
	                var titleLink = e.Item.FindControl("_titleLink") as HyperLink;
	                var type = e.Item.FindControl("_type") as Literal;
	                var author = e.Item.FindControl("_author") as Literal;
	                var created = e.Item.FindControl("_created") as Literal;
	                var site = e.Item.FindControl("_site") as Literal;
	                var status = e.Item.FindControl("_status") as Literal;
	                var icon = e.Item.FindControl("_icon") as Image;

	                titleLink.Text = doc.Title;
	                titleLink.NavigateUrl = "document.aspx?id=" + doc.Id;
	                status.Text = doc.Status;
			
	                if (doc.Type == DocumentType.Generic)
	                {
	                    // you know, this isn't strictly how the rule should be, generic docs should be
	                    // able to go against channels as well, so we may have to revise this later.

	                    type.Text = doc.Type + " (" + doc.Sections[0].Name + ")";
	                    site.Text = doc.Sections[0].ParentSite.Name;
	                }
	                else
	                {
	                    type.Text = doc.Type.ToString();
	                    foreach (var section in doc.Sections)
	                        site.Text += section.ParentChannel.ParentSite.Name + "<br />\n";
	                }
			
	                author.Text = Functions.GetFormalUsername(doc.Author);
	                created.Text = doc.Created.ToShortDateString() +  " - " + doc.Created.ToShortTimeString();

	                switch (doc.Type)
	                {
	                    case DocumentType.News:
	                        icon.ImageUrl = "/_images/silk/newspaper.png";
	                        break;
	                    case DocumentType.Article:
	                        icon.ImageUrl = "/_images/silk/layout.png";
	                        break;
	                    case DocumentType.Generic:
	                        if (doc.Sections[0].DefaultDocument != null && doc.Sections[0].DefaultDocument.Id == doc.Id)
	                            icon.ImageUrl = "/_images/silk/page_white_link.png";
	                        else
	                            icon.ImageUrl = "/_images/silk/page_white.png";
	                        break;
	                }
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
		/// Handles the form search button.
		/// </summary>
		protected void FindDocumentsHandler(object sender, ImageClickEventArgs ea) 
		{
			ExecuteSearch();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Performs the document search.
		/// </summary>
		private void ExecuteSearch() 
		{
			var finder = _server.ContentServer.NewDocumentFinder();
			var	range = _functions.Range(_range.SelectedItem.Value);

            var title = (!string.IsNullOrEmpty(_keyword.Text)) ? _keyword.Text.Trim() : string.Empty;
            var tag = (!string.IsNullOrEmpty(_tag.Text)) ? _tag.Text.Trim() : string.Empty;
            var type = (_byType.Checked) ? _type.SelectedValue : string.Empty;
            var author = (_byAuthor.Checked) ? _author.SelectedValue : string.Empty;
            var site = (_bySite.Checked) ? _site.SelectedValue : string.Empty;
            var status = (_byStatus.Checked) ? _status.SelectedValue : string.Empty;

		    var statusTypes = new List<string>();
            if (string.IsNullOrEmpty(status))
                statusTypes.AddRange(Apollo.Server.Instance.Types.DocumentStatus.Where(s => s != "Incoming"));
            else
                statusTypes.Add(status);

            _grid.DataSource = finder.GetDocumentsByCriteria(title, tag, type, statusTypes, author, site, range.From, range.To, 100);
			_grid.DataBind();
			_grid.Visible = true;
		}
		#endregion
	}
}