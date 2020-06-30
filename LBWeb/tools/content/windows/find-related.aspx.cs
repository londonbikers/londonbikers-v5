using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Apollo;
using Apollo.Content;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Content.Windows
{
	public partial class find_related : System.Web.UI.Page
	{
		#region members
		protected string _prompt;
		private Server _server;
		private Logic.Functions _functions;
		private List<long> _idList;
		private bool _enumerateDocument = true;
		private int _position;
		private Document _document;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_server = Apollo.Server.Instance;
			_functions = new Logic.Functions();

			if (Request.QueryString["search"] != null)
				ExecuteSearch();

			if (Request.QueryString["add"] == "true")
				_body.Attributes.Add("onload", "parent._currentRelated.location.replace('current-related.aspx?container=" + Request.QueryString["container"] + "');");
		}

		private void ExecuteSearch() 
		{
			var finder = _server.ContentServer.NewDocumentFinder();
			if (Request.QueryString["keyword"] != null && Request.QueryString["keyword"].Trim() != string.Empty)
				finder.FindLike(Request.QueryString["keyword"].Trim(), "Title");

			if (Request.QueryString["tag"] != null && Request.QueryString["tag"].Trim () != string.Empty)
				finder.FindLike(Request.QueryString["tag"].Trim(), "Tags");

			if (Request.QueryString["tuse"] != null && Request.QueryString["tuse"] == "1")
				finder.FindValue(((int)Enum.Parse(typeof(DocumentType), Request.QueryString["type"])).ToString(), "Type");

			finder.OrderBy("Created", FinderOrder.Desc);
			_idList = finder.Find(100);
			BindGrid();
		}

		private void BindGrid() 
		{
			if (_idList != null && _idList.Count > 0)
			{
				_results.DataSource = _idList;
                _results.DataBind();
                _results.Visible = true;
                _prompt = "Found " + _idList.Count.ToString() + " documents (100 max).";
			}
			else
			{
                _results.Visible = false;
                _prompt = "No documents found.";
			}
		}

		protected string Property(string field) 
		{
			var property = string.Empty;
			if (_enumerateDocument)
			{
				_document = _server.ContentServer.GetDocument((long)_idList[_position]);
				_enumerateDocument = false;
			}

			switch (field)
			{
				case "Icon":
					property = (_document.Type == DocumentType.Article) ? "layout" : "newspaper";
					break;
				case "Tag":
                    property = string.Format("<a href=\"/{0}/tags/{1}\" class=\"GreyLink\" target=\"_blank\">{2}</a>", _document.Sections[0].UrlIdentifier, WebUtils.SimpleUrlEncode(_document.Tags[0].Name), Logic.Functions.FormatTagNameForTitle(_document.Tags[0].Name));
					break;
				case "Title":
                    property = string.Format("<a href=\"/{0}/{1}/{2}\" class=\"blacklink\" target=\"_blank\">{3}</a>", _document.Sections[0].UrlIdentifier, _document.Id, WebUtils.ToUrlString(_document.Title), _document.Title);
					break;
				case "Created":
                    property = _document.Created.ToLongDateString();
					break;
				case "Author":
                    property = _document.Author.Firstname + " " + _document.Author.Lastname;
					break;
				case "ID":
                    property = _document.Id.ToString();
                    _position++;
                    _enumerateDocument = true;
					break;
			}

			return property;
		}

		protected void AddRelated(object sender, ImageClickEventArgs ea) 
		{
            var document = _functions.GetContainer(Request.QueryString["container"]).ApolloDocument;
			foreach (var element in Request.Form.Cast<string>().Where(Helpers.IsNumeric))
			    document.RelatedDocuments.Add(long.Parse(element));

			Response.Redirect("find-related.aspx?container=" + Request.QueryString["container"] + "&search=1&keyword=" + Request.QueryString["keyword"] + "&add=true");
		}
	}
}