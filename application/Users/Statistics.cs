using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Apollo.Utilities;

namespace Apollo.Users
{
	/// <summary>
	/// Provides brief statistical information on Users within the system.
	/// </summary>
	public class Statistics
	{
		#region members
		private int _users;
		private int _activeUsers;
		private int _suspendedUsers;
		private int _deletedUsers;
        private DateTime _lastForumStatsUpdate;
        private DateTime _lastMonthlyCommunityStatsUpdate;
        private MonthlyCommunityStats _cachedMonthlyCommunityStats;
        private List<CommunityPoster> _cachedTopForumPosters;
		#endregion

		#region accessors
		public int UsersCount 
		{
			get
			{
                if (_users == -1)
					BuildStatistics();

                return _users;
			}
		}
		public int ActiveUsersCount 
		{
			get
			{
                if (_activeUsers == -1)
                    BuildStatistics();

                return _activeUsers;
			}
		}
		public int SuspendedUsers 
		{
			get
			{
                if (_suspendedUsers == -1)
					BuildStatistics();

                return _suspendedUsers;
			}
		}
		public int DeletedUsers 
		{
			get
			{
                if (_deletedUsers == -1)
                    BuildStatistics();

                return _deletedUsers;
			}
		}
		#endregion

		#region constructors
		internal Statistics() 
		{
			_users = -1;
            _activeUsers = -1;
            _suspendedUsers = -1;
            _deletedUsers = -1;
            _lastForumStatsUpdate = DateTime.MinValue;
            _lastMonthlyCommunityStatsUpdate = DateTime.MinValue;
		}
		#endregion

        #region public methods
        /// <summary>
		/// Queries the system for the statistical data.
		/// </summary>
		public void BuildStatistics() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetUserStats", connection) {CommandType = CommandType.StoredProcedure};
            SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();
			    {
			        reader.Read();
			        _users = (int)Sql.GetValue(typeof(int), reader["TotalUsers"]);
			        _activeUsers = (int)Sql.GetValue(typeof(int), reader["ActiveUsers"]);
			        _suspendedUsers = (int)Sql.GetValue(typeof(int), reader["SuspendedUsers"]);
			        _deletedUsers = (int)Sql.GetValue(typeof(int), reader["DeletedUsers"]);
			    }
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}
		}

        /// <summary>
        /// Retrieves a set number of the top forum posters for the current month. Excluding moderators and admins.
        /// </summary>
        public List<CommunityPoster> GetTopForumPostersThisMonth(int maxResults)
        {
            // cache results for 24 hours.
            var timeDiff = DateTime.Now.Subtract(_lastForumStatsUpdate);
            if (_lastForumStatsUpdate != DateTime.MinValue && timeDiff.TotalDays < 1)
                return _cachedTopForumPosters;

            var posters = new List<CommunityPoster>();
            lock (posters)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetTopForumPostersForLastMonth", connection) {CommandType = CommandType.StoredProcedure};
                command.Parameters.Add(new SqlParameter("@MaxResults", maxResults));
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var poster = new CommunityPoster
                        {
                            ForumUserId = (int) Sql.GetValue(typeof (int), reader["UserID"]),
                            Username = Sql.GetValue(typeof (string), reader["Username"]) as string,
                            Posts = (int) Sql.GetValue(typeof (int), reader["TotalPosts"])
                        };
                        posters.Add(poster);
                    }

                    _lastForumStatsUpdate = DateTime.Now;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }

            _cachedTopForumPosters = posters;
            return posters;
        }

        /// <summary>
        /// Retrieves basic statistics for the community for the current month.
        /// </summary>
        public MonthlyCommunityStats GetMonthlyCommunityStats()
        {
            // cache results for 24 hours.
            var timeDiff = DateTime.Now.Subtract(_lastMonthlyCommunityStatsUpdate);
            if (_lastMonthlyCommunityStatsUpdate != DateTime.MinValue && timeDiff.TotalDays < 1)
                return _cachedMonthlyCommunityStats;

            _cachedMonthlyCommunityStats = new MonthlyCommunityStats();
            lock (_cachedMonthlyCommunityStats)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetMonthlyCommunityStats", connection) {CommandType = CommandType.StoredProcedure};
                SqlDataReader reader = null;

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    {
                        reader.Read();
                        _cachedMonthlyCommunityStats.NewMembersThisMonth = (int)Sql.GetValue(typeof(int), reader["NewUsers"]);
                        _cachedMonthlyCommunityStats.PostsThisMonth = (int)Sql.GetValue(typeof(int), reader["NewPosts"]);
                        _cachedMonthlyCommunityStats.PrivateMessagesThisMonth = (int)Sql.GetValue(typeof(int), reader["NewPMs"]);
                    }

                    _lastMonthlyCommunityStatsUpdate = DateTime.Now;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }

            return _cachedMonthlyCommunityStats;
        }
		#endregion
	}

    public class CommunityPoster
    {
        public int Posts { get; set; }
        public int ForumUserId { get; set; }
        public string Username { get; set; }
    }

    public class MonthlyCommunityStats
    {
        public int PostsThisMonth { get; set; }
        public int PrivateMessagesThisMonth { get; set; }
        public int NewMembersThisMonth { get; set; }
    }
}