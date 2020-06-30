namespace Tetron.Tools.Content.Windows
{
	public partial class ImageContainer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			_currentImages.Attributes["src"] = "current-images.aspx?container=" + Request.QueryString["container"];
			_findImages.Attributes["src"] = "find-images.aspx?container=" + Request.QueryString["container"];

			var functions = new Logic.Functions();
			var document = functions.GetContainer(Request.QueryString["container"]).ApolloDocument;

			// only show the import button if the document has been persisted.
            if (!document.HasBeenPersisted)
				_importDiv.Visible = false;
		}
	}
}