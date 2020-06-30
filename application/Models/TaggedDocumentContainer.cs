using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Provides access to Documents for a specific Section with specific Tags.
	/// </summary>
	public class TaggedDocumentContainer : ITaggedDocumentContainer
	{
		#region mebers
		private readonly ISection _section;
		private readonly ITag _tag;
		private readonly ILatestDocuments _latestDocuments;
		#endregion

		#region constructors
		/// <summary>
		/// Returns a new TaggedDocumentContainer object.
		/// </summary>
		public TaggedDocumentContainer(ISection section, ITag tag) 
		{
			_section = section;
			_tag = tag;
			_latestDocuments = new LatestDocuments(_section, _tag, 100);
		}
		#endregion

		#region accessors
		/// <summary>
		/// The Tag to customise the contents for.
		/// </summary>
		public ISection Section { get { return _section; } }
		/// <summary>
		/// The Tag to customise the contents for.
		/// </summary>
		public ITag Tag { get { return _tag; } }
		/// <summary>
		/// Provides access to the latest documents in the section with this tag.
		/// </summary>
		public ILatestDocuments LatestDocuments { get { return _latestDocuments; } }
		/// <summary>
		/// Generates a unique ID within the context of the application. Used for caching purposes primarily. Custom implementation.
		/// </summary>
		public string ApplicationUniqueId { get { return GetApplicationUniqueId(Section, Tag); } }
		#endregion

		#region static methods
		/// <summary>
		/// Returns an application unique ID for a particular Section/Tag combination.
		/// </summary>
		public static string GetApplicationUniqueId(ISection section, ITag tag) 
		{
			return String.Format("{0}:{1}:{2}",  typeof(TaggedDocumentContainer).FullName, section.Id, tag.Name);
		}
		#endregion
	}
}