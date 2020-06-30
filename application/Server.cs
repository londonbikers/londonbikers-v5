using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.BackgroundTasks;
using Apollo.Comments;
using Apollo.Models;
using Apollo.Models.Interfaces;
using Apollo.Users;
using Apollo.Utilities;
using Apollo.Assets;
using Apollo.Content;
using Apollo.Directory;
using Apollo.Galleries;
using Apollo.Legacy;
using Apollo.Communication;
using Apollo.Caching;

namespace Apollo
{
	/// <summary>
	/// The Apollo application starting point. Employs a singleton pattern.
	/// </summary>
	public class Server
	{
		#region members
	    private List<ISite> _sites;
		#endregion

		#region accessors
	    /// <summary>
	    /// Provides access to the asset server, responsible for managing all externally-accessed assets.
	    /// </summary>
	    public AssetServer AssetServer { get; private set; }

	    /// <summary>
	    /// Provides access to the content server, responsible for managing all editorial content.
	    /// </summary>
	    public ContentServer ContentServer { get; private set; }

	    /// <summary>
	    /// Represents various collections of common application object types.
	    /// </summary>
	    public ApolloTypes Types { get; private set; }

	    /// <summary>
	    /// Provides access to the user server,+ responsible for managing all of the Users.
	    /// </summary>
	    public UserServer UserServer { get; private set; }

	    /// <summary>
	    /// Provides access to the gallery server, responsible for managing all of the photo/video Galleries.
	    /// </summary>
	    public GalleryServer GalleryServer { get; private set; }

	    /// <summary>
	    /// Provides access to the directory server, responsible for managing all of the Directory.
	    /// </summary>
	    public DirectoryServer DirectoryServer { get; private set; }

	    /// <summary>
	    /// Provides access to the communication server, responsible for all external communications.
	    /// </summary>
	    public CommunicationServer CommunicationServer { get; private set; }

	    /// <summary>
	    /// Provides access to legacy services or operations.
	    /// </summary>
	    public LegacyServer LegacyServer { get; private set; }

	    /// <summary>
	    /// Provides access to caching related functionality.
	    /// </summary>
	    public CacheServer CacheServer { get; private set; }

	    /// <summary>
	    /// Provides access to the MemberCommentsServer.
	    /// </summary>
	    public MemberCommentsServer MemberCommentsServer { get; private set; }

	    /// <summary>
	    /// Retrieves the single instance of the Apollo application.
	    /// </summary>
	    public static Server Instance { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new instance of the Apollo application.
		/// </summary>
		protected Server() 
		{
            Types = new ApolloTypes();
            UserServer = new UserServer();
            CacheServer = new CacheServer();
            AssetServer = new AssetServer();
            GalleryServer = new GalleryServer();
            ContentServer = new ContentServer();
            DirectoryServer = new DirectoryServer();
            LegacyServer = new LegacyServer();
			CommunicationServer = new CommunicationServer();
            MemberCommentsServer = new MemberCommentsServer();

            // background tasks.
            new ArrayList { new PopularContentProcessor() };
		}

	    static Server()
	    {
	        Instance = new Server();
	    }
	    #endregion

		#region public methods
		/// <summary>
		/// Retrieves a collection of the sites managed by the application.
		/// </summary>
		public List<ISite> GetSites() 
		{
			if (_sites != null)
				return _sites;

			// sites not cached, retrieve afresh.
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetSites", connection) {CommandType = CommandType.StoredProcedure};
		    SqlDataReader reader = null;

			_sites = new List<ISite>();
            lock (_sites)
            {
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    if (reader.Read())
                        _sites.Add(GetSite((int)Sql.GetValue(typeof(int), reader["ID"])));
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }

            return _sites;
		}

		/// <summary>
		/// Retrieves a specific Site object.
		/// </summary>
		/// <param name="siteId">The numeric ID for the site.</param>
		public ISite GetSite(int siteId) 
		{
			var site = CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof(Site), siteId)) as Site;
			if (site == null)
			{
				// site not in the cache, retrieve from database.
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetSite", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@ID", siteId));
				SqlDataReader reader = null;

				try
				{
					connection.Open();
					reader = command.ExecuteReader();

			        if (reader.Read())
			        {
			            site = new Site
                        {
                            Id = siteId,
                            Name = (string) Sql.GetValue(typeof (string), reader["Name"]),
                            Url = (string) Sql.GetValue(typeof (string), reader["URL"])
                        };
			        }

				    // add the site to the cache.
					if (site != null)
						CacheManager.AddItem(site, site.ApplicationUniqueId);
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}

			return site;
		}

		/// <summary>
		/// Retrieves a specific Channel object.
		/// </summary>
		/// <param name="channelId">The numeric ID for the channel.</param>
		public IChannel GetChannel(int channelId) 
		{
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(Channel), channelId);
			var channel = CacheManager.RetrieveItem(cacheId) as Channel;

			if (channel == null)
			{
				// site not in the cache, retrieve from database.
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				var command = new SqlCommand("GetChannel", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@ID", channelId));
				SqlDataReader reader = null;

				try
				{
					connection.Open();
					reader = command.ExecuteReader();

					if (reader.Read())
					{
						channel = new Channel
                        {
                            Id = (int) Sql.GetValue(typeof (int), reader["ID"]),
                            Name = (string) Sql.GetValue(typeof (string), reader["Name"]),
                            ShortDescription = (string) Sql.GetValue(typeof (string), reader["ShortDescription"]),
                            ParentSiteId = (int) Sql.GetValue(typeof (int), reader["SiteID"])
                        };
					}

					// add the channel to the cache.
					if (channel != null)
                        CacheManager.AddItem(channel, cacheId);
				}
				finally
				{
				    if (reader != null)
						reader.Close();

				    connection.Close();
				}
			}

			return channel;
		}

		/// <summary>
		/// Retrieves a Tag object.
		/// </summary>
		public ITag GetTag(string name) 
		{
            // a tag is quite a simple object to create, the collection properties are all lazy-loaded.
            var cacheId = CacheManager.GetApplicationUniqueId(typeof(Tag), name);
            var tag = CacheManager.RetrieveItem(cacheId) as Tag;
            if (tag == null)
            {
                tag = new Tag { Name = name.Trim().ToLower() };
                CacheManager.AddItem(tag, cacheId);
            }

		    return tag;
		}

		/// <summary>
		/// Returns a new TagCollection object.
		/// </summary>
		public ITagCollection GetTagCollection() 
		{
			return new TagCollection();
		}
		#endregion
	}
}