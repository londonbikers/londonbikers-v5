using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a major topic area, contains gallery categories and sections.
	/// </summary>
	public class Channel : CommonBase, IChannel
	{
		#region members
		private List<ISection> _sections;
	    private ISection _newsSection;
		private ISection _articlesSection;
		private ISite _parentSite;
	    #endregion

		#region accessors
	    /// <summary>
	    /// The public name of the Channel.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// A short description that can be used for leading information.
	    /// </summary>
	    public string ShortDescription { get; set; }

	    /// <summary>
	    /// The part to be used in the construction of url's for the Channel.
	    /// </summary>
	    public string UrlIdentifier { get; set; }

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
		/// The section specifically for news.
		/// </summary>
		public ISection News 
		{ 
			get 
			{ 
				if (_newsSection == null)
				{
					foreach (var section in Sections.Where(section => section.ContentType == ContentType.News))
					{
					    _newsSection = section;
					    break;
					}
				}

				return _newsSection; 
			} 
		}
		/// <summary>
		/// The section specifically for articles.
		/// </summary>
		public ISection Articles 
		{ 
			get 
			{ 
				if (_articlesSection == null)
				{
					foreach (var section in Sections.Where(section => section.ContentType == ContentType.Article))
					{
					    _articlesSection = section;
					    break;
					}
				}

				return _articlesSection; 
			} 
		}
		/// <summary>
		/// The Site that this Channel belongs to.
		/// </summary>
		public ISite ParentSite 
		{
			get
			{
				if (_parentSite == null && ParentSiteId > 0)
					_parentSite = Server.Instance.GetSite(ParentSiteId);

				return _parentSite;
			}
			set
			{
				_parentSite = value;
			}
		}

	    /// <summary>
	    /// Used internally to allow the ParentSite to be lazy-loaded at a later time.
	    /// </summary>
	    internal int ParentSiteId { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Channel object.
		/// </summary>
		internal Channel() 
		{
			Name = string.Empty;
			UrlIdentifier = string.Empty;
			ShortDescription = string.Empty;
			DerivedType = GetType();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Retrieves the Sections associated with this Channel.
		/// </summary>
		private void RetrieveSections() 
		{
			_sections = new List<ISection>();
            lock (_sections)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetChannelSections", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@ChannelID", Id));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        Sections.Add(Server.Instance.ContentServer.GetSection((int)Sql.GetValue(typeof(int), reader["ID"])));
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