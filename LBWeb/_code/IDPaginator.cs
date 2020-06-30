using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Tetron.Logic
{
	/// <summary>
	/// Responsible for simplifying the process of paginating large amounts of ArrayList collection entries.
	/// </summary>
	public class IdPaginator
	{
		#region members
		private int _pageSize;
	    private List<long> _dataSource;
		#endregion

		#region accessors
		/// <summary>
		/// Determines the number of items to be returned in each page.
		/// </summary>
		public int PageSize { get { return _pageSize; } set { _pageSize = value; } }

	    /// <summary>
	    /// The total number of pages that exist in the collection.
	    /// </summary>
	    public int TotalPages { get; private set; }

	    /// <summary>
	    /// Indicates which the last page to be requested from the Paginator.
	    /// </summary>
	    public int CurrentPage { get; private set; }

	    /// <summary>
		/// The data to be paginated.
		/// </summary>
		public List<long> DataSource 
		{ 
			get { return _dataSource; } 
			set 
			{ 
				_dataSource = value; 
				InitialisePaginator();
			} 
		}
		/// <summary>
		/// Returns a HTML fragment representing the pagination controls for this datasource.
		/// </summary>
		public string PaginationControls { get { return BuildPaginatorControls(); } }
		#endregion

		#region constructors
		/// <summary>
		/// Create a new static ArrayListPaginator object.
		/// </summary> 
		public IdPaginator() 
		{
			// set a default page size.
			PageSize = 25;
			CurrentPage = 1;
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns the data in a collection for a specific page.
		/// </summary>
        /// <param name="pageNumber">The page of data to return, i.e. page 2.</param>
		public List<long> GetPage(int pageNumber) 
		{
			if (pageNumber > TotalPages)
				pageNumber = TotalPages;

			var page = new List<long>();
			if (DataSource.Count == 0)
				return page;

			var start = (pageNumber - 1) * _pageSize;			
			var end = start + _pageSize;

			// ensure the end doesn't go out of bounds.
			if (end > DataSource.Count)
				end = DataSource.Count;

			CurrentPage = pageNumber;

			// build the new sub-collection.			
			for (var i = start; i < end; i++)
				page.Add(DataSource[i]);

			return page;
		}
		#endregion

		#region private methods
		/// <summary>
		/// When a new datasource is supplied, this method will configure the class.
		/// </summary>
		private void InitialisePaginator() 
		{
			var totalPages = Math.Ceiling(Convert.ToDouble(DataSource.Count) / Convert.ToDouble(PageSize));
			TotalPages = Convert.ToInt32(totalPages);
			
			if (TotalPages == 0)
                TotalPages = 1;

            CurrentPage = 1;
		}
        
		/// <summary>
		/// Builds a set of href controls to allow the user to navigate the paged data.
		/// </summary>
		private string BuildPaginatorControls() 
		{
			if (TotalPages == 1)
				return string.Empty;

			var control = new StringBuilder();

			// determine the base url.
			var url = HttpContext.Current.Request.Url.AbsoluteUri;
			if (url.IndexOf("&p=") > -1)
				url = url.Substring(0, url.IndexOf("&p="));

			int outerPage;
			
			// previous page control.
			if (CurrentPage > 1)
                control.AppendFormat("<b><a href=\"{0}&p={1}\" class=\"Flat\">previous</a></b> | ", url, (CurrentPage - 1));
			else
				control.Append("<a disabled=\"disabled\" class=\"Flat\">previous</a> | ");

			// show the leading pages.
			if (CurrentPage > 1)
			{
				if (CurrentPage < 5)
					outerPage = 1;
				else
					outerPage = CurrentPage - 3;

				for (var i = outerPage; i < CurrentPage; i++)
					control.AppendFormat("<a href=\"{0}&p={1}\" class=\"Flat\">{1}</a>, ", url, i);
			}

			// current page isn't a control.
			control.Append(CurrentPage);

			// show the trailing pages.
			if (CurrentPage > (TotalPages - 4))
				outerPage = TotalPages;
			else
				outerPage = CurrentPage + 3;

			for (var i = CurrentPage + 1; i <= outerPage; i++)
				control.AppendFormat(", <a href=\"{0}&p={1}\" class=\"Flat\">{1}</a>", url, i);

			// next page control.
			if (CurrentPage < TotalPages)
				control.AppendFormat(" | <b><a href=\"{0}&p={1}\" class=\"Flat\">next</a></b>", url, (CurrentPage + 1));
			else
				control.Append(" | <a disabled=\"disabled\" class=\"Flat\">next</a>");

			return control.ToString();
		}
		#endregion
	}
}