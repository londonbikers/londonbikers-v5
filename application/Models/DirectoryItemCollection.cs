using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a collection of Directory Item objects.
	/// </summary>
	public class DirectoryItemCollection : IEnumerable, IItemCollection
	{
		#region members
		private List<DirectoryItem> _list;
		#endregion

		#region accessors
		/// <summary>
		/// Returns the number of Items in the collection.
		/// </summary>
		public int Count { get { return _list.Count; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new ItemCollection object.
		/// </summary>
		internal DirectoryItemCollection() 
		{
			_list = new List<DirectoryItem>();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Directory Item to the collection.
		/// </summary>
		public bool Add(DirectoryItem directoryItem) 
		{
			return Add(directoryItem, true);
		}
        
		/// <summary>
		/// Adds a Directory Item to the collection.
		/// </summary>
		/// <param name="directoryItem">The Item to add</param>
		/// <param name="sortCollection">Determines whether or not the collection should be sorted afterwards.</param>
		public bool Add(DirectoryItem directoryItem, bool sortCollection)
		{
		    if (Contains(directoryItem))
		        return false;

		    _list.Add(directoryItem);
		    if (sortCollection)
		        _list.Sort();

		    return true;
		}

	    /// <summary>
		/// Removes a Directory Item from the collection.
		/// </summary>
		public bool Remove(DirectoryItem directoryItem) 
		{
            lock (_list)
            {
                for (var i = 0; i < _list.Count; i++)
                {
                    if (_list[i].Id != directoryItem.Id) continue;
                    _list.RemoveAt(i);
                    return true;
                }
            }

			return false;
		}
        
		/// <summary>
		/// Returns a specific Directory Item from the collection.
		/// </summary>
		public DirectoryItem GetItem(long itemId) 
		{
            lock (_list)
            {
                foreach (var item in _list.Where(item => item.Id == itemId))
                    return item;
            }

			return null;
		}
        
		/// <summary>
		/// Determines whether or not the collection contains a specific Directory Item.
		/// </summary>
		public bool Contains(DirectoryItem directoryItem) 
		{
			if (_list == null)
			{
				_list = new List<DirectoryItem>();
				return false;
			}

		    return _list.Any(localItem => localItem.Id == directoryItem.Id);
		}
        
		/// <summary>
		/// Public default indexer.
		/// </summary>
		public DirectoryItem this[int index] 
		{
			get 
			{
				if (index > _list.Count - 1)				
					throw new System.IndexOutOfRangeException();

				return _list[index];
			}
			set 
			{
				_list[index] = value;
			}
		}
        
		/// <summary>
		/// Returns an enumerator for the collection.
		/// </summary>
		public IEnumerator GetEnumerator() 
		{
			return _list.GetEnumerator();
		}
		#endregion
	}
}