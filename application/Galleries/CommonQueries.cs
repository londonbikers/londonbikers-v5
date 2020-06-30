using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Apollo.Models;

namespace Apollo.Galleries
{
	public class CommonQueries
	{
        public List<long> LatestPublicGalleriesForTag(string[] tags, int maxItems)
        {
            // going old-school: create that query manually!
            var galleries = new List<long>();
            var query = new StringBuilder();
            query.AppendFormat("SELECT TOP {0} [ID] FROM apollo_galleries WHERE f_status = 1 AND (", maxItems);
            for (var i = 0; i < tags.Length; i++)
            {
                query.AppendFormat("f_title LIKE '%{0}%' ", tags[i]);
                if (i < tags.Length - 1)
                    query.Append("OR ");
            }
            query.Append(") ORDER BY f_creation_date DESC");

            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var cmd = new SqlCommand(query.ToString(), connection);
            SqlDataReader reader = null;

            try
			{
				connection.Open();
				reader = cmd.ExecuteReader();

			    while (reader.Read())
			        galleries.Add((long) reader["ID"]);
			}
			finally
            {
                if (reader != null)
					reader.Close();

                connection.Close();
            }

            return galleries;
        }

		/// <summary>
		/// Returns a collection of the newest public Gallery objects.
		/// </summary>
		public List<Gallery> LatestPublicGalleries(int maxItems) 
		{
			var galleries = new List<Gallery>();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var cmd = new SqlCommand("LatestPublicGalleries", connection) {CommandType = CommandType.StoredProcedure};
		    var server = Server.Instance;
			SqlDataReader reader = null;
			var count = 0;

			try
			{
				connection.Open();
				reader = cmd.ExecuteReader();

			    while (reader.Read() && count < maxItems)
			    {
			        var gallery = server.GalleryServer.GetGallery((long)reader["ID"]);
			        if (gallery != null && gallery.Photos.Count > 0)
			            galleries.Add(gallery);

			        count++;
			    }
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return galleries;
		}
	}
}