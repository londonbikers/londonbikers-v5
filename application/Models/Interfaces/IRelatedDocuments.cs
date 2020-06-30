using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IRelatedDocuments
    {
        /// <summary>
        /// The number of related documents.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The related documents collection.
        /// </summary>
        List<Document> List { get; }

        /// <summary>
        /// The default indexer.
        /// </summary>
        Document this[int index] { get; }

        /// <summary>
        /// Associates another object with this collection.
        /// </summary>
        /// <param name="id">The numeric identifier for the object to associate with.</param>
        void Add(long id);

        /// <summary>
        /// Removes a related object from the collection.
        /// </summary>
        /// <param name="id">The numeric identifier for the object to unassociate.</param>
        void Remove(long id);
    }
}