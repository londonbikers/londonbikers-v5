using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a collection of URL's for sites blacklisted.
	/// </summary>
	public class BlacklistedReferrersCollection : CollectionBase, IBlacklistedReferrersCollection
	{
		#region constructors
		/// <summary>
		/// Builds a collection with a list of the blacklisted referrers.
		/// </summary>
		internal BlacklistedReferrersCollection()
		{
			RetrieveBlacklistedReferrers();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Adds a URL to the collection.
		/// </summary>
		public bool Add(string url)
		{
		    if (Contains(url))
		        return false;

		    List.Add(url);
		    PersistBlacklistedReferrer(url);
		    return true;
		}


	    /// <summary>
		/// Removes a URL from the collection.
		/// </summary>
		public bool Remove(string url) 
		{
			for (var i = 0; i < List.Count; i++)
			{
			    if ((string) List[i] != url) continue;
			    List.RemoveAt(i);
			    UnpersistBlacklistedReferrer(url);
			    return true;
			}

			return false;
		}


		/// <summary>
		/// Determines whether or not the collection contains a specific URL.
		/// </summary>
		public bool Contains(string url)
		{
		    return List.Cast<string>().Any(localUrl => url.IndexOf(localUrl) > -1);
		}


	    /// <summary>
		/// Public default indexer.
		/// </summary>
		public string this[int index] 
		{
			get 
			{
				if (index > List.Count - 1)				
					throw new IndexOutOfRangeException();

				return (string)List[index];
			}
			set 
			{
				List[index] = value;
			}
		}
		#endregion

		#region private methods
		/// <summary>
		/// Queries the database for a list of the blacklisted referrers.
		/// </summary>
		public void RetrieveBlacklistedReferrers() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetBlacklistedReferrers", connection) {CommandType = CommandType.StoredProcedure};
		    SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			        List.Add(Sql.GetValue(typeof(string), reader["Url"]));
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}
		}


		/// <summary>
		/// Persists a new referrer back to the database.
		/// </summary>
		public void PersistBlacklistedReferrer(string url) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("InsertBlacklistedReferrer", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@Url", url));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			finally
			{
				connection.Close();
			}
		}


		/// <summary>
		/// Removes a specific referrer from the database.
		/// </summary>
		public void UnpersistBlacklistedReferrer(string url) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("DeleteBlacklistedReferrer", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@Url", url));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
			}
			finally
			{
				connection.Close();
			}
		}
		#endregion
	}
}