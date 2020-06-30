using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Caching;

namespace Apollo.Directory
{
	/// <summary>
	/// The controller for the Directory module. All major Directory object operations are performed via 
	/// </summary>
	public class DirectoryServer
	{
		#region members
		private readonly CommonQueries _commonQueries;
		private readonly LatestCacheItems _latestCacheItems;
		#endregion

		#region accessors
		/// <summary>
		/// Provides access to commonly-requested Directory queries.
		/// </summary>
		public CommonQueries CommonQueries { get { return _commonQueries; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Directory Server object.
		/// </summary>
		internal DirectoryServer() 
		{
			_latestCacheItems = new LatestCacheItems(ApplicationContentType.DirectoryItem, 75);
			_commonQueries = new CommonQueries();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a new Directory Item object.
		/// </summary>
		public DirectoryItem NewItem() 
		{
			return new DirectoryItem(ObjectCreationMode.New);
		}
        
		/// <summary>
		/// Returns an empty ItemCollection object.
		/// </summary>
		public DirectoryItemCollection NewItemCollection() 
		{
			return new DirectoryItemCollection();
		}
        
		/// <summary>
		/// Returns a new Directory Category object.
		/// </summary>
		public DirectoryCategory NewCategory() 
		{
			return new DirectoryCategory(ObjectCreationMode.New);
		}
        		
		/// <summary>
		/// Persists any changes to a Directory Item object.
		/// </summary>
		public void UpdateItem(DirectoryItem directoryItem) 
		{
			if (directoryItem.Title == String.Empty)
				throw new Exception("No Item title supplied!");

			if (directoryItem.Description == String.Empty)
				throw new Exception("No Item description supplied!");

			if (directoryItem.Submiter == null)
				throw new Exception("No submitter supplied!");

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UpdateDirectoryItem", connection) {CommandType = CommandType.StoredProcedure};
		    var idParam = new SqlParameter("@ID", directoryItem.Id);

			command.Parameters.Add(idParam);
			command.Parameters.Add(new SqlParameter("@Title", directoryItem.Title));
			command.Parameters.Add(new SqlParameter("@Description", directoryItem.Description));
			command.Parameters.Add(new SqlParameter("@TelephoneNumber", directoryItem.TelephoneNumber));
			command.Parameters.Add(new SqlParameter("@Images", Helpers.CollectionToDelimitedString(directoryItem.Images)));
			command.Parameters.Add(new SqlParameter("@Keywords", Helpers.CollectionToDelimitedString(directoryItem.Keywords)));
			command.Parameters.Add(new SqlParameter("@Links", Helpers.CollectionToDelimitedString(directoryItem.Links)));
			command.Parameters.Add(new SqlParameter("@NumberOfRatings", directoryItem.NumberOfRatings));
			command.Parameters.Add(new SqlParameter("@Rating", directoryItem.Rating));
			command.Parameters.Add(new SqlParameter("@Status", (int)directoryItem.Status));
			command.Parameters.Add(new SqlParameter("@Submiter", directoryItem.Submiter.Uid));
            command.Parameters.Add(new SqlParameter("@Postcode", directoryItem.Postcode.Trim()));
			command.Parameters.Add(new SqlParameter("@Latitude", directoryItem.Latitude));
			command.Parameters.Add(new SqlParameter("@Longitude", directoryItem.Longitude));

			try
			{
				connection.Open();
                directoryItem.Id = Convert.ToInt64(command.ExecuteScalar());

				// one category association is always required.
				if (directoryItem.DirectoryCategories.Count > 0)
				{
					// persist the item categories.
					command.CommandText = "FlushDirectoryItemCategories";
					command.Parameters.Clear();
					command.Parameters.Add(idParam);
					command.ExecuteNonQuery();

				    command.CommandText = "CreateDirectoryItemCategoryRelation";
				    command.Parameters.Clear();
				    command.Parameters.Add(idParam);

				    var catParam = new SqlParameter("@CategoryID", SqlDbType.BigInt);
				    command.Parameters.Add(catParam);

				    foreach (DirectoryCategory category in directoryItem.DirectoryCategories)
				    {
					    // add the item to the category, as the category may be cached already.
					    category.DirectoryItems.Add(directoryItem);
					    catParam.Value = category.Id;
					    command.ExecuteNonQuery();
				    }
                }

				if (!directoryItem.IsPersisted)
				{
					// add the item to the caches.
                    CacheManager.AddItem(directoryItem, CacheManager.GetApplicationUniqueId(typeof(DirectoryItem), directoryItem.Id));
					directoryItem.IsPersisted = true;
					_latestCacheItems.RefreshItems();
				}

				// reset for future use.
				directoryItem.HasChanged = false;
			}
			finally
			{
				connection.Close();
			}
		}
        
		/// <summary>
		/// Retrieves a Directory Item from the persistence store.
		/// </summary>
		public DirectoryItem GetItem(long id) 
		{
			if (id < 1)
				return null;

			// attempt to retrieve the Item for the cache.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(DirectoryItem), id);
            var item = CacheManager.RetrieveItem(cacheId) as DirectoryItem;
			if (item == null)
			{
				// retrieve the item from the database.
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetDirectoryItem", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@ID", id));
				SqlDataReader reader = null;

				try
				{
					connection.Open();
					reader = command.ExecuteReader();

			        if (reader.Read())
			        {
			            item = new DirectoryItem(ObjectCreationMode.Retrieve)
                        {
                            ObjectMode = ObjectMode.Populating,
                            Id = id,
                            Title = Sql.GetValue(typeof (string), reader["Title"]) as string,
                            Description = Sql.GetValue(typeof (string), reader["Description"]) as string,
                            TelephoneNumber = Sql.GetValue(typeof (string), reader["TelephoneNumber"]) as string,
                            Created = (DateTime) Sql.GetValue(typeof (DateTime), reader["Created"]),
                            Updated = (DateTime) Sql.GetValue(typeof (DateTime), reader["Updated"]),
                            Submiter = Server.Instance.UserServer.GetUser((Guid) reader["Submiter"]),
                            Status = (DirectoryStatus) (byte) reader["Status"],
                            RatingSum = (long) Sql.GetValue(typeof (long), reader["Rating"]),
                            NumberOfRatings = (int) Sql.GetValue(typeof (int), reader["NumberOfRatings"]),
                            Postcode = Sql.GetValue(typeof (string), reader["Postcode"]) as string,
                            Longitude = (double) Sql.GetValue(typeof (double), reader["Longitude"]),
                            Latitude = (double) Sql.GetValue(typeof (double), reader["Latitude"])
                        };

			            // to retire.
			            Helpers.DelimitedStringToCollection(item.Images, Sql.GetValue(typeof(string), reader["Images"]) as string);
			            Helpers.DelimitedStringToCollection(item.Links, Sql.GetValue(typeof(string), reader["Links"]) as string);
			            Helpers.DelimitedStringToCollection(item.Keywords, Sql.GetValue(typeof(string), reader["Keywords"]) as string);

			            // essential to un-pause regular object processing.
			            item.ObjectMode = ObjectMode.Normal;
			        }

				    // add the item to the cache.
                    CacheManager.AddItem(item, cacheId);
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}

			return item;
		}
        
		/// <summary>
		/// Persists any changes to a Directory Category object.
		/// </summary>
		public bool UpdateCategory(DirectoryCategory directoryCategory) 
		{
			if (string.IsNullOrEmpty(directoryCategory.Name))
				throw new Exception("No name supplied!");

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UpdateDirectoryCategory", connection);
			var idParam = new SqlParameter("@ID", directoryCategory.Id);
			SqlTransaction transaction = null;

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(idParam);
			command.Parameters.Add(new SqlParameter("@Name", directoryCategory.Name));
			command.Parameters.Add(new SqlParameter("@Description", directoryCategory.Description));
			command.Parameters.Add(new SqlParameter("@RequiresMembership", directoryCategory.RequiresMembership));
			command.Parameters.Add(new SqlParameter("@Keywords", Helpers.CollectionToDelimitedString(directoryCategory.Keywords)));

			object parentValue;
			if (directoryCategory.ParentDirectoryCategory != null)
			{
				parentValue = directoryCategory.ParentDirectoryCategory.Id;

				// add the category to the parent category sub-cats collection to keep any cached
				// categories up to date.
				if (!directoryCategory.ParentDirectoryCategory.SubDirectoryCategories.Contains(directoryCategory))
					directoryCategory.ParentDirectoryCategory.SubDirectoryCategories.Add(directoryCategory);
			}
			else
			{
				parentValue = DBNull.Value;
			}

			command.Parameters.Add(new SqlParameter("@ParentCategoryID", parentValue));

			try
			{
				connection.Open();
				transaction = connection.BeginTransaction();
				command.Transaction = transaction;
                directoryCategory.Id = Convert.ToInt64(command.ExecuteScalar());
				
				if (directoryCategory.HasChanged)
				{
					// remove the item/category associations first.
					command.Parameters.Clear();
					command.Parameters.Add(idParam);
					command.CommandText = "FlushDirectoryCategoryItems";
					command.ExecuteNonQuery();
                    
					// persist the items.
					idParam.ParameterName = "@CategoryID";
					command.CommandText = "InsertDirectoryCategoryItem";
					var itemIdParam = new SqlParameter("@ItemID", SqlDbType.BigInt);
					command.Parameters.Add(itemIdParam);

					foreach (DirectoryItem item in directoryCategory.DirectoryItems)
					{
						itemIdParam.Value = item.Id;
						command.ExecuteNonQuery();
					}

					// persist the categories.
					command.CommandText = "FlushCategorySubCategories";
					command.Parameters.Clear();
					command.Parameters.Add(idParam);
					command.ExecuteNonQuery();
					
					command.CommandText = "InsertDirectorySubCategory";
					var subCatIdParam = new SqlParameter("@SubCategoryID", SqlDbType.BigInt);
					command.Parameters.Add(subCatIdParam);

					foreach (DirectoryCategory subCategory in directoryCategory.SubDirectoryCategories)
					{
						subCatIdParam.Value = subCategory.Id;
						command.ExecuteNonQuery();
					}

					if (!directoryCategory.IsPersisted)
					{
						// add the category to the cache.
						directoryCategory.IsPersisted = true;
                        CacheManager.AddItem(directoryCategory, CacheManager.GetApplicationUniqueId(typeof(DirectoryCategory), directoryCategory.Id));
					}

					// reset for future use.
					directoryCategory.HasChanged = false;
				}

				transaction.Commit();
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex, "DirectoryServer.UpdateCategory()");
			    if (transaction != null) transaction.Rollback();
			    return false;
			}
			finally
			{
				connection.Close();
			}
		}
        
		/// <summary>
		/// Retrieves a Directory Category from the persistence store.
		/// </summary>
		public DirectoryCategory GetCategory(long id) 
		{
			if (id < 1)
				return null;

			// attempt to retrieve the Item for the cache.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(DirectoryCategory), id);
            var category = CacheManager.RetrieveItem(cacheId) as DirectoryCategory;

			if (category == null)
			{
				// retrieve the Category from the database.
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetDirectoryCategory", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@ID", id));
				SqlDataReader reader = null;

				try
				{
					connection.Open();
					reader = command.ExecuteReader();

			        if (reader.Read())
			        {
			            category = new DirectoryCategory(ObjectCreationMode.Retrieve)
                        {
                            Id = id,
                            Name = Sql.GetValue(typeof (string), reader["Name"]) as string,
                            Description = Sql.GetValue(typeof (string), reader["Description"]) as string,
                            RequiresMembership = (bool) Sql.GetValue(typeof (bool), reader["RequiresMembership"])
                        };

			            Helpers.DelimitedStringToCollection(category.Keywords, Sql.GetValue(typeof(string), reader["Keywords"]) as string);
			        }

				    // add the category to the cache.
                    CacheManager.AddItem(category, cacheId);
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
		/// Permanently removes a Directory Item from the system.
		/// </summary>
		public void DeleteItem(DirectoryItem directoryItem) 
		{
			// the item must be removed from all in-memory
            lock (directoryItem.DirectoryCategories)
            {
                foreach (DirectoryCategory parent in directoryItem.DirectoryCategories)
                    parent.DirectoryItems.Remove(directoryItem);
            }

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteDirectoryItem", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ID", directoryItem.Id));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			finally
			{
				connection.Close();
			}

			// remove the item from the cache.
            CacheManager.RemoveItem(CacheManager.GetApplicationUniqueId(typeof(DirectoryItem), directoryItem.Id));
		}

		/// <summary>
		/// Permanently removes a Directory Category from the system. All contained Items will be
		/// orphaned and must be managed subsequently.
		/// </summary>
		public void DeleteCategory(DirectoryCategory directoryCategory) 
		{
			// remove from parent category.
			if (directoryCategory.ParentDirectoryCategory != null)
                lock (directoryCategory.ParentDirectoryCategory.SubDirectoryCategories)
				    directoryCategory.ParentDirectoryCategory.SubDirectoryCategories.Remove(directoryCategory);

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteDirectoryCategory", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ID", directoryCategory.Id));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			finally
			{
				connection.Close();
			}

			// remove the category from the cache.
            CacheManager.RemoveItem(CacheManager.GetApplicationUniqueId(typeof(DirectoryCategory), directoryCategory.Id));
		}

		/// <summary>
		/// Returns a new ItemFinder object.
		/// </summary>
		public ItemFinder NewItemFinder() 
		{
			return new ItemFinder();
		} 

		/// <summary>
		/// Returns a new CategoryFinder object.
		/// </summary>
		public CategoryFinder NewCategoryFinder() 
		{
			return new CategoryFinder();
		}

		/// <summary>
		/// Returns a set number of the latest directory Item objects.
		/// </summary>
		public DirectoryItemCollection GetLatestItems(int itemsToRetrieve) 
		{
            return _latestCacheItems.RetrieveItems(itemsToRetrieve) as DirectoryItemCollection;
		}
		#endregion
	}
}