using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Files;
using Apollo.Utilities.Web;
using Tetron.Logic;

namespace Tetron.Tools.Gallery
{
	public partial class GalleryPage : System.Web.UI.Page
	{
		#region members
	    private Apollo.Server _server;
	    private Apollo.Models.Gallery _gallery;
	    private bool _deleteSourceImages;
	    private long _categoryId;
	    private long _id;
	    #endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			// define the page-containers properties.
			_header.PageTitle = "Tools";
			_header.PageType = "gallery";
			_header.PageBackgroundType = "none";

			_server = Apollo.Server.Instance;
			_categoryId = long.Parse(Request.QueryString["category"]);
			_deleteGalleryLink.Visible = false;

			if (!Page.IsPostBack)
                WebUtils.PopulateDropDownFromEnum(_status, new GalleryStatus(), false);

			// PAGE-MODE DECISIONS
			if (!string.IsNullOrEmpty(Request.QueryString["id"]))
			{
				_pageMode.Text = "Edit";
				_id = long.Parse(Request.QueryString["id"]);
				_gallery = _server.GalleryServer.GetGallery(_id);

				// valid gallery uid?
				if (_gallery == null)
					Response.Redirect("./");

				if (!Page.IsPostBack)
					ShowDetails();

				_deleteGalleryLink.NavigateUrl = string.Format("gallery.aspx?category={0}&id={1}&action=delgallery", _categoryId, _id);
				_deleteGalleryLink.Text = "- (delete)";
				_deleteGalleryLink.Attributes.Add("onclick", "if (!confirm('Do you really wish to delete this Gallery?')){return false;}");
				_deleteGalleryLink.Visible = true;

				if (_gallery.Photos.Count == 0)
					_exhibitsDiv.Visible = false;
			}
			else if (!string.IsNullOrEmpty(Request.QueryString["category"]))
			{
				_pageMode.Text = "Create";
			}
			else
			{
				// no page-mode instructions supplied.
				Response.Redirect("./");
			}

            if (_gallery == null || !_gallery.IsPersisted)
            {
                _persistedView.Visible = false;
                return;
            }
            else
            {
                _persistedView.Visible = true;
            }

			// have some image been uploaded elsewhere?
			if (!string.IsNullOrEmpty(Request.QueryString["p"]))
				ProcessUploadedFiles();

            if (!Page.IsPostBack)
            {
                RenderExhibits();
                ConfigureFtpImport();
            }

            RenderFtpTree();
		    ConfigureUploader();
		   
			if (Request.QueryString["delimg"] != null)
				RemoveImage();

			if (Request.QueryString["action"] == "delgallery")
				DeleteGallery();

            if (!string.IsNullOrEmpty(Request.QueryString["ftp"]) && !IsPostBack)
                RenderFtpContentView(Server.UrlDecode(Request.QueryString["ftp"]));
		}
        
		#region event handlers
		/// <summary>
		/// Handles the updating of the Gallery basic data.
		/// </summary>
		protected void UpdateGalleryHandler(object sender, EventArgs ea) 
		{
			if (_gallery == null)
			{
				CreateGalleryObject();
			}
			else
			{
				if (_title.Text == String.Empty)
				{
					_metaPrompt.Text = "* You must supply a title for this gallery";
				}
				else
				{
					_gallery.Title = _title.Text;
					_gallery.Description = _description.Text;
					_gallery.Status = (GalleryStatus)Enum.Parse(new GalleryStatus().GetType(), _status.SelectedValue);

					if (Helpers.IsDate(_publishDate.Text))
						_gallery.CreationDate = DateTime.Parse(_publishDate.Text);

					_server.GalleryServer.UpdateGallery(_gallery);
				}
			}

			RefreshPage();
		}

		/// <summary>
		/// Handles the updating of the exhibit data.
		/// </summary>
		protected void UpdateExhibitsHandler(object sender, EventArgs ea) 
		{
			foreach (GalleryImage image in _gallery.Photos)
			{
				if (Request.Form["i_name_" + image.Id] != String.Empty)
					image.Name = Request.Form["i_name_" + image.Id];

				image.Credit = Request.Form["i_credit_" + image.Id];
				image.Comment = Request.Form["i_comment_" + image.Id];
			}

			_server.GalleryServer.UpdateGallery(_gallery);
			RefreshPage();
		}

        protected void ImportFtpPhotosHandler(object sender, EventArgs ea)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ftp"]))
            {
                Logger.LogWarning("Tools Gallery: Cannot FTP import files. No path supplied!");
                return;
            }

            var path = Server.UrlDecode(Request.QueryString["ftp"]);
            if (!System.IO.Directory.Exists(path))
            {
                Logger.LogWarning("Gallery FTP import path doesn't exist: " + path);
                return;
            }

            // look for supported image formats, including the new HD Photo .hdp format.
            var filenames = (from f in System.IO.Directory.GetFiles(path)
                             where f.ToLower().EndsWith(".jpg") ||
                                   f.ToLower().EndsWith(".jpeg")
                             select f).ToList();

            if (filenames.Count() == 0)
            {
                Logger.LogWarning("Tools Gallery: No files found for import! Path: " + path);
                return; 
            }

            ProcessFileList(filenames);
            RefreshPage();
        }
		#endregion

		#region private methods
        private void ConfigureFtpImport()
        {
            _ftpImportLocation.RepeatDirection = RepeatDirection.Horizontal;
            var locations = ConfigurationManager.AppSettings["Tetron.FtpImportPaths"].ToLower().Split(',');

            foreach (var location in locations)
            {
                var isSelected = _ftpImportLocation.Items.Count == 0 ? true : false;
                if (!string.IsNullOrEmpty(Request.QueryString["ftp"]))
                    isSelected = Server.UrlDecode(Request.QueryString["ftp"]).Contains(location);

                _ftpImportLocation.Items.Add(new ListItem(location.Substring(location.LastIndexOf('\\') + 1), location) { Selected = isSelected});
            }
        }

        private void RenderFtpTree()
        {
            if (_ftpImportLocation.SelectedValue == null)
                return;

            _ftpContentView.Visible = false;
            _ftpTree.Nodes.Clear();
            _ftpTree.ShowLines = true;
            var root = new TreeNode(_ftpImportLocation.SelectedItem.Text, _ftpImportLocation.SelectedValue) { Expanded = true, NavigateUrl = GetBasePageUrl() + "&ftp=" + Server.UrlEncode(_ftpImportLocation.SelectedValue) };
            _ftpTree.Nodes.Add(root);

            foreach (var dir in System.IO.Directory.GetDirectories(_ftpImportLocation.SelectedValue))
            {
                var node = new TreeNode(dir.Substring(dir.LastIndexOf('\\') + 1), dir.ToLower()) { Expanded = true, NavigateUrl = GetBasePageUrl() + "&ftp=" + Server.UrlEncode(dir.ToLower()) };
                RecurseFtpNode(node, dir);
                root.ChildNodes.Add(node);
            }
        }

        private void RecurseFtpNode(TreeNode node, string directory)
        {
            var subDirectories = System.IO.Directory.GetDirectories(directory);
            foreach (var dir in subDirectories)
            {
                var childNode = new TreeNode(dir.Substring(dir.LastIndexOf('\\') + 1), dir.ToLower()) { Expanded = true, NavigateUrl = GetBasePageUrl() + "&ftp=" + Server.UrlEncode(dir.ToLower()) };
                node.ChildNodes.Add(childNode);
                RecurseFtpNode(childNode, dir.ToLower());
            }
        }

		/// <summary>
		/// Draws out the exhibits onto the page, providing controls for editing details and viewing larger
		/// versions of the exhibits in new windows.
		/// </summary>
		private void RenderExhibits() 
		{
			if (_gallery == null || _gallery.Photos.Count <= 0)
				return;

		    const int rowWidth = 2;
		    var exhibitPos = 0;
		    GalleryImage galleryImage;

		    var grid = new StringBuilder();
		    var mediaRoot = ConfigurationManager.AppSettings["Global.MediaLibraryURL"];
		    var rows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(_gallery.Photos.Count) / Convert.ToDouble(rowWidth)));

		    grid.Append("<b>Gallery Images</b><br /><br />\n");
		    grid.Append("<table cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">\n");

		    for (var i = 0; i < rows; i++)
		    {
		        grid.Append("<tr>\n");
					
		        for (var x = 0; x < rowWidth; x++)
		        {
		            if (exhibitPos >= _gallery.Photos.Count) continue;
		            grid.Append("<td valign=\"top\">\n");
		            galleryImage = _gallery.Photos[exhibitPos] as GalleryImage;

		            grid.Append("<table style=\"margin-bottom: 10px; margin-right: 10px;\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">");
		            grid.Append("<tr>");
		            grid.AppendFormat("<td class=\"GalleryThumbDark\" valign=\"top\" width=\"10\"><a href=\"{0}galleries/1024/{1}\" target=\"_blank\"><img src=\"{2}galleries/thumb/{3}\" border=\"0\" /></a></td>", mediaRoot, galleryImage.GalleryImages.OneThousandAndTwentyFour, mediaRoot, galleryImage.GalleryImages.Thumbnail);
		            grid.Append("<td class=\"normal\" style=\"padding: 5px; background-color: #F2F1EC\" valign=\"top\">");
		            grid.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tr><td class=\"normal\">");

		            // image title
		            grid.Append("Title:<br />");
		            grid.AppendFormat("<input type=\"text\" style=\"margin-bottom: 3px;\" class=\"box\" name=\"i_name_{0}\" value=\"{1}\" /><br />", galleryImage.Id, galleryImage.Name);

		            grid.AppendFormat("</td><td valign=\"top\" align=\"right\"><a href=\"gallery.aspx?category={0}&id={1}&delimg={2}\"><img src=\"../resources/images/ico_trash.gif\" width=\"15\" height=\"16\" title=\"delete this exhibit\" border=\"0\" /></a></td>", Request.QueryString["category"], Request.QueryString["id"], galleryImage.Id);
		            grid.Append("</tr></table>");

		            // image credit
		            grid.Append("Credit:<br />");
		            grid.AppendFormat("<input type=\"text\" style=\"margin-bottom: 3px;\" class=\"box\" name=\"i_credit_{0}\" value=\"{1}\" /><br />", galleryImage.Id, galleryImage.Credit);

		            // image description
		            grid.Append("Comment:<br />");
		            grid.AppendFormat("<textarea style=\"width: 100%; height: 30px;\" class=\"box\" name=\"i_comment_{0}\">{1}</textarea>", galleryImage.Id, galleryImage.Comment);

		            grid.Append("</td>");
		            grid.Append("</tr>");
		            grid.Append("</table>");
		            grid.Append("</div></td>");

		            exhibitPos++;
		        }
					
		        grid.Append("</tr>");
		    }

		    grid.Append("</table>");
		    _exhibitGrid.Text = grid.ToString();
		}
        
		/// <summary>
		/// Sets the Uploader applet up so that it can postback to this page.
		/// </summary>
		private void ConfigureUploader() 
		{
			_uploader.ScriptURL = Request.Url.AbsoluteUri;
		    _uploader.ScriptURL = "upload.ashx";
            _uploader.RedirectURL = string.Format("{0}&p=1", Request.Url.AbsoluteUri);
            _uploader.FileFilters.Add("Images", "*.jpg,*.jpeg,*.jpe");
            _uploader.Headers.Add("UserUid=" + Functions.GetCurrentUser().Uid);
            _uploader.Width = 545;
		}
        
		/// <summary>
		/// Enumerates the uploaded file list and processes each image into a Gallery Exhibit.
		/// </summary>
		private void ProcessUploadedFiles() 
		{
            var key = string.Format("UploadFilenames_User_{0}", Functions.GetCurrentUser().Uid);
            var uploadedFiles = (List<string>)Application[key];
		    if (uploadedFiles == null)
			{
				Logger.LogWarning("gallery.aspx.cs - Uploaded file list not found! Key: " + key);
				throw new Exception("Uploaded file list not found!");
			}

			ProcessFileList(uploadedFiles, true);
			Application.Remove(key);
			RefreshPage();
		}

        /// <summary>
        /// After the source files have been put onto the server/network, this will resize and persist each image in the file list in parallel.
        /// </summary>
        /// <param name="fileList">The list of full paths to the image files to be imported.</param>
        /// <param name="deleteSourceImages">Once a photo is processed, the source can be delted if desired. Useful for web-uploads.</param>
        private void ProcessFileList(IEnumerable<string> fileList, bool deleteSourceImages = false)
        {
            // using a class member as I haven't worked out how to pass in additional parameters beyond creating a container class.
            _deleteSourceImages = deleteSourceImages;
            Parallel.ForEach(fileList, ProcessFile);
            _server.GalleryServer.UpdateGallery(_gallery);
        }

        /// <summary>
        /// Locates the source image file and causes a gallery image to be created, i.e. resized, moved and persisted.
        /// </summary>
        /// <param name="path">The full disk/network path to the source image file.</param>
        private void ProcessFile(string path)
        {
            // validate the image size.
            using (var sourceImage = System.Drawing.Image.FromFile(path))
            {
                if (sourceImage.Width >= sourceImage.Height && sourceImage.Width < 985)
                {
                    Logger.LogWarning(string.Format("Cannot import file: {0} due to it being too narrow (985px min); width: {1}", path, sourceImage.Width));
                    return;
                }
                if (sourceImage.Height > sourceImage.Width && sourceImage.Height < 1024)
                {
                    Logger.LogWarning(string.Format("Cannot import file: {0} due to it being too short (985px min); height: {1}", path, sourceImage.Height));
                    return;
                }
            }

            // create the exhibit image
            var galleryImage = _server.GalleryServer.NewImage();

            // process the image
            galleryImage.Name = Helpers.FilenameToFriendlyName(Path.GetFileNameWithoutExtension(path));
            galleryImage.MakeImages(path);
            _gallery.AddPhoto(galleryImage);

            // delete the source image file.
            if (_deleteSourceImages)
                Files.DeleteFile(1000, path);
        }
        
		/// <summary>
		/// Creates the Gallery upfront so images can be assigned to it.
		/// </summary>
		private void CreateGalleryObject() 
		{
			if (string.IsNullOrEmpty(_title.Text))
			{
                _metaPrompt.Text = "* You must supply a title for this gallery";
			}
			else
			{
                _gallery = _server.GalleryServer.NewGallery();
                _gallery.Title = _title.Text;
                _gallery.Description = _description.Text;
                _gallery.Status = GalleryStatus.Pending;
                _gallery.Type = GalleryType.Structured;

                var galleryCategory = _server.GalleryServer.GetCategory(_categoryId);
                _gallery.Categories.Add(galleryCategory);

                _server.GalleryServer.UpdateGallery(_gallery);
                _id = _gallery.Id;
			}
		}

		/// <summary>
		/// Fills out the pages forms after each event.
		/// </summary>
		private void ShowDetails() 
		{
            _title.Text = _gallery.Title;
            _description.Text = _gallery.Description;
            _status.Items.FindByText(_gallery.Status.ToString()).Selected = true;
            _publishDate.Text = string.Format("{0} {1}", _gallery.CreationDate.ToLongDateString(), _gallery.CreationDate.ToShortTimeString());
		}
        
		/// <summary>
		/// If the gallery has just been created, we need to keep track of it by qs uid.
		/// </summary>
		private void RefreshPage() 
		{
            Response.Redirect(GetBasePageUrl(), true);
		}

        /// <summary>
        /// Returns the basic url needed to show the gallery page.
        /// </summary>
        private string GetBasePageUrl()
        {
            return _gallery == null ? string.Format("gallery.aspx?category={0}", Request.QueryString["category"]) : string.Format("gallery.aspx?category={0}&id={1}", Request.QueryString["category"], _gallery.Id);
        }

	    /// <summary>
		/// Removes the relationship between an Image and the Gallery.
		/// </summary>
		private void RemoveImage() 
		{
            var imageId = long.Parse(Request.QueryString["delimg"]);
			var root = ConfigurationManager.AppSettings["Global.MediaLibraryPath"];

			foreach (var image in _gallery.Photos.Cast<GalleryImage>().Where(image => image.Id == imageId))
			{
			    try
			    {
			        Files.DeleteFile(200, string.Format("{0}galleries\\thumb\\{1}", root, image.GalleryImages.Thumbnail));
			        Files.DeleteFile(200, string.Format("{0}galleries\\800\\{1}", root, image.GalleryImages.EightHundred));
			        Files.DeleteFile(200, string.Format("{0}galleries\\1024\\{1}", root, image.GalleryImages.OneThousandAndTwentyFour));
			        Files.DeleteFile(200, string.Format("{0}galleries\\1600\\{1}", root, image.GalleryImages.SixteenHundred));
			    }
			    catch (Exception ex)
			    {
			        Logger.LogException(ex, "gallery.aspx.RemoveImage() - Cannot delete imaage files.");
			    }

			    _gallery.RemovePhoto(image);
			    break;
			}

			_server.GalleryServer.UpdateGallery(_gallery);
			RefreshPage();
		}

		/// <summary>
		/// Permenantly deletes the Gallery.
		/// </summary>
		private void DeleteGallery() 
		{
            if (_gallery != null)
			{
				var root = ConfigurationManager.AppSettings["Global.MediaLibraryPath"];
				foreach (var exhibit in _gallery.Photos)
				{
				    // delete the image files.
				    try
				    {
				        File.Delete(string.Format("{0}galleries\\thumb\\{1}", root, (exhibit).GalleryImages.Thumbnail));
                    }
                    catch
                    {
                    }
                    try
				    {
				        File.Delete(string.Format("{0}galleries\\800\\{1}", root, (exhibit).GalleryImages.EightHundred));
                    }
                    catch
                    {
                    }
                    try
				    {
				        File.Delete(string.Format("{0}galleries\\1024\\{1}", root, (exhibit).GalleryImages.OneThousandAndTwentyFour));
                    }
                    catch
                    {
                    }
                    try
				    {
				        File.Delete(string.Format("{0}galleries\\1600\\{1}", root, (exhibit).GalleryImages.SixteenHundred));
                    }
                    catch
                    {
                    }
				}

                _server.GalleryServer.DeleteGallery(_gallery);
				Response.Redirect("./");
			}
			else
			{
                _metaPrompt.Text = "* Oops, couldn't delete the Gallery";
			}
		}

        private void RenderFtpContentView(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                Logger.LogWarning("Gallery FTP import path doesn't exist: " + path);
                return;
            }

            _ftpContentView.Visible = true;

            // look for supported image formats, including the new HD Photo .hdp format.
            var filenames = from f in System.IO.Directory.GetFiles(path)
                            where f.ToLower().EndsWith(".jpg") ||
                            f.ToLower().EndsWith(".jpeg") ||
                            f.ToLower().EndsWith(".png") ||
                            f.ToLower().EndsWith(".gif") ||
                            f.ToLower().EndsWith("bmp") ||
                            f.ToLower().EndsWith(".hdp")
                            select f;

            if (filenames.Count() == 0)
            {
                _ftpContentSummary.Text = "No photos found at that FTP location!";
                _ftpImportBtn.Visible = false;
            }
            else
            {
                var filesize = filenames.Select(filePath => new FileInfo(filePath)).Select(fi => fi.Length).Sum();
                var friendlyFilesize = Files.GetFriendlyFilesize(filesize);
                _ftpContentSummary.Text = string.Format("<b>{0}</b> Photos Found!<div> Totalling {1}</div>", filenames.Count(), friendlyFilesize);
                _ftpImportBtn.Visible = true;
            }
        }
		#endregion	
	}
}