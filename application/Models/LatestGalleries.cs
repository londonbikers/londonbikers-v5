using System.Collections.Generic;
using System.Linq;
using Apollo.Caching;
using Apollo.Galleries;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Provides simple access to the latest galleries for a site or channel
	/// lestening the database query load when looking for the newest galleries.
	/// </summary>
	public class LatestGalleries : ILatestGalleries
	{
		#region members
		private List<long> _ids;
	    private readonly Site _site;
		#endregion

		#region constructors

	    /// <summary>
	    /// Creates a new LatestGalleries object.
	    /// </summary>
	    /// <param name="site">The site to cache galleries for.</param>
	    internal LatestGalleries(Site site) 
		{
		    _site = site;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of the latest items for this content type.
		/// </summary>
		/// <param name="galleriesToRetrieve">The number of galleries to retrieve.</param>
		public List<Gallery> RetrieveGalleries(int galleriesToRetrieve) 
		{
			if (_ids == null)
				RefreshGalleries();

		    if (_ids == null)
                return null;

		    if (galleriesToRetrieve > _ids.Count)
		        galleriesToRetrieve = _ids.Count;

		    // select the items to retrieve.
			var pulledIDs = new List<long>();
			for (var i = 0; i < galleriesToRetrieve; i++)
				pulledIDs.Add(_ids[i]);

			// retrieve the items in the correct format.
			return RetrieveGalleriesList(pulledIDs);
		}


		/// <summary>
		/// When an item is added or removed from the system, this method will allow the latest items
		/// list to be rebuilt with the most up to date items.
		/// </summary>
		internal void RefreshGalleries() 
		{
            _ids = new GalleryFinder().GetLatestGalleryIDsForSite(_site.Id);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Returns a typed collection of Galleries from the list.
		/// </summary>
		private static List<Gallery> RetrieveGalleriesList(IEnumerable<long> idList) 
		{
		    return idList.Select(id => CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof (Gallery), id)) as Gallery ?? Server.Instance.GalleryServer.GetGallery(id)).ToList();
		}
		#endregion
	}
}