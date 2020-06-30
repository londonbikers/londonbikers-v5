using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Apollo.Models;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron.Tools.Content
{
	public partial class ImageImporter : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e) 
		{
			// have some image been uploaded elsewhere?
			if (Request.QueryString["p"] != null)
				ProcessUploadedFiles();

			ConfigureUploader();
		}

		#region private methods
		/// <summary>
		/// Sets the Uploader applet up so that it can postback to this page.
		/// </summary>
		private void ConfigureUploader() 
		{
			_uploader.ScriptURL = Request.Url.AbsoluteUri;
			_uploader.ScriptURL = "upload.ashx";
			_uploader.RedirectURL = Request.Url.AbsoluteUri + "&p=1";
			_uploader.FileFilters.Add("Images", "*.jpg,*.gif,*.png,*.jpeg");
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
			var workingPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "editorial\\";
			var functions = new Logic.Functions();
			var document = functions.GetContainer(Request.QueryString["container"]).ApolloDocument;
			EditorialImage editorialImage;

			if (uploadedFiles == null)
			{
				Logger.LogWarning("image-importer.aspx.cs - Uploaded file list not found!");

				#if DEBUG
				throw new Exception("Uploaded file list not found!");
				#endif
			}

			foreach (var filename in uploadedFiles)
			{
				// create a new Content Image object.
				editorialImage = new EditorialImage(ObjectCreationMode.New)
	            {
	                Type = ContentImageType.SlideShow,
	                Name = Path.GetFileNameWithoutExtension(filename.ToLower()).Replace("-", " ").Replace("_", " "),
	                Filename = filename
	            };

			    // collect image dimensions.
				using (var file = System.Drawing.Image.FromFile(workingPath + filename))
				{
				    editorialImage.Width = file.Width;
				    editorialImage.Height = file.Height;
				}

			    // persist the image.
				Apollo.Server.Instance.ContentServer.UpdateImage(editorialImage);

				// add image to document.
				document.EditorialImages.Add(editorialImage.Id);
			}
			
			// finished process, close window and refresh current image listings.
			Application.Remove(key);
			Apollo.Server.Instance.ContentServer.UpdateDocument(document);
			_body.Attributes.Add("onload", "window.close();");
		}
		#endregion
	}
}