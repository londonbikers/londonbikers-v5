using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models;

namespace Apollo.Directory
{
	/// <summary>
	/// Summary description for CommonQueries.
	/// </summary>
	public class CommonQueries
	{
		#region constructors
		/// <summary>
		/// Returns a new CommonQueries object.
		/// </summary>
		internal CommonQueries()
		{
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of Categories which have no parent, i.e. the top-level ones.
		/// </summary>
		public DirectoryCategoryCollection RootCategories() 
		{
			var categories = new DirectoryCategoryCollection();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetRootDirectoryCategories", connection) {CommandType = CommandType.StoredProcedure};
		    SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			        categories.Add(Server.Instance.DirectoryServer.GetCategory((long)reader["ID"]), false);
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return categories;
		}
		#endregion
	}
}
