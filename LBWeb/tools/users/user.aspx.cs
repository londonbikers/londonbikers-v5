using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron.Tools.Users
{
	public partial class User : System.Web.UI.Page
	{
		#region members
		private	Server _server;
		private Apollo.Models.User _user;
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "users";
			_header.PageBackgroundType = "none";
			_server = Apollo.Server.Instance;

			if (!Helpers.IsGuid(Request.QueryString["uid"]))
				Response.Redirect("./");

			_user = _server.UserServer.GetUser(new Guid(Request.QueryString["uid"]));

			// users cannot edit their own accounts here, it's too risky.
            var clientUser = Functions.GetCurrentUser();

			if (_user == null)
				Response.Redirect("./");

			if (!clientUser.HasRole(_server.UserServer.Security.GetRole("admin")))
				Response.Redirect("../notauthorised.aspx");			

			if (clientUser.Uid == _user.Uid)
				DisablePage("You cannot edit your own account.");
			else if (_user.ForumUserId == 2)
				DisablePage("You cannot edit this persons account.");
			
			if (!IsPostBack)
				BuildPage();
		}
        
		#region public methods
		/// <summary>
		/// Changes a Users status.
		/// </summary>
		protected void HandleUserStatusChange(object sender, EventArgs e) 
		{
			_user.Status = (UserStatus)Enum.Parse(typeof(UserStatus), _status.SelectedValue);
			_server.UserServer.UpdateUser(_user);
		}

		/// <summary>
		/// Adds a security role to a User.
		/// </summary>
		protected void HandleRoleAddition(object sender, EventArgs e) 
		{
		    if (_availableRoles.SelectedValue == String.Empty) return;
		    _user.AddRole(_server.UserServer.Security.GetRole(_availableRoles.SelectedItem.Text));
		    BuildPage();
		}

		/// <summary>
		/// Removes a security role from a User.
		/// </summary>
		protected void HandleRoleRemoval(object sender, EventArgs e) 
		{
		    if (_userRoles.SelectedValue == String.Empty) return;
		    _user.RemoveRole(_server.UserServer.Security.GetRole(_userRoles.SelectedItem.Text));
		    BuildPage();
		}


		/// <summary>
		/// Updates the Users details.
		/// </summary>
		protected void HandleUserDetailsUpdate(object sender, ImageClickEventArgs e) 
		{
			// check if a new username is wanted, and if it's unique.
			if (_username.Text != _user.Username)
			{
				var testUser = _server.UserServer.GetUser(_username.Text.Trim());
				if (testUser == null)
					_user.Username = _username.Text.Trim();
				else
					_prompt.Text += "The new username is already in use, cannot change username.";
			}

			_user.Firstname = _editFirstname.Text.Trim();
			_user.Lastname = _editLastname.Text.Trim();

			if (Helpers.IsEmail(_editEmail.Text))
				_user.Email = _editEmail.Text;
			else
				_prompt.Text += "The new email address doesn't appear to be valid, cannot change email address.";

			if (_editPassword.Text.Trim() != String.Empty)
				_user.Password = _editPassword.Text;

			_server.UserServer.UpdateUser(_user);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Populates all of the page's controls.
		/// </summary>
		private void BuildPage() 
		{
			// populate user info controls.
			_username.Text = _user.Username;
			_name.Text = _user.Firstname + " " + _user.Lastname;
			_created.Text = _user.Created.ToLongDateString() + " " + _user.Created.ToShortTimeString();

			// attempt to retrieve a forum image.
            //var forumUser = InstantASP.InstantForum.Business.User.SelectUser(_user.ForumUserId);
            //if (forumUser.AvatarURL != String.Empty)
            //    _image.ImageUrl = forumUser.AvatarURL;
            //else
                _image.Visible = false;

			// user status control population.
			_status.DataSource = Enum.GetNames(typeof(UserStatus));
			_status.DataBind();
			_status.SelectedIndex = _status.Items.IndexOf(_status.Items.FindByValue(_user.Status.ToString()));

			// user roles control population.
			_userRoles.Items.Clear();
			_userRoles.DataSource = _user.Roles;
			_userRoles.DataTextField = "Name";
			_userRoles.DataValueField = "ID";
			_userRoles.DataBind();

			if (_userRoles.Items.Count == 0)
				_userRoles.Items.Add("None");

			// available roles control population.
		    var roles = _server.UserServer.Security.Roles;
			_availableRoles.Items.Clear();

			foreach (var role in from role in roles
			                     let roleExists = _user.Roles.Cast<Role>().Any(userRole => userRole.Id == role.Id)
			                     where !roleExists
			                     select role)
			{
			    _availableRoles.Items.Add(new ListItem(role.Name, role.Id.ToString()));
			}

			// edit details section.
            _editUsername.Text = _user.Username;
			_editFirstname.Text = _user.Firstname;
			_editLastname.Text = _user.Lastname;
			_editEmail.Text = _user.Email;
		}


		/// <summary>
		/// Disables the page's forms so no actions can be taken.
		/// </summary>
		private void DisablePage(string message) 
		{
			_updateDetailsBtn.Visible = false;
			_roleAdditionBtn.Enabled = false;
			_roleRemovalBtn.Enabled = false;
			_status.Enabled = false;
			_prompt.Text = "* " + message;
		}
		#endregion
	}
}