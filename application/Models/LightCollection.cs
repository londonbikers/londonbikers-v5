using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
    /// <summary>
    /// Represents a collection of ICommonBase objects.
    /// </summary>
    public class LightCollection<T> : IEnumerable<T>, ILightCollection<T> where T : class, ICommonBase
	{
		#region members
		private readonly List<long> _list;
		private readonly bool _validateActions;
		#endregion

		#region accessors
		/// <summary>
		/// Returns the number of items in the collection.
		/// </summary>
		public int Count { get { return _list.Count; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new LightCollection object.
		/// </summary>
		internal LightCollection() 
		{
			_list = new List<long>();
			_validateActions = false;
		}
        
		/// <summary>
		/// Creates a new LightCollection object.
		/// </summary>
		/// <param name="validateActions">
		/// Determines whether or not all additions or removals are completed. Potentially slower for large collections but of use to consumers
		/// that need to know if an item being added is a duplicate, for instance.
		/// </param>
		internal LightCollection(bool validateActions) 
		{
			_list = new List<long>();
			_validateActions = validateActions;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a item to the collection.
		/// </summary>
		public virtual bool Add(T item) 
		{
			if (item == null)
				throw new ArgumentNullException("item");

			return Add(item.Id);
		}
        
		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="itemId">The ID for the item being added.</param>
		public virtual bool Add(long itemId) 
		{
			if (itemId < 1)
				throw new ArgumentNullException("itemId");

			if (_validateActions)
			{
				if (_list.Any(id => id == itemId))
				    return false;
				
				_list.Add(itemId);
				return true;
			}

		    _list.Add(itemId);
		    return true;
		}
        
		/// <summary>
		/// Removes an item from the collection.
		/// </summary>
		public virtual bool Remove(long itemId) 
		{
			for (var i = 0; i < _list.Count; i++)
			{
			    if (_list[i] != itemId) continue;
			    _list.RemoveAt(i);
			    return true;
			}

			return false;
		}
        
		/// <summary>
		/// Removes an item from the collection.
		/// </summary>
		public virtual bool Remove(T item) 
		{
			return Remove(item.Id);
		}
        
		/// <summary>
		/// Determines whether or not the collection contains a specific item.
		/// </summary>
		public bool Contains(T item)
		{
		    return _list.Any(id => id == item.Id);
		}

        /// <summary>
        /// Retrieves a number of the first items from the collection.
        /// </summary>
        /// <param name="number">The number of items to retrieve. If more is requested than exist then the entire collection contents will be returned.</param>
        public List<T> Retrieve(int number)
        {
            var domainType = GetType().GetGenericArguments()[0];
            var list = new List<T>();
            var ceiling = number < _list.Count ? number : _list.Count;
            for (var i = 0; i < ceiling; i++)
            {
                if (domainType == typeof(IDocument))
                    list.Add(Server.Instance.ContentServer.GetDocument(_list[i]) as T);
                if (domainType == typeof(IGallery))
                    list.Add(Server.Instance.GalleryServer.GetGallery(_list[i]) as T);
            }

            return list;
        }

        /// <summary>
        /// Retrieves a list the top x amount of the raw content id's.
        /// </summary>
        public List<long> RetrieveRaw(int number)
        {
            return _list.Take(number).ToList();
        }

        /// <summary>
        /// Returns a just a specific page's  worth of the collection.
        /// </summary>
        /// <param name="page">The page to extract.</param>
        /// <param name="pageSize">The number of items per page.</param>
        public LightCollection<T> GetPage(int page = 1, int pageSize = 50)
        {
            var results = new LightCollection<T>();
            var end = pageSize * page;
            var start = end - pageSize;

            if (start > _list.Count)
            {
                start = 0;
                end = start + pageSize;
            }

            if (end > _list.Count -1)
                end = _list.Count - 1;

            for (var i = start; i < end; i++)
                results.Add(_list[i]);

            return results;
        }

        /// <summary>
        /// Public default indexer.
        /// </summary>
        public T this[int index] 
		{
			get 
			{
				if (index > _list.Count - 1)
					throw new IndexOutOfRangeException();

                // type specific retrieval.
			    var type = GetType();
			    var genericArgs = type.GetGenericArguments();

                var domainType = genericArgs[0];
                if (domainType == typeof(IDocument))
                    return Server.Instance.ContentServer.GetDocument(_list[index]) as T;
			    if (domainType == typeof(IGallery))
			        return Server.Instance.GalleryServer.GetGallery(_list[index]) as T;

			    throw new Exception("Unsupported domain type!");
			}
		}

		/// <summary>
		/// Clears the entire contents of the collection.
		/// </summary>
		internal void Clear() 
		{
			_list.Clear();
		}
		#endregion

        #region enumerators
        public IEnumerator<T> GetEnumerator()
        {
            return new LightCollectionEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}