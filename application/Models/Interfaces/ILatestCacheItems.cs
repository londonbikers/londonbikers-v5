using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ILatestCacheItems
    {
        /// <summary>
        /// Returns a collection of the latest items for this content type.
        /// </summary>
        /// <param name="itemsToRetrieve">The number of items to retrieve.</param>
        object RetrieveItems(int itemsToRetrieve);

        /// <summary>
        /// When an item is added or removed from the system, this method will allow the latest items
        /// list to be rebuilt with the most up to date items.
        /// </summary>
        void RefreshItems();

        /// <summary>
        /// Returns a typed collection of Documents from the list.
        /// </summary>
        List<Document> RetrieveEditorialDocuments(IEnumerable<long> idList);

        /// <summary>
        /// Returns a typed collection of Galleries from the list.
        /// </summary>
        List<Gallery> RetrieveGalleries(IEnumerable<long> idList);

        /// <summary>
        /// When the class is first instantiated, the list should be populated.
        /// </summary>
        void PopulateItems();
    }
}