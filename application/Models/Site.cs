using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a website, a place where content is published.
	/// </summary>
	public class Site : CommonBase, ISite
	{
		#region members
	    private List<ISection> _sections;
		private List<IChannel> _channels;
		private List<IGalleryCategory> _rootGalleryCategories;
		private LatestGalleries _latestGalleries;
		#endregion

		#region accessors
	    /// <summary>
	    /// The friendly name for this Site.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// The fully-qualified URL for this site.
	    /// </summary>
	    public string Url { get; set; }

	    /// <summary>
		/// The collection of Channels associated with this Site.
		/// </summary>
		public List<IChannel> Channels 
		{ 
			get 
			{
                if (_channels == null)
					RetrieveChannels();

                return _channels; 
			} 
		}
		/// <summary>
		/// The collection of Section objects associated with this Channel.
		/// </summary>
		public List<ISection> Sections 
		{ 
			get 
			{ 
				if (_sections == null)
					RetrieveSections();

                return _sections; 
			} 
		}
		/// <summary>
		/// The collection of root categories for all channels.
		/// </summary>
		public List<IGalleryCategory> GalleryCategories 
		{
			get
			{
                if (_rootGalleryCategories == null)
					RetrieveGalleryCategories();

                return _rootGalleryCategories;
			}
		}
		/// <summary>
		/// Provides access to the latest galleries for the whole Site.
		/// </summary>
		public LatestGalleries RecentGalleries 
		{
			get { return _latestGalleries ?? (_latestGalleries = new LatestGalleries(this)); }
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new Site object.
		/// </summary>
		internal Site() 
		{
			Name = string.Empty;
			Url = string.Empty;
			DerivedType = GetType();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Re-loads the gallery categories for this site.
		/// </summary>
		public void RefreshGalleryCategories() 
		{
			RetrieveGalleryCategories();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Retrieves the Channels associated with this Site.
		/// </summary>
		private void RetrieveChannels() 
		{
			_channels = new List<IChannel>();
            lock (_channels)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetSiteChannels", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@SiteID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        Channels.Add(Server.Instance.GetChannel((int)Sql.GetValue(typeof(int), reader["ID"])));
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
		/// Retrieves the root gallery categories for all Channel's associated with this Site.
		/// </summary>
		private void RetrieveGalleryCategories() 
		{
			_rootGalleryCategories = new List<IGalleryCategory>();
            lock (_rootGalleryCategories)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetSiteGalleryCategories", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@SiteID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        _rootGalleryCategories.Add(Server.Instance.GalleryServer.GetCategory((long)Sql.GetValue(typeof(long), reader[0])));
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
		/// Retrieves the Sections associated with this Channel.
		/// </summary>
		private void RetrieveSections() 
		{
			_sections = new List<ISection>();
            lock (_sections)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetSiteSections", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@SiteID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        _sections.Add(Server.Instance.ContentServer.GetSection((int)Sql.GetValue(typeof(int), reader["ID"])));
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