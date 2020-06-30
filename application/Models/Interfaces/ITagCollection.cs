using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface ITagCollection
    {
        /// <summary>
        /// Returns the number of tags in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a Tag to the collection.
        /// </summary>
        bool Add(ITag tag);

        /// <summary>
        /// Adds a Tag to the collection and sort it afterwards, optionally.
        /// </summary>
        bool Add(ITag tag, bool sortCollection);

        /// <summary>
        /// Removes a Tag from the collection.
        /// </summary>
        bool Remove(ITag tag);

        /// <summary>
        /// Returns a specific Tag from the collection.
        /// </summary>
        ITag GetTag(string name);

        /// <summary>
        /// Determines whether or not the collection contains a specific Tag.
        /// </summary>
        bool Contains(ITag tag);

        /// <summary>
        /// Removes all Tags from the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Public default indexer.
        /// </summary>
        ITag this[int index] { get; set; }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        IEnumerator GetEnumerator();

        /// <summary>
        /// Converts the tag collection to a comma-seperated string.
        /// </summary>
        string ToCsv();
    }
}