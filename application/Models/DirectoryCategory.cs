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
	/// Represents a Category in the Directory structure.
	/// </summary>
	public class DirectoryCategory : CommonBase, IComparable, IDirectoryCategory
	{ 
		#region members
	    private DirectoryItemCollection _directoryItems;
	    private DirectoryCategoryCollection _subDirectoryCategories;
		private DirectoryCategory _parentDirectoryCategory;
		private long _parentCategoryId;
		#endregion

		#region accessors
	    /// <summary>
	    /// The name for this Category.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// The description for this Category.
	    /// </summary>
	    public string Description { get; set; }

	    /// <summary>
	    /// Determines whether or not site membership is required to access this Category and it's items.
	    /// </summary>
	    public bool RequiresMembership { get; set; }

	    /// <summary>
		/// The collection of Directory Items associated with this Category.
		/// </summary>
		public DirectoryItemCollection DirectoryItems 
		{ 
			get 
			{ 
				if (_directoryItems == null)
				{
					HasChanged = true;
					RetrieveItems();
				}

				return _directoryItems; 
			} 
		}

	    /// <summary>
	    /// A collection of keywords associated with this Category.
	    /// </summary>
	    public List<string> Keywords { get; private set; }

	    /// <summary>
		/// The collection of sub-Categories associated with this Category.
		/// </summary>
		public DirectoryCategoryCollection SubDirectoryCategories 
		{ 
			get 
			{
                if (_subDirectoryCategories == null)
				{
					HasChanged = true;
					RetrieveSubCategories();
				}

                return _subDirectoryCategories; 
			} 
		}
		/// <summary>
		/// Returns any applicable parent Category for this Category.
		/// </summary>
		public DirectoryCategory ParentDirectoryCategory 
		{
			get
			{
                if (_parentDirectoryCategory == null)
					RetrieveParentCategory();

                return _parentDirectoryCategory;
			}
			set 
			{
                _parentDirectoryCategory = value;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Category object.
		/// </summary>
		internal DirectoryCategory(ObjectCreationMode mode)
		{
			if (mode == ObjectCreationMode.New)
				IsPersisted = false;

			InitialiseObject();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a filtered version of the Category Items collection.
		/// </summary>
		/// <param name="status">Determines which items of the relevant DirectoryStatus should be returned.</param>
		public DirectoryItemCollection FilteredItems(DirectoryStatus status) 
		{
			var items = new DirectoryItemCollection();
            lock (DirectoryItems)
            {
                foreach (var item in DirectoryItems.Cast<DirectoryItem>().Where(item => item.Status == status))
                    items.Add(item, false);
            }

			return items;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Assigns class members their default values.
		/// </summary>
		private void InitialiseObject() 
		{
			Name = string.Empty;
			Description = string.Empty;
			RequiresMembership = false;
			_parentCategoryId = 0;
			Keywords = new List<string>();
		}

		/// <summary>
		/// Retrieves the Item objects associated with this Category.
		/// </summary>
		private void RetrieveItems() 
		{
			_directoryItems = new DirectoryItemCollection();
            lock (_directoryItems)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetDirectoryCategoryItems", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@CategoryID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    // loading individually by id here is less efficient than bulk up-front loading of course,
                    // but we're relying on the benefits the CacheManager brings to boost overall performance.

                    while (reader.Read())
                        DirectoryItems.Add(Server.Instance.DirectoryServer.GetItem((long)reader["ID"]), false);
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
		/// Retrieves the Categories associated with this Category.
		/// </summary>
		private void RetrieveSubCategories() 
		{
			// retrieve the Category from the database.
			_subDirectoryCategories = new DirectoryCategoryCollection(this);
            lock (_subDirectoryCategories)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetDirectoryCategorySubCategories", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@CategoryID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    // loading individually by id here is less efficient than bulk up-front loading of course,
                    // but we're relying on the benefits the CacheManager brings to boost overall performance.

                    while (reader.Read())
                        SubDirectoryCategories.Add(Server.Instance.DirectoryServer.GetCategory((long)reader["ID"]), false);
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
		/// If there's a parent Category for this instance, then it will be retreived here.
		/// </summary>
		private void RetrieveParentCategory() 
		{
			if (_parentCategoryId < 1)
				return;

			_parentDirectoryCategory = Server.Instance.DirectoryServer.GetCategory(_parentCategoryId);
		}

		/// <summary>
		/// Builds the collection of parent categories.
		/// </summary>
		private DirectoryCategoryCollection RetrieveParentCategories() 
		{
			var parents = new DirectoryCategoryCollection();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetCategoryParents", connection);
			command.Parameters.Add(new SqlParameter("@ID", Id));
			command.CommandType = CommandType.StoredProcedure;
			SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			        parents.Add(Server.Instance.DirectoryServer.GetCategory((long)reader["ID"]), false);
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return parents;
		}
		#endregion

		#region IComparable Members
		public int CompareTo(object obj) 
		{
			return Name.CompareTo(((DirectoryCategory)obj).Name);
		}
		#endregion
	}
}