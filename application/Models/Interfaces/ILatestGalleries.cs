using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ILatestGalleries
    {
        /// <summary>
        /// Returns a collection of the latest items for this content type.
        /// </summary>
        /// <param name="galleriesToRetrieve">The number of galleries to retrieve.</param>
        List<Gallery> RetrieveGalleries(int galleriesToRetrieve);
    }
}