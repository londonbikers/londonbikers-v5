using System.Collections;

namespace Apollo.Directory
{
	public class ItemFinder : Finder
	{
		/// <summary>
		/// Creates a new ItemFinder object.
		/// </summary>
		public ItemFinder() 
		{
			var fields = new Hashtable();
			fields["Title"] = "Title";
			fields["Description"] = "Description";
			fields["Keywords"] = "Keywords";
			fields["Links"] = "Links";
			fields["Images"] = "Images";
			fields["Status"] = "Status";
			fields["Created"] = "Created";
			fields["Submiter"] = "Submiter";
			fields["Rating"] = "Rating";
            fields["Views"] = "Views";

			Fields = fields;
			SqlTable = "DirectoryItems";
		}
	}
}