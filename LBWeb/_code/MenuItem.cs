using System;

namespace Tetron.Logic
{
	/// <summary>
	/// Represents an item in a navigational menu.
	/// </summary>
	public class MenuItem
	{
		#region accessors
	    /// <summary>
	    /// The identifier for this MenuItem.
	    /// </summary>
	    public string Id { get; set; }

	    /// <summary>
	    /// The textual label for this menu item.
	    /// </summary>
	    public string Text { get; set; }

	    /// <summary>
	    /// The on-hover text to appear when the menu item is selected.
	    /// </summary>
	    public string ToolTip { get; set; }

	    /// <summary>
	    /// The URL that the user is to be taken to, when the menu item is selected.
	    /// </summary>
	    public string Url { get; set; }

	    /// <summary>
	    /// An optional way of helping the menu identify current menu items, i.e. "/forums/" can help tag all
	    /// pages in the forums folder. These values must uniquely identify the folder.
	    /// </summary>
	    public string UrlFragment { get; set; }

	    /// <summary>
	    /// Indicates whether or not this menu item is selected, or not.
	    /// </summary>
	    public bool Selected { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new MenuItem object.
		/// </summary>
		public MenuItem()
		{
			Id = String.Empty;
			Text = String.Empty;
			ToolTip = String.Empty;
			Url = String.Empty;
			UrlFragment = String.Empty;
			Selected = false;
		}
		#endregion
	}
}
