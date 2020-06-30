using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a gallery of items for exhibition.
	/// </summary>
	public class Gallery : CommonBase, IGallery
	{
		#region members
	    private List<GalleryCategory> _categories;
	    private readonly Comments _comments;
		#endregion

		#region accessors
	    /// <summary>
	    /// The unique identifier for this Gallery.
	    /// </summary>
	    public Guid Uid { get; set; }

	    /// <summary>
	    /// The title of the Gallery.
	    /// </summary>
	    public string Title { get; set; }

	    /// <summary>
	    /// The description of the Gallery, can be used as an introduction of any length.
	    /// </summary>
	    public string Description { get; set; }

	    /// <summary>
	    /// The time the gallery was created in Apollo.
	    /// </summary>
	    public DateTime CreationDate { get; set; }

	    /// <summary>
	    /// The collection of Images being shown in this Gallery.
	    /// </summary>
	    public List<IGalleryImage> Photos { get; private set; }

	    /// <summary>
	    /// The type of Gallery this is. I.E loose or specific.
	    /// </summary>
	    public GalleryType Type { get; set; }

	    /// <summary>
	    /// Denotes whether or not this Gallery is public, to be implemented so that only Apollo users can view the Gallery.
	    /// </summary>
	    public bool IsPublic { get; set; }

	    /// <summary>
	    /// Denotes whether or not a gallery is active or not.
	    /// </summary>
	    public GalleryStatus Status { get; set; }

		/// <summary>
		/// Provides a list of the categories to which this Gallery belongs to.
		/// </summary>
		public List<GalleryCategory> Categories 
		{ 
			get 
			{
				if (_categories == null)
					CollectCategories();

				return _categories; 
			}
		}

		/// <summary>
		/// Retrieves a specific Exhibit in the gallery.
		/// </summary>
		public object this [long id] 
		{
			get { return Photos.Cast<object>().FirstOrDefault(exhibit => ((GalleryImage) exhibit).Id == id); }
		}

		/// <summary>
		/// Provides access to any user-comments that may relate to this Gallery.
		/// </summary>
		public Comments Comments { get { return _comments; } }
		#endregion

		#region constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mode">Denotes whether this is a new gallery or an existing one. Used to pre-populate default values.</param>
		public Gallery(ObjectCreationMode mode) 
		{
			Title = string.Empty;
			Description = string.Empty;
			CreationDate = DateTime.Now;
			Photos = new List<IGalleryImage>();
			IsPublic = true;
			Type = GalleryType.Specific;
			Status = GalleryStatus.Pending;
		    _comments = new Comments(this);
			
			IsPersisted = mode != ObjectCreationMode.New;
		}
		#endregion

		#region public methods
		/// <summary>
		/// If not already present, this will add an Image to the exhibits collection.
		/// </summary>
		public void AddPhoto(GalleryImage galleryImage) 
		{
		    if (Photos.Contains(galleryImage)) return;
		    galleryImage.ParentGalleryId = Id;
		    Photos.Add(galleryImage);
		    HasChanged = true;
		}

		/// <summary>
		/// Removes an Image exhibit from the Gallery.
		/// </summary>
		public void RemovePhoto(GalleryImage galleryImage) 
		{
            var pos = -1;
            for (var i = 0; i < Photos.Count; i++)
            {
                if (((GalleryImage) Photos[i]).Id == galleryImage.Id)
                    pos = i;
            }

		    if (pos <= -1) return;
		    Photos.RemoveAt(pos);
		    HasChanged = true;
		}
		
		/// <summary>
		/// Determines the position within the collection of a particular image. 1-based.
		/// </summary>
		/// <param name="galleryImage">An Image belonging to the Gallery.</param>
		public int GetImagePosition(GalleryImage galleryImage) 
		{
			for (var i = 0; i < Photos.Count; i++)
			{
                if (Photos[i].Id == galleryImage.Id)
					return i + 1;
			}

			return -1;
		}

		/// <summary>
		/// Retrieves a specific Image from the Gallery.
		/// </summary>
		public GalleryImage GetImage(long imageId)
		{
		    return Photos.Cast<GalleryImage>().Where(image => image.Id == imageId).FirstOrDefault();
		}

	    /// <summary>
		/// Facilitates sequential viewing of Images within the Gallery.
		/// </summary>
		/// <param name="currentGalleryImage">To know what Image to serve next, the current one is required.</param>
		public IGalleryImage NextImage(IGalleryImage currentGalleryImage) 
		{
			var newPosition = ExhibitIndexInCollection(currentGalleryImage);
			if (newPosition == -1)
				newPosition = 0;

			// if this null is supplied, we should pull the first image.
			if (currentGalleryImage != null)
			{
				// if the supplied image doesn't belong to this gallery, pull the first image.
				if (currentGalleryImage.ParentGalleryId == Id)
				{
				    if (newPosition == Photos.Count - 1)
						return null;
				    newPosition++;
				}
			}

			// find next image.
			for (var i = newPosition; i < Photos.Count; i++)
			{
                if (Photos[i] is GalleryImage)
                    return Photos[i] as GalleryImage;
			}

			// whoah, no image in the collection
			return null;
		}

		/// <summary>
		/// Facilitates sequential viewing of Images within the Gallery.
		/// </summary>
		/// <param name="currentGalleryImage">To know what Image to serve next, the current one is required.</param>
		public IGalleryImage PreviousImage(IGalleryImage currentGalleryImage) 
		{
			var newPosition = ExhibitIndexInCollection(currentGalleryImage);
			if (newPosition == -1)
				newPosition = 0;

				// if this null is supplied, we should pull the first image.
			if (currentGalleryImage != null)
			{
				// if the supplied image doesn't belong to this gallery, pull the first image.
				if (currentGalleryImage.ParentGalleryId == Id)
				{
				    if (newPosition == 0) return null;
				    newPosition--;
				}
			}

			// find previous image.
			for (var i = newPosition; i >= 0; i--)
			{
                if (Photos[i] is GalleryImage)
                    return Photos[i] as GalleryImage;
			}

			// whoah, no image in the collection
			return null;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Determines what position a given exhibit lays at in the exhibit collection.
		/// </summary>
		/// <param name="exhibit">The exhibit to look for</param>
		private int ExhibitIndexInCollection(object exhibit) 
		{
			if (exhibit == null)
				return -1;

			var index = 0;
            var id = (exhibit is GalleryImage) ? (exhibit as GalleryImage).Id : ((GalleryVideo) exhibit).Id;

			foreach (var collectionExhibit in Photos)
			{
                if (collectionExhibit is GalleryImage && (collectionExhibit as GalleryImage).Id == id)
					return index;

				index++;
			}

			return -1;
		}
        
        /// <summary>
		/// Populates _categories with collection of categories.
		/// </summary>
		private void CollectCategories() 
		{
			_categories = new List<GalleryCategory>();
            lock (_categories)
            {
                var server = Server.Instance;
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetGalleryCategories", connection) {CommandType = CommandType.StoredProcedure};
                var idParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = Id};
                command.Parameters.Add(idParam);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = server.GalleryServer.GetCategory(reader.GetInt64(reader.GetOrdinal("CategoryID")));
                    if (category != null)
                        _categories.Add(category);
                }

                reader.Close();
                connection.Close();
            }
		}
		#endregion
	}
}