using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface IItemCollection
    {
        /// <summary>
        /// Returns the number of Items in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a Directory Item to the collection.
        /// </summary>
        bool Add(DirectoryItem directoryItem);

        /// <summary>
        /// Adds a Directory Item to the collection.
        /// </summary>
        /// <param name="directoryItem">The Item to add</param>
        /// <param name="sortCollection">Determines whether or not the collection should be sorted afterwards.</param>
        bool Add(DirectoryItem directoryItem, bool sortCollection);

        /// <summary>
        /// Removes a Directory Item from the collection.
        /// </summary>
        bool Remove(DirectoryItem directoryItem);

        /// <summary>
        /// Returns a specific Directory Item from the collection.
        /// </summary>
        DirectoryItem GetItem(long itemId);

        /// <summary>
        /// Determines whether or not the collection contains a specific Directory Item.
        /// </summary>
        bool Contains(DirectoryItem directoryItem);

        /// <summary>
        /// Public default indexer.
        /// </summary>
        DirectoryItem this[int index] { get; set; }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        IEnumerator GetEnumerator();
    }
}