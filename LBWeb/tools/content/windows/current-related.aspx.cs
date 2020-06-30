using System;
using System.Linq;
using System.Web.UI;
using Apollo.Models;
using Tetron.Logic;
using Apollo;
using Apollo.Content;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Tools.Content.Windows
{
	public partial class current_related : System.Web.UI.Page
	{
		#region members
		private Functions	_functions;
	    private int	_position;
		private Document _document;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
			_functions = new Logic.Functions();
		    _document = _functions.GetContainer(Request.QueryString["container"]).ApolloDocument;

			if (_document.RelatedDocuments.Count > 0)
			{
                _currentRelated.DataSource = _document.RelatedDocuments.List;
                _currentRelated.DataBind();
                _prompt.Text = String.Empty;
			}
			else
			{
                _prompt.Text = "<br />&nbsp;&nbsp<b>none.</b>";
			}
		}
        
		protected void RemoveRelated(object sender, ImageClickEventArgs ea)
		{
		    foreach (var element in Request.Form.Cast<string>().Where(Helpers.IsNumeric))
		        _document.RelatedDocuments.Remove(long.Parse(element));

		    Response.Redirect("current-related.aspx?container=" + Request.QueryString["container"]);
		}

	    protected string Property(string field) 
		{
			var property = String.Empty;
            var document = _document.RelatedDocuments[_position];

			switch (field)
			{
				case "Icon":
					property = (document.Type == DocumentType.Article) ? "layout" : "newspaper";
					break;
				case "Tag":
                    property = string.Format("<a href=\"/{0}/tags/{1}\" class=\"GreyLink\" target=\"_blank\">{2}</a>", ((Section) document.Sections[0]).UrlIdentifier, WebUtils.SimpleUrlEncode(document.Tags[0].Name), Functions.FormatTagNameForTitle(document.Tags[0].Name));
					break;
				case "Title":
                    var href = string.Format("/{0}/{1}/{2}", ((Section)document.Sections[0]).UrlIdentifier, document.Id, WebUtils.ToUrlString(document.Title));
                    property = string.Format("<a href=\"{0}\" class=\"blacklink\" target=\"_blank\">{1}</a>", href, document.Title);
					break;
				case "Created":
					property = document.Created.ToLongDateString();
					break;
				case "Author":
                    property = Functions.GetFormalUsername(document.Author);
					break;
				case "ID":
					property = document.Id.ToString();
					_position++;
					break;
			}

			return property;
		}
	}
}