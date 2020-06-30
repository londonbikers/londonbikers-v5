using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Represents a television listing.
	/// </summary>
	public class TvListing : IComparable, ITvListing
	{
		#region accessors
	    /// <summary>
	    /// The unique identifier for this listing in the context of the current listings batch.
	    /// </summary>
	    public int Id { get; set; }

	    /// <summary>
	    /// The Date/Time of when the programme begins.
	    /// </summary>
	    public DateTime Start { get; set; }

	    /// <summary>
	    /// The Date/Time of when the programme ends.
	    /// </summary>
	    public DateTime End { get; set; }

	    /// <summary>
	    /// The name of the programme.
	    /// </summary>
	    public string Name { get; set; }

	    /// <summary>
	    /// The type, or genre of the programme.
	    /// </summary>
	    public string Type { get; set; }

	    /// <summary>
	    /// The name of the channel the programme is on.
	    /// </summary>
	    public string Channel { get; set; }

	    /// <summary>
	    /// The programme description.
	    /// </summary>
	    public string Description { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Listing object.
		/// </summary>
		internal TvListing()
		{
			Id = 0;
			Start = DateTime.MinValue;
			End = DateTime.MinValue;
			Name = string.Empty;
			Type = string.Empty;
			Channel = string.Empty;
			Description = string.Empty;
		}
		#endregion

		#region IComparable Members
		/// <summary>
		/// Sorts by StartDate.
		/// </summary>
		public int CompareTo(object obj) 
		{
			return Start.CompareTo(((TvListing)obj).Start);
		}
		#endregion
	}
}
