using Apollo.Models;
using Apollo.Models.Interfaces;

namespace Apollo.Assets
{
	/// <summary>
	/// The controller for the Asset module. All major Asset object operations are performed via 
	/// </summary>
	public class AssetServer
	{
		#region members
		private BlacklistedReferrersCollection _blacklistedReferrers;
		#endregion

		#region accessors
		/// <summary>
		/// Contains a list of URL's for sites that have been blacklisted and cannot hot-link media from 
		/// within the application.
		/// </summary>
		public BlacklistedReferrersCollection BlacklistedReferrers 
		{ 
			get { return _blacklistedReferrers ?? (_blacklistedReferrers = new BlacklistedReferrersCollection()); }
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new AssetServer object.
		/// </summary>
		internal AssetServer() 
		{
		}
		#endregion

		#region public methods
		public DomainObjectType GetDomainObjecType(ICommonBase domainObject)
		{
		    if (domainObject is Document)
				return DomainObjectType.Document;
		    if (domainObject is EditorialImage)
		        return DomainObjectType.DocumentImage;
		    if (domainObject is DirectoryCategory)
		        return DomainObjectType.DirectoryCategory;
		    if (domainObject is DirectoryItem)
		        return DomainObjectType.DirectoryItem;
		    if (domainObject is GalleryCategory)
		        return DomainObjectType.DirectoryCategory;
		    if (domainObject is GalleryImage)
		        return DomainObjectType.GalleryImage;

		    return DomainObjectType.Unknown;
		}
	    #endregion
	}
}