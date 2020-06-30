using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Users;
using Tetron.Logic;

namespace Tetron.Controls
{
    public partial class HomepageMemberStats : UserControl
    {
        #region members
        private int _topPostersIndex = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var monthlyStats = Apollo.Server.Instance.UserServer.Statistics.GetMonthlyCommunityStats();
            _newPosts.Text = monthlyStats.PostsThisMonth.ToString("###,###");
            _newPMs.Text = monthlyStats.PrivateMessagesThisMonth.ToString("###,###");
            _newMembers.Text = monthlyStats.NewMembersThisMonth.ToString("###,###");

            _joinUsLink.Visible = !Functions.IsUserLoggedIn();
            _totalMemberCount.Text = Apollo.Server.Instance.UserServer.Statistics.ActiveUsersCount.ToString("###,###,###");
            _topPosters.DataSource = Apollo.Server.Instance.UserServer.Statistics.GetTopForumPostersThisMonth(10);
            _topPosters.DataBind();
        }

        #region event handlers
        protected void ItemCreatedHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var communityPoster = ea.Item.DataItem as CommunityPoster;
            var userLink = ea.Item.FindControl("_userLink") as HyperLink;
            var postCount = ea.Item.FindControl("_postCount") as Literal;
            var position = ea.Item.FindControl("_position") as Literal;

            position.Text = _topPostersIndex.ToString();
            userLink.Text = communityPoster.Username;
            //userLink.NavigateUrl = string.Format("/forums/UserInfo{0}.aspx", communityPoster.ForumUserId);
            userLink.NavigateUrl = string.Format("/forums/{0}", communityPoster.Username.ToLower());
            postCount.Text = communityPoster.Posts.ToString("###,###");

            _topPostersIndex++;
        }
        #endregion
    }
}