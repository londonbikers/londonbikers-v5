using System;
using System.Data.SqlClient;

namespace Apollo.Utilities
{
	public class Sql
	{
		/// <summary>
		/// Determines if a SqlDataReader result set has a specific column within it.
		/// </summary>
		/// <param name="reader">The executed and record-advanced SqlDataReader/</param>
		/// <param name="columnName">The name of the SQL column to check for.</param>
		public static bool DoesColumnExistInResult(SqlDataReader reader, string columnName) 
		{
			try
			{
                #pragma warning disable 168
			    var val = reader[columnName];
                #pragma warning restore 168
			    return true;
			}
			catch (IndexOutOfRangeException)
			{
				return false;
			}
		}

		public static Guid GetGuid(object potentialGuid) 
		{
			Guid guid;
			if (potentialGuid == null || potentialGuid is DBNull)
				guid = Guid.Empty;
			else
				guid = (Guid)potentialGuid;

			return guid; 
		} 

		public static string ToSqlDateTime(DateTime date) 
		{
			return ToUsFormatDateString(date); 
		} 

		private static string ToUsFormatDateString(DateTime date) 
		{
		    var array1 = new string[11];
			var num1 = date.Month;
			array1[0] = num1.ToString();
			array1[1] = "/";
			var num2 = date.Day;
			array1[2] = num2.ToString();
			array1[3] = "/";
			var num3 = date.Year;
			array1[4] = num3.ToString();
			array1[5] = " ";
			var num4 = date.Hour;
			array1[6] = num4.ToString();
			array1[7] = ":";
			var num5 = date.Minute;
			array1[8] = num5.ToString();
			array1[9] = ":";
			var num6 = date.Second;
			array1[10] = num6.ToString();
			var text1 = string.Concat(array1);
			var text2 = text1;
			return text2; 
		} 

		/// <summary>
		/// Helps with the retrival of values from SQL Server by converting null values to the equivilent
		/// .net empty value.
		/// </summary>
		/// <param name="type">The Type of the source data.</param>
		/// <param name="databaseValue">The value taken from a DataReader, or the like.</param>
		/// <returns>The equivilent .net object in either value or empty form.</returns>
		public static object GetValue(Type type, object databaseValue)
		{
		    if (type.Equals(typeof(bool)) && databaseValue == DBNull.Value)
				return false;
		    if (type.Equals(typeof(Int16)) && databaseValue == DBNull.Value ||
		        type.Equals(typeof(Int32)) && databaseValue == DBNull.Value ||
		        type.Equals(typeof(Int64)) && databaseValue == DBNull.Value ||
		        type.Equals(typeof(decimal)) && databaseValue == DBNull.Value ||
		        type.Equals(typeof(byte)) && databaseValue == DBNull.Value ||
		        type.Equals(typeof(long)) && databaseValue == DBNull.Value)
		        return 0;
		    if (type.Equals(typeof(double)) && databaseValue == DBNull.Value)
		        return double.Parse("0.0");
		    if (type.Equals(typeof(string)) && databaseValue == DBNull.Value)
		        return String.Empty;
		    if (type.Equals(typeof(DateTime)) && databaseValue == DBNull.Value)
		        return DateTime.MinValue;
		    if (type.Equals(typeof(Guid)) && databaseValue == DBNull.Value)
		        return Guid.Empty;

		    return databaseValue;
		}

	    /// <summary> 
		/// Ensures that any literal being used in a plain-text SQL query is properly escaped so that 
		/// a SQL Server error is not thrown when the query is executed. 
		/// </summary> 
		/// <param name="criteria">The string representing the criteria</param> 
		/// <returns>An escaped SQL criteria string.</returns> 
		public static string EscapePlainTextQueryCriteria(string criteria) 
		{ 
			return criteria.Replace("'", "''"); 
		} 

		/// <summary> 
		/// Functions as EscapePlainTextQueryCriteria() does, but tailored specifically for LIKE criteria. 
		/// </summary> 
		/// <param name="criteria">The string representing the criteria</param> 
		/// <returns>An escaped SQL criteria string.</returns> 
		public static string EscapePlainTextQueryLikeCriteria(string criteria) 
		{ 
			criteria = EscapePlainTextQueryCriteria(criteria); 
			criteria = criteria.Replace("[", "[[]"); 
			criteria = criteria.Replace("%", "[%]"); 
			criteria = criteria.Replace("_", "[_]"); 
			criteria = criteria.Replace("-", "[-]"); 

			return criteria; 
		}

        /// <summary>
        /// Converts single qoutes to double qoutes, for text mode sql statements.
        /// </summary>
        public static string ToSql(string statement)
        {
            return statement.Replace("'", "''");
        }
	}
}