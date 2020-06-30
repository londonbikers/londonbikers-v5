using System;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web.SessionState;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron
{
	/// <summary>
	/// Creates a dynamic image and returns it in the outputstream for an img src
	///	This takes away the overhead of using an aspx page.   
	/// </summary>
	/// <remarks>
	/// This can be used by either an html img or an asp img control.
	/// This is for JPG images only. Requires the image path and principle axis dimension.
	/// The watermark is not shown for images smaller than the watermark itself.
	/// </remarks>
    public class ImageHandler : IHttpHandler, IReadOnlySessionState
	{
		#region enums
		private enum ImageType
		{
			Unknown,
			GalleryThumbnail,
			GalleryImage,
			EditorialImage,
			ForumAttachmentImage,
			ForumAvatarImage
		}
		#endregion

		#region public methods
		/// <summary>
		/// Performs the bulk of the operation.
		/// </summary>
		public void ProcessRequest(HttpContext context) 
		{
			var maxWidth = Convert.ToInt32(ConfigurationManager.AppSettings["Tetron.MaxImageHandlerDimension"]);
			var desiredSize = maxWidth;
		    var desiredHeight = 0;
		    string imageId;
			string imagePath;
			var showWatermark = true;
			var dontLog = false;
			var alwaysSizeByWidth = false;
			var constrainToSquare = false;
            var constrainToLetterbox = false;
		    var constrainToSetSize = false;
			ImageType imageType;

			// defining these allows us to switch between varying image sources so that one resizer can be used for the whole app.
			var galleryThumbPath = string.Format("{0}galleries\\thumb\\", ConfigurationManager.AppSettings["Global.MediaLibraryPath"]);
			var galleryImagePath = string.Format("{0}galleries\\1024\\", ConfigurationManager.AppSettings["Global.MediaLibraryPath"]);
			var editorialPath = string.Format("{0}editorial\\", ConfigurationManager.AppSettings["Global.MediaLibraryPath"]);
            var forumAttachmentPath = string.Format("{0}\\", ConfigurationManager.AppSettings["InstantASP_AttachmentsFolderPath"]);
			var forumAvatarPath = string.Format("{0}\\", ConfigurationManager.AppSettings["InstantASP_AvatarsFolderPath"]);

            #region reconcile parameters
            if (context.Request.QueryString["e"] != null)
			{
				// encrypted params.
                var parameters = DeserialiseParameters(context.Request.QueryString["e"]);
				imageId = parameters["id"] as string;
				
				if (parameters.ContainsKey("w"))
					desiredSize = int.Parse(parameters["w"] as string);

                if (parameters.ContainsKey("h"))
                    desiredHeight = int.Parse(parameters["h"] as string);

				if (parameters.ContainsKey("nw") && parameters["nw"] as string == "1")
					showWatermark = false;

				if (parameters.ContainsKey("wr") && parameters["wr"] as string == "1")
					alwaysSizeByWidth = true;

				if (parameters.ContainsKey("c") && parameters["c"] as string == "1")
					constrainToSquare = true;

                if (parameters.ContainsKey("l") && parameters["l"] as string == "1")
                    constrainToLetterbox = true;

                if (parameters.Contains("ss") && parameters["ss"] as string == "1")
                    constrainToSetSize = true;

				// decide the image source.
                if (parameters.ContainsKey("s") && parameters["s"] as string == "gt")
                {
                    imagePath = galleryThumbPath;
                    imageType = ImageType.GalleryThumbnail;
                }
                else if (parameters.ContainsKey("s") && parameters["s"] as string == "gi")
                {
                    imagePath = galleryImagePath;
                    imageType = ImageType.GalleryImage;
                }
                else if (parameters.ContainsKey("s") && parameters["s"] as string == "fa")
                {
                    imagePath = forumAvatarPath;
                    imageType = ImageType.ForumAvatarImage;
                    showWatermark = false;
                }
                else if (parameters.ContainsKey("s") && parameters["s"] as string == "fla")
                {
                    imagePath = forumAttachmentPath;
                    imageType = ImageType.ForumAttachmentImage;
                    showWatermark = false;
                }
                else
                {
                    imagePath = editorialPath;
                    imageType = ImageType.EditorialImage;
                }
			}
			else
			{
				// simple params.
				imageId = context.Request.QueryString["id"].ToLower();
			    if (context.Request.QueryString["dl"] != null)
					dontLog = true;

				if (context.Request.QueryString["w"] != null)
					desiredSize = int.Parse(context.Request.QueryString["w"]);

                if (context.Request.QueryString["h"] != null)
                    desiredHeight = int.Parse(context.Request.QueryString["h"]);

				if (context.Request.QueryString["nw"] == "1")
					showWatermark = false;

				if (context.Request.QueryString["wr"] != null && context.Request.QueryString["wr"] == "1")
					alwaysSizeByWidth = true;

				if (context.Request.QueryString["c"] != null && context.Request.QueryString["c"] == "1")
					constrainToSquare = true;

                if (context.Request.QueryString["l"] != null && context.Request.QueryString["l"] == "1")
                    constrainToLetterbox = true;

                if (context.Request.QueryString["ss"] != null && context.Request.QueryString["ss"] == "1")
                    constrainToSetSize = true;

				// decide the image source.
                if (context.Request.QueryString["s"] != null && context.Request.QueryString["s"] == "gt")
                {
                    imagePath = galleryThumbPath;
                    imageType = ImageType.GalleryThumbnail;
                }
                else if (context.Request.QueryString["s"] != null && context.Request.QueryString["s"] == "gi")
                {
                    imagePath = galleryImagePath;
                    imageType = ImageType.GalleryImage;
                }
                else if ((context.Request.QueryString["s"] != null && context.Request.QueryString["s"] == "f") || (context.Request.QueryString["s"] != null && context.Request.QueryString["s"] == "fla"))
                {
                    imagePath = forumAttachmentPath;
                    showWatermark = false;
                    imageType = ImageType.ForumAttachmentImage;
                }
                else if (context.Request.QueryString["s"] != null && context.Request.QueryString["s"] == "fa")
                {
                    imagePath = forumAvatarPath;
                    imageType = ImageType.ForumAvatarImage;
                    showWatermark = false;
                }
                else
                {
                    imagePath = editorialPath;
                    imageType = ImageType.EditorialImage;
                }
            }
            #endregion

            //== log any views necessary.
			if (imageType == ImageType.EditorialImage && !dontLog)
			{
				var contentImage = Server.Instance.ContentServer.GetImage(imageId);
				if (contentImage.Type == ContentImageType.SlideShow)
				{
                    var user = Functions.GetCurrentUser();
				    contentImage.MarkViewed(user);
				}
			}
			
			//== check for max values
			if (desiredSize > maxWidth) desiredSize = maxWidth;
			
			//== check that the image exists, if not, send nothing
			var fullImagePath = imagePath + imageId;
			if (!File.Exists(fullImagePath)) return;
		    
		    var sourceImage = Image.FromFile(fullImagePath);
            var fileInfo = new FileInfo(fullImagePath);
            SetOutputContentType(fullImagePath, fileInfo.CreationTimeUtc);

            if (sourceImage.Width > desiredSize || constrainToSquare || constrainToLetterbox || constrainToSetSize)
            {
                // determine the new scaled image dimensions.
                var scaledHeight = sourceImage.Height;
                var scaledWidth = sourceImage.Width;
                var xPlacement = 0;
                var yPlacement = 0;
                Image newCanvas;

                #region determine canvas size
                if (constrainToSquare)
                {
                    // determine floating image size.
                    if (sourceImage.Width >= sourceImage.Height)
                    {
                        // landscape
                        if (sourceImage.Height < desiredSize)
                        {
                            // do not enlarge the picture.
                            desiredSize = sourceImage.Height;
                        }
                        else
                        {
                            // get new size based on new height.
                            scaledWidth = Convert.ToInt32(Helpers.RoundUp((double)desiredSize * (double)sourceImage.Width / (double)sourceImage.Height));
                            scaledHeight = desiredSize;
                        }

                        xPlacement = 0 - ((scaledWidth - desiredSize)/2);
                    }
                    else
                    {
                        // portrait
                        if (sourceImage.Width < desiredSize)
                        {
                            // do not enlarge the picture.
                            desiredSize = sourceImage.Width;
                        }
                        else
                        {
                            // get new size based on new width.
                            scaledWidth = desiredSize;
                            scaledHeight = Convert.ToInt32(Helpers.RoundUp((double)scaledWidth * (double)sourceImage.Height / (double)sourceImage.Width));
                        }

                        yPlacement = 0 - ((scaledHeight - desiredSize)/2);
                    }

                    // canvas is now square according to the primary axis.
                    newCanvas = new Bitmap(desiredSize, desiredSize);
                }
                else if (constrainToLetterbox)
                {
                    scaledWidth = desiredSize;
                    scaledHeight = Convert.ToInt32(Helpers.RoundUp((double)scaledWidth * (double)sourceImage.Height / (double)sourceImage.Width));

                    if (scaledHeight > desiredHeight)
                    {
                        newCanvas = new Bitmap(desiredSize, desiredHeight);
                        yPlacement = 0 - ((scaledHeight - desiredHeight) / 2);
                    }
                    else
                    {
                        newCanvas = new Bitmap(desiredSize, scaledHeight);
                        yPlacement = 0 - ((scaledHeight - scaledHeight) / 2);
                    }
                }
                else if (constrainToSetSize)
                {
                    scaledWidth = desiredSize;
                    scaledHeight = Convert.ToInt32(Helpers.RoundUp((double)scaledWidth * (double)sourceImage.Height / (double)sourceImage.Width));

                    if (scaledHeight > desiredHeight)
                    {
                        newCanvas = new Bitmap(desiredSize, desiredHeight);
                        yPlacement = 0 - ((scaledHeight - desiredHeight) / 2);
                    }
                    else
                    {
                        // are we under-sized?
                        if (scaledHeight < desiredHeight)
                        {
                            var ratio = (double)sourceImage.Width / (double)sourceImage.Height;
                            scaledHeight = desiredHeight;
                            scaledWidth = Convert.ToInt32((double)scaledHeight * (double)ratio);

                            newCanvas = new Bitmap(desiredSize, scaledHeight);
                            xPlacement = 0 - ((scaledWidth - desiredSize) / 2);
                        }
                        else
                        {
                            newCanvas = new Bitmap(desiredSize, scaledHeight);
                            yPlacement = 0 - ((scaledHeight - scaledHeight) / 2);
                        }
                    }
                }
                else
                {
                    if (sourceImage.Width >= sourceImage.Height || alwaysSizeByWidth)
                    {
                        // landscape
                        scaledWidth = desiredSize;
                        scaledHeight = Convert.ToInt32(Helpers.RoundUp((double)scaledWidth * (double)sourceImage.Height / (double)sourceImage.Width));
                    }
                    else
                    {
                        // portrait
                        scaledWidth = Convert.ToInt32(Helpers.RoundUp((double)desiredSize * (double)sourceImage.Width / (double)sourceImage.Height));
                        scaledHeight = desiredSize;
                    }

                    newCanvas = new Bitmap(scaledWidth, scaledHeight);
                }
                #endregion

                using (var graphic = Graphics.FromImage(newCanvas))
                {
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = CompositingQuality.HighQuality;
                    graphic.DrawImage(sourceImage, xPlacement, yPlacement, scaledWidth, scaledHeight);

                    // don't show the watermark if specifically not asked for, or if the image is too small to show it.
                    if (showWatermark && newCanvas.Width > 200)
                        using (Image watermarkedImage = WatermarkImage(newCanvas))
                            SaveImageToOutput(watermarkedImage);

                    // send the un-watermarked image to the browser.
                    SaveImageToOutput(newCanvas);
                    newCanvas.Dispose();
                    sourceImage.Dispose();
                }
            }
            else
            {
                // if there's no width instruction supplied, then we can assume it's for a view of the high-resolution
                // version, and thus need to watermark it.
                if (showWatermark) sourceImage = WatermarkImage(sourceImage);

                // send the image to the browser without resizing.
                SaveImageToOutput(sourceImage);
                sourceImage.Dispose();
            }
		}
	
	    /// <summary>
		/// Determines is subsequent requests can reuse this handler.
		/// </summary>
		public bool IsReusable 
		{
			get { return false; }
		}
		#endregion

		#region private methods
		/// <summary>
		/// Adds the site logo to the image as a watermark.
		/// </summary>
		private static Bitmap WatermarkImage(Image sourceImage) 
		{
			var phWidth = sourceImage.Width;
			var phHeight = sourceImage.Height;

			// create an image object containing the watermark
		    var watermarkPath = ConfigurationManager.AppSettings["Apollo.WatermarkImagePath"];
            using (Image imgWatermark = new Bitmap(watermarkPath))
			{
			    var wmWidth = imgWatermark.Width;
			    var wmHeight = imgWatermark.Height;

			    // create a Bitmap the Size of the original photograph
			    using (var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format32bppRgb))
			    {
			        bmPhoto.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

			        // load the Bitmap into a Graphics object 
                    using (var grPhoto = Graphics.FromImage(bmPhoto))
                    {

                        // set the rendering quality for this Graphics object
                        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                        grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        grPhoto.CompositingQuality = CompositingQuality.HighQuality;

                        // draws the photo Image object at original size to the graphics object.
                        grPhoto.DrawImage(
                            sourceImage, // Photo Image object
                            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                            0, // x-coordinate of the portion of the source image to draw. 
                            0, // y-coordinate of the portion of the source image to draw. 
                            phWidth, // Width of the portion of the source image to draw. 
                            phHeight, // Height of the portion of the source image to draw. 
                            GraphicsUnit.Pixel); // Units of measure 

                        //------------------------------------------------------------
                        //Step #2 - Insert Watermark image
                        //------------------------------------------------------------

                        // create a Bitmap based on the previously modified photograph Bitmap.
                        var bmWatermark = new Bitmap(bmPhoto);
                        bmWatermark.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

                        // load this Bitmap into a new Graphic Object
                        using (var grWatermark = Graphics.FromImage(bmWatermark))
                        {

                            // to achieve a transulcent watermark we will apply (2) color 
                            // manipulations by defineing a ImageAttributes object and 
                            // seting (2) of its properties.
                            var imageAttributes = new ImageAttributes();

                            // the second color manipulation is used to change the opacity of the 
                            // watermark.  This is done by applying a 5x5 matrix that contains the 
                            // coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
                            // to 0.3f we achive a level of opacity
                            float[][] colorMatrixElements = {
                                                                new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                                                new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                                                new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                                                new[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},
                                                                new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                                            };

                            var wmColorMatrix = new ColorMatrix(colorMatrixElements);
                            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                                                           ColorAdjustType.Bitmap);

                            #region watermark position
                            // middle bottom position.
                            //int bottomOffset = 5;
                            //int xPosOfWm = ((phWidth / 2) - (wmWidth / 2));
                            //int yPosOfWm = ((phHeight - wmHeight) - bottomOffset);

                            // bottom-right position.
                            //const int offset = 0;
                            //var xPosOfWm = ((phWidth - wmWidth) - offset);
                            //var yPosOfWm = ((phHeight - wmHeight) - offset);

                            // top-left position.
                            //int offset = 5;
                            //int xPosOfWm = offset;
                            //int yPosOfWm = offset;

                            // top-right position.
                            //int offset = 5;
                            //int xPosOfWm = ((phWidth - wmWidth) - offset);
                            //int yPosOfWm = offset;

                            // bottom-left position.
                            int offset = 0;
                            int xPosOfWm = offset;
                            int yPosOfWm = ((phHeight - wmHeight) - offset);
                            #endregion

                            grWatermark.DrawImage(imgWatermark,
                                                  new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),
                                                  //Set the detination Position
                                                  0, // x-coordinate of the portion of the source image to draw. 
                                                  0, // y-coordinate of the portion of the source image to draw. 
                                                  wmWidth, // Watermark Width
                                                  wmHeight, // Watermark Height
                                                  GraphicsUnit.Pixel, // Unit of measurment
                                                  imageAttributes); //ImageAttributes Object

                            return bmWatermark;
                        }
                    }
			    }
			}
		}

		/// <summary>
		/// Decrypts the DES encrypted parameters.
		/// </summary>
		private static Hashtable DeserialiseParameters(string desEncryptedParameters) 
		{
			var parameters = new Hashtable();
			var decryptedString = SecurityHelpers.DesDecrypt(desEncryptedParameters);
			var groups = decryptedString.Split(char.Parse("&"));

			foreach (var pair in groups.Select(t => t.Split(char.Parse("="))).Where(pair => pair.Length == 2))
			    parameters.Add(pair[0], pair[1]);

			return parameters;
		}

        /// <summary>
        /// Set's the page output's content mime-type.
        /// </summary>
        private static void SetOutputContentType(string imageFilePath, DateTime imageCreatedDate)
        {
            var extension = Path.GetExtension(imageFilePath);
            //var filename = Path.GetFileName(imageFilePath);
            if (extension == null) return;
            switch (extension.ToLower())
            {
                case ".jpg":
                    HttpContext.Current.Response.ContentType = "image/jpeg";
                    break;
                case ".jpeg":
                    HttpContext.Current.Response.ContentType = "image/jpeg";
                    break;
                case ".gif":
                    HttpContext.Current.Response.ContentType = "image/gif";
                    break;
                case ".png":
                    HttpContext.Current.Response.ContentType = "image/png";
                    break;
            }

            // ensure the output is cachable by the client.
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            //HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            //HttpContext.Current.Response.AddHeader("Last-Modified", imageCreatedDate.ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT");
            //HttpContext.Current.Response.AddHeader("content-disposition", "inline; filename=" + filename);
            HttpContext.Current.Response.AddHeader("Expires", DateTime.Now.AddDays(364).ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT");
        }

        /// <summary>
        /// Saves an image to the output stream using high-quality image settings.
        /// </summary>
        private static void SaveImageToOutput(Image image)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            var encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)ImageQuality.UltraHigh);
            image.Save(HttpContext.Current.Response.OutputStream, info[1], encParams);
        }
		#endregion
	}
}