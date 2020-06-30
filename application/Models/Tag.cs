using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// A tag represents a keyword, a way of linking together different domain objects by context. Tags are objects that can be
	/// created and thrown away easily, they can be customised to contain domain objects for different contexts, i.e. for a 
	/// section, for a global search or just ad-hoc.
	/// </summary>
	public class Tag : ITag
	{
        #region members
	    private IList<IDocument> _documents;
        private ILightCollection<IDocument> _latestCommentedDocuments;
	    private IList<IDocumentTypeGroup> _popularDocuments;
	    private IList<IDocumentTypeGroup> _latestDocuments;
	    #endregion

		#region accessors
	    /// <summary>
	    /// The name of the tag.
	    /// </summary>
	    public string Name { get; set; }

        /// <summary>
        /// A collection of documents grouped by sub-type, i.e. stories, articles, blogs, etc.
        /// </summary>
        public IList<IDocumentTypeGroup> PopularDocuments
        {
            get { return _popularDocuments ?? (_popularDocuments = RetrieveDocumentsByFilterType(ContentFilterType.MostPopular)); }
            set { _popularDocuments = value; }
        }

	    /// <summary>
	    /// A collection of documents grouped by sub-type, i.e. stories, articles, blogs, etc.
	    /// </summary>
	    public IList<IDocumentTypeGroup> LatestDocuments
	    {
            get { return _latestDocuments ?? (_latestDocuments = RetrieveDocumentsByFilterType(ContentFilterType.Latest)); }
	        set { _latestDocuments = value; }
	    }

	    /// <summary>
	    /// The latest published documents to be commented upon.
	    /// </summary>
	    public ILightCollection<IDocument> LatestCommentedDocuments
	    {
	        get { return _latestCommentedDocuments ?? (_latestCommentedDocuments = RetrieveLatestCommentedDocuments()); }
            set { _latestCommentedDocuments = value; }
	    }

        /// <summary>
        /// A collection of the latest galleries for this tag.
        /// </summary>
        public ILightCollection<IGallery> LatestGalleries
        {
            get
            {
                return RetrieveLatestGalleries();
            }
        }

        // -- this functionality requires proper tagging on galleries/photos.
        ///// <summary>
        ///// A collection of the latest galleries for this tag.
        ///// </summary>
        //public ILightCollection<IGallery> PopularGalleries
        //{
        //    get
        //    {
        //        if (_popularGalleries == null)
        //            RetrievePopularGalleries();

        //        return _popularGalleries;
        //    }
        //    set
        //    {
        //        _popularGalleries = value;
        //    }
        //}

	    /// <summary>
		/// Generates a unique ID for the Tag within the context of the application.
		/// </summary>
		public string ApplicationUniqueId { get { return GetApplicationUniqueId(); } }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Tag object.
		/// </summary>
		internal Tag() 
		{
		}

	    /// <summary>
	    /// Creates a new Tag that contains Documents for a specific Section.
	    /// </summary>
	    /// <param name="name">The name of the Tag. I.e. 'motogp'.</param>
	    internal Tag(string name) 
		{
			Name = name;
		}
		#endregion

		#region static methods
	    /// <summary>
	    /// Builds a collection of Tags from a comma-delimited string.
	    /// </summary>
	    /// <param name="delimitedTags">Tags in a comma-seperated format, i.e. 'honda, suzuki, yamaha'.</param>
	    internal static TagCollection DelimitedTagsToCollection(string delimitedTags) 
		{
			var tags = new TagCollection();
			if (delimitedTags == String.Empty)
				return tags;

			ITag tag;
	        var rawTags = delimitedTags.ToLower().Split(char.Parse(","));

			foreach (var t in rawTags.Where(t => t.Trim() != string.Empty))
			{
                //tag = new Tag();
                //var rawTag = t.Trim();
                //tag.Name = rawTag;

			    tag = Server.Instance.GetTag(t.Trim());
			    tags.Add(tag);
			}

			return tags;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Gets a unique ID for this Tag, within the context of a specific Section.
		/// </summary>
		/// <param name="section">The Section to associate with the Tag.</param>
		public string GetApplicationUniqueIdForSection(Section section) 
		{
			return string.Format("{0}:{1}:{2}", GetType().FullName, section.Id, Name);
		}

        /// <summary>
        /// Transient use only: Allows Documents to be stored against the tag.
        /// </summary>
        public IList<IDocument> Documents
        {
            get { return _documents ?? (_documents = new List<IDocument>()); }
        }

	    public int DocumentCount { get; set; }
	    #endregion
       
        #region private methods
        private IList<IDocumentTypeGroup> RetrieveDocumentsByFilterType(ContentFilterType filterType)
        {
            var documents = new List<IDocumentTypeGroup>();
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand {Connection = connection, CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(new SqlParameter("@Tag", Name));
            SqlDataReader reader = null;
            command.Parameters.Add(new SqlParameter("@MaxResults", 100));

            if (filterType == ContentFilterType.MostPopular)
            {
                command.Parameters.Add(new SqlParameter("@PeriodDays", int.Parse(ConfigurationManager.AppSettings["Apollo.ContentPopularityRetrievalPeriodDays"])));
                command.CommandText = "GetTagPopularDocuments";
            }
            else
            {
                // we don't really want to set a max period here.
                command.Parameters.Add(new SqlParameter("@PeriodDays", 9999));
                command.CommandText = "GetTagLatestDocuments";
            }

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                // news stories
                var newsGroup = new DocumentTypeGroup { Type = DocumentType.News };
                documents.Add(newsGroup);
                if (reader.HasRows)
                    while (reader.Read())
                        newsGroup.Items.Add(reader.GetInt64(reader.GetOrdinal("id")));

                // articles
                reader.NextResult();
                var articlesGroup = new DocumentTypeGroup { Type = DocumentType.Article };
                documents.Add(articlesGroup);
                if (reader.HasRows)
                    while (reader.Read())
                        articlesGroup.Items.Add(reader.GetInt64(reader.GetOrdinal("id")));
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return documents;
        }

        private ILightCollection<IDocument> RetrieveLatestCommentedDocuments()
        {
            var documents = new LightCollection<IDocument>();
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            
            var command = new SqlCommand { Connection = connection, CommandType = CommandType.StoredProcedure };
            command.Parameters.Add(new SqlParameter("@Tag", Name));
            command.Parameters.Add(new SqlParameter("@MaxResults", 100));
            //command.Parameters.Add(new SqlParameter("@PeriodDays", int.Parse(ConfigurationManager.AppSettings["Apollo.ContentPopularityRetrievalPeriodDays"])));
            command.Parameters.Add(new SqlParameter("@PeriodDays", 9999));
            command.CommandText = "GetLatestCommentedDocumentsForTag";
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                    documents.Add(reader.GetInt64(reader.GetOrdinal("id")));
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return documents;
        }

        /// <summary>
        /// Returns a collection of the latest galleries for variations (synonyms) of the tag name. This is a temporary solution until
        /// we can convert galleries to support tags and change the retrieval/cache mechanism to be light object based.
        /// </summary>
        /// <returns></returns>
        private ILightCollection<IGallery> RetrieveLatestGalleries()
        {
            //-- ADD IN OTHER CATEGORIES, I.E MOTORCYCLES AND SHOWS!
            
            // galleries can be pulled out for known tags only at this point. in the future this can be changed once tags have been added to galleries.
            const int maxGalleries = 7;
            
            var supportedTags = new[] {"motogp", "bsb", "wsb", "british mx", "world mx", "iom tt", "trials", "x-fighters"};
            if (!supportedTags.Contains(Name.ToLower()))
            {
                // try and do a category match.
                switch (Name.ToLower())
                {
                    case "racing":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(1), maxGalleries);
                    case "products":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(3), maxGalleries);
                    case "trackdays":
                    case "track days":
                    case "track-days":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(4), maxGalleries);
                    case "shows":
                    case "bike shows":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(5), maxGalleries);
                    case "bike meets":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(7), maxGalleries);
                    case "ride-outs":
                    case "rideouts":
                        return GetGalleriesInCategory(Server.Instance.GalleryServer.GetCategory(9), maxGalleries);
                }
            }

            var galleries = new LightCollection<IGallery>();
            if (!supportedTags.Contains(Name.ToLower())) 
                return galleries;

            // resolve the tag(s).
            string[] keywords = null;
            switch (Name.ToLower())
            {
                case "motogp":
                    keywords = new[] { "motogp", "moto gp" };
                    break;
                case "bsb":
                    keywords = new[] { "bsb", "british superbike" };
                    break;
                case "wsb":
                    keywords = new[] { "wsb", "sbk", "world superbike" };
                    break;
                case "british mx":
                    keywords = new[] { "british mx", "british motocross", "uk motocross" };
                    break;
                case "world mx":
                    keywords = new[] { "world mx", "world motocross" };
                    break;
                case "iom tt":
                    keywords = new[] { "iom tt", "isle of man tt" };
                    break;
                case "trials":
                    keywords = new[] { "trial world championship", "world trials" };
                    break;
                case "x-fighters":
                    keywords = new[] { "x-fighters" };
                    break;
            }

            var ids = Server.Instance.GalleryServer.CommonQueries.LatestPublicGalleriesForTag(keywords, maxGalleries);
            foreach (var id in ids)
                galleries.Add(id);

            return galleries;
        }

        private static ILightCollection<IGallery> GetGalleriesInCategory(IGalleryCategory category, int maxGalleries)
        {
            var list = new LightCollection<IGallery>();
            var source = category.Galleries.RetrieveRaw(maxGalleries);
            foreach (var id in source)
                list.Add(id);
            return list;
        }

        /// <summary>
        /// Generates the ApplicationUniqueID, depending on the context of use.
        /// </summary>
        private string GetApplicationUniqueId()
        {
            return string.Format("{0}:{1}", GetType().FullName, Name);
        }
    	#endregion
	}
}