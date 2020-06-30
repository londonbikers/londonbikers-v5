using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a collection of Tag objects.
	/// </summary>
	public class TagCollection : IEnumerable, ITagCollection
	{
		#region members
		private readonly List<ITag> _list;
	    #endregion

		#region accessors
		/// <summary>
		/// Returns the number of tags in the collection.
		/// </summary>
		public int Count { get { return _list.Count; } }

	    /// <summary>
	    /// Provides access to the internal removed-tags list.
	    /// </summary>
	    internal List<ITag> RemovedTags { get; private set; }
        
        /// <summary>
        /// Provides access to the internal added-tags list.
        /// </summary>
        internal List<ITag> AddedTags { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new TagCollection object.
		/// </summary>
		internal TagCollection() 
		{
			_list = new List<ITag>();
			RemovedTags = new List<ITag>();
            AddedTags = new List<ITag>();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Tag to the collection.
		/// </summary>
		public bool Add(ITag tag) 
		{
			return Add(tag, false);
		}

		/// <summary>
		/// Adds a Tag to the collection and sort it afterwards, optionally.
		/// </summary>
		public bool Add(ITag tag, bool sortCollection)
		{
		    if (Contains(tag))
		        return false;

		    // if this tag was already removed but is being re-added then we need to remove that marking.
		    if (RemovedListContains(tag))
		        RemoveRemovedTag(tag);

		    _list.Add(tag);

            // keep a reference to this tag to help with persistence.
            AddedTags.Add(tag);

		    if (sortCollection)
		        _list.Sort();

		    return true;
		}

	    /// <summary>
		/// Removes a Tag from the collection.
		/// </summary>
		public bool Remove(ITag tag) 
		{
			for (var i = 0; i < _list.Count; i++)
			{
			    if (_list[i].Name != tag.Name) continue;
			    // before removing, keep a reference to this tag to help with persistence.
			    RemovedTags.Add(_list[i]);
			    _list.RemoveAt(i);
			    return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a specific Tag from the collection.
		/// </summary>
		public ITag GetTag(string name)
		{
		    return _list.FirstOrDefault(tag => tag.Name == name);
		}

	    /// <summary>
		/// Determines whether or not the collection contains a specific Tag.
		/// </summary>
		public bool Contains(ITag tag)
	    {
	        return _list.Any(localTag => localTag.Name == tag.Name);
	    }

	    /// <summary>
		/// Removes all Tags from the collection.
		/// </summary>
		public void Clear() 
		{
			foreach (var tag in _list)
				RemovedTags.Add(tag);

			_list.Clear();
		}

		/// <summary>
		/// Public default indexer.
		/// </summary>
		public ITag this[int index] 
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

        /// <summary>
        /// Converts the tag collection to a comma-seperated string.
        /// </summary>
        public string ToCsv()
        {
            var tags = new StringBuilder();
            for (var i = 0; i < _list.Count; i++)
            {
                tags.Append(_list[i].Name);
                if (i < _list.Count - 1)
                    tags.Append(", ");
            }

            return tags.ToString();
        }
        #endregion

        #region internal methods
        /// <summary>
        /// Resets the collection after it's been persisted.
        /// </summary>
        internal void PersistenceReset() 
		{
			// clear all removed-tag references.
			RemovedTags.Clear();
            AddedTags.Clear();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Determines whether or not the removed-tags collection contains a specific instance.
		/// </summary>
		private bool RemovedListContains(ITag tag)
		{
		    return RemovedTags.Any(localTag => localTag.Name == tag.Name);
		}

	    /// <summary>
		/// Removes a specific Tag from the removed-tags collection.
		/// </summary>
		private void RemoveRemovedTag(ITag tag) 
		{
			for (var i = 0; i < RemovedTags.Count; i++)
			{
				if (RemovedTags[i].Name == tag.Name)
					RemovedTags.RemoveAt(i);
			}
		}
		#endregion
	}
}