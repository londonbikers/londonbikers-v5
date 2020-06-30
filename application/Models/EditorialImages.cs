using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	public class EditorialImages : IEditorialImages
	{
		#region members
		private List<EditorialImage> _collection;
	    private int _coverImageIndex;
		private int _introImageIndex;
	    private readonly ICommonBase _parent;
		#endregion
		
		#region accessors
	    /// <summary>
		/// The number of images in the collection.
		/// </summary>
		public int Count 
		{ 
			get
			{
			    if (_collection == null)
					PopulateCollection();

			    return _collection != null ? _collection.Count : 0;
			}
		}
		/// <summary>
		/// The index of the Cover Image within the collection.
		/// </summary>
		public int CoverImage 
		{ 
			get 
			{ 
				if (_collection == null)
					PopulateCollection();

				return _coverImageIndex; 
			} 
			set { _coverImageIndex = value; } 
		}
		/// <summary>
		/// The index of the Intro Image within the collection.
		/// </summary>
		public int IntroImage 
		{ 
			get 
			{
                if (_collection == null)
					PopulateCollection();

                return _introImageIndex; 
			}
            set { _introImageIndex = value; } 
		}
		/// <summary>
		/// The default indexer.
		/// </summary>
		public EditorialImage this[int index] 
		{ 
			get
			{
			    if (_collection == null)
					PopulateCollection();

			    return _collection != null ? _collection[index] : null;
			}
		}
		/// <summary>
		/// The image collection.
		/// </summary>
		public List<EditorialImage> List 
		{ 
			get 
			{ 
				if (_collection == null)
					PopulateCollection();

				return _collection; 
			} 
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Images object.
		/// </summary>
        /// <param name="parent">The parent object.</param>
		public EditorialImages(ICommonBase parent) 
		{
			_coverImageIndex = -1;
            _introImageIndex = -1;
		    _parent = parent;
		} 
		#endregion

		#region public methods
        /// <summary>
        /// Attempts to retrieve an image of a specific type. Only of use for cover and intro images really.
        /// </summary>
        public EditorialImage GetImage(ContentImageType type, bool randomise = false, bool fallbackToCover = true)
        {
            switch (type)
            {
                case ContentImageType.Cover:
                    try
                    {
                        return this[CoverImage];
                    }
                    catch
                    {
                        return null;
                    }
                case ContentImageType.Intro:
                    try
                    {
                        return this[IntroImage];
                    }
                    catch
                    {
                        return null;
                    }
                case ContentImageType.SlideShow:
                    return GetSlideShowImage(randomise, fallbackToCover);
                default:
                    return null;
            }
        }

	    /// <summary>
		/// Adds a content image to the collection.
		/// </summary>
		/// <param name="id">The ID of the image to add.</param>
		public void Add(long id) 
		{
	        EditorialImage image1;
	        IDisposable disposable1;
			var flag1 = false;
            if (_collection == null)
                PopulateCollection();

	        if (_collection == null) return;
	        var enumerator1 = _collection.GetEnumerator();
			
	        try
	        {
	            while (enumerator1.MoveNext())
	            {
	                image1 = enumerator1.Current;
	                if (image1 != null && image1.Id == id)
	                    flag1 = true;
	            }
	        }
	        finally
	        {
	            disposable1 = enumerator1;
	            disposable1.Dispose();
	        }

	        if (flag1) return;
	        _collection.Add(Server.Instance.ContentServer.GetImage(id));
		} 
        
		/// <summary>
		/// Removes a content image from the collection.
		/// </summary>
		/// <param name="id">The ID of the image to remove.</param>
		public void Remove(long id) 
		{
		    EditorialImage image2;
		    IDisposable disposable1;
			EditorialImage image1 = null;
            if (_collection == null)
                PopulateCollection();

		    if (_collection == null) return;
		    var enumerator1 = _collection.GetEnumerator();
			
		    try
		    {
		        while (enumerator1.MoveNext())
		        {
		            image2 = enumerator1.Current;
		            if (image2 != null && image2.Id == id)
		                image1 = image2;
		        }
		    }
		    finally
		    {
		        disposable1 = enumerator1;
		        disposable1.Dispose();
		    }

		    _collection.Remove(image1);
		} 

		/// <summary>
		/// Attempts to retrieve a specific image from the collection.
		/// </summary>
		/// <param name="id">The ID of the image to retrieve.</param>
		public EditorialImage GetImage(long id)
		{
		    return List.FirstOrDefault(image => image.Id == id);
		}

	    /// <summary>
		/// Returns a sub-collection of Images with a specific Type.
		/// </summary>
		public List<EditorialImage> FilterImages(ContentImageType typeToReturn) 
		{
	        return List.Where(image => image.Type == typeToReturn).ToList();
		}

		/// <summary>
		/// Counts how many images there are in the collection of a specific type.
		/// </summary>
		/// <param name="imageType">The type of images to count.</param>
		public int ImageTypeCount(ContentImageType imageType) 
		{
		    return List.Count(image => image.Type == imageType);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Persists the image collection against the consuming object.
		/// </summary>
		internal void Save() 
		{
            if (_collection == null) 
                return;

			var counter = 0;
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = connection.CreateCommand();
			command.CommandText = "FlushContentImages";
			command.CommandType = CommandType.StoredProcedure;
			SqlTransaction transaction = null;
			command.Parameters.AddWithValue("@ID", _parent.Id);

			try
			{
				connection.Open();
				transaction = connection.BeginTransaction();

				// peculiar.
				command.Connection = connection;
				command.Transaction = transaction;
				command.ExecuteNonQuery();

				command.CommandText = "AddContentImage";
				foreach (var image in _collection)
				{
					command.Parameters.Clear();
					command.Parameters.Add(new SqlParameter("@content", _parent.Id));
					command.Parameters.Add(new SqlParameter("@image", image.Id));
					command.Parameters.Add(new SqlParameter("@coverImage", (_coverImageIndex == counter) ? true : false));
					command.Parameters.Add(new SqlParameter("@IntroImage", (_introImageIndex == counter) ? true : false));
					command.ExecuteNonQuery();
					counter++;
				}

				transaction.Commit();
			}
			catch
			{
				if (transaction != null)
					transaction.Rollback();

				throw;
			}
			finally
			{
				connection.Close();
			}		
		} 

		/// <summary>
		/// Retrieve the images from the database.
		/// </summary>
		private void PopulateCollection() 
		{
			var i = 0;
			_collection = new List<EditorialImage>();
            lock (_collection)
            {
                // no point querying the database when the consumer is not persisted yet; there won't be any images.
                if (_parent == null || _parent.Id == 0)
                    return;

                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetContentImages", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@ContentID", _parent.Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        _collection.Add(Server.Instance.ContentServer.GetImage(reader.GetInt64(reader.GetOrdinal("ID"))));

                        if (reader.GetBoolean(reader.GetOrdinal("Cover")))
                            _coverImageIndex = i;

                        if (reader.GetBoolean(reader.GetOrdinal("Intro")))
                            _introImageIndex = i;

                        i++;
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
		}

        private EditorialImage GetSlideShowImage(bool randomise, bool fallbackToCover = true)
        {
            var slides = List.Where(q => q.Type == ContentImageType.SlideShow).ToList();
            if (slides.Count == 0 && CoverImage != -1)
                return fallbackToCover ? this[CoverImage] : null;
            if (slides.Count == 0)
                return null;

            var index = 0;
            if (randomise)
            {
                var r = new Random(DateTime.Now.Millisecond + Convert.ToInt32(_parent.Id));
                index = r.Next(slides.Count);
            }

            return slides[index];
        }
		#endregion
	}
}