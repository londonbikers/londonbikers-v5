using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Directory
{
    public partial class Item : Page
    {
        #region members
        protected bool HaveMap;
        protected double Longitude;
        protected double Latitude;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // url validation.
            if (!Functions.IsUrlIdValid(this))
            {
                Logger.LogWarning("Invalid url: " + Request.Url.AbsoluteUri);
                Response.Redirect("/directory/");
            }

            if (Master != null) ((SiteMaster)Master).NavigationArea = Constants.NavigationArea.Database;
            if (Master != null) ((SiteMaster)Master).PageTitle = "The Directory";

            ShowDetails();
        }

        #region event handlers
        /// <summary>
        /// Handles the rendering of the Links list.
        /// </summary>
        protected void LinkDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var link = ea.Item.FindControl("_listLink") as HyperLink;
            var source = ea.Item.DataItem as string;

            if (source == null)
                return;

            link.Text = Helpers.ToShortString(source.Replace("http://", String.Empty), 50);
            link.NavigateUrl = source;
        }

        /// <summary>
        /// Handles the rendering of the Keywords list.
        /// </summary>
        protected void KeywordDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var link = ea.Item.FindControl("_listKeyword") as HyperLink;
            var source = ea.Item.DataItem as string;

            if (source == null)
                return;

            link.Text = source;
            link.NavigateUrl = string.Format("/directory/results.aspx?s={0}&m=m", source);
        }

        /// <summary>
        /// Handles the rendering of the Images list.
        /// </summary>
        protected void ImageDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var image = ea.Item.FindControl("_listImage") as Image;
            var source = ea.Item.DataItem as string;

            if (source == null)
                return;

            image.ImageUrl = source;
        }

        /// <summary>
        /// Handles the rendering of the Categories list.
        /// </summary>
        protected void CategoryDataBoundHandler(object sender, RepeaterItemEventArgs ea)
        {
            if (ea.Item.ItemType != ListItemType.Item && ea.Item.ItemType != ListItemType.AlternatingItem) return;
            var links = ea.Item.FindControl("_listCategory") as Literal;
            var cat = ea.Item.DataItem as DirectoryCategory;

            if (cat == null)
                return;

            links.Text = Functions.DirectoryCategoryToBreadcrumb(cat, Constants.BreadcrumbStyle.Listing);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Renders the Category data on the form.
        /// </summary>
        private void ShowDetails()
        {
            var id = Convert.ToInt64(RouteData.Values["id"]);
            var item = Apollo.Server.Instance.DirectoryServer.GetItem(id);
            if (item == null)
                Response.Redirect("~/directory/");
            
            var user = Functions.GetCurrentUser();
            item.MarkViewed(user);
            
            _title.Text = item.Title;
            if (Master != null) ((SiteMaster)Master).PageTitle = "Item: " + item.Title;
            _description.InnerHtml = WebUtils.PlainTextToHtml(item.Description);
            _comments.CommentsCollection = item.Comments;

            if (item.Latitude != 0)
            {
                HaveMap = true;
                Latitude = item.Latitude;
                Longitude = item.Longitude;
                _mapHolder.Visible = true;
            }

            if (item.TelephoneNumber == String.Empty)
                _phoneDiv.Visible = false;
            else
                _telephoneNumberDiv.InnerHtml = "<img src=\"/_images/silk/telephone.png\" align=\"absmiddle\" width=\"16\" height=\"16\" /> " + item.TelephoneNumber;

            if (item.Keywords.Count == 0)
            {
                _keywordDiv.Visible = false;
            }
            else
            {
                _keywords.DataSource = item.Keywords;
                _keywords.DataBind();
            }

            if (item.Links.Count == 0)
            {
                _linksDiv.Visible = false;
            }
            else
            {
                _links.DataSource = item.Links;
                _links.DataBind();
            }

            _categories.DataSource = item.DirectoryCategories;
            _categories.DataBind();

            if (item.Images.Count == 0)
            {
                _imagesHolder.Visible = false;
            }
            else
            {
                _images.DataSource = item.Images;
                _images.DataBind();
            }
        }
        #endregion
    }
}