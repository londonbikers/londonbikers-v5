using System.Collections;

namespace Apollo.Content
{
	public class ImageFinder : Finder
	{
		public ImageFinder()
		{
			var fields = new Hashtable();

			fields["Name"] = "f_name";
			fields["Filename"] = "f_filename";
			fields["Width"] = "f_width";
			fields["Height"] = "f_height";
			fields["Type"] = "Type";
			fields["Created"] = "f_created";

			Fields = fields;
			SqlTable = "apollo_images";
		}
	}
}