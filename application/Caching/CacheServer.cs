using System.Collections.Generic;
using Apollo.Models;

namespace Apollo.Caching
{
	/// <summary>
	/// Provides access to CacheManager functionality.
	/// </summary>
	public class CacheServer
	{
		#region accessors
		/// <summary>
		/// The number of items currently being cached.
		/// </summary>
		public int ItemsCached { get { return CacheManager.ItemCount; } }

        /// <summary>
        /// Provides direct access to the cache items, this should only be used for lower-level processes. Use the AddItem/RemoveItem methods ordinarily.
        /// </summary>
        internal Dictionary<string, CacheItem> Items { get { return CacheManager.Items; } }
		#endregion

		#region constructors
		/// <summary>
		/// Returns a new CacheServer object.
		/// </summary>
		internal CacheServer() 
		{
		}
		#endregion

		#region public methods
		/// <summary>
		/// Clears all items from the Cache, causing subsequent object requests to rebuild from fresh.
		/// </summary>
		public void FlushCache() 
		{
			CacheManager.FlushCache();
		}

		/// <summary>
		/// Retrieves the top X amount of most popular CacheItems.
		/// </summary>
		/// <param name="count">The number of items to retrieve.</param>
		public List<CacheItem> RetrieveMostPopularItems(int count) 
		{
			return CacheManager.RetrieveTopItems(count);
		}
		#endregion
	}
}
