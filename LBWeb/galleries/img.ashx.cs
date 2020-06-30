using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Configuration;
using Apollo.Utilities;
using Tetron.Logic;

namespace Tetron.Galleries
{
    /// <summary>
    /// Image handler for the galleries.
    /// </summary>
    public class GalleryImageHandler : IHttpHandler, IReadOnlySessionState
    {
        #region members
        private int _resolution = 1024;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            if (!Helpers.IsNumeric(context.Request.QueryString["g"]) || !Helpers.IsNumeric(context.Request.QueryString["i"]))
                return;

            RenderImage(context);
        }

        #region private methods
        /// <summary>
        /// Renders a specific gallery image to the browser.
        /// </summary>
        private void RenderImage(HttpContext context)
        {
            Image image;
            var info = ImageCodecInfo.GetImageEncoders();
            var encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(Encoder.Quality, 95L); // no diff between 95 and 100, but double the filesize.
            context.Response.ContentType = info[1].MimeType;

            if (!IsRequestLegitimate(context))
            {
                // render an image alerting the user that the image they seek
                // is not available from their location.
                image = Image.FromFile("../_images/no-hotlinking.jpg");
                image.Save(context.Response.OutputStream, info[1], encParams);
                image.Dispose();
            }
            else
            {
                // collect the image.
                var gallery = Apollo.Server.Instance.GalleryServer.GetGallery(long.Parse(context.Request.QueryString["g"]));
                var appImage = gallery.GetImage(long.Parse(context.Request.QueryString["i"]));
                appImage.MarkViewed(Functions.GetCurrentUser());

                // higher resolutions are only available from the gallery pages to members signed in.
                _resolution = Functions.GetUsersGalleryImageSizePreference();

                // perhaps not all resolutions exist for this image.
                _resolution = appImage.GetActualSizeForDesiredSize(_resolution);
                var path = string.Format("{0}galleries\\{1}\\{2}", ConfigurationManager.AppSettings["Global.MediaLibraryPath"], _resolution, appImage.GalleryImages.OneThousandAndTwentyFour);

                if (!File.Exists(path))
                {
                    Logger.LogWarning("No first choice gallery image found at: " + path);

                    _resolution = 1024;
                    path = string.Format("{0}galleries\\{1}\\{2}", ConfigurationManager.AppSettings["Global.MediaLibraryPath"], _resolution, appImage.GalleryImages.OneThousandAndTwentyFour);

                    if (!File.Exists(path))
                    {
                        Logger.LogWarning("No last resort gallery image found at: " + path);
                        return;
                    }
                }
                
                HttpContext.Current.Response.AddHeader("Expires", DateTime.Now.AddDays(364).ToString("ddd, dd MMM yyyy hh:mm:ss") + " GMT");

                image = Image.FromFile(path);
                image = WatermarkImage(image);
                image.Save(context.Response.OutputStream, info[1], encParams);
                image.Dispose();
            }
        }

        /// <summary>
        /// Decides if the request for this image meets usage rules.
        /// </summary>
        private static bool IsRequestLegitimate(HttpContext context)
        {
            if (context.Request.UrlReferrer == null)
            {
                // no referrer supplied, assume valid.
                return true;
            }

            if (context.Request.UrlReferrer.Host != context.Request.Url.Host)
            {
                // request is not from the site, is the site black-listed?
                if (Apollo.Server.Instance.AssetServer.BlacklistedReferrers.Contains(context.Request.UrlReferrer.AbsoluteUri))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Adds the site logo to the image as a watermark.
        /// </summary>
        private Bitmap WatermarkImage(Image sourceImage)
        {
            var phWidth = sourceImage.Width;
            var phHeight = sourceImage.Height;

            // create a Bitmap the Size of the original photograph
            var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format32bppRgb);
            bmPhoto.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

            // load the Bitmap into a Graphics object 
            var grPhoto = Graphics.FromImage(bmPhoto);

            // create an image object containing the watermark
            Image imgWatermark = _resolution == 1600 ? new Bitmap(ConfigurationManager.AppSettings["Apollo.HDWatermarkImagePath"]) : new Bitmap(ConfigurationManager.AppSettings["Apollo.WatermarkImagePath"]);

            var wmWidth = imgWatermark.Width;
            var wmHeight = imgWatermark.Height;

            // set the rendering quality for this Graphics object
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;

            // draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
                sourceImage,                            // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            //------------------------------------------------------------
            //Step #2 - Insert Watermark image
            //------------------------------------------------------------

            // create a Bitmap based on the previously modified photograph Bitmap.
            var bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

            // load this Bitmap into a new Graphic Object
            var grWatermark = Graphics.FromImage(bmWatermark);

            // to achieve a transulcent watermark we will apply (2) color 
            // manipulations by defineing a ImageAttributes object and 
            // seting (2) of its properties.
            var imageAttributes = new ImageAttributes();

            // the second color manipulation is used to change the opacity of the 
            // watermark.  This is done by applying a 5x5 matrix that contains the 
            // coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            // to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = { 
												new[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},
												new[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};

            var wmColorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int offset;
            int xPosOfWm;
            int yPosOfWm;

            if (_resolution == 1600)
            {
                // center.
                offset = (phHeight / 2) - (wmHeight / 2);
                xPosOfWm = ((phWidth / 2) - (wmWidth / 2));
                yPosOfWm = ((phHeight - wmHeight) - offset);
            }
            else
            {
                // middle bottom position.
                //int bottomOffset = 5;
                //int xPosOfWm = ((phWidth / 2) - (wmWidth / 2));
                //int yPosOfWm = ((phHeight - wmHeight) - bottomOffset);

                // bottom-right position.
                //offset = 0;
                //xPosOfWm = ((phWidth - wmWidth) - offset);
                //yPosOfWm = ((phHeight - wmHeight) - offset);

                // top-left position.
                //int offset = 5;
                //int xPosOfWm = offset;
                //int yPosOfWm = offset;

                // top-right position.
                //int offset = 5;
                //int xPosOfWm = ((phWidth - wmWidth) - offset);
                //int yPosOfWm = offset;

                // bottom-left position.
                offset = 0;
                xPosOfWm = offset;
                yPosOfWm = ((phHeight - wmHeight) - offset);
            }

            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the destination position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   // ImageAttributes Object

            // replace the original photgraphs bitmap with the new Bitmap
            sourceImage.Dispose();
            bmPhoto.Dispose();
            imgWatermark.Dispose();
            grPhoto.Dispose();
            grWatermark.Dispose();

            return bmWatermark;
        }
        #endregion

        public bool IsReusable { get { return false; } }
    }
}