using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Models
{
	/// <summary>
	/// Represents and Editorial image within the system.
	/// </summary>
	public class EditorialImage : CommonBase, IEditorialImage
	{
		#region accessors
	    /// <summary>
	    /// When this Image object was created.
	    /// </summary>
	    public DateTime Created { get; set; }

	    /// <summary>
	    /// The filesystem name for the image file.
	    /// </summary>
	    public string Filename { get; set; }

	    /// <summary>
	    /// The pixel height of the image file.
	    /// </summary>
	    public int Height { get; set; }

	    /// <summary>
	    /// The pixel width of the image file.
	    /// </summary>
	    public int Width { get; set; }

	    /// <summary>
	    /// The name given to this Image.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// The type of Image this is, i.e 'cover', 'full' or 'intro'.
	    /// </summary>
	    public ContentImageType Type { get; set; }

	    /// <summary>
		/// Returns the full file-system path of the image, i.e."c:\image.jpg".
		/// </summary>
		public string FullPath { get { return GetFullPath(); } }

	    /// <summary>
	    /// The unique identifier for this Image object.
	    /// </summary>
	    public Guid Uid { get; set; }

	    /// <summary>
		/// The number of other objects this Image is associated with for content purposes.
		/// </summary>
		public int AssociationCount { get { return GetAssociationsCount(); } }
		#endregion

		#region constructors
		/// <summary>
		/// Returns a new Image object.
		/// </summary>
		public EditorialImage(ObjectCreationMode mode) 
		{
			if (mode == ObjectCreationMode.New)
			{
				Uid = Guid.NewGuid();
				Created = DateTime.Now;
			}

			DerivedType = GetType();
		} 
		#endregion

		#region private methods
		/// <summary>
		/// Finds out how many associations this Image has with other Apollo objects.
		/// </summary>
		private int GetAssociationsCount() 
		{
			int count;
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("GetContentImageAssociationsCount", connection) {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@ID", Id));

			try
			{
				connection.Open();
                count = (int)Sql.GetValue(typeof(int), command.ExecuteScalar());
			}
			finally
			{
				connection.Close();
			}

			return count;
		}

		/// <summary>
		/// Returns the full path of the Image, if a filename is set.
		/// </summary>
		private string GetFullPath()
		{
		    if (string.IsNullOrEmpty(Filename))
				return string.Empty;

		    return string.Format("{0}editorial\\{1}", ConfigurationManager.AppSettings["Global.MediaLibraryPath"], Filename);
		}
	    #endregion
	}
}