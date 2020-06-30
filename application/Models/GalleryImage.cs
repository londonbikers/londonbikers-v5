using System;
using System.IO;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.Files;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a gallery image, contains properties for various sizes of images.
	/// </summary>
	public class GalleryImage : CommonBase, IGalleryImage, IComparable
	{
		#region members
	    private string _name;
		private string _comment;
		private string _baseUrl;
		private string _credit;
		private DateTime _captureDate;
	    private readonly Comments _comments;
		#endregion members

		#region constructors
		/// <summary>
		/// Creates a new Image object.
		/// </summary>
		/// <param name="mode">Determines how the object is built.</param>
		public GalleryImage(ObjectCreationMode mode) 
		{
            Uid = Guid.Empty;
            _name = string.Empty;
            _comment = string.Empty;
            _baseUrl = string.Empty;
            _credit = string.Empty;
            _captureDate = DateTime.Now;
            CreationDate = DateTime.Now;
            ParentGalleryId = 0;
            _comments = new Comments(this);

			if (mode == ObjectCreationMode.New)
			{
				IsPersisted = false;
			}
			else
			{
				// as this is an existing image, we should instantiate the Images class.
				GalleryImages = new GalleryImages();
				IsPersisted = true;
			}

			DerivedType = GetType();
		}
		#endregion

		#region accessors
	    /// <summary>
	    /// The unique-identifer for this Image.
	    /// </summary>
	    public Guid Uid { get; set; }

	    /// <summary>
		/// The name of the image.
		/// </summary>
		public string Name 
		{ 
			get { return _name; } 
			set 
			{ 
				_name = value; 
				HasChanged = true;
			} 
		}
		/// <summary>
		/// The comment for this Image.
		/// </summary>
		public string Comment 
		{
			get { return _comment; }
			set
			{
				_comment = value;
				HasChanged = true;
			}
		}
		/// <summary>
		/// Any additional credits for this Image.
		/// </summary>
		public string Credit 
		{
			get { return _credit; }
			set 
			{
				_credit = value;
				HasChanged = true;
			}
		}
		/// <summary>
		/// The date this Image was taken on.
		/// </summary>
		public DateTime CaptureDate 
		{
			get { return _captureDate; }
			set
			{
				_captureDate = value;
				HasChanged = true;
			}
		}

	    /// <summary>
	    /// The date this Image was created in the Gallery.
	    /// </summary>
	    public DateTime CreationDate { get; set; }

	    /// <summary>
		/// If this Image is stored on an third-party server, this is the base-url to prefix the filenames with.
		/// </summary>
		public string BaseUrl 
		{
			get { return _baseUrl; }
			set
			{
				_baseUrl = value;
				HasChanged = true;
			}
		}

	    /// <summary>
	    /// The collection of filenames to the specific-size Images.
	    /// </summary>
	    public GalleryImages GalleryImages { get; private set; }

	    /// <summary>
	    /// Provides an indirect reference to the Gallery containing this Image.
	    /// </summary>
	    public long ParentGalleryId { get; set; }

	    /// <summary>
        /// Provides access to any user-comments that may relate to this Document.
        /// </summary>
        public Comments Comments { get { return _comments; } }
		#endregion

		#region public methods
		/// <summary>
		/// Used to re/define the source image to use for the size-specific Images. Use of this method triggers the downsampling process immediately.
		/// </summary>
        /// <param name="sourceImageFilename">The filename to the source image. This should be located in the gallery's contentstore root.</param>
		public void MakeImages(string sourceImageFilename) 
		{
			TriggerImageCreation(sourceImageFilename);
		}

        /// <summary>
        /// Invariably, not all images are available at larger sizes. This determines what the best-match size would be for a given one.
        /// </summary>
        /// <param name="desiredSize">The desired primary dimension size.</param>
        /// <returns>An integer representing the best-match primary dimension size for the size sought. This may be the actual desired size, or one smaller.</returns>
        public int GetActualSizeForDesiredSize(int desiredSize)
        {
            if (desiredSize != 1024 && desiredSize != 1600)
                throw new ArgumentException("desired size not recognised as an acceptable one!");

            switch (desiredSize)
            {
                case 1024:
                    return GalleryImages.OneThousandAndTwentyFour != string.Empty ? 1024 : 800;
                case 1600:
                    if (GalleryImages.SixteenHundred != string.Empty)
                        return 1600;
                    return GalleryImages.OneThousandAndTwentyFour != string.Empty ? 1024 : 800;
            }

            return 0;
        }
		#endregion

		#region private methods
		/// <summary>
		/// Underlying function for the MakeImages() overloads.
		/// </summary>
		private void TriggerImageCreation(string sourceImageFilename, bool populateDetailsFromMetaData = true) 
		{
			// make sure the image is an expected format.
		    var path = Path.GetExtension(sourceImageFilename);
		    if (path != null)
		    {
		        var extension = path.ToLower();
		        if (extension != ".jpg" && extension != ".jpeg" && extension != ".jpe")
		        {
		            Logger.LogWarning("Galleries.Image.TriggerImageCreation() - Invalid file format, format: " + extension);
		            throw new Exception("Invalid file format, Web formats expected.");
		        }
		    }

		    // if there's previous local images, then they should be deleted first.
            if (GalleryImages != null && _baseUrl == string.Empty)
                GalleryImages.DeletePreviousImages();

            GalleryImages = new GalleryImages(sourceImageFilename);
            GalleryImages.DownSampleImages();

            if (!populateDetailsFromMetaData)
                return;

            // populate the GalleryImage properties from the source-image meta-data!
		    var md = MetadataParser.RetrieveMetaData(sourceImageFilename, false);
		    if (md == null) return;
		    if (md.Captured != DateTime.MinValue) CaptureDate = md.Captured;
		    if (!string.IsNullOrEmpty(md.Comment)) Comment = md.Comment;
		    if (!string.IsNullOrEmpty(md.Creator)) Credit = md.Creator;
		    if (!string.IsNullOrEmpty(md.Name)) Name = md.Name;
		}
		#endregion

        public int CompareTo(object obj)
        {
            var image = (GalleryImage)obj;
            var compareResult = GalleryImages.OneThousandAndTwentyFour.CompareTo(image.GalleryImages.OneThousandAndTwentyFour);
            if (compareResult == 0)
                compareResult = Name.CompareTo(image.Name);

            return compareResult;
        }
    }
}