using System;
using System.Collections;
using Apollo.Models;

namespace Tetron.Logic
{
	/// <summary>
	/// Defines a specific period of time.
	/// </summary>
	public class DateRange 
	{
	    /// <summary>
	    /// The start-date of the range.
	    /// </summary>
	    public DateTime From { get; set; }

	    /// <summary>
	    /// The end-date of the range.
	    /// </summary>
	    public DateTime To { get; set; }
	}

	/// <summary>
	/// Facilitates the transport of application objects between pages in a safe and contained manner.
	/// </summary>
	public class Container 
	{
		#region members
		private readonly Guid _uid;
	    #endregion

		#region accessors
		/// <summary>
		/// The unique-identifier for this Container object.
		/// </summary>
		public Guid Uid { get { return _uid; } }

	    /// <summary>
	    /// A possible collection of UID's.
	    /// </summary>
	    public ArrayList UidList { get; set; }

	    /// <summary>
	    /// A possible Apollo Document.
	    /// </summary>
	    public Document ApolloDocument { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Create a new Container object.
		/// </summary>
		public Container() 
		{
			_uid = Guid.NewGuid();
		}
		#endregion
	}
}