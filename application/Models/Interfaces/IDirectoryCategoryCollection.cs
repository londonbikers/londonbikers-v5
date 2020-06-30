using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface IDirectoryCategoryCollection
    {
        /// <summary>
        /// Returns the number of Categories in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a Directory Category to the collection.
        /// </summary>
        bool Add(DirectoryCategory directoryCategory);

        /// <summary>
        /// Adds a Directory Category to the collection.
        /// </summary>
        /// <param name="directoryCategory">The Category to add.</param>
        /// <param name="sortCollection">Determines whether or not the collection is sorted afterwards.</param>
        bool Add(DirectoryCategory directoryCategory, bool sortCollection);

        /// <summary>
        /// Removes a Directory Category from the collection.
        /// </summary>
        bool Remove(DirectoryCategory directoryCategory);

        /// <summary>
        /// Removes a Directory Category from the collection.
        /// </summary>
        bool Remove(long categoryId);

        /// <summary>
        /// Returns a specific Directory Category from the collection.
        /// </summary>
        DirectoryCategory GetCategory(long categoryId);

        /// <summary>
        /// Determines whether or not the collection contains a specific Directory Category.
        /// </summary>
        bool Contains(DirectoryCategory directoryCategory);

        /// <summary>
        /// Retrieves a specific Category object from the collection by its index position.
        /// </summary>
        DirectoryCategory this[int index] { get; set; }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        IEnumerator GetEnumerator();
    }
}