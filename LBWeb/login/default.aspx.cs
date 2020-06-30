using System;
using System.Web.Security;
using System.Web.UI;
using Apollo.Models;
using Tetron.Logic;

namespace Tetron.Login
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster) Master).ShowRhsColumn = true;
            if (!Page.IsPostBack && Request.QueryString["r"] != null)
            {
                _redirectUrl.Visible = true;
                _redirectUrl.Value = Server.UrlDecode(Request.QueryString["r"]);
            }

            // handle a logged-in user viewing this page.
            if (!Functions.IsUserLoggedIn()) return;
            Response.Redirect("/");
        }

        #region event handlers
        protected void LoginHandler(object sender, EventArgs ea)
        {
			if (!Page.IsValid)
			{
				ShowPrompt("We're missing some details!");
				return;
			}

			var user = Apollo.Server.Instance.UserServer.GetUser(_username.Text.Trim());

			// does the user exist?
			if (user != null)
			{
			    // check the user is allowed to login.
                //if (InstantASP.InstantForum.Business.BannedIPAddresses.IsIPBanned(null))
                //{
                //    this.ShowPrompt("Sorry, your IP address has been <b>banned</b> by LB. You cannot sign-in or re-register. If you believe this is a mistake, please <a href=\"mailto:contact@londonbikers.com\">contact us</a>.");
                //    return;
                //}

			    switch (user.Status)
			    {
			        case UserStatus.Deleted:
			            ShowPrompt("That account has been <b>deleted</b>, sorry!");
			            return;
			        case UserStatus.Suspended:
			            ShowPrompt("Your account has been <b>suspended</b>, you cannot sign-in at this time. If you believe this is a mistake, please <a href=\"mailto:contact@londonbikers.com\">contact us</a>.");
			            return;
			        default:
			            if (user.Password != _password.Text.Trim())
			            {
			                ShowPrompt("<b>Wrong password.</b> Who did you say you were again? Try our <a href=\"/forgottendetails\">reminder service</a>, if you're having problems remembering your password.");
			                return;
			            }
			            if (user.Status == UserStatus.Active && user.Password == _password.Text.Trim())
			            {
			                Functions.RemoveGuestVisitor();
			                Functions.LogUserIn(user, _rememberMe.Checked);

			                // get the hell outta here.
			                var redirectUrl = (!string.IsNullOrEmpty(_redirectUrl.Value)) ? _redirectUrl.Value : "~/";
			                Response.Redirect(redirectUrl);
			            }
			            break;
			    }
			}
			else
			{
				ShowPrompt("Sorry, no such member found. Try our <a href=\"/forgottendetails\">reminder service</a>, if you're having problems remembering your details.");
			}
        }
        #endregion

		#region private methods
		private void ShowPrompt(string message)
		{
		    _responseBox.InnerHtml = message;
            _responseBox.Visible = true;
		}
		#endregion
    }
}