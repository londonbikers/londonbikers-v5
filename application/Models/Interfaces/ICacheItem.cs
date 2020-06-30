using System;

namespace Apollo.Models.Interfaces
{
    public interface ICacheItem
    {
        /// <summary>
        /// When the item was first cached, i.e. it's lifespan.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// The number of times this object has been requested from the cache.
        /// </summary>
        int RequestCount { get; set; }

        /// <summary>
        /// The actual object that is being cached.
        /// </summary>
        object Item { get; set; }
    }
}