using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a gallery video, contains properties for path and type.
	/// </summary>
	public class GalleryVideo : CommonBase, IGalleryVideo
	{
		#region members
	    private string _name;
		private string _comment;
		private string _filename;
		private string _thumbnailPath;
		private DateTime _captureDate;
	    private Codec _codec;
	    #endregion

		#region accessors
	    public Guid Uid { get; set; }

	    public string Name 
		{
			get { return _name; }
			set
			{
				_name = value;
				HasChanged = true;
			}
		}

		public string Comment 
		{
			get { return _comment; }
			set
			{
				_comment = value;
				HasChanged = true;
			}
		}

		public DateTime CaptureDate 
		{
			get { return _captureDate; }
			set
			{
				_captureDate = value;
				HasChanged = true;
			}
		}

	    public DateTime CreationDate { get; set; }

	    public string Filename 
		{
			get { return _filename; }
			set
			{
				_filename = value;
				HasChanged = true;
			}
		}

		public Codec Codec 
		{
			get { return _codec; }
			set
			{
				_codec = value;
				HasChanged = true;
			}
		}

		public string Thumbnail 
		{
			get { return _thumbnailPath; }
			set
			{
				_thumbnailPath = value;
				HasChanged = true;
			}
		}

	    /// <summary>
	    /// Provides an indirect reference to the Gallery containing this Video.
	    /// </summary>
	    public Guid ParentGalleryUid { get; set; }
	    #endregion

		#region constructors
		public GalleryVideo(ObjectCreationMode mode)
		{
			Uid = Guid.Empty;
			_name = string.Empty;
			_comment = string.Empty;
			_filename = string.Empty;
			_thumbnailPath = string.Empty;
			_captureDate = DateTime.Now;
			CreationDate = DateTime.Now;
			_codec = Codec.Unknown;
			ParentGalleryUid = Guid.Empty;

			if (mode == ObjectCreationMode.New)
			{
				Uid = Guid.NewGuid();
				IsPersisted = false;
			}
			else
			{
				IsPersisted = true;
			}
		}
		#endregion
	}
}