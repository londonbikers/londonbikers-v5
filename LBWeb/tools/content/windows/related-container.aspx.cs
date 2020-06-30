using System;
using Apollo;
using Apollo.Models;

namespace Tetron.Tools.Content.Windows
{
	public partial class RelatedContainer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			_currentRelated.Attributes["src"] = "current-related.aspx?container=" + Request.QueryString["container"];
			_findRelated.Attributes["src"] = "find-related.aspx?container=" + Request.QueryString["container"];

			// populate type dropdown.
			_typeDropDown.Text = "<select name=\"type\">\n";

			var values = Enum.GetValues(typeof(DocumentType));
			for (var i = 0; i < values.Length; i++)
				_typeDropDown.Text += string.Format("<option value=\"{0}\">{0}</option>\n", values.GetValue(i));

			_typeDropDown.Text += "</select>";
		}
	}
}