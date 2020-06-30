using System.Configuration;
using System.Data.SqlClient;

namespace Apollo.Galleries
{
	/// <summary>
	/// Provides static access to the most popular Gallery activity statistics.
	/// </summary>
	public class Statistics
	{
		#region accessors
	    /// <summary>
	    /// Returns the total number of Galleries in the system.
	    /// </summary>
	    public int GalleryCount { get; private set; }

	    /// <summary>
	    /// Returns the total number of Images in the system. Net total, not including those in multiple Galleries.
	    /// </summary>
	    public int ImageCount { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		public Statistics() 
		{
			GalleryCount = 0;
            ImageCount = 0;

			InitStatistics();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Rebuilds the statistics.
		/// </summary>
		public void Refresh() 
		{
			InitStatistics();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Tasked with building the cache of statistics for the Gallery. Run once on application start.
		/// </summary>
		private void InitStatistics() 
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var sqlCommand = new SqlCommand {Connection = connection};

		    try
			{
				connection.Open();

				// get the number of galleries.
				sqlCommand.CommandText = "StatGalleryCount";
				GalleryCount = (int)sqlCommand.ExecuteScalar();

				// get the number of images.
				sqlCommand.CommandText = "StatGalleryImageCount";
				ImageCount = (int)sqlCommand.ExecuteScalar();
			}
			finally
			{
				connection.Close();
			}
		}
		#endregion		
	}
}