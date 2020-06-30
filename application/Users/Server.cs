using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Apollo.Users
{
	public class UserServer
	{
		#region class members
		private readonly Security _security;
		private readonly Statistics _statistics;
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new UserServer object.
		/// </summary>
		internal UserServer() 
		{
			_security = new Security();
			_statistics = new Statistics();
		}
		#endregion

		#region accessors
		/// <summary>
		/// User security functionality.
		/// </summary>
		public Security Security { get { return _security; } }
		/// <summary>
		/// Provides access to statistical data on Users.
		/// </summary>
		public Statistics Statistics { get { return _statistics; } }
		#endregion

		#region public methods
		/// <summary>
		/// Returns a new instance of the UserFinder object.
		/// </summary>
		public UserFinder NewUserFinder() 
		{
			return new UserFinder();
		}

		/// <summary>
		/// Returns a populated User object by using the UID to identify them.
		/// </summary>
		public User GetUser(Guid uid) 
		{
			return GetUserByField("GetUser", uid);
		} 
		
		/// <summary>
		/// Returns a populated User object by using the Username to identify them.
		/// </summary>
		public User GetUser(string username) 
		{
			return GetUserByField("GetUserByUsername", username);
		}

		/// <summary>
		/// Returns a populated User object by using the registered E-mail address to identify them.
		/// </summary>
		public User GetUserByEmail(string email) 
		{
			return GetUserByField("GetUserByEmail", email);
		} 

		/// <summary>
		/// Returns a populated User object by using the Forum ID to identify them.
		/// </summary>
		public User GetUser(int forumUserId) 
		{
			return GetUserByField("GetUserByForumID", forumUserId);
		}

		/// <summary>
		/// Returns an empty User object, ready for use.
		/// </summary>
		public User NewUser() 
		{
			return new User(ObjectCreationMode.New);
		}
		
		/// <summary>
		/// Persists any changes made to a User object.
		/// </summary>
		public void UpdateUser(User user) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UpdateUser", connection) {CommandType = CommandType.StoredProcedure};

		    command.Parameters.Add(new SqlParameter("@UID", user.Uid));
			command.Parameters.Add(new SqlParameter("@Firstname", user.Firstname));
			command.Parameters.Add(new SqlParameter("@Lastname", user.Lastname));
			command.Parameters.Add(new SqlParameter("@Password", user.Password));
			command.Parameters.Add(new SqlParameter("@Email", user.Email));
			command.Parameters.Add(new SqlParameter("@Username", user.Username));
			command.Parameters.Add(new SqlParameter("@Status", (int)user.Status));
			command.Parameters.Add(new SqlParameter("@ForumUserID", user.ForumUserId));
			
			try
			{
				connection.Open();
				command.ExecuteNonQuery();

                if (user.ForumUserId > 0)
                {
                    //var forumUser = InstantASP.InstantForum.Business.User.SelectUser(user.ForumUserId);
                    //if (forumUser.EmailAddress != user.Email)
                    //    InstantASP.Common.Business.User.UpdateEmailAddress(user.ForumUserId, user.Email);

                    //if (forumUser.Password != user.Password)
                    //    InstantASP.Common.Business.User.UpdatePassword(user.ForumUserId, user.Password);

                    //if (forumUser.Username != user.Username)
                    //    InstantASP.Common.Business.User.UpdateUsername(user.ForumUserId, user.Username);
                }
                else
                {
                    Logger.LogWarning(string.Format("Updating a user ({0}), no forum id found to sync user with.", user.Username));
                }

                // update the stats in-case this is a new or status-changed user.
                Statistics.BuildStatistics();
			}
			finally
			{
				connection.Close();
			}
		} 
		#endregion

		#region private methods
	    /// <summary>
	    /// Returns a populated User object, identified by a particular property.
	    /// </summary>
	    /// <param name="storedProcedure">The name of the SQL procedure to run to get the user data.</param>
	    /// <param name="parameter">The parameter used to get the User object.</param>
	    private static User GetUserByField(string storedProcedure, object parameter) 
		{
			var forumId = 0;
			var uid = Guid.Empty;
			var email = string.Empty;
			var username = string.Empty;

			// validate the parameter.
			switch (storedProcedure)
			{
			    case "GetUserByEmail":
			    case "GetUserByUsername":
			        if (storedProcedure == "GetUserByUsername")
			            username = parameter as string;
			        else
			            email = parameter as string;
			        break;
			    case "GetUserByForumID":
			        forumId = (int)parameter;
			        break;
			    default:
			        uid = (Guid)parameter;
			        break;
			}

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand(storedProcedure, connection) {CommandType = CommandType.StoredProcedure};
	        SqlDataReader reader = null;
			User user = null;

			switch (storedProcedure)
			{
			    case "GetUser":
			        command.Parameters.Add(new SqlParameter("@UID", uid));
			        break;
			    case "GetUserByForumID":
			        command.Parameters.Add(new SqlParameter("@ForumID", forumId));
			        break;
			    case "GetUserByUsername":
			        command.Parameters.Add(new SqlParameter("@username", username));
			        break;
			    case "GetUserByEmail":
			        command.Parameters.Add(new SqlParameter("@email", email));
			        break;
			}

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    if (reader.Read())
			    {
			        // build the user object.
			        user = new User(ObjectCreationMode.Retrieve)
			        {
			            Uid = (Guid) Sql.GetValue(typeof (Guid), reader["f_uid"]),
			            Firstname = (string) Sql.GetValue(typeof (string), reader["f_firstname"]),
			            Lastname = (string) Sql.GetValue(typeof (string), reader["f_lastname"]),
			            Email = (string) Sql.GetValue(typeof (string), reader["f_email"]),
			            Username = (string) Sql.GetValue(typeof (string), reader["f_username"]),
			            Password = (string) Sql.GetValue(typeof (string), reader["f_password"]),
			            Created = (DateTime) Sql.GetValue(typeof (DateTime), reader["f_created"]),
			            ForumUserId = (int) Sql.GetValue(typeof (int), reader["ForumUserID"]),
			            Status = (UserStatus) Enum.Parse(typeof (UserStatus), reader["Status"].ToString())
			        };

			        // forum specific avatar
                    //var avatarUrl = (string)Sql.GetValue(typeof(string), reader["AvatarURL"]);
                    //if (!string.IsNullOrEmpty(avatarUrl))
                    //    user.AvatarUri = new Uri((string)Sql.GetValue(typeof(string), reader["AvatarURL"]));

			        // attempt to convert avatar Uri to local path.
			        if (user.AvatarUri != null && user.AvatarUri.AbsoluteUri.StartsWith(ConfigurationManager.AppSettings["Global.SiteURL"]))
			        {
			            // v3 or v4 folder structure?
			            var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			            if (appPath != null) appPath = appPath.Substring(0, appPath.LastIndexOf("\\bin"));
			            var filename = WebUtils.PageNameFromUrl(user.AvatarUri.AbsoluteUri);

			            if (user.AvatarUri.AbsolutePath.ToLower().StartsWith("/forums/attachments"))
			            {
			                // v3
			                user.AvatarPath = appPath + @"\forums\attachments\" + filename;
			            }
			            else
			            {
			                // v4
			                user.AvatarPath = appPath + @"\forums\uploads\avatars\" + filename;
			            }
			        }
			    }

			    // read in the user roles.
			    if (reader.NextResult())
			    {
			        while (reader.Read())
			        {
			            var role = new Role
			            {
			                Id = (int) Sql.GetValue(typeof (int), reader["ID"]),
			                Name = (string) Sql.GetValue(typeof (string), reader["Name"])
			            };

			            // use this method to avoid persistence.
			            if (user != null) user.Roles.Add(role);
			        }
			    }
			    else
			    {
			        Logger.LogWarning("No user roles query part found when building user: GetUserByField()");
			    }
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

	        return user; 
		}
		#endregion
	}
}