using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Apollo.Models.Interfaces;
using Apollo.Utilities;
using Apollo.Utilities.GeoCoding;

namespace Apollo.Models
{
	/// <summary>
	/// Represents an item within the Directory.
	/// </summary>
	public class DirectoryItem : CommonBase, IComparable, IDirectoryItem
	{
		#region members
	    private DirectoryCategoryCollection _directoryCategories;
	    private string _postcode;
	    #endregion

		#region accessors
	    /// <summary>
	    /// The unique-identifier for this Directory Item.
	    /// </summary>
	    public Guid Uid { get; set; }

	    /// <summary>
		/// A collection of Categories that this Item belongs to.
		/// </summary>
		public DirectoryCategoryCollection DirectoryCategories 
		{ 
			get 
			{ 
				if (_directoryCategories == null)
					RetrieveCategories();

				return _directoryCategories; 
			} 
		}

	    /// <summary>
	    /// A collection of Keyword objects that relate to this Item.
	    /// </summary>
	    public List<string> Keywords { get; private set; }

	    /// <summary>
	    /// A collection of URL's that relate to this Item.
	    /// </summary>
	    public List<string> Links { get; private set; }

	    /// <summary>
	    /// A collection of Image URL's that relate to this Item.
	    /// </summary>
	    public List<string> Images { get; private set; }

	    /// <summary>
	    /// The title of this Item.
	    /// </summary>
	    public string Title { get; set; }

	    /// <summary>
	    /// The description for this Item.
	    /// </summary>
	    public string Description { get; set; }

	    /// <summary>
	    /// The telephone number for this Item.
	    /// </summary>
	    public string TelephoneNumber { get; set; }

	    /// <summary>
		/// The calculated rating for this Item.
		/// </summary>
		public double Rating 
		{ 
			get
			{
			    var rating = 0D;
			    if (NumberOfRatings > 0)
					rating = Convert.ToDouble(RatingSum) / Convert.ToDouble(NumberOfRatings);

			    return rating;
			}
		}

	    /// <summary>
	    /// The number of ratings that make up the Item rating.
	    /// </summary>
	    public long NumberOfRatings { get; set; }

	    /// <summary>
	    /// The sum of all the individual ratings, used to calculate the actual rating.
	    /// </summary>
	    internal long RatingSum { get; set; }

	    /// <summary>
	    /// The person that submitted this Item.
	    /// </summary>
	    public User Submiter { get; set; }

	    /// <summary>
	    /// The status of this Item.
	    /// </summary>
	    public DirectoryStatus Status { get; set; }

	    /// <summary>
	    /// The date on which this Item was created.
	    /// </summary>
	    public DateTime Created { get; set; }

	    /// <summary>
	    /// The date on which this Item was last updated.
	    /// </summary>
	    public DateTime Updated { get; set; }

	    /// <summary>
		/// The postal code for the address that may represent this item.
		/// </summary>
		public string Postcode 
		{ 
			get { return _postcode; } 
			set 
			{
			    if (value == _postcode) return;
			    var pc = value;
			    pc = pc.Trim();//.Replace(" ", String.Empty);

			    // only run if there's a change.
			    if (pc == _postcode) return;
			    _postcode = pc;

			    if (ObjectMode == ObjectMode.Normal)
			        RetrieveCoordinates();
			} 
		}

	    /// <summary>
	    /// The global longitude position of the address for this item.
	    /// </summary>
	    public double Longitude { get; set; }

	    /// <summary>
	    /// The global latitude position of the address for this item.
	    /// </summary>
	    public double Latitude { get; set; }

	    /// <summary>
	    /// Provides access to any user-comments that may relate to this Item.
	    /// </summary>
	    public Comments Comments { get; private set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Item object.
		/// </summary>
		internal DirectoryItem(ObjectCreationMode mode) 
		{
			if (mode == ObjectCreationMode.New)
			{
				Uid = Guid.NewGuid();
				IsPersisted = false;
			}

			DerivedType = GetType();
			InitialiseObject();
		}
		#endregion

		#region public methods
		/// <summary>
		/// Allows for the rating of this Item. Ratings are between 1 and 10.
		/// </summary>
		public void AddRating(int rating) 
		{
			if (rating < 1 || rating > 10)
				throw new Exception("Rating out of bounds, must be between 1 and 10.");

			NumberOfRatings++;
			RatingSum += rating;

			// persist the changes.
			Server.Instance.DirectoryServer.UpdateItem(this);
		}
		#endregion

		#region private methods
		/// <summary>
		/// Sets all class members their default values.
		/// </summary>
		private void InitialiseObject() 
		{
            Title = string.Empty;
            Description = string.Empty;
            TelephoneNumber = string.Empty;
            _postcode = string.Empty;
            Status = DirectoryStatus.Pending;
            Created = DateTime.Now;
            Images = new List<string>();
            Keywords = new List<string>();
			Links = new List<string>();
            Comments = new Comments(this);
		}
        
		/// <summary>
		/// Retrieves the list of Categories that this Item belongs to.
		/// </summary>
		private void RetrieveCategories() 
		{
			_directoryCategories = new DirectoryCategoryCollection(this);
            if (!IsPersisted)
                return;

            lock (_directoryCategories)
            {
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetDirectoryItemCategories", connection) {CommandType = CommandType.StoredProcedure};
                SqlDataReader reader = null;

                var idParam = new SqlParameter("@ID", SqlDbType.BigInt) {Value = Id};
                command.Parameters.Add(idParam);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        _directoryCategories.Add(Server.Instance.DirectoryServer.GetCategory((long)Sql.GetValue(typeof(long), reader["CategoryID"])), false);
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
		/// Attempts to convert the postcode to a latitude & longitude pair.
		/// </summary>
		private void RetrieveCoordinates() 
		{
			if (string.IsNullOrEmpty(Postcode))
			{
				Latitude = 0;
                Longitude = 0;
			}
			else
			{
                var coordinate = Geocode.GetCoordinates(Postcode + ", United Kingdom");
                Latitude = coordinate.Latitude;
                Longitude = coordinate.Longitude;
			}
		}
		#endregion

		#region IComparable Members
		public int CompareTo(object obj) 
		{
			// compare using the name AND rating.
			var comp1 = Title.CompareTo(((DirectoryItem)obj).Title);
			var comp2 = Rating.CompareTo(((DirectoryItem)obj).Rating);
			
			// rating takes presidence over character order.
			switch (comp2)
			{
			    case -1:
			        return -1;
			    case 0:
			        switch (comp1)
			        {
			            case -1:
			                return -1;
			            case 0:
			                return 0;
			            default:
			                return 1;
			        }
			    default:
			        return 1;
			}
		}
		#endregion
	}
}