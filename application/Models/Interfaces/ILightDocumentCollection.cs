using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface ILightDocumentCollection : IEnumerable
    {
        /// <summary>
        /// Returns the number of documents in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds a Document to the collection.
        /// </summary>
        bool Add(IDocument document);

        /// <summary>
        /// Adds a Document to the collection.
        /// </summary>
        /// <param name="documentId">The ID for the document being added.</param>
        bool Add(long documentId);

        /// <summary>
        /// Removes a Document from the collection.
        /// </summary>
        bool Remove(long documentId);

        /// <summary>
        /// Removes a Document from the collection.
        /// </summary>
        bool Remove(IDocument document);

        /// <summary>
        /// Determines whether or not the collection contains a specific Document.
        /// </summary>
        bool Contains(IDocument document);

        /// <summary>
        /// Public default indexer.
        /// </summary>
        IDocument this[int index] { get; }
    }
}