using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Acts as a container-class for CacheManager items.
	/// </summary>
	public class CacheItem : ICacheItem
	{
		#region accessors
	    /// <summary>
	    /// When the item was first cached, i.e. it's lifespan.
	    /// </summary>
	    public DateTime Created { get; set; }

	    /// <summary>
	    /// The number of times this object has been requested from the cache.
	    /// </summary>
	    public int RequestCount { get; set; }

	    /// <summary>
	    /// The actual object that is being cached.
	    /// </summary>
	    public object Item { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Createa a new CacheItem object.
		/// </summary>
		internal CacheItem()
		{
			RequestCount = 0;
			Created = DateTime.Now;
		}
		#endregion
	}
}
