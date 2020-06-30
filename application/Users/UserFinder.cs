using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Apollo.Users
{
	public class UserFinder : Finder
	{
		#region constructors
		/// <summary>
		/// Creates a new UserFinder object.
		/// </summary>
		public UserFinder()
		{
			var fields = new Hashtable();
			fields["FirstName"] = "f_firstname";
			fields["LastName"] = "f_lastname";
			fields["Password"] = "f_password";
			fields["Email"] = "f_email";
			fields["Username"] = "f_username";
			fields["Created"] = "f_created";
			fields["ForumUsername"] = "ForumUsername";
			fields["ForumPassword"] = "ForumPassword";

            PrimaryKey = "f_uid";
			Fields = fields;
			SqlTable = "apollo_users";
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of User's who have the Staff role(s).
		/// </summary>
		public List<Guid> GetStaff() 
		{
			var command = new SqlCommand("GetUsersByRole") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@Role", "staff"));
			return PerformLegacyCustomQuery(command);
		}
		#endregion
	}
}