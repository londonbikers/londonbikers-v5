using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Apollo.Galleries
{
	public class GalleryFinder : Finder
	{
		#region constructors
		/// <summary>
		/// Returns a new GalleryFinder.
		/// </summary>
		public GalleryFinder() 
		{
			var fields = new Hashtable();
			fields["CreationDate"] = "f_creation_date";
			fields["Description"] = "f_description";
			fields["IsPublic"] = "f_is_public";
			fields["Status"] = "f_status";
			fields["Title"] = "f_title";

			Fields = fields;
			SqlTable = "apollo_galleries";
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of Gallery ID's relating a specific Site.
		/// </summary>
		internal List<long> GetLatestGalleryIDsForSite(long siteId)
		{
			var command = new SqlCommand("GetLatestGalleriesForSite") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ParentID", siteId));
			return PerformCustomQuery(command);
		}
		#endregion
	}
}