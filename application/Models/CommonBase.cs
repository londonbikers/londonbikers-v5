using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Common functionality used by the domain objects.
	/// </summary>
	public abstract class CommonBase : ICommonBase
	{
		#region members
	    protected CommonBase()
	    {
	        IsPersisted = true;
	    }
	    #endregion

		#region accessors
	    /// <summary>
	    /// Internal to Apollo, determines if the consuming object requires persistance.
	    /// </summary>
	    public bool IsPersisted { get; set; }

	    /// <summary>
	    /// For application efficiency, this allows the controllers to determine if the derived object needs updating or not.
	    /// </summary>
	    public bool HasChanged { get; set; }

	    /// <summary>
	    /// The Type of the derived class. Used for generating unique id's.
	    /// </summary>
	    public Type DerivedType { get; set; }

	    /// <summary>
	    /// The identifier for the derived object.
	    /// </summary>
	    public long Id { get; set; }

	    /// <summary>
	    /// The number of times the content has been viewed.
	    /// </summary>
	    public long Views { get; internal set; }

	    /// <summary>
	    /// For GUID based consumers, the GUID ID should be passed through to enable correct ApplicationUniqueID's.
	    /// </summary>
	    public Guid LegacyId { get; set; }

	    /// <summary>
		/// Generates a unique ID for the derived object within the context of the application. Used for objects that implement numeric ID's.
		/// </summary>
		public string ApplicationUniqueId 
		{ 
			get 
			{ 
				var id = (Id > 0) ? Id.ToString() : LegacyId.ToString();
				return DerivedType.FullName + ":" + id;
			} 
		}

	    /// <summary>
	    /// The current mode the object is operating under.
	    /// </summary>
	    public ObjectMode ObjectMode { get; set; }
	    #endregion

		#region public methods
		/// <summary>
		/// A User is able to vote for a piece of content to show their approval or interest. There can only be
		/// one voting per User, though if a user has already voted, no vote will be made an no response will be given.
		/// </summary>
		/// <param name="user">The User who is voting for the piece of content.</param>
		public void MarkVoted(User user)
		{
			if (user == null || user.Uid == Guid.Empty)
				throw new ArgumentException("User cannot be null or unpersisted.");

            throw new NotImplementedException("CommonBase.MarkVoted() Not implemented!");
		}

		/// <summary>
		/// Marks the content as has being viewed. Members of staff do not have views counted.
		/// </summary>
		/// <param name="user">
		/// If present (null otherwise), this allows the application to determine whether or not the
		/// person is a member of staff.
		/// </param>
		public void MarkViewed(User user)
		{
			// we don't log staff views.
			if (user != null && user.HasRole("staff"))
				return;

			// not all domain-objects are supported.
			var doType = GetDomainObjectType();
            if (doType != DomainObjectType.Document &&
                doType != DomainObjectType.DocumentImage &&
                doType != DomainObjectType.GalleryImage &&
                doType != DomainObjectType.DirectoryItem)
            {
                Logger.LogWarning("CommonBase.MarkView() received doType of unknown type: " + doType);
                return;
            }

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("MarkContentViewed", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ContentID", Id));
			command.Parameters.Add(new SqlParameter("@ContentTypeID", (byte)doType));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				Views++;
			}
			finally
			{
				connection.Close();
			}
		}
		
        public DomainObjectType GetDomainObjectType()
		{
		    if (DerivedType == typeof(Document))
				return DomainObjectType.Document;
		    if (DerivedType == typeof(EditorialImage))
		        return DomainObjectType.DocumentImage;
		    if (DerivedType == typeof(GalleryImage))
		        return DomainObjectType.GalleryImage;
		    return DerivedType == typeof(DirectoryItem) ? DomainObjectType.DirectoryItem : DomainObjectType.Unknown;
		}
	    #endregion
	}
}