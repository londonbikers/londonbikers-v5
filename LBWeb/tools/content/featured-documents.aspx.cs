using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;

namespace Tetron.Tools.Content
{
	public partial class FeaturedDocumentsPage : Page
	{
		#region members
		private	Server _server;
	    #endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
            _header.PageType = "editorial";
            _header.PageBackgroundType = "none";
            _server = Apollo.Server.Instance;

		    if (IsPostBack) return;
		    if (Request.QueryString["a"] == "remove")
		    {
		        RemoveFeaturedDocument();
		        _sectionList.SelectedIndex = _sectionList.Items.IndexOf(_sectionList.Items.FindByValue(Request.QueryString["s"]));
		    }

		    ExecuteSearch();
		}

		#region public methods
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

		                var doc = e.Item.DataItem as Document;
		                var titleLink = e.Item.FindControl("_titleLink") as HyperLink;
		                var removeLink = e.Item.FindControl("_removeLink") as HyperLink;
		                var author = e.Item.FindControl("_author") as Literal;
		                var published = e.Item.FindControl("_published") as Literal;
		                var status = e.Item.FindControl("_status") as Literal;
		                var icon = e.Item.FindControl("_icon") as Image;

		                titleLink.Text = doc.Title;
		                titleLink.NavigateUrl = "document.aspx?id=" + doc.Id;
		                status.Text = doc.Status;
		                author.Text = doc.Author.Firstname +  " " + doc.Author.Lastname;
		                published.Text = doc.Published.ToShortDateString() +  " - " + doc.Published.ToShortTimeString();
		                removeLink.ImageUrl = "../resources/images/btn_remove.gif";
		                removeLink.NavigateUrl = string.Format("featured-documents.aspx?s={0}&d={1}&a=remove", _sectionList.SelectedValue, doc.Id);

		                switch (doc.Type)
		                {
		                    case DocumentType.News:
		                        icon.ImageUrl = "/_images/silk/newspaper.png";
		                        break;
		                    case DocumentType.Article:
		                        icon.ImageUrl = "/_images/silk/layout.png";
		                        break;
		                    case DocumentType.Generic:
		                        icon.ImageUrl = doc.Sections[0].DefaultDocument.Id == doc.Id ? "/_images/silk/page_white_link.png" : "/_images/silk/page_white.png";
		                        break;
		                }
		            }
		            break;
		        case ListItemType.Footer:
		            {
		                var footer = e.Item.FindControl("_footer") as Literal;
		                footer.Text = "Showing " + _documents.Items.Count + " document";
		                footer.Text += (_documents.Items.Count > 1) ?  "s." : ".";
		                footer.Text += string.Format(" Max {0}.", 5);
		            }
		            break;
		    }
		}


	    /// <summary>
		/// Handles the addition of a document to the featured-docs list.
		/// </summary>
		protected void AddDocumentHandler(object sender, ImageClickEventArgs ea) 
		{
			var section = Apollo.Server.Instance.ContentServer.GetSection(int.Parse(_sectionList.SelectedValue));
			section.FeaturedDocuments.Add(int.Parse(_documentsToAddList.SelectedValue));
			section.FeaturedDocuments.Reload(); // need to reload to cause ordering to take effect. Not idea.
			ExecuteSearch();
		}
        
		/// <summary>
		/// Previews the 
		/// </summary>
		protected void PreviewDocumentHandler(object sender, ImageClickEventArgs ea) 
		{
			var section = Apollo.Server.Instance.ContentServer.GetSection(int.Parse(_sectionList.SelectedValue));
			Response.Redirect(string.Format("{0}{1}/{2}", section.ParentChannel.ParentSite.Url, section.UrlIdentifier, _documentsToAddList.SelectedValue));
		}

		/// <summary>
		/// Handles the form search button.
		/// </summary>
		protected void FindDocumentsHandler(object sender, EventArgs ea) 
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
			if (!Page.IsPostBack)
			{
				// build a list of sites and major sections (news/articles).
				_sectionList.Items.Clear();
                foreach (Site site in _server.GetSites())
				{
					if (site.Channels.Count == 1)
					{
                        _sectionList.Items.Add(new ListItem(site.Name + ", " + site.Channels[0].News.Name, site.Channels[0].News.Id.ToString()));
                        _sectionList.Items.Add(new ListItem(site.Name + ", " + site.Channels[0].Articles.Name, site.Channels[0].Articles.Id.ToString()));
					}
					else
					{
						foreach (Channel channel in site.Channels)
						{
                            _sectionList.Items.Add(new ListItem(site.Name + ", " + channel.News.Name, channel.News.Id.ToString()));
                            _sectionList.Items.Add(new ListItem(site.Name + ", " + channel.Articles.Name, channel.Articles.Id.ToString()));
						}
					}
				}
			}

			var section = Apollo.Server.Instance.ContentServer.GetSection(int.Parse(_sectionList.SelectedValue));

			// populate the add-documents list.
            _documentsToAddList.DataSource = section.LatestDocuments.RetrieveDocuments(40);
            _documentsToAddList.DataTextField = "Title";
            _documentsToAddList.DataValueField = "ID";
            _documentsToAddList.DataBind();			

			// populate the main document list.
            _documents.DataSource = section.FeaturedDocuments;
            _documents.DataBind();
            _documents.Visible = true;
		}

		/// <summary>
		/// Removes a specific document from the featured list for a given section.
		/// </summary>
		private void RemoveFeaturedDocument()
		{
			var section = Apollo.Server.Instance.ContentServer.GetSection(int.Parse(Request.QueryString["s"]));
			section.FeaturedDocuments.Remove(long.Parse(Request.QueryString["d"]));
			section.FeaturedDocuments.Reload(); // need to reload to cause ordering to take effect. Not ideal.
			ExecuteSearch();
		}
		#endregion
	}
}