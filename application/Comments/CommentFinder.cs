using System.Collections;

namespace Apollo.Comments
{
	public class CommentsFinder : Finder
	{
		/// <summary>
		/// Creates a new CommentsFinder object.
		/// </summary>
        public CommentsFinder() 
		{
			var fields = new Hashtable();

			fields["Author.ID"] = "AuthorID";
			fields["Text"] = "Comment";
			fields["Owner.ID"] = "OwnerID";
            fields["Owner.UID"] = "OwnerUID";

			PrimaryKey = "ID";
			Fields = fields;
			SqlTable = "Comments";
		}
	}
}