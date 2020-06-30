using System;
using System.Web.UI;
using Apollo.Utilities;

namespace Tetron
{
    public partial class SectionPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var did = RouteData.Values["did"];
            var sid = RouteData.Values["sid"];

            if (did == null && sid == null)
                Response.Redirect("~/");

            var server = Apollo.Server.Instance;
            if (did != null && Helpers.IsNumeric(did.ToString()))
            {
                // section document.
                var document = server.ContentServer.GetDocument(long.Parse(did.ToString()));
                if (document == null)
                {
                    Logger.LogWarning("No such dynamic document: " + did);
                    Response.Redirect("/");
                }

                ((SiteMaster)Master).PageTitle = document.Sections[0].Name;
                _content.Text = document.Body;
            }
            else
            {
                // section index.
                var section = server.ContentServer.GetSection(int.Parse(sid.ToString()));
                ((SiteMaster)Master).PageTitle = section.Name;
                if (section.DefaultDocument == null)
                {
                    Logger.LogWarning("No default document found for this section! Section: " + Request.QueryString["sid"]);
                    Response.Redirect("/");
                }

                var index = section.DefaultDocument;
                if (index == null)
                {
                    Logger.LogWarning("Default document is null! Section: " + sid);
                    Response.Redirect("/");
                }

                if (index.Status != "Published")
                {
                    Logger.LogWarning("Default document not published! Section: " + sid);
                    Response.Redirect("/");
                }

                _content.Text = index.Body;
            }
        }
    }
}