using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	public class User : IUser
	{
		#region accessors
	    /// <summary>
	    /// The email address for this User.
	    /// </summary>
	    public string Email { get; set; }

	    /// <summary>
	    /// The Username for this User. The value must be distinct in the system.
	    /// </summary>
	    public string Username { get; set; }

	    /// <summary>
	    /// The users christian name.
	    /// </summary>
	    public string Firstname { get; set; }

	    /// <summary>
	    /// The users surname.
	    /// </summary>
	    public string Lastname { get; set; }

	    /// <summary>
	    /// The users chosen login password. Represented as clear-text.
	    /// </summary>
	    public string Password { get; set; }

	    /// <summary>
	    /// The exact date the User was created in the system.
	    /// </summary>
	    public DateTime Created { get; set; }

	    /// <summary>
	    /// A collection of any Roles that the user may have associated with them.
	    /// </summary>
	    public ArrayList Roles { get; private set; }

	    /// <summary>
	    /// The unique identifier for this User.
	    /// </summary>
	    public Guid Uid { get; set; }

	    /// <summary>
	    /// A reference to the users account within the forums.
	    /// </summary>
	    public int ForumUserId { get; set; }

	    /// <summary>
	    /// The status of the User within the system.
	    /// </summary>
	    public UserStatus Status { get; set; }

	    /// <summary>
	    /// If the users avatar image file is stored off-site or not network-accessible then this will contain the uri reference. Otherwise use the loca path property.
	    /// </summary>
	    public Uri AvatarUri { get; set; }

	    /// <summary>
	    /// References the local or network-accessible user avatar image file.
	    /// </summary>
	    public string AvatarPath { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Returns a new user object.
		/// </summary>
		/// <param name="mode">Denotes whether or not this object is being retrieved or created.</param>
		internal User(ObjectCreationMode mode) 
		{
			Uid = Guid.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            Created = DateTime.Now;
            ForumUserId = 0;
            Status = UserStatus.Active;
            Roles = new ArrayList();
            AvatarPath = string.Empty;
			
			if (mode == ObjectCreationMode.New)
                Uid = Guid.NewGuid();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Immediately associate a Role with the User object.
		/// </summary>
		public void AddRole(Role role) 
		{
			if (HasRole(role))
			    return;
				
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("AddUserRole", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@UID", Uid));
			command.Parameters.Add(new SqlParameter("@RoleID", role.Id));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
                lock (Roles)
				    Roles.Add(role);
			}
			finally
			{
				connection.Close();
			}
		} 

		/// <summary>
		/// Determines whether or not the User has a Role already associated with them.
		/// </summary>
		public bool HasRole(Role role) 
		{
            lock (Roles)
			    if (Roles.Cast<Role>().Any(userRole => userRole.Id == role.Id))
			        return true;

			return false;
		}

		public bool HasRole(string roleName)
		{
			var role = Server.Instance.UserServer.Security.GetRole(roleName);
			return role != null && HasRole(role);
		}

		/// <summary>
		/// Immediately disassociates a Role with the User object.
		/// </summary>
		public void RemoveRole(Role role) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("RemoveUserRole", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@UID", Uid));
			command.Parameters.Add(new SqlParameter("@RoleID", role.Id));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				RemoveRoleFromCollection(role);
			}
	        finally
			{
				connection.Close();
			}
		} 
		#endregion

		#region private methods
		private void RemoveRoleFromCollection(Role role) 
		{
            lock (Roles)
            {
                foreach (var inspectionRole in Roles.Cast<Role>().Where(inspectionRole => inspectionRole.Id == role.Id))
                {
                    Roles.Remove(inspectionRole);
                    return;
                }
            }
		}
		#endregion
	}
}