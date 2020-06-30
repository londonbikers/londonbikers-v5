using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace Apollo.Utilities
{
	/// <summary>
	/// Persists extraordinary application events to a log.
	/// </summary>
	public class Logger
	{
		#region public methods
		/// <summary>
		/// Persists the occurance of an exception being thrown to the log.
		/// </summary>
		/// <param name="exception">The specific exception that was thrown.</param>
		public static void LogException(Exception exception) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("InsertLogEntry", connection) {CommandType = CommandType.StoredProcedure};

		    var logTypeIdParam = new SqlParameter("@LogTypeID", SqlDbType.Int) {Value = 1};
		    command.Parameters.Add(logTypeIdParam);

			var messageParam = new SqlParameter("@Message", SqlDbType.Text) {Value = exception.Message};
		    command.Parameters.Add(messageParam);

			var stackTraceParam = new SqlParameter("@StackTrace", SqlDbType.Text) {Value = exception.StackTrace};
		    command.Parameters.Add(stackTraceParam);

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
		/// Persists the occurance of an exception being thrown to the log.
		/// </summary>
		/// <param name="exception">The specific exception that was thrown.</param>
		/// <param name="contextualInfo">Any information relevant to the context of the Exception.</param>
		public static void LogException(Exception exception, string contextualInfo) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("InsertLogEntry", connection) {CommandType = CommandType.StoredProcedure};

		    var logTypeIdParam = new SqlParameter("@LogTypeID", SqlDbType.Int) {Value = 1};
		    command.Parameters.Add(logTypeIdParam);

			var messageParam = new SqlParameter("@Message", SqlDbType.Text) {Value = exception.Message};
		    command.Parameters.Add(messageParam);

			var stackTraceParam = new SqlParameter("@StackTrace", SqlDbType.Text) {Value = exception.StackTrace};
		    command.Parameters.Add(stackTraceParam);

			var contextParam = new SqlParameter("@Context", SqlDbType.Text) {Value = contextualInfo};
		    command.Parameters.Add(contextParam);

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
		/// Persists the occurance of a warning being thrown to the log.
		/// </summary>
		/// <param name="warningText">Any information relevant to the warning being thrown.</param>
		public static void LogWarning(string warningText) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("InsertLogEntry", connection) {CommandType = CommandType.StoredProcedure};

		    var logTypeIdParam = new SqlParameter("@LogTypeID", SqlDbType.Int) {Value = 2};
		    command.Parameters.Add(logTypeIdParam);

			var messageParam = new SqlParameter("@Message", SqlDbType.Text) {Value = warningText};
		    command.Parameters.Add(messageParam);

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
		/// Retrieves a collection of the latest log entries.
		/// </summary>
		/// <param name="maximum">The maximum number of log entries to retrieve. 100 by default.</param>
		public static ArrayList GetLogs(int maximum) 
		{
			var logs = new ArrayList();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetLatestLogEntries", connection);
			SqlDataReader reader = null;
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(new SqlParameter("@Maximum", maximum));

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			    {
			        var log = new LogEntry
			        {
			            Id = (int) Sql.GetValue(typeof (int), reader["LogID"]),
			            Message = Sql.GetValue(typeof (string), reader["Message"]) as string,
			            StackTrace = Sql.GetValue(typeof (string), reader["StackTrace"]) as string,
			            Context = Sql.GetValue(typeof (string), reader["Context"]) as string,
			            When = (DateTime) Sql.GetValue(typeof (DateTime), reader["When"]),
			            Type = (LogEntryType) Enum.Parse(typeof (LogEntryType), Enum.GetName(typeof (LogEntryType), (byte) reader["Type"]))
			        };

			        logs.Add(log);
			    }
			}
			catch (Exception ex)
			{
				// oh the potential irony.
				LogException(ex, "Logger.GetLogs()");
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return logs;
		}

		/// <summary>
		/// Retrieves a specific log entry from the database.
		/// </summary>
		/// <param name="id">The identifier for the log to retrieve.</param>
		public static LogEntry GetLog(int id) 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetLog", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ID", id));
			SqlDataReader reader = null;
			LogEntry log = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    if (reader.Read())
			    {
			        log = new LogEntry
			        {
			            Id = (int) Sql.GetValue(typeof (int), reader["LogID"]),
			            Message = Sql.GetValue(typeof (string), reader["Message"]) as string,
			            StackTrace = Sql.GetValue(typeof (string), reader["StackTrace"]) as string,
			            Context = Sql.GetValue(typeof (string), reader["Context"]) as string,
			            When = (DateTime) Sql.GetValue(typeof (DateTime), reader["When"]),
			            Type = (LogEntryType) Enum.Parse(typeof (LogEntryType), Enum.GetName(typeof (LogEntryType), (byte) reader["Type"]))
			        };
			    }
			}
			catch (Exception ex)
			{
				// oh the potential irony.
				LogException(ex, "Logger.GetLog()");
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return log;
		}
		#endregion

		/// <summary>
		/// Denotes the type of a LogEntry.
		/// </summary>
		public enum LogEntryType 
		{
			Warning = 1,
			Error = 2
		}

		/// <summary>
		/// Represents an entry within the Log database.
		/// </summary>
		public class LogEntry 
		{
		    /// <summary>
		    /// The identifier for this LogEntry.
		    /// </summary>
		    public int Id { get; set; }

		    /// <summary>
		    /// When the log instance occured.
		    /// </summary>
		    public DateTime When { get; set; }

		    /// <summary>
		    /// The type of this LogEntry.
		    /// </summary>
		    public LogEntryType Type { get; set; }

		    /// <summary>
		    /// The top-level descriptor for this entry.
		    /// </summary>
		    public string Message { get; set; }

		    /// <summary>
		    /// Any full stack-trace for this entry.
		    /// </summary>
		    public string StackTrace { get; set; }

		    /// <summary>
		    /// The context of this entry, i.e. the source or handler.
		    /// </summary>
		    public string Context { get; set; }
		}
	}
}