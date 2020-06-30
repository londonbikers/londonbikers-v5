using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Provides additional functionality for the LightDocumentCollection to facilitate
	/// the section featured documents.
	/// </summary>
	public class FeaturedDocumentsCollection : LightDocumentCollection, IFeaturedDocumentsCollection
	{
		#region members
		private readonly Section _parentSection;
		private readonly int _maxDocuments;
		#endregion

		#region constructors

	    /// <summary>
	    /// Creates a new FeaturedDocumentCollection.
	    /// </summary>
	    /// <param name="parentSection">The Section that this list is containing featured documents for.</param>
	    /// <param name="maxDocuments">Defines the maximum number of Documents to retrieve.</param>
	    internal FeaturedDocumentsCollection(Section parentSection, int maxDocuments) : base(true) 
		{
			if (parentSection == null)
				throw new ArgumentException("No parent section provided.");

			if (maxDocuments < 1)
				throw new ArgumentException("maxDocuments must be 1 or more.");

			_parentSection = parentSection;
			_maxDocuments = maxDocuments;
			RetrieveDocuments();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a Document to the featured documents collection.
		/// </summary>
		public override bool Add(long documentId)
		{
		    if (!base.Add(documentId))
		        return false;

		    // persist this addition.
		    var command = new SqlCommand("AddSectionFeaturedDocument") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", _parentSection.Id));
		    command.Parameters.Add(new SqlParameter("@DocumentID", documentId));

		    var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
		    command.Connection = connection;

		    try
		    {
		        connection.Open();
		        command.ExecuteNonQuery();
		    }
		    finally
		    {
		        if (connection != null)
		            connection.Close();
		    }

		    return true;
		}

	    /// <summary>
		/// Adds a Document to the featured documents collection.
		/// </summary>
		public override bool Add(IDocument document) 
		{
			if (document == null)
				throw new ArgumentNullException("document");

			return Add(document.Id);
		}
        
		/// <summary>
		/// Removes a Document from the featured documents collection.
		/// </summary>
		public override bool Remove(IDocument document) 
		{
			if (document == null)
				throw new ArgumentNullException("document");

			return Remove(document.Id);
		}

		/// <summary>
		/// Removes a Document from the featured documents collection.
		/// </summary>
		public override bool Remove(long documentId)
		{
		    if (!base.Remove(documentId))
		        return false;

		    // persist this removal.
		    var command = new SqlCommand("RemoveSectionFeaturedDocument") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", _parentSection.Id));
		    command.Parameters.Add(new SqlParameter("@DocumentID", documentId));

		    var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
		    command.Connection = connection;

		    try
		    {
		        connection.Open();
		        command.ExecuteNonQuery();
		    }
		    finally
		    {
		        if (connection != null)
		            connection.Close();
		    }

		    return true;
		}

	    /// <summary>
		/// Gets the latest featured documents from the database.
		/// </summary>
		public void Reload() 
		{
			Clear();
			RetrieveDocuments();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Initialises the collection by querying the database for the featured documents.
		/// </summary>
		private void RetrieveDocuments() 
		{
			SqlDataReader reader = null;
			var command = new SqlCommand("GetSectionFeaturedDocuments") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", _parentSection.Id));
			command.Parameters.Add(new SqlParameter("@MaxResults", _maxDocuments));
			
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			command.Connection = connection;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    if (reader != null)
			        while (reader.Read())
			            base.Add((long)Sql.GetValue(typeof(long), reader["DocumentID"]));
			}
			finally
			{
				if (reader != null)
					reader.Close();

				if (connection != null)
					connection.Close();
			}
		}
		#endregion
	}
}