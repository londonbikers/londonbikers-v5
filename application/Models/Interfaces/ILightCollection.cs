using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ILightCollection<T> where T : class, ICommonBase
    {
        /// <summary>
        /// Returns the number of items in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a item to the collection.
        /// </summary>
        bool Add(T item);

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="itemId">The ID for the item being added.</param>
        bool Add(long itemId);

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        bool Remove(long itemId);

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        bool Remove(T item);

        /// <summary>
        /// Determines whether or not the collection contains a specific item.
        /// </summary>
        bool Contains(T item);

        /// <summary>
        /// Returns a number of objects from the collection.
        /// </summary>
        /// <param name="number">The number of objects to retrieve from the collection.</param>
        List<T> Retrieve(int number);

        List<long> RetrieveRaw(int number);

        LightCollection<T> GetPage(int page = 1, int pageSize = 50);

        /// <summary>
        /// Public default indexer.
        /// </summary>
        T this[int index] { get; }

        IEnumerator<T> GetEnumerator();
    }
}