using System.Collections.Generic;
using System.Linq;
using Apollo.Caching;
using Apollo.Content;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Provides simple access to the latest documents, lestening the database
	/// query load when looking for the newest documents.
	/// </summary>
	public class LatestDocuments : ILatestDocuments
	{
		#region members
		private List<long> _ids;
		private readonly int _size;
		private readonly long _sectionId;
		private readonly ITag _tag;
	    #endregion

        #region accessors
        public int Count 
        { 
            get
            {
                if (_ids == null)
                    PopulateDocuments();

                return _ids != null ? _ids.Count : 0;
            }
        }
        #endregion

        #region constructors
        /// <summary>
		/// Creates a new LatestDocuments object for a Section.
		/// </summary>
		/// <param name="size">Determines how many items should be tracked.</param>
		/// <param name="section">The Section for which documents are being tracked.</param>
		internal LatestDocuments(ICommonBase section, int size) 
		{
			_size = size;
			_sectionId = section.Id;
		}

		/// <summary>
		/// Creates a new Latest object for a specific Section and Tag.
		/// </summary>
		/// <param name="section">The Section for which documents are being tracked.</param>
		/// <param name="tag">The Tag which the documents must have.</param>
		/// <param name="size">Determines how many items should be tracked.</param>
		internal LatestDocuments(ISection section, ITag tag, int size) 
		{
			_size = size;
			_tag = tag;
			_sectionId = section.Id;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of the latest items for this content type.
		/// </summary>
		/// <param name="documentsToRetrieve">The number of documents to retrieve.</param>
		public List<Document> RetrieveDocuments(int documentsToRetrieve) 
		{
			if (_ids == null)
				PopulateDocuments();

            if (_ids == null)
                return null;

            if (documentsToRetrieve > _ids.Count)
                documentsToRetrieve = _ids.Count;

		    // select the items to retrieve.
			var pulledIDs = new List<long>();
			for (var i = 0; i < documentsToRetrieve; i++)
				pulledIDs.Add(_ids[i]);

			// retrieve the items in the correct format.
			return RetrieveDocumentsList(pulledIDs);
		}

		/// <summary>
		/// When an item is added or removed from the system, this method will allow the latest items
		/// list to be rebuilt with the most up to date items.
		/// </summary>
		public void RefreshDocuments() 
		{
			_ids = null;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Returns a typed collection of Documents from the list.
		/// </summary>
		private static List<Document> RetrieveDocumentsList(IEnumerable<long> idList) 
		{
		    return idList.Select(id => CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof (Document), id)) as Document ?? Server.Instance.ContentServer.GetDocument(id)).ToList();
		}

		/// <summary>
		/// When the class is first instantiated, the list should be populated.
		/// </summary>
		private void PopulateDocuments()
		{
		    if (_sectionId == 0) return;
		    _ids = _tag == null ? new DocumentFinder().GetPublishedSectionDocuments(_sectionId, _size) : new DocumentFinder().GetPublishedSectionDocumentsWithTag(_sectionId, _tag, _size);
		}
	    #endregion
	}
}