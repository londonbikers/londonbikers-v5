using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using Apollo.Models;
using Apollo.Utilities;

namespace Apollo
{
	public abstract class Finder
	{
		#region members
		private int _ceiling;
	    private readonly ArrayList _dateRange;
	    private readonly ArrayList _findByLike;
		private readonly ArrayList _findByValue;
		private readonly ArrayList _orderBy;
		#endregion

		#region accessors
	    protected Hashtable Fields { get; set; }
	    protected string SqlTable { get; set; }
	    protected string PrimaryKey { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Returns a new Finder object.
		/// </summary>
		protected Finder() 
		{
			_findByValue = new ArrayList();
            _findByLike = new ArrayList();
            _dateRange = new ArrayList();
            _orderBy = new ArrayList();
            _ceiling = 1000;
            SqlTable = string.Empty;
            PrimaryKey = "ID";
		}
		#endregion

		#region public methods
		/// <summary>
		/// Performs the query after the criteria has been defined, and returns the results.
		/// </summary>
		/// <param name="ceiling">The maximum number of records to retrieve. Keep this as low as possible to aid performance.</param>
		/// <returns>A list of long ID's for the objects found.</returns>
		public List<long> Find(int ceiling) 
		{
			_ceiling = ceiling;
			var ids = new List<long>();
			var query = BuildQuery();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
		    var command = new SqlCommand(query, connection);
			SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

			    while (reader.Read())
			        ids.Add(reader.GetInt64(0));
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return ids; 
		}

        /// <summary>
        /// Performs the query after the criteria has been defined, and returns the results.
        /// </summary>
        /// <param name="ceiling">The maximum number of records to retrieve. Keep this as low as possible to aid performance.</param>
        /// <returns>A list of long ID's for the objects found.</returns>
        public List<Guid> FindLegacy(int ceiling)
        {
            _ceiling = ceiling;
            var ids = new List<Guid>();
            var query = BuildQuery();
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand(query, connection);
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                    ids.Add(reader.GetGuid(0));
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return ids;
        } 

		/// <summary>
		/// Filter results by specifying a property and a partial value. Wildcard filter.
		/// </summary>
		public void FindLike(string expression, string property) 
		{
			_findByLike.Add(new[]{property, expression});
		}
        		
		/// <summary>
		/// Filter results so that they fall in-between two dates.
		/// </summary>
		public void FindRange(DateTime from, DateTime to, string property) 
		{
            _dateRange.Add(new[] { from.ToString(), to.ToString(), property });
		} 
        		
		/// <summary>
		/// Filter results for a specific property, with a specific value.
		/// </summary>
		public void FindValue(string expression, string property) 
		{
			_findByValue.Add(new[]{property, expression});
		} 

		/// <summary>
		/// Order the results by a specific property, and choose a direction (ASC/DESC).
		/// </summary>
		public void OrderBy(string property, FinderOrder order) 
		{
			_orderBy.Add(new[]{property, order.ToString()});
		} 

		/// <summary>
		/// Handles the execution of a custom query for the derived class
		/// </summary>
		/// <param name="command">The SqlCommand object with procedure name and parameters defined.</param>
		/// <returns>A populated DataTable, containing the results of the custom query.</returns>
		protected List<long> PerformCustomQuery(SqlCommand command) 
		{
			var results = new List<long>();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			command.Connection = connection;
			SqlDataReader reader = null;

			try
			{
				connection.Open();
				reader = command.ExecuteReader();

                while (reader.Read())
                    results.Add(reader.GetInt64(0));
			}
			finally
			{
			    if (reader != null)
					reader.Close();

			    connection.Close();
			}

		    return results;
		}

        /// <summary>
        /// Handles the execution of a custom query for the derived class.
        /// </summary>
        /// <param name="command">The SqlCommand object with procedure name and parameters defined.</param>
        /// <returns>A populated DataTable, containing the results of the custom query.</returns>
        protected List<Guid> PerformLegacyCustomQuery(SqlCommand command)
        {
            var results = new List<Guid>();
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            command.Connection = connection;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                    results.Add(reader.GetGuid(0));
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                connection.Close();
            }

            return results;
        }
		#endregion
		
		#region private methods
		/// <summary>
		/// Determines whether or not a particular field is being used in an 'or' comparison by looking for previous
		/// uses of the same field.
		/// </summary>
		private static bool IsOrComparison(string field, string property, IList collection) 
		{
		    int num3;
			int num4;
		    var num1 = 0;
			var num2 = 0;
			
			for (num3 = 0; num3 < collection.Count; num3++)
			{
				if (((string[]) collection[num3])[1] == property)
					num2 = num3;
			}
			
			for (num4 = num2; num4 < collection.Count; num4++)
			{
				if (((string[]) collection[num4])[0] == field)
					num1++;
			}

			var flag1 = (num1 > 1) ? true : false;
			return flag1; 
		} 
        
		/// <summary>
		/// Constructs the TSQL query statement to be used to perform the search.
		/// </summary>
		private string BuildQuery() 
		{
			var query = new StringBuilder();
			var criteriaCount = _findByValue.Count + _findByLike.Count + _dateRange.Count;
			var num2 = 0;
			int num3;
			int num4;
			int num5;
			int num6;
		    var tmpHolder = String.Empty;
			var inOrBuild = false;

            query.AppendFormat("SELECT TOP {0} ", _ceiling);
            query.AppendFormat(" {0} ", PrimaryKey);
			query.Append("FROM ");
            query.AppendFormat(" {0} ", SqlTable);
		
			if (criteriaCount > 0)
				query.Append("WHERE");
			
			for (num3 = 0; num3 < _findByValue.Count; num3++)
			{
				tmpHolder += " ";
				tmpHolder += (Fields[((string[])_findByValue[num3])[0]]);
				tmpHolder += " = '";
				tmpHolder += Sql.EscapePlainTextQueryCriteria(((string[])_findByValue[num3])[1]);
				tmpHolder += "' ";

				if (num2 < (criteriaCount - 1))
				{
					if (IsOrComparison(((string[]) _findByValue[num3])[0], ((string[]) _findByValue[num3])[1], _findByValue))
					{
						if (!inOrBuild)
							tmpHolder = "(" + tmpHolder;

						tmpHolder += " OR ";
						inOrBuild = true;
					}
					else
					{
						// complete OR statement build if one exists.
						if (inOrBuild)
							tmpHolder += ")";

						inOrBuild = false;
						tmpHolder += " AND ";
					}
				}

				// close the OR block if it's open and we're at the end.
				if (num2 == (criteriaCount -1) && inOrBuild && tmpHolder.IndexOf(")") == -1)
					tmpHolder += ")";
				
				num2++;
			}

			query.Append(tmpHolder);
			
			for (num4 = 0; num4 < _findByLike.Count; num4++)
			{
				var array2 = new string[5];
				array2[0] = " ";
				array2[1] = ((string)Fields[((string[]) _findByLike[num4])[0]]);
				array2[2] = " LIKE '%";
				array2[3] = Sql.EscapePlainTextQueryLikeCriteria(((string[]) _findByLike[num4])[1]);
				array2[4] = "%' ";
				query.Append(string.Concat(array2));
				
				if (num2 < (criteriaCount - 1))
				{
				    query.Append(IsOrComparison(((string[]) _findByLike[num4])[0], ((string[]) _findByLike[num4])[1], _findByLike)
				                     ? " OR "
				                     : " AND ");
				}
				
				num2++;
			}
			
			for (num5 = 0; num5 < _dateRange.Count; num5++)
			{
				var array3 = new string[7];
				array3[0] = " ";
				array3[1] = ((string)Fields[((string[])_dateRange[num5])[2]]);
				array3[2] = " BETWEEN '";
                array3[3] = ((string[])_dateRange[num5])[0];
                array3[4] = "' AND '";
                array3[5] = ((string[])_dateRange[num5])[1];

				array3[6] = "'";
				query.Append(string.Concat(array3));
				
				if (num2 < (criteriaCount - 1))
					query.Append(" AND ");
	
				num2++;
			}
			
			if (_orderBy.Count > 0)
				query.Append(" ORDER BY");
	
			for (num6 = 0; num6 < _orderBy.Count; num6++)
			{
				query.Append(" " + Fields[((string[])_orderBy[num6])[0]] + " " + ((string[])_orderBy[num6])[1]);
			
				if (num6 < (_orderBy.Count - 1))
					query.Append(", ");
			}
			
			return query.ToString();
		}
		#endregion
	}
}