using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Caching;

namespace Apollo.Galleries
{
	/// <summary>
	/// Point-Of-Entry for the Gallery system, provides access to statistics and the offers 
	/// the ability to create new galleries, collect existing ones and persist them back.
	/// </summary>
	public class GalleryServer
	{
		#region members
		private readonly Statistics _statistics;
		private readonly CommonQueries _queries;
		private readonly LatestCacheItems _latestCacheGalleries;
		#endregion

		#region constructors
		/// <summary>
		/// Create a new GalleryServer object.
		/// </summary>
		internal GalleryServer() 
		{
			_statistics = new Statistics();
            _queries = new CommonQueries();
            _latestCacheGalleries = new LatestCacheItems(ApplicationContentType.GalleryGallery, 10);
		}
		#endregion
		
		#region accessors
		/// <summary>
		/// Supplies general statistical information about Gallieres.
		/// </summary>
		public Statistics Statistics { get { return _statistics; } }
		/// <summary>
		/// Contains queries that will return common Gallery objects.
		/// </summary>
		public CommonQueries CommonQueries { get { return _queries; } }
		#endregion

		#region public methods
		/// <summary>
		/// Returns a new Image object, for use with a Gallery object.
		/// </summary>
		public GalleryImage NewImage() 
		{
			return new GalleryImage(ObjectCreationMode.New);
		}
	
		/// <summary>
		/// Returns a new Gallery object.
		/// </summary>
		public Gallery NewGallery() 
		{
			return new Gallery(ObjectCreationMode.New);
		}

		/// <summary>
		/// Returns a new Category object.
		/// </summary>
		public GalleryCategory NewCategory() 
		{
			return new GalleryCategory(ObjectCreationMode.New);
		}
	
		/// <summary>
		/// Collects a gallery object from the datastore. Populates the items collection with all the Images and Video's for the Gallery.
		/// </summary>
		/// <param name="id">The numeric-identifier of the Gallery.</param>
		/// <returns>A complete Gallery object.</returns>
		public Gallery GetGallery(long id) 
		{
			if (id < 1)
				return null;

			// attempt to get the Gallery from the Cache.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(Gallery), id);
            var gallery = CacheManager.RetrieveItem(cacheId) as Gallery;

			if (gallery != null)
			    return gallery;
            
            // Gallery not in the cache, retrieve it from the database.
			gallery = new Gallery(ObjectCreationMode.Retrieve);
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetGalleryObject", connection) {CommandType = CommandType.StoredProcedure};
		    SqlDataReader reader = null;

			var idParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = id};
		    command.Parameters.Add(idParam);

			try
			{
				connection.Open();
				reader = command.ExecuteReader();
			    
			    if (!reader.HasRows)
			    {
			        reader.Close();
			        connection.Close();
			        return null;
			    }
			    else
			    {
			        reader.Read();

			        // build the Gallery object.
			        gallery.Id = reader.GetInt64(reader.GetOrdinal("ID"));
			        gallery.Title = reader.GetString(reader.GetOrdinal("Title"));
			        gallery.CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate"));
			        gallery.Description = reader.GetString(reader.GetOrdinal("Description"));
			        gallery.Type = (GalleryType) (byte) reader["Type"];
			        gallery.Status = (GalleryStatus) (byte) reader["Status"];
			        gallery.IsPublic = reader.GetBoolean(reader.GetOrdinal("IsPublic"));

			        // build the categories collection.
			        reader.NextResult();
			        while (reader.Read())
			            gallery.Categories.Add(Server.Instance.GalleryServer.GetCategory(reader.GetInt64(reader.GetOrdinal("CategoryID"))));

			        reader.NextResult();
			        while (reader.Read())
			        {
			            // building Gallery object up-front to optimise the process.
			            var image = new GalleryImage(ObjectCreationMode.Retrieve)
			            {
			                Id = reader.GetInt64(reader.GetOrdinal("ID")),
			                BaseUrl = reader.GetString(reader.GetOrdinal("BaseUrl")),
			                CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
			                CaptureDate = reader.GetDateTime(reader.GetOrdinal("CaptureDate")),
			                Name = reader.GetString(reader.GetOrdinal("Name")),
			                Comment = reader.GetString(reader.GetOrdinal("Comment")),
			                Credit = reader.GetString(reader.GetOrdinal("Credit")),
			                Views = reader.GetInt64(reader.GetOrdinal("Views")),
			                ParentGalleryId = reader.GetInt64(reader.GetOrdinal("GalleryID")),
			                GalleryImages =
			                {
			                    EightHundred = reader.GetString(reader.GetOrdinal("Filename800")),
			                    OneThousandAndTwentyFour = reader.GetString(reader.GetOrdinal("Filename1024")),
			                    SixteenHundred = reader.GetString(reader.GetOrdinal("Filename1600")),
			                    Thumbnail = reader.GetString(reader.GetOrdinal("ThumbnailFilename"))
			                }
			            };

			            // not using AddPhoto() as so not to alter hasChanged flag.
			            gallery.Photos.Add(image);
			        }
			    }

			    CacheManager.AddItem(gallery, cacheId);
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return gallery;
		}

	    /// <summary>
	    /// Takes in a Gallery object and persists it to the data-store. If this is a new Gallery, then it'll be created, else updated.
	    /// The only requirement is that a Title is present for the Gallery, update will fail otherwise. The Gallery's items (Image & Video)
	    /// collection is also persisted.
	    /// </summary>
	    /// <param name="gallery">The Gallery object to persist.</param>
	    /// <returns>A bool representing whether or not the Gallery object was sucessfully persisted or not. If not, 
	    /// it's advised to check all the properties for validity.</returns>
	    public void UpdateGallery(Gallery gallery) 
		{
			// we require a title for the gallery.
			if (string.IsNullOrEmpty(gallery.Title))
			    return;

	        var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UpdateGalleryObject", connection) {CommandType = CommandType.StoredProcedure};

		    command.Parameters.Add(new SqlParameter("@ID", gallery.Id));
			command.Parameters.Add(new SqlParameter("@Title", gallery.Title));
			command.Parameters.Add(new SqlParameter("@CreationDate", gallery.CreationDate));
			command.Parameters.Add(new SqlParameter("@Description", gallery.Description));
			command.Parameters.Add(new SqlParameter("@Type", (int)gallery.Type));
			command.Parameters.Add(new SqlParameter("@Status", (int)gallery.Status));
			command.Parameters.Add(new SqlParameter("@IsPublic", gallery.IsPublic));

			try
			{
				// update the Gallery object data.
				connection.Open();
                gallery.Id = Convert.ToInt64(command.ExecuteScalar());
				
				if (!gallery.IsPersisted)
				{
					gallery.IsPersisted = true;
                    CacheManager.AddItem(gallery, CacheManager.GetApplicationUniqueId(typeof(Gallery), gallery.Id));
					_latestCacheGalleries.RefreshItems();
				}

                // sort the photos by filename.
			    gallery.Photos.Sort();

				// only update the exhibits if they've been updated.
				// first we must see if any image data has been changed.
				var updateExhibits = false;
				foreach (var image in gallery.Photos.Where(image => image.HasChanged))
				{
				    updateExhibits = true;

				    // reset their status.
				    image.HasChanged = false;
				}

				if (updateExhibits || gallery.HasChanged)
				{
					// clear all exhibit relationships
					command.Parameters.Clear();
					command.CommandText = "ClearGalleryExhibitRelationships";
					command.Parameters.Add(new SqlParameter("@ID", gallery.Id));
					command.ExecuteNonQuery();

					foreach (var image in gallery.Photos)
					{
						command.Parameters.Clear();
						command.CommandText = "UpdateGalleryImage";

						command.Parameters.Add(new SqlParameter("@ID", image.Id));
						command.Parameters.Add(new SqlParameter("@Name", image.Name));
						command.Parameters.Add(new SqlParameter("@Comment", image.Comment));
						command.Parameters.Add(new SqlParameter("@Credit", image.Credit));
						command.Parameters.Add(new SqlParameter("@CreationDate", image.CreationDate));
						command.Parameters.Add(new SqlParameter("@CaptureDate", image.CaptureDate));
						command.Parameters.Add(new SqlParameter("@BaseUrl", image.BaseUrl));
						command.Parameters.Add(new SqlParameter("@800", image.GalleryImages.EightHundred));
						command.Parameters.Add(new SqlParameter("@1024", image.GalleryImages.OneThousandAndTwentyFour));
						command.Parameters.Add(new SqlParameter("@1600", image.GalleryImages.SixteenHundred));
						command.Parameters.Add(new SqlParameter("@Thumbnail", image.GalleryImages.Thumbnail));
						command.Parameters.Add(new SqlParameter("@GalleryID", image.ParentGalleryId));

                        image.Id = Convert.ToInt64(command.ExecuteScalar());
					}

					// delete orphaned images.
					command.Parameters.Clear();
					command.CommandText = "DeleteOrphanedImages";
					command.ExecuteNonQuery();
				}

				// reset for future updates as this object is cached.
				gallery.HasChanged = false;

				// update the categories this gallery is to be shown in.
				foreach (var category in gallery.Categories)
				{
					category.AddGallery(gallery);
					Server.Instance.GalleryServer.UpdateCategory(category);
					category.ParentSite.RecentGalleries.RefreshGalleries();
				}
			}
			finally
			{
				connection.Close();
			}

	        return;
		}

		/// <summary>
		/// Returns a specific Gallery Category based on it's ID.
		/// </summary>
		public GalleryCategory GetCategory(long id) 
		{
			if (id < 1)
				return null;

			// attempt to retrieve the Category from the Cache.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(GalleryCategory), id);
            var category = CacheManager.RetrieveItem(cacheId) as GalleryCategory;

			if (category == null)
			{
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetGalleryCategory", connection) {CommandType = CommandType.StoredProcedure};
			    SqlDataReader reader = null;
				
				var param = new SqlParameter("@ID", SqlDbType.BigInt) {Value = id};
			    command.Parameters.Add(param);

				try
				{
					connection.Open();
					reader = command.ExecuteReader();
					if (reader.Read())
					{
						category = BuildCategoryObject(reader);
                        CacheManager.AddItem(category, cacheId);
					}
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}

			return category;
		}

		/// <summary>
		/// Persists changes to an existing, or new Category object.
		/// </summary>
		public void UpdateCategory(GalleryCategory galleryCategory) 
		{
			if (galleryCategory.IsPersisted && galleryCategory.HasChanged == false)
				return;

			if (galleryCategory.Owner == null)
				throw new Exception("No owner specified.");

			if (string.IsNullOrEmpty(galleryCategory.Name))
				throw new Exception("No name specified.");

			if (galleryCategory.ParentSite == null)
				throw new Exception("No Parent Site specified.");

		    var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = connection.CreateCommand();
			SqlTransaction transaction;

			command.Connection = connection;
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "UpdateGalleryCategory";

			var idParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = galleryCategory.Id};
		    command.Parameters.Add(idParam);

			command.Parameters.Add(new SqlParameter("@Name", galleryCategory.Name));
			command.Parameters.Add(new SqlParameter("@Description", galleryCategory.Description));
			command.Parameters.Add(new SqlParameter("@OwnerUID", galleryCategory.Owner.Uid));
			command.Parameters.Add(new SqlParameter("@Type", (int)galleryCategory.Type));
			command.Parameters.Add(new SqlParameter("@Active", galleryCategory.Active));
			command.Parameters.Add(new SqlParameter("@ParentSiteID", galleryCategory.ParentSite.Id));

			try
			{
				connection.Open();
				transaction = connection.BeginTransaction();
				command.Transaction = transaction;

				// run the update-category procedure.
                galleryCategory.Id = Convert.ToInt64(command.ExecuteScalar());

				// persist the categories galleries.
				if (galleryCategory.IsPersisted)
				{
					command.Parameters.Clear();
					command.CommandText = "ClearGalleryCategoryGalleries";
					command.Parameters.Add(idParam);
					command.ExecuteNonQuery();

				    command.CommandText = "CreateGalleryCategoryGalleryRelation";
				    var galleryParam = new SqlParameter("@GalleryID", SqlDbType.BigInt);
				    command.Parameters.Add(galleryParam);

				    foreach (var gallery in galleryCategory.AllGalleries)
				    {
					    galleryParam.Value = gallery.Id;
					    command.ExecuteNonQuery();
                    }
                }

				transaction.Commit();
			    galleryCategory.ParentSite.RefreshGalleryCategories();
				galleryCategory.RefreshGalleries();

                if (!galleryCategory.IsPersisted)
                {
                    CacheManager.AddItem(galleryCategory, CacheManager.GetApplicationUniqueId(typeof(GalleryCategory), galleryCategory.Id));
                    galleryCategory.IsPersisted = true;
                }
			}
			finally
			{
				connection.Close();				
			}
		}

		/// <summary>
		/// Permenantly removes a category from the system. Category must be empty of galleries and sub-categories.
		/// </summary>
		public void DeleteCategory(GalleryCategory galleryCategory) 
		{
			if (galleryCategory.AllGalleries.Count > 0)
				throw new Exception("Category is not empty of galleries.");

			if (galleryCategory.Id < 1)
				throw new Exception("Invalid Category ID.");

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteGalleryCategory", connection) {CommandType = CommandType.StoredProcedure};
		    var idParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = galleryCategory.Id};
		    command.Parameters.Add(idParam);

			try
			{
				connection.Open();
				command.ExecuteNonQuery();

				// remove category from caches.
				galleryCategory.ParentSite.RefreshGalleryCategories();
                CacheManager.RemoveItem(CacheManager.GetApplicationUniqueId(typeof(GalleryCategory), galleryCategory.Id));
			}
			finally
			{
				connection.Close();
			}
		}

		/// <summary>
		/// Permenantly deletes the relationships between images and a gallery, then deletes the gallery itself.
		/// </summary>
		public void DeleteGallery(Gallery gallery) 
		{
			if (gallery == null)
				throw new Exception("Invalid Gallery supplied.");

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteGallery", connection) {CommandType = CommandType.StoredProcedure};

		    var uidParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = gallery.Id};
		    command.Parameters.Add(uidParam);

			try
			{
				CacheManager.RemoveItem(gallery.Id);
				connection.Open();
				command.ExecuteNonQuery();

                foreach (var cat in gallery.Categories)
                    cat.RefreshGalleries();
			}
			finally
			{
				connection.Close();
			}
		}

		/// <summary>
		/// Facilitates the finding and retrieval of Gallery objects.
		/// </summary>
		public GalleryFinder NewGalleryFinder() 
		{
			return new GalleryFinder();
		}

		/// <summary>
		/// Retrieves a specific image. Less efficient than getting the gallery first and retrieving the image via that.
		/// </summary>
		public IGalleryImage GetImage(long id) 
		{
			if (id < 1)
				return null;
            
            var image = CacheManager.RetrieveGalleryImage(id);
			if (image != null)
				return image;

			// get the parent gallery info and retrieve that way.
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetGalleryImageParentID", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ImageID", id));

			try
			{
				connection.Open();
                var galleryId = (long)Sql.GetValue(typeof(long), command.ExecuteScalar());
				var parentGallery = GetGallery(galleryId);
				image = parentGallery.GetImage(id);
			}
			finally
			{
				connection.Close();
			}

			return image;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Worker method for the two ways of retrieving a Category object in this class (GetRootCategories and GetCategory)
		/// </summary>
		/// <param name="reader">The SqlDataReader containing the required fields to populate the object, must be at a valid 
		/// row or an exception will be thrown.</param>
		private static GalleryCategory BuildCategoryObject(IDataRecord reader) 
		{
			var category = new GalleryCategory(ObjectCreationMode.Retrieve)
            {
                Id = reader.GetInt64(reader.GetOrdinal("ID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                ParentSite = Server.Instance.GetSite(reader.GetInt32(reader.GetOrdinal("ParentSiteID"))),
                Owner = reader["Owner"].ToString() != String.Empty ? Server.Instance.UserServer.GetUser((Guid) reader["Owner"]) : null
            };

		    return category;
		}
		#endregion
	}
}