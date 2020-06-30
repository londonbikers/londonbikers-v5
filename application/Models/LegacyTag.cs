using System;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// A tag represents a keyword, a way of linking together different domain objects by context. Tags are objects that can be
	/// created and thrown away easily, they can be customised to contain domain objects for different contexts, i.e. for a 
	/// section, for a global search or just ad-hoc.
	/// </summary>
	public class LegacyTag : ILegacyTag
	{
		#region members
	    private List<Document> _documents;
	    #endregion

		#region accessors
	    /// <summary>
	    /// The name of the tag.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
		/// Generates a unique ID for the Tag within the context of the application.
		/// </summary>
		public string ApplicationUniqueId { get { return GetApplicationUniqueId(); } }

		/// <summary>
		/// Transient use only: Allows Documents to be stored against the tag.
		/// </summary>
		public List<Document> Documents 
		{
			get { return _documents ?? (_documents = new List<Document>()); }
		}

	    /// <summary>
	    /// Transient use only: Denotes how many documents are using this tag, for the specific context this tag was instantiated for.
	    /// </summary>
	    public int DocumentCount { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Tag object.
		/// </summary>
		internal LegacyTag() 
		{
		}

	    /// <summary>
	    /// Creates a new Tag that contains Documents for a specific Section.
	    /// </summary>
	    /// <param name="name">The name of the Tag. I.e. 'motogp'.</param>
        internal LegacyTag(string name) 
		{
			Name = name;
		}
		#endregion

		#region static methods
	    /// <summary>
	    /// Builds a collection of Tags from a comma-delimited string.
	    /// </summary>
	    /// <param name="delimitedTags">Tags in a comma-seperated format, i.e. 'honda, suzuki, yamaha'.</param>
	    internal static TagCollection DelimitedTagsToCollection(string delimitedTags) 
		{
			var tags = new TagCollection();
			if (delimitedTags == String.Empty)
				return tags;

			Tag tag;
	        var rawTags = delimitedTags.ToLower().Split(char.Parse(","));

			foreach (var t in rawTags.Where(t => t.Trim() != string.Empty))
			{
			    tag = new Tag();
			    var rawTag = t.Trim();
			    tag.Name = rawTag;
			    tags.Add(tag);
			}

			return tags;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Gets a unique ID for this Tag, within the context of a specific Section.
		/// </summary>
		/// <param name="section">The Section to associate with the Tag.</param>
		public string GetApplicationUniqueIdForSection(Section section) 
		{
			return string.Format("{0}:{1}:{2}", GetType().FullName, section.Id, Name);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Generates the ApplicationUniqueID, depending on the context of use.
		/// </summary>
		private string GetApplicationUniqueId() 
		{
			return string.Format("{0}:{1}", GetType().FullName, Name);
		}
		#endregion
	}
}