using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	public class GalleryCategory : CommonBase, IGalleryCategory
	{
		#region members
		private Guid _uid;
		private string _name;
		private string _description;
		private CategoryType _type;
		private User _owner;
        private ILightCollection<IGallery> _galleries;
	    private ILightCollection<IGallery> _allGalleries;
		private bool _active;
		private ISite _parentSite;
		#endregion

		#region accessors
		/// <summary>
		/// Determines whether or not this Category is active.
		/// </summary>
		public bool Active 
		{
			get { return _active; }
			set
			{
				_active = value;
				HasChanged = true;
			}
		}

		/// <summary>
		/// The identifier for this Category.
		/// </summary>
		public Guid Uid 
		{
			get { return _uid; } 
			set 
			{ 
				_uid = value;
				LegacyId = _uid;
			} 
		}
		
		/// <summary>
		/// The textual name for this Category.
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
		/// The textual description for this Category.
		/// </summary>
		public string Description 
		{ 
			get { return _description; } 
			set 
			{ 
				_description = value; 
				HasChanged = true;
			} 
		}
		
		/// <summary>
		/// Denotes the type this Category is.
		/// </summary>
		public CategoryType Type 
		{ 
			get { return _type; } 
			set 
			{ 
				_type = value; 
				HasChanged = true;
			} 
		}
		
		/// <summary>
		/// Denotes the owner of this Category. Categories can be public, or user-owned.
		/// </summary>
		public User Owner 
		{ 
			get { return _owner; } 
			set 
			{ 
				_owner = value; 
				HasChanged = true;
			} 
		}
		
		/// <summary>
		/// The public galleries shown in this category.
		/// </summary>
        public ILightCollection<IGallery> Galleries 
		{ 
			get 
			{ 
				if (_galleries == null)
					CollectGalleries();

				return _galleries; 
			} 
		}

        /// <summary>
        /// All the galleries shown in this category.
        /// </summary>
        public ILightCollection<IGallery> AllGalleries
        {
            get
            {
                if (_allGalleries == null)
                    CollectAllGalleries();

                return _allGalleries;
            }
        }

		/// <summary>
		/// The Site to which this Category belongs.
		/// </summary>
		public ISite ParentSite 
		{ 
			get { return _parentSite; } 
			set 
			{ 
				_parentSite = value; 
				HasChanged = true;
			} 
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Category object.
		/// </summary>
		public GalleryCategory(ObjectCreationMode mode) 
		{
			_uid = Guid.Empty;
			_name = String.Empty;
			_description = String.Empty;
			_type = CategoryType.Generic;
			_active = true;
			DerivedType = GetType();

			if (mode == ObjectCreationMode.New)
			{
				_uid = Guid.NewGuid();
				IsPersisted = false;
			}
			else
			{
				IsPersisted = true;
			}
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Gallery to the collection.
		/// </summary>
		public void AddGallery(Gallery gallery) 
		{
		    if (gallery == null) 
                return;
            if (_galleries == null)
		        CollectGalleries();
            if (_allGalleries == null)
                CollectAllGalleries();

		    if (_galleries != null && !_galleries.Contains(gallery))
		        _galleries.Add(gallery);

            if (_allGalleries != null && !_allGalleries.Contains(gallery))
                _allGalleries.Add(gallery);

		    HasChanged = true;
		}     

		/// <summary>
		/// Removes a specific Gallery from the collection.
		/// </summary>
		public void RemoveGallery(Gallery gallery) 
		{
		    if (!_galleries.Contains(gallery)) 
                return;

		    _galleries.Remove(gallery);
            _allGalleries.Remove(gallery);

		    HasChanged = true;
		}

		/// <summary>
		/// Re-loads the galleries collection.
		/// </summary>
		internal void RefreshGalleries() 
		{
			CollectGalleries();
            CollectAllGalleries();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Builds the galleries collection on demand.
		/// </summary>
		private void CollectGalleries()
		{
		    _galleries = new LightCollection<IGallery>();
            lock (_galleries)
            {
                SqlDataReader reader = null;
                const string commandText = "GetGalleryCategoryGalleries";
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand(commandText, connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@ID", Id));

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        _galleries.Add((long) Sql.GetValue(typeof (long), reader["ID"]));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
		}

        /// <summary>
        /// Builds the galleries collection on demand.
        /// </summary>
        private void CollectAllGalleries()
        {
            _allGalleries = new LightCollection<IGallery>();
            lock (_allGalleries)
            {
                SqlDataReader reader = null;
                const string commandText = "GetAllGalleryCategoryGalleries";
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand(commandText, connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@ID", Id));

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        _allGalleries.Add((long)Sql.GetValue(typeof(long), reader["ID"]));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
        }
		#endregion
	}
}