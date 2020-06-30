using System.Collections.Generic;
using System.Linq;
using Apollo.Caching;
using Apollo.Content;
using Apollo.Galleries;
using Apollo.Directory;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Provides simple access to the latest items for various application objects, lestening the database
	/// query load when looking for the newest items.
	/// </summary>
	public class LatestCacheItems : ILatestCacheItems
	{
		#region members
		private readonly ApplicationContentType _contentType;
		private List<long> _ids;
		private readonly int _size;
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new LatestCacheItems object.
		/// </summary>
		/// <param name="contentType">Lets the class know what type of content we're dealing with.</param>
		/// <param name="size">Determines how many items should be tracked.</param>
		internal LatestCacheItems(ApplicationContentType contentType, int size) 
		{
			_size = size;
			_contentType = contentType;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of the latest items for this content type.
		/// </summary>
		/// <param name="itemsToRetrieve">The number of items to retrieve.</param>
		public object RetrieveItems(int itemsToRetrieve) 
		{
            if (_ids == null)
				RefreshItems();

            if (_ids == null)
                return null;

            if (itemsToRetrieve > _ids.Count)
                itemsToRetrieve = _ids.Count;

			// select the items to retrieve.
			var pulledIDs = new List<long>();
			for (var i = 0; i < itemsToRetrieve; i++)
                pulledIDs.Add(_ids[i]);

			// retrieve the items in the correct format.
            switch (_contentType)
            {
                case ApplicationContentType.EditorialArticle:
                case ApplicationContentType.EditorialStory:
                    return RetrieveEditorialDocuments(pulledIDs);
                case ApplicationContentType.DirectoryCategory:
                    break;
                case ApplicationContentType.DirectoryItem:
                    return RetrieveDirectoryItems(pulledIDs);
                case ApplicationContentType.GalleryCategory:
                    break;
                case ApplicationContentType.GalleryGallery:
                    return RetrieveGalleries(pulledIDs);
            }

			return null;
		}
        
		/// <summary>
		/// When an item is added or removed from the system, this method will allow the latest items
		/// list to be rebuilt with the most up to date items.
		/// </summary>
		public void RefreshItems() 
		{
			PopulateItems();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Returns a typed collection of Items from the list.
		/// </summary>
		private static DirectoryItemCollection RetrieveDirectoryItems(IEnumerable<long> idList) 
		{
		    var items = new DirectoryItemCollection();
			foreach (var item in idList.Select(id => CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof (DirectoryItem), id)) as DirectoryItem ?? Server.Instance.DirectoryServer.GetItem(id)))
			    items.Add(item, false);

			return items;
		}

		/// <summary>
		/// Returns a typed collection of Documents from the list.
		/// </summary>
		public List<Document> RetrieveEditorialDocuments(IEnumerable<long> idList) 
		{
		    return idList.Select(id => CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof (Document), id)) as Document ?? Server.Instance.ContentServer.GetDocument(id)).ToList();
		}

		/// <summary>
		/// Returns a typed collection of Galleries from the list.
		/// </summary>
		public List<Gallery> RetrieveGalleries(IEnumerable<long> idList) 
		{
		    return idList.Select(id => CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof (Gallery), id)) as Gallery ?? Server.Instance.GalleryServer.GetGallery(id)).ToList();
		}

		/// <summary>
		/// When the class is first instantiated, the list should be populated.
		/// </summary>
		public void PopulateItems()
		{
		    switch (_contentType)
		    {
		        case ApplicationContentType.EditorialArticle:
		            {
		                var finder = new DocumentFinder();
		                finder.FindValue(((int)DocumentType.Article).ToString(), "Type");
		                finder.FindValue("Published", "Status");
		                finder.OrderBy("Created", FinderOrder.Desc);
		                _ids = finder.Find(_size);
		            }
		            break;
		        case ApplicationContentType.EditorialStory:
		            {
		                var finder = new DocumentFinder();
		                finder.FindValue(((int)DocumentType.News).ToString(), "Type");
		                finder.FindValue("Published", "Status");
		                finder.OrderBy("Created", FinderOrder.Desc);
		                _ids = finder.Find(_size);
		            }
		            break;
		        case ApplicationContentType.GalleryGallery:
		            {
		                var finder = new GalleryFinder();
		                finder.FindValue(((int)GalleryStatus.Published).ToString(), "Status");
		                finder.OrderBy("CreationDate", FinderOrder.Desc);
		                _ids = finder.Find(_size);
		            }
		            break;
		        case ApplicationContentType.DirectoryItem:
		            {
		                var finder = new ItemFinder();
		                finder.FindValue(((int)DirectoryStatus.Active).ToString(), "Status");
		                finder.OrderBy("Created", FinderOrder.Desc);
		                _ids = finder.Find(_size);
		            }
		            break;
		    }
		}
	    #endregion
	}
}