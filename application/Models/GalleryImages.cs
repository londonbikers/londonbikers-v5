using System;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Used by Galleries.Image as an object to contain the paths/url's for the various size images.
	/// </summary>
	public class GalleryImages : IGalleryImages
	{
		#region members
	    private readonly string _sourceImageFilename;
	    #endregion

		#region constructors
		/// <summary>
		/// Class constructor, parameters supplied to help communicate details fundamental to image paths.
		/// </summary>
		public GalleryImages(string sourceImageFilename) 
		{
			_sourceImageFilename = sourceImageFilename;
			InitClass();
		}
		
		/// <summary>
		/// Overloaded constructor used by the GalleryServer when instantiating an existing image.
		/// </summary>
		public GalleryImages() 
		{
			InitClass();
		}
		#endregion
		
		#region accessors
	    /// <summary>
	    /// The filename of the 800 x 600 image.
	    /// </summary>
	    public string EightHundred { get; set; }
	    /// <summary>
	    /// The filename of the 1024 x 768 image.
	    /// </summary>
	    public string OneThousandAndTwentyFour { get; set; }
	    /// <summary>
	    /// The filename of the 1600 x 1200 image.
	    /// </summary>
	    public string SixteenHundred { get; set; }
	    /// <summary>
	    /// The filename of the thumbnail image.
	    /// </summary>
	    public string Thumbnail { get; set; }
	    #endregion

		#region internal methods
		/// <summary>
		/// Deletes image files previously created by this class. 
		/// An exception is thrown if any images are not found.
		/// </summary>
		internal void DeletePreviousImages() 
		{
		    var galleryPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "\\galleries\\";

		    if (!string.IsNullOrEmpty(EightHundred))
		        File.Delete(string.Format("{0}800\\{1}", galleryPath, EightHundred));

		    if (!string.IsNullOrEmpty(OneThousandAndTwentyFour))
		        File.Delete(string.Format("{0}1024\\{1}", galleryPath, OneThousandAndTwentyFour));

		    if (!string.IsNullOrEmpty(SixteenHundred))
		        File.Delete(string.Format("{0}1600\\{1}", galleryPath, SixteenHundred));

		    if (!string.IsNullOrEmpty(Thumbnail))
		        File.Delete(string.Format("{0}thumb\\{1}", galleryPath, Thumbnail));
		}

		/// <summary>
		/// Tasked with sampling the image down to the appropriate gallery sizes. 
		/// An exception is thrown if the image isn't in a valid format.
		/// </summary>
		internal void DownSampleImages() 
		{
            // check in the image store for filename uniqueness.
		    var directory = Path.GetDirectoryName(_sourceImageFilename) + "\\";
		    var filename = Path.GetFileName(_sourceImageFilename);
		    if (filename == null) return;
		    var targetFilename = Helpers.GetUniqueFilename(directory, Helpers.MakeSafeFilename(filename.ToLower()));
		    var galleryPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "galleries\\";
		    var sourceImage = Image.FromFile(_sourceImageFilename);
		    ImageOrientation orientation;
		    Image resizedImage;
		    int primaryDimension;
		    var thumbWidth = Convert.ToInt32(ConfigurationManager.AppSettings["Apollo.Galleries.ThumbnailWidth"]);

		    // output jpeg settings.
		    var info = ImageCodecInfo.GetImageEncoders();    
		    var encParams = new EncoderParameters(1);
		    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)ImageQuality.UltraHigh);

		    if (sourceImage.Width > sourceImage.Height)
		        orientation = ImageOrientation.Landscape;
		    else if (sourceImage.Width == sourceImage.Height)
		        orientation = ImageOrientation.Square;
		    else
		        orientation = ImageOrientation.Portrait;

		    // check the image is larger than the desired size.
		    if (orientation == ImageOrientation.Landscape || orientation == ImageOrientation.Square)
		        primaryDimension = sourceImage.Width;
		    else
		        primaryDimension = sourceImage.Height;

		    // - ideally the source image should be equal or larger than 1600 x ~1200 pixels, though if it's not, we'll
		    //   just skip the larger sizes and downsample to the smaller ones.

		    // - if the source image is exactly the same dimensional size to a specific output size, then we don't
		    //   need to resample/resize it, we can just use it. This improves image quality, as no additional resampling occurs.

		    // PRINT SIZE
		    if (primaryDimension >= 1600)
		    {
		        if (primaryDimension == 1600)
		        {
		            // image is of acceptable dimensions already, just copy over, don't resample.
		            File.Copy(_sourceImageFilename, string.Format("{0}1600\\{1}", galleryPath, targetFilename));
		        }
		        else
		        {
		            resizedImage = ResizeImage(1600, sourceImage);
		            resizedImage.Save(string.Format("{0}1600\\{1}", galleryPath, targetFilename), info[1], encParams);
		            resizedImage.Dispose();
		        }
			
		        SixteenHundred = targetFilename;
		    }
		
		    // LARGE SIZE
		    if (primaryDimension >= 985)
		    {
		        if (primaryDimension == 985)
		        {
		            // image is of acceptable dimensions already, just copy over, don't resample.
		            File.Copy(_sourceImageFilename, string.Format("{0}1024\\{1}", galleryPath, targetFilename));
		        }
		        else
		        {
		            resizedImage = ResizeImage(985, sourceImage);
		            resizedImage.Save(string.Format("{0}1024\\{1}", galleryPath, targetFilename), info[1], encParams);
		            resizedImage.Dispose();
		        }

		        OneThousandAndTwentyFour = targetFilename;
		    }
			
		    // THUMBNAIL SIZE
		    if (primaryDimension >= thumbWidth)
		    {
		        resizedImage = ResizeImage(thumbWidth, sourceImage);
		        resizedImage.Save(string.Format("{0}thumb\\{1}", galleryPath, targetFilename), info[1], encParams);
		        resizedImage.Dispose();

		        Thumbnail = targetFilename;
		    }
	
		    sourceImage.Dispose();
		}
		#endregion
		
		#region private methods
	    /// <summary>
		/// Resizes the larger, original image down to the correct size required. Resizes either Landscape or Portrait images.
		/// </summary>
		private static Image ResizeImage(int width, Image imageToResize) 
		{
			int scaledHeight;
			int scaledWidth;

			// landscape or portrait?
			if (imageToResize.Width > imageToResize.Height)
			{
				scaledWidth = width;
                // don't remove castings, will break the math.
                // ReSharper disable RedundantCast
                scaledHeight = Convert.ToInt32(Helpers.RoundUp((double)scaledWidth * (double)imageToResize.Height / (double)imageToResize.Width));
                // ReSharper restore RedundantCast
			}
			else
			{
                // don't remove castings, will break the math.
                // ReSharper disable RedundantCast
                scaledWidth = Convert.ToInt32(Helpers.RoundUp((double)width * (double)imageToResize.Width / (double)imageToResize.Height));
                // ReSharper restore RedundantCast
				scaledHeight = width;
			}

			Image newImage = new Bitmap(scaledWidth, scaledHeight);
			var graphic = Graphics.FromImage(newImage);

			graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphic.SmoothingMode = SmoothingMode.HighQuality;
			graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphic.CompositingQuality = CompositingQuality.HighQuality;
				
			graphic.DrawImage(imageToResize, 0, 0, scaledWidth, scaledHeight);
			graphic.Dispose();

			return newImage;
		}

		/// <summary>
		/// Initialises class members.
		/// </summary>
		private void InitClass() 
		{
			EightHundred = string.Empty;
			OneThousandAndTwentyFour = string.Empty;
			SixteenHundred = string.Empty;
			Thumbnail = string.Empty;
		}
		#endregion
	}
}