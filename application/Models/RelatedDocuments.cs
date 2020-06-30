using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	public class RelatedDocuments : IRelatedDocuments
	{
		#region members
	    private readonly ICommonBase _parent;
		private List<Document> _collection;
		#endregion

		#region accessors
		/// <summary>
		/// The number of related documents.
		/// </summary>
		public int Count 
		{ 
			get
			{
			    if (_collection == null)
					PopulateCollection();

			    return _collection != null ? _collection.Count : 0;
			}
		}
		/// <summary>
		/// The default indexer.
		/// </summary>
		public Document this[int index] 
		{ 
			get
			{
			    if (_collection == null)
					PopulateCollection();

			    return _collection != null ? _collection[index] : null;
			}
		}
		/// <summary>
		/// The related documents collection.
		/// </summary>
		public List<Document> List 
		{ 
			get 
			{ 
				if (_collection == null)
					PopulateCollection();

				return _collection; 
			} 
		}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a new RelatedDocuments object.
		/// </summary>
        /// <param name="parent">The object using this collection.</param>
		public RelatedDocuments(ICommonBase parent) 
		{
		    _parent = parent;
		} 
		#endregion

		#region public methods
		/// <summary>
		/// Associates another object with this collection.
		/// </summary>
		/// <param name="id">The numeric identifier for the object to associate with.</param>
		public void Add(long id) 
		{
		    Document document1;
		    IDisposable disposable1;
			var flag1 = false;
			var enumerator1 = _collection.GetEnumerator();
		
			try
			{
				while (enumerator1.MoveNext())
				{
				    document1 = enumerator1.Current;
				    if (document1 != null && document1.Id == id)
				        flag1 = true;
				}
			}
			finally
			{
			    disposable1 = enumerator1;
			    disposable1.Dispose();
			}

		    if (!flag1)
				_collection.Add(Server.Instance.ContentServer.GetDocument(id));
		} 
        
		/// <summary>
		/// Collects the objects related to the one which implements this class.
		/// </summary>
		private void PopulateCollection()
		{
		    _collection = new List<Document>();
		    lock (((ICollection) _collection).SyncRoot)
		    {
	            SqlDataReader reader = null;
	            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
	            var command = new SqlCommand("GetEditorialRelatedObjects", connection) {CommandType = CommandType.StoredProcedure};
		        command.Parameters.Add(new SqlParameter("@ObjectID", _parent.Id));

	            try
	            {
	                connection.Open();
	                reader = command.ExecuteReader();

                    while (reader.Read())
                        _collection.Add(Server.Instance.ContentServer.GetDocument(reader.GetInt64(reader.GetOrdinal("ID"))));
	            }
	            finally
	            {
	                if (reader != null)
	                    reader.Close();

	                connection.Close();
	            }
		    }
        } 
        
		/// <summary>
		/// Removes a related object from the collection.
		/// </summary>
		/// <param name="id">The numeric identifier for the object to unassociate.</param>
		public void Remove(long id) 
		{
		    Document document2;
		    IDisposable disposable1;
			Document document1 = null;
			var enumerator1 = _collection.GetEnumerator();
		
			try
			{
				while (enumerator1.MoveNext())
				{
				    document2 = enumerator1.Current;
				    if (document2 != null && document2.Id == id)
				        document1 = document2;
				}
			}
			finally
			{
			    disposable1 = enumerator1;
			    disposable1.Dispose();
			}

		    _collection.Remove(document1);
		} 
        
		/// <summary>
		/// Persists the related-objects collection.
		/// </summary>
		internal void Save() 
		{
            if (_collection == null)
                return;

			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("FlushEditorialRelatedObjects", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ObjectID", _parent.Id));

			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				command.CommandText = "InsertEditorialRelatedObject";
				
				var docParam = new SqlParameter("@RelatedObjectID", SqlDbType.BigInt);
				command.Parameters.Add(docParam);

				foreach (var doc in _collection)
				{
					docParam.Value = doc.Id;
					command.ExecuteNonQuery();
				}
			}
			finally
			{
				connection.Close();
			}
		} 
		#endregion
	}
}