using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;
using Apollo.Users;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Content
{
	public partial class DocumentPage : Page
	{
		#region members
		protected string GenericDivCssDisplay;
		private	User _user;
		private	Server _server;
		private	Document _document;
		private	Logic.Functions _functions;
	    #endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "editorial";
			_header.PageBackgroundType = "none";
			_user = Logic.Functions.GetCurrentUser();
			_server = Apollo.Server.Instance;
			_functions = new Logic.Functions();

		    if (IsPostBack) return;

		    // actions regardless of load state.
		    WebUtils.PopulateDropDownFromEnum(_type, new DocumentType(), false);

		    // get the staff list.
		    string name;
		    User staffer;

		    foreach (var uid in new UserFinder().GetStaff())
		    {
		        staffer = _server.UserServer.GetUser(uid);
		        name = staffer.Firstname + " " + staffer.Lastname;
		        if (name.Trim() == String.Empty)
		            name = staffer.Username;

		        var item = new ListItem(name, staffer.Uid.ToString());
		        if (staffer.Uid == _user.Uid)
		            item.Selected = true;

		        _authorList.Items.Add(item);
		    }

		    _status.DataSource = _server.Types.DocumentStatus;
		    _status.DataBind();

		    // build the sections list for generic documents.
		    foreach (Site site in _server.GetSites())
		        foreach (var section in site.Sections)
		            _sectionList.Items.Add(new ListItem(site.Name + " - " + section.Name, section.Id.ToString()));

		    // --------------------------------------------------------------------------------------

		    if (!string.IsNullOrEmpty(Request.QueryString["id"]))
		    {
		        // open document for editing.
		        if (!Helpers.IsNumeric(Request.QueryString["id"]))
		            Response.Redirect("./");

		        _document = _server.ContentServer.GetDocument(long.Parse(Request.QueryString["id"]));

		        // populate form.
		        _title.Text = _document.Title;
		        _abstract.Text = _document.Abstract;
					
		        // re-process author list.
		        var isAuthorStaff = _authorList.Items.Cast<ListItem>().Where(item => _document.Author.Uid.ToString() == item.Value).Any();

		        // if the author isn't a staffer, add them in here.
		        if (!isAuthorStaff)
		            _authorList.Items.Add(new ListItem(_document.Author.Firstname + " " + _document.Author.Lastname, _document.Author.Uid.ToString()));

		        // select the author.
		        _authorList.SelectedIndex = _authorList.Items.IndexOf(_authorList.Items.FindByValue(_document.Author.Uid.ToString()));
		        _type.SelectedIndex = _type.Items.IndexOf(_type.Items.FindByValue(_document.Type.ToString()));

		        // generic documents just need a basic textbox to avoid any html-entry issues.
		        if (_document.Type == DocumentType.Generic)
		        {
		            GenericDivCssDisplay = "block";
		            _sectionList.SelectedIndex = _sectionList.Items.IndexOf(_sectionList.Items.FindByValue(_document.Sections[0].Id.ToString()));
		            _mceContainer.Style.Add("display", "none");
		            _bodyPlain.Style.Add("display", "block");
		            _bodyPlain.Text = _document.Body;
		        }
		        else
		        {
		            GenericDivCssDisplay = "none";
		            _mceContainer.Style.Add("display", "block");
		            _bodyPlain.Style.Add("display", "none");

		            // pre-v2 docs had no html formatting.
		            _body.Text = _document.Body.IndexOf("<br />") == -1 ? WebUtils.PlainTextToHtml(_document.Body) : _document.Body;
		        }
		    }
		    else
		    {
		        // create a new document.
		        _document = _server.ContentServer.NewDocument();
		        _document.Author = _user;
		        _mceContainer.Style.Add("display", "block");
		        _bodyPlain.Style.Add("display", "none");
		        GenericDivCssDisplay = "none";
		    }

		    _status.SelectedIndex = _status.Items.IndexOf(_status.Items.FindByValue(_document.Status));
		    _created.Text = _document.Created.ToLongDateString() +  " - " + _document.Created.ToShortTimeString();

		    // store the document object for other panes to use.
		    var container = _functions.NewContainer();
		    container.ApolloDocument = _document;
		    container.Uid.ToString();
		    _functions.UpdateContainer(container);

		    _documentContainer.Value = container.Uid.ToString();
		    _sidepane.Attributes["src"] = "windows/image-container.aspx?container=" + container.Uid;
		    PrepareForm();
		}

		#region event handlers
		/// <summary>
		/// Handles the form action for updating of the Document object.
		/// </summary>
		protected void SaveDocumentEventHandler(object sender, ImageClickEventArgs e) 
		{
			if (UpdateDocument())
				_prompt.Text = "* Document saved!";

			PrepareForm();
		}

        /// <summary>
        /// Handles the deletion of the document event.
        /// </summary>
        protected void DeleteDocumentHandler(object sender, ImageClickEventArgs ea)
        {
            var doc = Apollo.Server.Instance.ContentServer.GetDocument(long.Parse(Request.QueryString["id"]));
            if (doc == null)
                return;

            var isIncoming = doc.Status == "Incoming";
            Apollo.Server.Instance.ContentServer.DeleteDocument(doc);
            Response.Redirect(isIncoming ? "incoming.aspx" : "./");
        }
		#endregion

		#region private methods
		/// <summary>
		/// Performs the actual persistence of the Document object.
		/// </summary>
		private bool UpdateDocument() 
		{
			// collect the document from the container.
            _document = _functions.GetContainer(_documentContainer.Value).ApolloDocument;

			if (!IsFormValid())
				return false;

			// update document object.
            _document.Title = _title.Text.Trim();
            _document.Abstract = _abstract.Text.Trim();
            _document.Type = (DocumentType)Enum.Parse(typeof(DocumentType), _type.SelectedItem.Value);
            _document.Body = (_document.Type == DocumentType.Generic) ? _bodyPlain.Text.Trim() : _body.Text.Trim();

			// assign user.
            if (_document.Author.Uid.ToString() != _authorList.SelectedValue || _authorID.Text.Trim() != String.Empty)
				_document.Author = _authorID.Text.Trim() != String.Empty ? _server.UserServer.GetUser(int.Parse(_authorID.Text.Trim())) : _server.UserServer.GetUser(new Guid(_authorList.SelectedValue));

			// update document status.
            if (_document.Status != _status.SelectedValue)
			{
                // publication rules!
                switch (_status.SelectedValue)
				{
                    case "New":
				        _document.Status = "New";
				        break;
					case "Pending":
                        if (_document.Status == "Incoming" || _document.Status == "New")
                            _document.Status = "Pending";
						break;
					case "Ready":
                        if (_document.Status == "Incoming" || _document.Status == "New" || _document.Status == "Pending" || _document.Status == "Inactive")
                            _document.Status = "Ready";
						break;
					case "Published":
                        _document.Status = "Published";
						break;
					case "Inactive":
                        if (_document.Status == "Published")
                            _document.Status = "Inactive";
						break;
				}
			}

			// for now there is only one site.
            _document.Sections.Clear();
			var site = Logic.Functions.GetConfiguredSite();

            switch (_type.SelectedValue)
            {
                case "News":
                    _document.Sections.Add(site.Channels[0].News);
                    break;
                case "Article":
                    _document.Sections.Add(site.Channels[0].Articles);
                    break;
                case "Generic":
                    _document.Sections.Add(_server.ContentServer.GetSection(int.Parse(_sectionList.SelectedValue)));
                    break;
            }

            _server.ContentServer.UpdateDocument(_document);
			return true;
		}
        
		/// <summary>
		/// Ensures the form is built correctly for the document's status.
		/// </summary>
		private void PrepareForm()
		{
		    // can the user change the author? only editors can.
		    if (!_user.HasRole(_server.UserServer.Security.GetRole("editor")))
		    {
		        _authorList.Enabled = false;
		        _authorID.Enabled = false;
		    }

		    if (_document.HasBeenPersisted)
		    {
		        GenericDivCssDisplay = _document.Type == DocumentType.Generic ? "block" : "none";
		    }
		    else if (Page.IsPostBack)
		    {
		        // work out if this new doc is generic or other type.
		        if (_body.Text != String.Empty)
		        {
		            // regular document.
		            GenericDivCssDisplay = "none";
		            _mceContainer.Style.Add("display", "block");
		            _bodyPlain.Style.Add("display", "none");
		        }
		        else
		        {
		            // generic document.
		            GenericDivCssDisplay = "block";
		            _mceContainer.Style.Add("display", "none");
		            _bodyPlain.Style.Add("display", "block");
		        }
		    }

		    if (!_document.HasBeenPersisted)
            {
                _deleteBtn.Visible = false;
		        return;
		    }

	        // hook-up the preview button.
		    _preview.Visible = true;
		    switch (_document.Type)
		    {
		        case DocumentType.Article:
		            _preview.NavigateUrl = string.Format("/articles/{0}/{1}", _document.Id, WebUtils.ToUrlString(_document.Title));
		            break;
		        case DocumentType.News:
		            _preview.NavigateUrl = string.Format("/news/{0}/{1}", _document.Id, WebUtils.ToUrlString(_document.Title));
		            break;
		        case DocumentType.Generic:
		            Document defaultDocument = null;
		            if (_document.Sections[0].DefaultDocument != null)
		                defaultDocument = _document.Sections[0].DefaultDocument;

		            if (defaultDocument != null && defaultDocument.Id == _document.Id)
		                _preview.NavigateUrl = "/" + _document.Sections[0].UrlIdentifier + "/";
		            else
		                _preview.NavigateUrl = string.Format("/{0}/{1}/{2}", _document.Sections[0].UrlIdentifier, _document.Id, WebUtils.ToUrlString(_document.Title));
		            break;
		    }
		}

		/// <summary>
		/// Performs validation upon the form to ensure it meets the domain-objects specification.
		/// </summary>
		private bool IsFormValid() 
		{
			var isValid = true;
			_prompt.Text = String.Empty;

            if (_title.Text.Trim() == String.Empty)
			{
                _prompt.Text += "* Please supply a title.<br />";
				isValid = false;
			}
            else if (_body.Text.Trim() == String.Empty && _bodyPlain.Text.Trim() == String.Empty)
			{
                _prompt.Text += "* A body for this document must be supplied.<br />";
				isValid = false;
			}

			// check publication privledges. <-- tbd

			// check manual author id entry.
            if (_authorID.Text != String.Empty)
			{
                if (!Helpers.IsNumeric(_authorID.Text))
				{
                    _prompt.Text += "* That author ID is not valid. Please enter a number.<br />";
					isValid = false;
				}
                else if (_server.UserServer.GetUser(int.Parse(_authorID.Text)) == null) 
				{
                    _prompt.Text += "* That author ID is not valid, no such user found.<br />";
					isValid = false;
				}
			}

			// check tags.
            if (_document.Tags.Count == 0)
			{
                _prompt.Text += "* At least one Tag is required for this document.<br />";
				isValid = false;
			}

			if (!isValid)
                _prompt.Text = "Please correct the following:<br />" + _prompt.Text;

			return isValid;
		}
		#endregion
	}
}