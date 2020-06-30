using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a collection of Directory Category objects.
	/// </summary>
	public class DirectoryCategoryCollection : IEnumerable, IDirectoryCategoryCollection
	{
		#region members
		private readonly DirectoryItem _consumingDirectoryItem;
		private readonly DirectoryCategory _consumingDirectoryCategory;
		private readonly List<DirectoryCategory> _list;
		#endregion

		#region accessors
		/// <summary>
		/// Returns the number of Categories in the collection.
		/// </summary>
		public int Count { get { return _list.Count; } }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new CategoryCollection object.
		/// </summary>
		internal DirectoryCategoryCollection() 
		{
            _list = new List<DirectoryCategory>();
		}
        
		/// <summary>
		/// If this collection is working with an Item, this constructor will allow for 
		/// proper management of the Categories added and removed.
		/// </summary>
		internal DirectoryCategoryCollection(DirectoryItem consumingDirectoryItem) 
		{
            _consumingDirectoryItem = consumingDirectoryItem;
            _list = new List<DirectoryCategory>();
		}
        
		/// <summary>
		/// If this collection is working with another Category, this constructor will allow for 
		/// proper management of the Categories added and removed.
		/// </summary>
		internal DirectoryCategoryCollection(DirectoryCategory consumingDirectoryCategory)
		{
			_consumingDirectoryCategory = consumingDirectoryCategory;
            _list = new List<DirectoryCategory>();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Directory Category to the collection.
		/// </summary>
		public bool Add(DirectoryCategory directoryCategory) 
		{
			return Add(directoryCategory, true);
		}
        
		/// <summary>
		/// Adds a Directory Category to the collection.
		/// </summary>
		/// <param name="directoryCategory">The Category to add.</param>
		/// <param name="sortCollection">Determines whether or not the collection is sorted afterwards.</param>
		public bool Add(DirectoryCategory directoryCategory, bool sortCollection)
		{
		    if (Contains(directoryCategory))
		        return false;
		    if (_consumingDirectoryCategory != null)
		        directoryCategory.ParentDirectoryCategory = _consumingDirectoryCategory;

		    _list.Add(directoryCategory);

		    if (sortCollection)
		        _list.Sort();

		    return true;
		}

	    /// <summary>
		/// Removes a Directory Category from the collection.
		/// </summary>
		public bool Remove(DirectoryCategory directoryCategory) 
		{
			return Remove(directoryCategory.Id);
		}
        
		/// <summary>
		/// Removes a Directory Category from the collection.
		/// </summary>
		public bool Remove(long categoryId) 
		{
			for (var i = 0; i < _list.Count; i++)
			{
			    if (_list[i].Id != categoryId) continue;
			    // remove any consuming item from the category as well, so that cached categories
			    // reflect the removal.

			    if (_consumingDirectoryCategory != null)
			        _list[i].ParentDirectoryCategory = null;

			    if (_consumingDirectoryItem != null)
			        _list[i].DirectoryItems.Remove(_consumingDirectoryItem);

			    _list.RemoveAt(i);
			    return true;
			}

			return false;
		}
        
		/// <summary>
		/// Returns a specific Directory Category from the collection.
		/// </summary>
		public DirectoryCategory GetCategory(long categoryId)
		{
		    return _list.FirstOrDefault(category => category.Id == categoryId);
		}

	    /// <summary>
		/// Determines whether or not the collection contains a specific Directory Category.
		/// </summary>
		public bool Contains(DirectoryCategory directoryCategory)
	    {
	        return _list.Any(localCategory => localCategory.Id == directoryCategory.Id);
	    }

	    /// <summary>
        /// Retrieves a specific Category object from the collection by its index position.
		/// </summary>
		public DirectoryCategory this[int index] 
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