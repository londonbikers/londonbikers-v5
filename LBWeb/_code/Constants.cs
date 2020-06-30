namespace Tetron
{
	public class Constants
    {
        #region v5
        /// <summary>
        /// Defines an area under the main navigation.
        /// </summary>
        public enum NavigationArea
        {
            Home,
            News,
            Features,
            Photos,
            Events,
            Database,
            Shop,
            Community
        }
        #endregion

        /// <summary>
		/// Defines a way of constructing a web-page.
		/// </summary>
		public enum PageLayout 
		{
			Normal,
			ShrinkableWidth,
			FullWidthMaximisedContentArea
		}

		/// <summary>
		/// Defines a set of common date-ranges for use in queries.
		/// </summary>
		public enum DateRange 
		{
			Today,
			ThisWeek,
			ThisMonth,
			All
		}

		/// <summary>
		/// Defines what format any advertisments should be.
		/// </summary>
		public enum AdvertFormat 
		{
			StandardBanner,
			WideSkyscraper,
			Rectangles
		}

		/// <summary>
		/// Defines how a breadcrumb trail should be presented.
		/// </summary>
		public enum BreadcrumbStyle 
		{
			Navigational,
			Listing,
			SearchResult
		}

		/// <summary>
		/// Defines what type of search can be initiated.
		/// </summary>
		public enum SearchType 
		{
			Title,
			Meta
		}
	}
}