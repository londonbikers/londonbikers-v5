using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a collection of Document objects.
	/// </summary>
	public class LightDocumentCollection : ILightDocumentCollection
	{
		#region members
		private readonly List<long> _list;
		private readonly bool _validateActions;
		#endregion

		#region accessors
		/// <summary>
		/// Returns the number of documents in the collection.
		/// </summary>
		public int Count { get { return _list.Count; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new LightDocumentCollection object.
		/// </summary>
		internal LightDocumentCollection() 
		{
			_list = new List<long>();
			_validateActions = false;
		}
        
		/// <summary>
		/// Creates a new LightDocumentCollection object.
		/// </summary>
		/// <param name="validateActions">
		/// Determines whether or not all additions or removals are completed. Potentially slower for large collections but of use to consumers
		/// that need to know if an item being added is a duplicate, for instance.
		/// </param>
		internal LightDocumentCollection(bool validateActions) 
		{
			_list = new List<long>();
			_validateActions = validateActions;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Document to the collection.
		/// </summary>
		public virtual bool Add(IDocument document) 
		{
			if (document == null)
				throw new ArgumentNullException("document");

			return Add(document.Id);
		}
        
		/// <summary>
		/// Adds a Document to the collection.
		/// </summary>
		/// <param name="documentId">The ID for the document being added.</param>
		public virtual bool Add(long documentId) 
		{
			if (documentId < 1)
				throw new ArgumentNullException("documentId");

			if (_validateActions)
			{
				if (_list.Any(id => id == documentId))
				    return false;
				
				_list.Add(documentId);
				return true;
			}

		    _list.Add(documentId);
		    return true;
		}
        
		/// <summary>
		/// Removes a Document from the collection.
		/// </summary>
		public virtual bool Remove(long documentId) 
		{
			for (var i = 0; i < _list.Count; i++)
			{
			    if (_list[i] != documentId) continue;
			    _list.RemoveAt(i);
			    return true;
			}

			return false;
		}
        
		/// <summary>
		/// Removes a Document from the collection.
		/// </summary>
		public virtual bool Remove(IDocument document) 
		{
			return Remove(document.Id);
		}
        
		/// <summary>
		/// Determines whether or not the collection contains a specific Document.
		/// </summary>
		public bool Contains(IDocument document)
		{
		    return _list.Any(id => id == document.Id);
		}

	    /// <summary>
		/// Public default indexer.
		/// </summary>
		public IDocument this[int index] 
		{
			get 
			{
				if (index > _list.Count - 1)				
					throw new IndexOutOfRangeException();

				return Server.Instance.ContentServer.GetDocument(_list[index]);
			}
		}

		/// <summary>
		/// Returns an enumerator for the collection.
		/// </summary>
		public IEnumerator GetEnumerator() 
		{
			return new LightDocumentCollectionEnumerator(this);
		}

		/// <summary>
		/// Clears the entire contents of the collection.
		/// </summary>
		internal void Clear() 
		{
			_list.Clear();
		}
		#endregion
	}
}