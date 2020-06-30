using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Apollo.Models;
using Apollo.Utilities;

namespace Apollo.Users
{
	/// <summary>
	/// Provides functionality regarding User security.
	/// </summary>
	public class Security
	{
		#region accessors
	    /// <summary>
	    /// Returns a collection of Role objects. These represent the available security roles.
	    /// </summary>
	    public List<Role> Roles { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Security object.
		/// </summary>
		internal Security()
		{
			CollectRoles();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a specific Role object.
		/// </summary>
		public Role GetRole(string name)
		{
		    if (Roles == null)
            {
                Logger.LogWarning("Apollo.UserManagement.Security.GetRole() called when collection empty! Role was: " + name);
                CollectRoles();
            }

		    return Roles != null ? Roles.FirstOrDefault(role => role.Name == name) : null;
		}
	    #endregion

		#region private methods
		/// <summary>
		/// Builds a collection of Role objects.
		/// </summary>
		private void CollectRoles() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("CollectUserRoles", connection) {CommandType = CommandType.StoredProcedure};
		    SqlDataReader reader = null;

			Roles = new List<Role>();
            lock (Roles)
            {
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var role = new Role
                                       {
                                           Id = (int) Sql.GetValue(typeof (int), reader["ID"]),
                                           Name = (string) Sql.GetValue(typeof (string), reader["Name"])
                                       };
                        Roles.Add(role);
                    }
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