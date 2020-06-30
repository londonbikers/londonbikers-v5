using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Caching
{
	/// <summary>
	/// Provides functionality to cache domain objects in memory.
	/// </summary>
	internal class CacheManager
	{
		#region members
		private static int _itemCeiling;
        private static readonly Dictionary<string, CacheItem> Cache;
		#endregion

		#region accessors
		/// <summary>
		/// Controls the maximum number of items that should be kept in the cache at any one time.
		/// </summary>
		public static int ItemCeiling { get { return _itemCeiling; } set { _itemCeiling = value; } }
		/// <summary>
		/// The number of items currently in the Cache.
		/// </summary>
		public static int ItemCount { get { return Cache.Count; } }

        /// <summary>
        /// Provides direct access to the cache items, this should only be used for lower-level processes. Use the AddItem/RemoveItem methods ordinarily.
        /// </summary>
        internal static Dictionary<string, CacheItem> Items { get { return Cache; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new CacheManager object.
		/// </summary>
		static CacheManager() 
		{
			// set a default ceiling.
			_itemCeiling = 1000;

			if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Apollo.Caching.ItemCeiling"]))
				_itemCeiling = int.Parse(ConfigurationManager.AppSettings["Apollo.Caching.ItemCeiling"]);

            Cache = new Dictionary<string, CacheItem>();
        }
		#endregion

		#region public methods
		/// <summary>
		/// Attempts to retrieve a gallery image from a cached gallery.
		/// </summary>
		/// <remarks>Far from ideal. Candidate for refactor.</remarks>
		internal static IGalleryImage RetrieveGalleryImage(long id)
		{
            lock (Cache)
            {
                foreach (var gi in from entry in Cache
                    where (entry.Value.Item is Gallery)
                    from gi in ((Gallery) entry.Value.Item).Photos.Where(gi => gi.Id == id)
                    select gi)
                {
                    return gi;
                }
            }
            
            return null;
		}

		/// <summary>
		/// Adds a new object to the cache.
		/// </summary>
		/// <param name="item">The actual object to store in the cache.</param>
		/// <param name="key">The unique-identifier for the object.</param>
		public static void AddItem(object item, string key)
		{
            // does the object already exist in the cache?
            lock (Cache)
            {
                if (Cache.ContainsKey(key))
                    return;

                var cacheItem = new CacheItem {Item = item};

                // is the cache full?
                if (Cache.Count == _itemCeiling)
                    RemoveUnpopularItem();

                Cache.Add(key, cacheItem);
            }
		}

	    /// <summary>
		/// Removes an object from the cache.
		/// </summary>
		/// <param name="key">The unique-identifier for the item to be removed.</param>
		public static void RemoveItem(object key) 
		{
            lock (Cache)
                Cache.Remove(key.ToString());
		}

		/// <summary>
		/// Collects an object that has been cached previously. Will return null if no such item found.
		/// </summary>
		/// <param name="key">The unique-identifier for the item to be found.</param>
		public static object RetrieveItem(object key) 
		{
            lock (Cache)
            {
                CacheItem item;
                Cache.TryGetValue(key.ToString(), out item);
                if (item == null)
                    return null;

                item.RequestCount++;
                return item.Item;
            }
		}

		/// <summary>
		/// Retrieve the date when a cached item was first cached.
		/// </summary>
		/// <param name="key">The unique-identifier for the item to be found.</param>
		/// <returns>A DateTime for a cached object, if no such cached object was found then DateTime.Min is returned.</returns>
		public static DateTime RetrieveItemCreationDate(object key) 
		{
            lock (Cache)
            {
                CacheItem item;
                Cache.TryGetValue(key.ToString(), out item);
                return item != null ? item.Created : DateTime.MinValue;
            }
		}
		
		/// <summary>
		/// Empties the cache of all items.
		/// </summary>
		public static void FlushCache() 
		{
            lock (Cache)
                Cache.Clear();
		}
		
		/// <summary>
		/// Retrieves the top X amount of most popular CacheItems.
		/// </summary>
		/// <param name="count">The number of items to retrieve.</param>
		public static List<CacheItem> RetrieveTopItems(int count) 
		{
			if (count > Cache.Count)
				count = Cache.Count;

            var items = new List<CacheItem>();
			CacheItem item;
			CacheItem topItem = null;

		    lock (Cache)
			{
				// manually sort the popular items out.
				for (var i = 0; i < count; i++)
				{
					var topCount = -1;

					// retrieve most popular item.
					foreach (var entry in Cache)
					{
					    item = entry.Value;
					    if (item == null || item.RequestCount <= topCount) continue;

					    // don't add any previous top items.
					    if (items.Contains(item)) continue;
					    topItem = item;
					    topCount = item.RequestCount;
					}

				    items.Add(topItem);
				}
			}

			return items;
		}

		/// <summary>
		/// Returns an application-unique identifier for a particular domain object.
		/// </summary>
		/// <param name="type">The type for the domain object.</param>
		/// <param name="id">The numeric ID for the domain object.</param>
		public static string GetApplicationUniqueId(Type type, long id) 
		{
			return string.Format("{0}:{1}", type.FullName, id);
		}

        /// <summary>
        /// Returns an application-unique identifier for a particular domain object.
        /// </summary>
        /// <param name="type">The type for the domain object.</param>
        /// <param name="id">The textual ID for the domain object.</param>
        public static string GetApplicationUniqueId(Type type, string id)
        {
            return string.Format("{0}:{1}", type.FullName, id);
        }
		#endregion

		#region private methods
		/// <summary>
		/// Removes the first unpopular item from the Cache to make room.
		/// </summary>
		private static void RemoveUnpopularItem() 
		{
            if (Cache.Count == 0)
                return;

			lock (Cache)
			{
                try
                {
                    var victim = Cache.SingleOrDefault(q => q.Value.RequestCount == Cache.Min(q2 => q2.Value.RequestCount));
                    Cache.Remove(victim.Key);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
			}
		}
		#endregion
	}
}