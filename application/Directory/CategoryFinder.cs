using System.Collections;

namespace Apollo.Directory
{
	public class CategoryFinder : Finder
	{
		/// <summary>
		/// Creates a new CategoryFinder object.
		/// </summary>
		public CategoryFinder() 
		{
			var fields = new Hashtable();

			fields["Name"] = "Name";
			fields["Description"] = "Description";
			fields["Keywords"] = "Keywords";
			fields["RequiresMembership"] = "RequiresMembership";

			Fields = fields;
			SqlTable = "DirectoryCategories";
		}
	}
}