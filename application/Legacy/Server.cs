using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models;
using Apollo.Utilities;

namespace Apollo.Legacy
{
    public class LegacyServer
    {
        #region constructors
        internal LegacyServer()
        {
        }
        #endregion

        #region public method
        /// <summary>
        /// Attempts to convert 
        /// </summary>
        /// <param name="type">The DocumentType representing the type of object we're trying to translate for.</param>
        /// <param name="legacyUid">The old guid identifier to be translated into the new numeric id.</param>
        public long[] GetNewIdForLegacyUid(DomainObjectType type, Guid legacyUid)
        {
            var newIDs = new long[2];
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            SqlDataReader reader = null;
            var command = new SqlCommand("ConvertLegacyID", connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(new SqlParameter("@UID", legacyUid));
            command.Parameters.Add(new SqlParameter("@DomainObjectType", type.ToString()));

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newIDs[0] = (long)Sql.GetValue(typeof(long), reader[0]);
                    if (type == DomainObjectType.DocumentImage)
                        newIDs[1] = (long)Sql.GetValue(typeof(long), reader[1]);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return newIDs;
        }
        #endregion
    }
}