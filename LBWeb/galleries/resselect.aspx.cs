using System;
using System.Web;
using System.Web.UI;

namespace Tetron.Galleries
{
    public partial class Resselect : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RouteData.Values["res"] != null)
                ChangeResolution();
            else
                Response.Redirect("~/galleries/");
        }

        #region private methods
        /// <summary>
        /// Sets the users preference for what size gallery images they would like to see.
        /// </summary>
        private void ChangeResolution()
        {
            var resolution = Convert.ToInt32(RouteData.Values["res"]);
            Session["GalleryImageResolutionPreference"] = resolution;
            var cookie = new HttpCookie("TetronGenericCookie");
            cookie.Values["GalleryImageResolutionPreference"] = resolution.ToString();
            cookie.Expires = DateTime.Now.AddYears(5);
            Response.Cookies.Add(cookie);
            Response.Redirect(Server.UrlDecode(Request.QueryString["ref"]));
        }
        #endregion
    }
}