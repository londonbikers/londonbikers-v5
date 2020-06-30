using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Caching;

namespace Apollo.Content
{
	public class ContentServer
	{
		#region members
	    private IList<IDocumentTypeGroup> _popularDocuments;
		#endregion

        #region accessors
	    public IList<IDocumentTypeGroup> PopularDocuments
	    {
	        get
	        {
	            if (_popularDocuments == null)
                    RetrievePopularDocuments();

	            return _popularDocuments;
	        }
            internal set
            {
                _popularDocuments = value;
            }
	    }
        #endregion

        #region constructors
        /// <summary>
		/// Creation of this object is limited to this namespace.
		/// </summary>
		internal ContentServer() 
		{
		}
		#endregion

		#region public methods
		/// <summary>
		/// Retrieves an editorial section, lightly-loaded.
		/// </summary>
		/// <param name="id">The numeric ID for the section.</param>
		public Section GetSection(int id) 
		{
			// attempt to retrieve the section from the cache.
			var section = CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof(Section), id)) as Section;

			if (section == null)
			{
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetContentSection", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@SectionID", id));
				SqlDataReader reader = null;
			
				try
				{
					connection.Open();
					reader = command.ExecuteReader();
			        if (reader.Read())
			        {
			            section = new Section
                        {
                            Id = id,
                            Name = Sql.GetValue(typeof (string), reader["Name"]) as string,
                            ShortDescription = Sql.GetValue(typeof (string), reader["ShortDescription"]) as string,
                            ContentType = (ContentType) (byte) reader["ContentTypeID"],
                            UrlIdentifier = Sql.GetValue(typeof (string), reader["UrlIdentifier"]) as string,
                            FavouriteTags = Tag.DelimitedTagsToCollection(Sql.GetValue(typeof (string), reader["FavouriteTags"]) as string), Status = (DomainObjectStatus) (byte) reader["Status"]
                        };

			            // generic sections can have default documents, which can be used as indexes.
			            if (reader["DefaultDocumentID"] != DBNull.Value)
			                section.DefaultDocument = GetDocument((long)Sql.GetValue(typeof(long), reader["DefaultDocumentID"]));

			            if ((int)Sql.GetValue(typeof(int), reader["ParentChannelID"]) > 0)
			            {
			                // section relates to a channel directly.
			                section.ParentChannel = Server.Instance.GetChannel((int)Sql.GetValue(typeof(int), reader["ParentChannelID"]));
			            }
			            else
			            {
			                // section relates to a site directly.
			                section.ParentSite = Server.Instance.GetSite((int)Sql.GetValue(typeof(int), reader["ParentSiteID"]));
			            }

			            // add the section to the cache.
			            CacheManager.AddItem(section, section.ApplicationUniqueId);
			        }
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}

			return section;
		}

		/// <summary>
		/// Retrieves the current version of a specific Document.
		/// </summary>
		public Document GetDocument(long id) 
		{
			// attempt to retrieve the document for the cache.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(Document), id);
            var document = CacheManager.RetrieveItem(cacheId) as Document;

			if (document == null)
			{
				// document not in the cache, retrieve from database.
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetEditorialDocument", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@ID", id));
				SqlDataReader reader = null;

				try
				{
					connection.Open();
					reader = command.ExecuteReader();
					document = InitialiseDocument(reader);

					// add the document to the cache.
					if (document != null)
                        CacheManager.AddItem(document, cacheId);
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}
			
			return document;
		}

	    /// <summary>
	    /// Returns a specific content Image object.
	    /// </summary>
	    /// <param name="id">The numeric-identifier for the Image to retrieve.</param>
	    public EditorialImage GetImage(long id) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetEditorialImage", connection) {CommandType = CommandType.StoredProcedure};
	        command.Parameters.Add(new SqlParameter("@ID", id));
			SqlDataReader reader = null;
			EditorialImage editorialImage = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			    {
			        editorialImage = new EditorialImage(ObjectCreationMode.Retrieve)
			        {
			            Id = (long) Sql.GetValue(typeof (long), reader["ID"]),
			            Name = Sql.GetValue(typeof (string), reader["Name"]) as string,
			            Filename = Sql.GetValue(typeof (string), reader["Filename"]) as string,
			            Width = (int) Sql.GetValue(typeof (int), reader["Width"]),
			            Height = (int) Sql.GetValue(typeof (int), reader["Height"]),
			            Created = (DateTime) Sql.GetValue(typeof (DateTime), reader["Created"]),
			            Type = (ContentImageType) (byte) reader["Type"]
			        };
			    }
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

	        return editorialImage;
		}

		/// <summary>
		/// Returns a specific content Image object.
		/// </summary>
		public EditorialImage GetImage(string filename)
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetEditorialImageByFilename", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@Filename", filename));
			SqlDataReader reader = null;
			EditorialImage editorialImage = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();
			    while (reader.Read())
			    {
			        editorialImage = new EditorialImage(ObjectCreationMode.Retrieve)
			        {
			            Id = reader.GetInt64(reader.GetOrdinal("ID")),
			            Name = reader.GetString(reader.GetOrdinal("Name")),
			            Filename = reader.GetString(reader.GetOrdinal("Filename")),
			            Width = reader.GetInt32(reader.GetOrdinal("Width")),
			            Height = reader.GetInt32(reader.GetOrdinal("Height")),
			            Created = reader.GetDateTime(reader.GetOrdinal("Created")),
			            Type = (ContentImageType) (byte) reader["Type"]
			        };
			    }
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return editorialImage;
		}

		/// <summary>
		/// Returns a new document for use.
		/// </summary>
		public Document NewDocument() 
		{
			return new Document(ObjectCreationMode.New, 0L);
		} 
		
		/// <summary>
		/// Returns a new DocumentFinder object.
		/// </summary>
		public DocumentFinder NewDocumentFinder() 
		{
			return new DocumentFinder();
		} 

		/// <summary>
		/// Returns a new content image for use.
		/// </summary>
		public EditorialImage NewImage() 
		{
			return new EditorialImage(ObjectCreationMode.New);
		} 

		/// <summary>
		/// Returns a finder capable of finding content images.
		/// </summary>
		public ImageFinder NewImageFinder() 
		{
			return new ImageFinder();
		} 
		
		/// <summary>
		/// Persists any changes to a Document object to the database.
		/// </summary>
		/// <param name="document">The Document object to persist.</param>
		public void UpdateDocument(Document document) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			connection.Open();

            // content processing.
		    document.Title = Text.ConvertUsSpellingToUk(document.Title);
		    document.LeadStatement = Text.ConvertUsSpellingToUk(document.LeadStatement);
		    document.Body = Text.ConvertUsSpellingToUk(document.Body);

			// is this the first time the document's been published?
			if (!document.IsPersisted && document.Status.ToLower() == "published")
			{
				document.Published = DateTime.Now;
			}
			else if (document.IsPersisted && document.Status.ToLower() == "published")
			{
				// query the db for the previous document status.
				var statusCommand = new SqlCommand("GetEditorialDocumentStatus", connection) {CommandType = CommandType.StoredProcedure};
			    statusCommand.Parameters.Add(new SqlParameter("@ID", document.Id));

				var status = Sql.GetValue(typeof(string), statusCommand.ExecuteScalar()) as string;
		        if (status != null && status.ToLower() != "published")
		            document.Published = DateTime.Now;
			}

			// create a simple tag collection from the Tag object collection.
			var simpleTags = (from Tag tag in document.Tags select tag.Name).ToList();
		    var command = new SqlCommand("UpdateDocument", connection) {CommandType = CommandType.StoredProcedure};

		    command.Parameters.Add(new SqlParameter("@ID", document.Id));
			command.Parameters.Add(new SqlParameter("@Title", document.Title));
			command.Parameters.Add(new SqlParameter("@Author", document.Author.Uid));
			command.Parameters.Add(new SqlParameter("@LeadStatement", document.LeadStatement));
			command.Parameters.Add(new SqlParameter("@Abstract", document.Abstract));
			command.Parameters.Add(new SqlParameter("@Body", document.Body));
			command.Parameters.Add(new SqlParameter("@Status", document.Status));
			command.Parameters.Add(new SqlParameter("@PublishedDate", document.Published));
			command.Parameters.Add(new SqlParameter("@Type", (int)document.Type));
			command.Parameters.Add(new SqlParameter("@Tags", Helpers.CollectionToDelimitedString(simpleTags)));

			try
			{
				// perform main document update.
                document.Id = Convert.ToInt64(command.ExecuteScalar());

                if (document.IsPersisted)
                {
                    // clear document mappings.
                    command.Parameters.Clear();
                    command.CommandText = "ClearDocumentMappings";
                    command.Parameters.Add(new SqlParameter("@DocumentID", document.Id));
                    command.ExecuteNonQuery();
                }

			    // persist/assert the document mappings.
				command.CommandText = "AddDocumentMapping";
				foreach (var section in document.Sections)
				{
					command.Parameters.Clear();
					command.Parameters.Add(new SqlParameter("@DocumentID", document.Id));
					command.Parameters.Add(new SqlParameter("@SectionID", section.Id));
					command.ExecuteNonQuery();
				}

                if (!document.IsPersisted)
                    CacheManager.AddItem(document, CacheManager.GetApplicationUniqueId(typeof(Document), document.Id));

				ResetDocumentCaches(document);
				document.Tags.PersistenceReset();
				document.EditorialImages.Save();
				document.RelatedDocuments.Save();

                // necessary to do this at this point to ensure the document is in the cache and other caches have been updated, but the document not marked as persisted.
                if (!document.IsPersisted)
			        document.IsPersisted = true;
			}
			finally
			{
				connection.Close();
			}
		} 
		
		/// <summary>
		/// Persists any changes to a content Image object to the database.
		/// </summary>
		/// <param name="editorialImage">The Image object to persist.</param>
		public void UpdateImage(EditorialImage editorialImage) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UpdateContentImage", connection) {CommandType = CommandType.StoredProcedure};

		    command.Parameters.Add(new SqlParameter("@ID", editorialImage.Id));
			command.Parameters.Add(new SqlParameter("@Name", editorialImage.Name));
			command.Parameters.Add(new SqlParameter("@Filename", editorialImage.Filename));
			command.Parameters.Add(new SqlParameter("@Type", (int)editorialImage.Type));
			command.Parameters.Add(new SqlParameter("@Width", editorialImage.Width));
			command.Parameters.Add(new SqlParameter("@Height", editorialImage.Height));

			try
			{
				connection.Open();
                editorialImage.Id = Convert.ToInt64(command.ExecuteScalar());
			}
			finally
			{
				connection.Close();
			}
		}

	    /// <summary>
	    /// Attempts to delete a Document object.
	    /// </summary>
	    /// <param name="document">The Document to attempt to delete.</param>
	    /// <returns>A boolean indicating whether or not the Document was delelted.</returns>
	    public void DeleteDocument(Document document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            if (!document.IsPersisted)
                throw new ArgumentException("Cannot delete Document as it isn't persisted!", "document");

            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("DeleteDocument", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add(new SqlParameter("@ID", document.Id));

            // delete all comments.
            foreach (var c in document.Comments.Items)
                Server.Instance.MemberCommentsServer.DeleteComment(c);

            // delete all images.
            foreach (var i in document.EditorialImages.List)
                DeleteImage(i, true);

            using (var t = new TransactionScope())
            {
                try
                {
                    // delete document.
                    connection.Open();
                    command.ExecuteNonQuery();

                    // remove from the cache.
                    CacheManager.RemoveItem(CacheManager.GetApplicationUniqueId(typeof(Document), document.Id));
                    t.Complete();
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

		/// <summary>
		/// Deletes a content image from the system.
		/// </summary>
		/// <param name="editorialImage">The content image to delete.</param>
		/// <param name="forceDelete">If true, the will be deleted regardless of whether or not it's used by any Documents.
		/// You cannot delete an Image with associations by default.</param>
		public void DeleteImage(EditorialImage editorialImage, bool forceDelete) 
		{
			if (editorialImage == null)
				return;

			if (editorialImage.Id < 1)
				return;

			if (!forceDelete && editorialImage.AssociationCount > 0)
			    return;

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteContentImage", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ID", editorialImage.Id));

			try
			{
				connection.Open();
                editorialImage.Id = Convert.ToInt64(command.ExecuteScalar());

			    try
			    {
                    // remove the image from the file-system.
                    File.Delete(editorialImage.FullPath);
			    }
			    catch (Exception ex)
			    {
                    Logger.LogException(ex);
			    }
			}
			finally
			{
				connection.Close();
			}
		}

        /// <summary>
        /// Checks if a document would be a duplicate by checking the title. Checks only if it's a duplicate for today.
        /// </summary>
        /// <param name="title">The prospective documents title.</param>
        public bool IsDocumentDuplicateToday(string title)
        {
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("IsDocumentDuplicateToday", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add(new SqlParameter("@Title", title));

            try
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 0 ? false : true;
            }
            finally
            {
                connection.Close();
            }
        }
	    #endregion

        #region infrastructure methods
        /// <summary>
        /// Returns a large collection of all the dynamic content in the system. Used for search-engines to query for content and build accurate site content maps.
        /// </summary>
        public IEnumerable<SiteMapItem> GetSiteMapItems()
        {
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("GetSimpleContentList", connection) {CommandType = CommandType.StoredProcedure};
            SqlDataReader reader = null;
            var items = new List<SiteMapItem>();

            try
            {
                // documents
                connection.Open();
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    var item = new SiteMapItem
                    {
                        ItemId = reader.GetInt64(reader.GetOrdinal("ID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        ContentType = ((byte) reader["Type"] == 0) ? SiteMapContentType.News : SiteMapContentType.Article,
                        LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified"))
                    };
                    items.Add(item);
                }

                // galleries.
                reader.NextResult();
                while (reader.Read())
                {
                    var item = new SiteMapItem
                    {
                        ItemId = reader.GetInt64(reader.GetOrdinal("ID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        ContentType = SiteMapContentType.Gallery,
                        LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified"))
                    };

                    items.Add(item);
                }

                // directory items.
                reader.NextResult();
                while (reader.Read())
                {
                    var item = new SiteMapItem
                    {
                        ItemId = reader.GetInt64(reader.GetOrdinal("ID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        ContentType = SiteMapContentType.DirectoryItem,
                        LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified"))
                    };

                    items.Add(item);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return items;
        }

        /// <summary>
        /// Returns a large collection of the latest Google-News suitable content in the system. Used for search-engines to query for content and build accurate site content maps.
        /// </summary>
        public IEnumerable<SiteMapItem> GetGoogleNewsSiteMapItems()
        {
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("GetGoogleNewsContentList", connection) {CommandType = CommandType.StoredProcedure};
            SqlDataReader reader = null;
            var items = new List<SiteMapItem>();

            try
            {
                // documents
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new SiteMapItem
                    {
                        ItemId = reader.GetInt64(reader.GetOrdinal("ID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        ContentType = ((byte) reader["Type"] == 0) ? SiteMapContentType.News : SiteMapContentType.Article,
                        LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
                        Keywords = reader.GetString(reader.GetOrdinal("Tags"))
                    };
                    items.Add(item);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return items;
        }
        #endregion

        #region private methods
        /// <summary>
		/// Performs the donkey-work of building a Document object from a SqlDataReader object.
		/// </summary>
		/// <param name="reader">The SqlDataReader with the Document record data.</param>
		private static Document InitialiseDocument(IDataReader reader) 
		{
			Document document = null;
			if (reader.Read())
			{
				document = new Document(ObjectCreationMode.Retrieve, (long)reader["ID"])
                {
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Created = reader.GetDateTime(reader.GetOrdinal("Created")),
                    LeadStatement = reader.GetString(reader.GetOrdinal("LeadStatement")),
                    Abstract = reader.GetString(reader.GetOrdinal("Abstract")),
                    Body = reader.GetString(reader.GetOrdinal("Body")),
                    Published = reader.GetDateTime(reader.GetOrdinal("Published")),
                    Author = Server.Instance.UserServer.GetUser(reader.GetGuid(reader.GetOrdinal("Author"))),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Tags = Tag.DelimitedTagsToCollection(reader.GetString(reader.GetOrdinal("Tags"))),
                    Type = (DocumentType) (byte) reader["Type"],
                    Views = reader.GetInt64(reader.GetOrdinal("Views"))
                };
			}

			return document;
		} 

        /// <summary>
        /// Ensures any references to a Document are updated to reflect it's addition/removal.
        /// </summary>
        private static void ResetDocumentCaches(IDocument document)
        {
            if (!document.IsPersisted)
            {
                // update the caches for the sections this document belongs to.
                foreach (var section in document.Sections)
                {
                    section.LatestDocuments.RefreshDocuments();
                    foreach (Tag tag in document.Tags)
                        CacheManager.RemoveItem(TaggedDocumentContainer.GetApplicationUniqueId(section, tag));
                }
            }

            // tags caches need updating if this was removed from any tags.
            foreach (var tag in document.Tags.RemovedTags)
            {
                tag.LatestDocuments = null;
                foreach (var section in document.Sections)
                    CacheManager.RemoveItem(TaggedDocumentContainer.GetApplicationUniqueId(section, tag));
            }

            // this needs to be done to ensure doc status changes are observed.
            foreach (ITag tag in document.Tags)
            {
                tag.LatestDocuments = null;
                foreach (var section in document.Sections)
                    CacheManager.RemoveItem(TaggedDocumentContainer.GetApplicationUniqueId(section, tag));
            }

            // this is a last-minute addition. there should be a way to navigate to the site from the document really...
            foreach (var site in Server.Instance.GetSites())
                site.Channels[0].News.LatestDocuments.RefreshDocuments();
        }

        private void RetrievePopularDocuments()
        {
            _popularDocuments = new List<IDocumentTypeGroup>();
            lock (_popularDocuments)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var popularityPeriodDays = int.Parse(ConfigurationManager.AppSettings["Apollo.ContentPopularityRetrievalPeriodDays"]);
                var articlePopularityPeriodDays = int.Parse(ConfigurationManager.AppSettings["Apollo.ArticlePopularityRetrievalPeriodDays"]);

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetPopularDocuments"
                };

                SqlDataReader reader = null;
                command.Parameters.Add(new SqlParameter("@PeriodDays", popularityPeriodDays));
                command.Parameters.Add(new SqlParameter("@ArticlePeriodDays", articlePopularityPeriodDays));
                command.Parameters.Add(new SqlParameter("@MaxResults", 100));

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    // news stories
                    var newsGroup = new DocumentTypeGroup {Type = DocumentType.News};
                    _popularDocuments.Add(newsGroup);
                    if (reader.HasRows)
                        while (reader.Read())
                            newsGroup.Items.Add(reader.GetInt64(reader.GetOrdinal("id")));

                    // articles
                    reader.NextResult();
                    var articlesGroup = new DocumentTypeGroup {Type = DocumentType.Article};
                    _popularDocuments.Add(articlesGroup);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            articlesGroup.Items.Add(reader.GetInt64(reader.GetOrdinal("id")));
                    }
                    else
                    {
                        Logger.LogWarning(string.Format("Second result set doesn't have any records! popularityPeriodDays: {0}. articlePopularityPeriodDays: {1}", popularityPeriodDays, articlePopularityPeriodDays));
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "RetrievePopularDocuments()");
                    throw;
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