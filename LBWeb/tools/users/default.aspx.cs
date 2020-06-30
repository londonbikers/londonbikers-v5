using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo;
using Apollo.Models;

namespace Tetron.Tools.Users
{
	public partial class DefaultPage : Page
	{
		#region members
		private	Server _server;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_header.PageTitle = "Tools";
			_header.PageType = "users";
			_header.PageBackgroundType = "none";
			_server = Apollo.Server.Instance;

		    if (IsPostBack) return;
		    _other.SelectedIndex = _other.Items.IndexOf(_other.Items.FindByValue("New Users"));
		    PerformSearch();
		}

		#region public methods
		/// <summary>
		/// Catches the event for the search form being posted.
		/// </summary>
		protected void HandleUserSearch(object sender, ImageClickEventArgs ea) 
		{
			PerformSearch();
		}

		/// <summary>
		/// Catches the event for the results table item creation.
		/// </summary>
		protected void HandleResultItemCreation(object sender, RepeaterItemEventArgs e)
		{
		    if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item) return;
		    if (e.Item.DataItem == null) return;
		    var uid = (Guid)e.Item.DataItem;
		    var user = _server.UserServer.GetUser(uid);

		    if (user == null) return;
		    var resultName = (Literal)e.Item.FindControl("_resultName");
		    var resultUsername = (Literal)e.Item.FindControl("_resultUsername");
		    var resultCreated = (Literal)e.Item.FindControl("_resultCreated");
		    var resultIcon = (Image)e.Item.FindControl("_resultIcon");
		    var resultEmail = (HyperLink)e.Item.FindControl("_resultEmail");
		    var resultManageLink = (HyperLink)e.Item.FindControl("_resultManageLink");

		    switch (user.Status)
		    {
		        case UserStatus.Active:
		            resultIcon.ImageUrl = "../resources/images/ico_user_small.gif";
		            resultIcon.ToolTip = "Active";
		            break;

		        case UserStatus.Deleted:
		            resultIcon.ImageUrl = "../resources/images/ico_user_deleted_small.gif";
		            resultIcon.ToolTip = "Deleted";
		            break;

		        case UserStatus.Suspended:
		            resultIcon.ImageUrl = "../resources/images/ico_user_suspended_small.gif";
		            resultIcon.ToolTip = "Suspended";
		            break;
		    }

		    resultName.Text = user.Firstname + " " + user.Lastname;
		    resultUsername.Text = user.Username;
		    resultCreated.Text = user.Created.ToLongDateString();
		    resultEmail.Text = user.Email;
		    resultEmail.NavigateUrl = "mailto:" + user.Email;
		    resultManageLink.ImageUrl = "../resources/images/btn_manage.gif";
		    resultManageLink.NavigateUrl = "user.aspx?uid=" + user.Uid;
		}
	    #endregion

		#region private methods
		/// <summary>
		/// Uses the search form criteria to search for Apollo Users.
		/// </summary>
		private void PerformSearch() 
		{
			var finder = _server.UserServer.NewUserFinder();
			var results = new List<Guid>();

			if (_other.SelectedIndex > 0)
			{
				switch (_other.SelectedValue)
				{
				    case "Staff":
				        results = finder.GetStaff();
				        break;
				    case "New Users":
				        finder.OrderBy("Created", FinderOrder.Desc);
				        results = finder.FindLegacy(50);
				        break;
				}
			}
			else
			{
				if (_name.Text != string.Empty)
				{
					var parts = _name.Text.Split(char.Parse(" "));
					finder.FindLike(parts[0], "FirstName");

					if (parts.Length == 2)
						finder.FindLike(parts[1], "LastName");
				}

				if (_username.Text != string.Empty)
					finder.FindLike(_username.Text.Trim(), "Username");

				if (_email.Text != string.Empty)
					finder.FindLike(_email.Text.Trim(), "Email");

				finder.OrderBy("FirstName", FinderOrder.Desc);
                results = finder.FindLegacy(50);
			}

            _users.DataSource = results;
			_users.DataBind();
			_resultsTable.Visible = _users.Items.Count != 0;
		}
		#endregion
	}
}