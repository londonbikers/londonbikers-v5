using System.Linq;
using System.Web.UI;
using Apollo.Models;

namespace Tetron.Tools.Content.Windows
{
	public partial class TagContainer : Page
	{
		#region members
		private Document _document;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_document = new Logic.Functions().GetContainer(Request.QueryString["container"]).ApolloDocument;
		    if (Page.IsPostBack) return;

		    // draw out the tags.
		    foreach (Tag tag in _document.Tags)
		        _tags.Text += tag.Name + ", ";

		    // remove trailing delimiter.
		    if (_tags.Text != string.Empty)
		        _tags.Text = _tags.Text.Substring(0, _tags.Text.Length - 2);
		}

		#region public methods
		/// <summary>
		/// Handles the persistence of the Tags to the document.
		/// </summary>
		protected void SaveTagsHandler(object sender, ImageClickEventArgs ea) 
		{
			// parse the tags.
			if (_tags.Text == string.Empty)
			{
				_prompt.Text = "* At least one Tag is required.";
				return;
			}

			_document.Tags.Clear();
			var tags = _tags.Text.Trim().Split(char.Parse(","));
			foreach (var tag in tags.Where(tag => tag.Trim() != string.Empty))
			    _document.Tags.Add(Apollo.Server.Instance.GetTag(tag.Trim()));

			_prompt.Text = "* Tags saved!";
		}
		#endregion
	}
}