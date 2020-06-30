using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a specialist topic area, contains categories and sub-sections.
	/// </summary>
	public class Section : CommonBase, ISection
	{
		#region members
		private List<long> _documents;
	    private ITagCollection _popularTags;
	    private ILatestDocuments _latestDocuments;
	    private IFeaturedDocumentsCollection _featuredDocuments;
		private DateTime _lastPopularTagRetrievalTime;
		private readonly int _cachePopularTagsPeriodHours;
		#endregion

		#region accessors
	    /// <summary>
	    /// The public name of the Section.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// A short description that can be used for leading information.
	    /// </summary>
	    public string ShortDescription { get; set; }

	    /// <summary>
	    /// The part to be used in the construction of url's for the Section.
	    /// </summary>
	    public string UrlIdentifier { get; set; }

	    /// <summary>
		/// Any document id's associated with this section at the root level. If a structure approach is required, use the categories.
		/// </summary>
		public List<long> Documents 
		{ 
			get 
			{ 
				if (_documents == null)
					RetrieveDocuments();

				return _documents; 
			}
		}

	    /// <summary>
	    /// Sections have favourite tags, those which are most important, i.e. more for generic concepts.
	    /// </summary>
	    public TagCollection FavouriteTags { get; set; }

	    /// <summary>
	    /// Provides access to the Channel which this Section is associated with.
	    /// </summary>
	    public IChannel ParentChannel { get; set; }

	    /// <summary>
	    /// If this Section is directly related to a Site then it is available here, otherwise you can navigate to the parent site through the parent channel.
	    /// </summary>
	    public ISite ParentSite { get; set; }

	    /// <summary>
	    /// Describes what type of content is listed in this Section.
	    /// </summary>
	    public ContentType ContentType { get; set; }

	    /// <summary>
		/// Provides convinient access to the newest documents within this Section.
		/// </summary>
		public ILatestDocuments LatestDocuments 
		{ 
			get { return _latestDocuments ?? (_latestDocuments = new LatestDocuments(this, 100)); }
		}

	    /// <summary>
	    /// If this Section is of type Generic, then it will need a default document to act as the index.
	    /// </summary>
	    public Document DefaultDocument { get; set; }

	    /// <summary>
	    /// Indicates the status of the Section.
	    /// </summary>
	    public DomainObjectStatus Status { get; set; }

	    /// <summary>
		/// Any featured-documents for this section.
		/// </summary>
		public IFeaturedDocumentsCollection FeaturedDocuments 
		{
			get { return _featuredDocuments ?? (_featuredDocuments = new FeaturedDocumentsCollection(this, 5)); }
		}

		/// <summary>
		/// An alphabetically-sorted collection of the most popular tags assigned to content within this section.
		/// </summary>
		public ITagCollection PopularTags
		{
			get
			{
				var timeDiff = DateTime.Now.Subtract(_lastPopularTagRetrievalTime);
				if (_popularTags == null || timeDiff.TotalHours > _cachePopularTagsPeriodHours)
					RetrievePopularTags();

				return _popularTags;
			}
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Section object.
		/// </summary>
		internal Section() 
		{
			ShortDescription = String.Empty;
			Name = String.Empty;
			UrlIdentifier = String.Empty;
			ContentType = ContentType.Generic;
			DerivedType = GetType();
			FavouriteTags = new TagCollection();
			Status = DomainObjectStatus.New;
			_cachePopularTagsPeriodHours = int.Parse(ConfigurationManager.AppSettings["Apollo.CachePopularTagsPeriodHours"]);
		}
		#endregion

		#region public methods
        ///// <summary>
        ///// Returns a container with documents for the section with a specific tag.
        ///// </summary>
        //public TaggedDocumentContainer GetDocumentsByTag(string tag) 
        //{
        //    return GetDocumentsByTag(Server.Instance.GetTag(tag));
        //}

        ///// <summary>
        ///// Returns a container with documents for the section with a specific tag.
        ///// </summary>
        //public TaggedDocumentContainer GetDocumentsByTag(ITag tag) 
        //{
        //    var cacheId = TaggedDocumentContainer.GetApplicationUniqueId(this, tag);
        //    var container = CacheManager.RetrieveItem(cacheId) as TaggedDocumentContainer;

        //    if (container == null)
        //    {
        //        container = new TaggedDocumentContainer(this, tag);
        //        if (container != null)
        //            CacheManager.AddItem(container, cacheId);
        //        else
        //            throw new Exception("Could not create TaggedDocumentContainer.");
        //    }

        //    return container;
        //}
		#endregion

		#region private methods
		/// <summary>
		/// Retrieves the list of document id's for those directly related to this section.
		/// </summary>
		private void RetrieveDocuments() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetSectionDocuments", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", Id));
			SqlDataReader reader = null;

            _documents = new List<long>();
            lock (_documents)
            {
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        Documents.Add(reader.GetInt64(reader.GetOrdinal("DocumentID")));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
		}

		/// <summary>
		/// Retrieves the most popular tags for this section, ordered alphabetically.
		/// </summary>
		private void RetrievePopularTags()
		{
			_popularTags = new TagCollection();
            lock (_popularTags)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetSectionPopularTags", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@SectionID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var tag = Server.Instance.GetTag((string)Sql.GetValue(typeof(string), reader["Tag"]));
                        tag.DocumentCount = (int)Sql.GetValue(typeof(int), reader["Occurrences"]);
                        _popularTags.Add(tag);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }

                _lastPopularTagRetrievalTime = DateTime.Now;
            }
		}
		#endregion
	}
}