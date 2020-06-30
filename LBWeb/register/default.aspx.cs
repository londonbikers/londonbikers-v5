using System;
using System.Configuration;
using System.Web.UI;
using System.Xml.Linq;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Register
{
    public partial class Default : Page
    {
        #region members
        private Server _server;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _server = Apollo.Server.Instance;
            _memberView.Visible = false;
            _prompt.EnableViewState = false;

            // hide the registration form and show the response content.
            if (Functions.IsUserLoggedIn())
            {
                _registerView.Visible = false;
                _memberView.Visible = true;
                _usernameTag.Text = Functions.GetCurrentUser().Username;
            }

            if (!Page.IsPostBack && Request.QueryString.ToString() != "registered")
            {
                // track an internal referer.
                if (Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.Contains(ConfigurationManager.AppSettings["Global.SiteURL"]))
                    Functions.GetCurrentUserSession().PageActions.Add(new PageAction(Request.UrlReferrer));
            }

            // registration complete.
            if (!Functions.IsUserLoggedIn()) return;
            var specificUrl = false;
            var onwardsUrl = ConfigurationManager.AppSettings["Global.SiteURL"];
            var userSession = Functions.GetCurrentUserSession();

            if (userSession.PageActions.Count > 0)
            {
                if (userSession.PageActions[0].OnCompleteRedirectUri != null)
                {
                    onwardsUrl = userSession.PageActions[0].OnCompleteRedirectUri.AbsoluteUri;
                    userSession.PageActions.Clear();
                    specificUrl = true;
                }
            }

            _messageBox.InnerHtml = string.Format(!specificUrl ? "&raquo; Return to the <a href=\"{0}\">homepage</a>" : "&raquo; <b>Back to your original page: <a href=\"{0}\">{0}</a>", onwardsUrl);
        }

        #region public methods
        /// <summary>
        /// Creates a new User within the system.
        /// </summary>
        protected void RegisterUser(object sender, EventArgs ea)
        {
            if (!IsValidForm())
                return;

            var email = _email.Text.Trim().ToLower();
            var password = _password.Text.Trim();
            var username = _username.Text.Trim();
            var firstName = Helpers.ToCapitalised(_firstname.Text.Trim());
            var lastName = Helpers.ToCapitalised(_lastname.Text.Trim());

            // --[ FORUM USER ]-----------------------------------------------------------------------------------------------------------------------

            var forumUser = new InstantASP.InstantForum.Components.User
            {
                EmailAddress = email,
                Password = password,
                Username = username,
                PrimaryRoleID = InstantASP.Common.Application.Settings.Instance().DefaultUserRoleID,
                Culture = string.Empty,
                TimeZoneOffset = 0,
                ObserveDaylightSavingTime = true
            };

            // add the user data to InstantForum_Users & InstantASP_Common tables
            var forumUserId = InstantASP.InstantForum.Business.User.InsertUpdateUser(forumUser);
            if (forumUserId > 0)
            {
                // create an instance of the forum user
                forumUser = new InstantASP.InstantForum.Components.User(forumUserId);
                
                // create the forms authentication ticket
                forumUser.Authenticate(_persistantAuth.Checked);
            }
            else
            {
                // problem creating account - probably a duplicate email address.
                ShowErrorPrompt("&raquo; Sorrry, that email address, or username, is already registered!");
                return;
            }

            // --[ APOLLO USER ]----------------------------------------------------------------------------------------------------------------------

            // now create the Apollo user.
            var user = _server.UserServer.NewUser();

            // required fields.
            user.Username = username;
            user.Email = email;
            user.Password = password;

            // optional fields.
            user.Firstname = firstName;
            user.Lastname = lastName;

            // assign forum-specific values.
            user.ForumUserId = forumUser.UserID;

            // registration complete, send welcome email.
            var arguments = new string[3];
            arguments[0] = (firstName != String.Empty) ? firstName : username;
            arguments[1] = username;
            arguments[2] = password;
            _server.CommunicationServer.SendMail(EmailType.RegistrationConfirmation, true, email, arguments);

            // persist the user.
            _server.UserServer.UpdateUser(user);

            // log the user in, and finish.
            Functions.LogUserIn(user, _persistantAuth.Checked);
            Functions.RemoveGuestVisitor();

            // does the user want to have auto-signin?
            if (_persistantAuth.Checked)
                Functions.WritePersistantAuthCookie();

            Response.Redirect("default.aspx?registered");
        }
        #endregion

        #region private methods
        /// <summary>
        /// Validates the form elements to ensure they're processable.
        /// </summary>
        private bool IsValidForm()
        {
            var isValid = true;
            var errorText = string.Empty;

            if (IsUserBlackListed(_email.Text.Trim()))
            {
                errorText += "<b>* You've been black-listed, you cannot register, sorry.</b>";
                isValid = false;
            }

            if (_username.Text == string.Empty)
            {
                errorText += "&raquo; No username supplied.<br />\n";
                isValid = false;
            }
            else
            {
                // check for invalid characters.
                var username = _username.Text.Trim();
                if (username.IndexOfAny(new[] { char.Parse("'"), char.Parse("\""), char.Parse("`") }) > -1)
                {
                    errorText += "&raquo; The following characters cannot be part of your username: ', \", '.<br />\n";
                    isValid = false;
                }
                else if (_server.UserServer.GetUser(username) != null)
                {
                    errorText += "&raquo; That username is already being used, please choose another.<br />\n";
                    isValid = false;
                }
            }

            if (_email.Text.Trim() == String.Empty)
            {
                errorText += "&raquo; No email address supplied.<br />\n";
                isValid = false;
            }
            else if (!Helpers.IsEmail(_email.Text.Trim().ToLower()))
            {
                errorText += "&raquo; That email address does not appear to be valid.<br />\n";
                isValid = false;
            }

            if (_password.Text == String.Empty)
            {
                errorText += "&raquo; No password was supplied.<br />\n";
                isValid = false;
            }
            else if (_passwordConfirmation.Text == String.Empty)
            {
                errorText += "&raquo; Please re-type your password in the box below.<br />\n";
                isValid = false;
            }
            else if (_password.Text != _passwordConfirmation.Text)
            {
                errorText += "&raquo; Those two passwords do not match.<br />\n";
                isValid = false;
            }

            if (!_agreement.Checked)
            {
                errorText += "&raquo; You must agree to our terms & conditions.<br />\n";
                isValid = false;
            }

            if (!Page.IsValid)
            {
                errorText += "&raquo; You didn't enter the right verification text.<br />\n";
                isValid = false;
            }

            if (!isValid)
            {
                errorText = errorText.Substring(0, errorText.Length - 7);
                ShowErrorPrompt(errorText);
            }

            return isValid;
        }

        /// <summary>
        /// Shows the error prompt with a title and text.
        /// </summary>
        private void ShowErrorPrompt(string text)
        {
            _prompt.InnerHtml = "<b>Whoops, there were problems:</b><br />\n";
            _prompt.InnerHtml += text;
            _prompt.Visible = true;
        }

        /// <summary>
        /// Checks to see if a users ip address is on the stopforumspam.com black-list.
        /// </summary>
        private bool IsUserBlackListed(string email)
        {
            var result = false;
            var enableLogging = bool.Parse(ConfigurationManager.AppSettings["Tetron.EnableAntiSpamLogging"]);

            try
            {
                var doc = XDocument.Load(string.Format("http://www.stopforumspam.com/api?email={0}&ip={1}", email, Request.UserHostAddress));
                result = doc.ToString().ToLower().Contains("<appears>yes</appears");
                if (enableLogging)
                    Logger.LogWarning(string.Format("StopForumSpam API call: Email = {0}. IP = {1}. Result: {2}", email, Request.UserHostAddress, result));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return result;
        }
        #endregion
    }
}