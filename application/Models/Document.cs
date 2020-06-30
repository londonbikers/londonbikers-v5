using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	public class Document : CommonBase, IDocument
	{
		#region members
	    private List<ISection> _sections;
	    #endregion

		#region accessors
	    /// <summary>
	    /// Denotes the original author of the document.
	    /// </summary>
	    public User Author { get; set; }

	    /// <summary>
	    /// The date on which the document was originally created.
	    /// </summary>
	    public DateTime Created { get; set; }

	    /// <summary>
	    /// The collection of content Images that may be associated with the document.
	    /// </summary>
	    public EditorialImages EditorialImages { get; private set; }

	    /// <summary>
	    /// Denotes when this document was first set to a Published status.
	    /// </summary>
	    public DateTime Published { get; set; }

	    /// <summary>
	    /// The collection of documents that this document may be related to.
	    /// </summary>
	    public RelatedDocuments RelatedDocuments { get; private set; }

	    /// <summary>
	    /// The current status of this document.
	    /// </summary>
	    public string Status { get; set; }

	    /// <summary>
	    /// The single-line lead statement for this document.
	    /// </summary>
	    public string LeadStatement { get; set; }

	    /// <summary>
	    /// The single-paragraph abstract for this document.
	    /// </summary>
	    public string Abstract { get; set; }

	    /// <summary>
	    /// The content title for this document.
	    /// </summary>
	    public string Title { get; set; }

	    /// <summary>
	    /// The content body for this document.
	    /// </summary>
	    public string Body { get; set; }

	    /// <summary>
		/// Denotes whether or not this Document has been persisted, and thus added to the cache, thus available for use.
		/// </summary>
		public bool HasBeenPersisted { get { return IsPersisted; } }

	    /// <summary>
	    /// Determines whether or not the viewer must be a member or not.
	    /// </summary>
	    public bool RequiresMembershipToBeViewed { get; set; }

	    /// <summary>
		/// Provides access to the collection of Sections this Document belongs to.
		/// </summary>
		public List<ISection> Sections 
		{ 
			get 
			{ 
				if (_sections == null)
					GetSections();

				return _sections; 
			} 
		}

	    /// <summary>
	    /// Defines what template the Document is using.
	    /// </summary>
	    public DocumentType Type { get; set; }

	    /// <summary>
	    /// Provides access to the Tags associate with this Document.
	    /// </summary>
	    public TagCollection Tags { get; set; }

	    /// <summary>
	    /// Provides access to any user-comments that may relate to this Document.
	    /// </summary>
	    public Comments Comments { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Returns a Document object.
		/// </summary>
		/// <param name="mode">If 'New' then an empty Document object is prepared, else a simple shell is returned to be populated.</param>
		/// <param name="id">The numeric-identifier for this Document if this is a pre-existing Document.</param>
		public Document(ObjectCreationMode mode, long id) 
		{
            Title = string.Empty;
            LeadStatement = string.Empty;
            Abstract = string.Empty;
            Body = string.Empty;
            Status = "New";
            Created = DateTime.Now;
            Published = DateTime.Parse("1/1/2000");
            Tags = new TagCollection();
            Comments = new Comments(this);
					
			if (mode == ObjectCreationMode.New)
				IsPersisted = false;
			else
                Id = id;
			
			RelatedDocuments = new RelatedDocuments(this);
            EditorialImages = new EditorialImages(this);
			DerivedType = GetType();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Retrieves the list of sections that this document is associated with.
		/// </summary>
		private void GetSections() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetDocumentSections", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@DocumentID", Id));
			SqlDataReader reader = null;
            _sections = new List<ISection>();

            lock (_sections)
            {
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        Sections.Add(Server.Instance.ContentServer.GetSection((int)Sql.GetValue(typeof(int), reader["ParentSectionID"])));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
		}
		#endregion
	}
}
