using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ILatestDocuments
    {
        int Count { get; }

        /// <summary>
        /// Returns a collection of the latest items for this content type.
        /// </summary>
        /// <param name="documentsToRetrieve">The number of documents to retrieve.</param>
        List<Document> RetrieveDocuments(int documentsToRetrieve);

        /// <summary>
        /// When an item is added or removed from the system, this method will allow the latest items
        /// list to be rebuilt with the most up to date items.
        /// </summary>
        void RefreshDocuments();
    }
}