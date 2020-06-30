using System;
using System.Configuration;
using System.Web.UI;
using System.IO;
using Apollo;
using Apollo.Models;
using Tetron.Logic;

namespace Tetron.Tools.Content
{
	public partial class ImagePage : Page
	{
		#region members
		private Server _server;
		#endregion

		protected void Page_Load(object sender, EventArgs e) 
		{
			_server = Apollo.Server.Instance;
		    if (Page.IsPostBack) return;

		    _type.DataSource = Functions.ContentImageTypeToArrayList();
		    _type.DataBind();
		}

		#region public methods
		protected void SaveImageEvnt(object sender, ImageClickEventArgs ea) 
		{
			var maxImageSize = int.Parse(ConfigurationManager.AppSettings["Apollo.Content.MaxImageSize"]);

            if (_name.Text == String.Empty)
			{
                _prompt.Text = "<br /><br /><br />*Please supply a name.";
                _intro.Visible = false;
			}
            else if (_file.PostedFile.ContentLength == 0)
			{
                _prompt.Text = "<br /><br /><br />* Please supply an image file";
                _intro.Visible = false;
			}
            else if (_file.PostedFile.ContentLength > maxImageSize)
			{
                _prompt.Text = "<br /><br /><br />* Files should be less than " + maxImageSize / 1024 + "k.";
                _intro.Visible = false;
			}
            else if (!IsValidFormat(Path.GetExtension(_file.PostedFile.FileName)))
			{
                _prompt.Text = "<br /><br /><br />* Only web formats allowed.";
                _intro.Visible = false;
			}
			else
			{
				try
				{
					// save or create an image.
					var image = _server.ContentServer.NewImage();
                    image.Type = (ContentImageType)Enum.Parse(typeof(ContentImageType), _type.SelectedItem.Value);
					image.Name = _name.Text;

					var extension = Path.GetExtension(_file.PostedFile.FileName);
					var filename = Guid.NewGuid() + extension;
					var fullPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "editorial\\" + filename;
					image.Filename = filename;

                    _file.PostedFile.SaveAs(fullPath);
					var imageFile = System.Drawing.Image.FromFile(fullPath);

					image.Width = imageFile.Width;
					image.Height = imageFile.Height;

					// check if this file meets the image type dimension constraints.
					var validates = true;
					var coverDimensions = ConfigurationManager.AppSettings["Apollo.Content.CoverImageDimensions"].Split(char.Parse("x"));
					var introDimensions = ConfigurationManager.AppSettings["Apollo.Content.IntroImageDimensions"].Split(char.Parse("x"));

					switch (image.Type)
					{
						case ContentImageType.Cover:
							if (image.Width > int.Parse(coverDimensions[0]) || image.Height > int.Parse(coverDimensions[1]))
								validates = false;
							break;
						case ContentImageType.Intro:
							if (image.Width > int.Parse(introDimensions[0]) || image.Height > int.Parse(introDimensions[1]))
								validates = false;
							break;
					}

					if (validates)
					{
                        _server.ContentServer.UpdateImage(image);
                    
						// all done, close window.
                        _body.Attributes.Add("onload", "window.close();");
					}
					else
					{
                        _prompt.Text = "<br /><br /><br />* Image dimensions too large.";
                        _intro.Visible = false;
					}
				}
				finally
				{
                    _file.Dispose();
				}
			}
		}
		#endregion

		#region private methods
		private static bool IsValidFormat(string extension)
		{
		    return extension == ".gif" || extension == ".jpg" || extension == ".jpe";
		}
	    #endregion
	}
}