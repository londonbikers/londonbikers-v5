using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface IFeaturedDocumentsCollection
    {
        /// <summary>
        /// Adds a Document to the featured documents collection.
        /// </summary>
        bool Add(long documentId);

        /// <summary>
        /// Adds a Document to the featured documents collection.
        /// </summary>
        bool Add(IDocument document);

        /// <summary>
        /// Removes a Document from the featured documents collection.
        /// </summary>
        bool Remove(IDocument document);

        /// <summary>
        /// Removes a Document from the featured documents collection.
        /// </summary>
        bool Remove(long documentId);

        /// <summary>
        /// Gets the latest featured documents from the database.
        /// </summary>
        void Reload();

        /// <summary>
        /// Returns the number of documents in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Determines whether or not the collection contains a specific Document.
        /// </summary>
        bool Contains(IDocument document);

        /// <summary>
        /// Public default indexer.
        /// </summary>
        IDocument this[int index] { get; }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
        IEnumerator GetEnumerator();
    }
}