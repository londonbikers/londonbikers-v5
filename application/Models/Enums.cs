namespace Apollo.Models
{
    #region infrastructure
    /// <summary>
    /// Indicates the status of a domain object.
    /// </summary>
    public enum DomainObjectStatus
    {
        New = 0,
        Pending = 1,
        Published = 2,
        Inactive = 3,
        LowVisibility = 4
    }

    public enum DomainObjectType
    {
        Unknown = 0,
        Document = 1,
        DocumentImage = 2,
        Gallery = 3,
        GalleryImage = 4,
        GalleryImageHandler = 5,
        GalleryCategory = 6,
        GallerySlideshow = 7,
        DirectoryCategory = 8,
        DirectoryItem = 9
    }

    /// <summary>
    /// Determines under what mode the domain object is operating.
    /// </summary>
    public enum ObjectMode
    {
        Normal = 0,
        Populating = 1
    }

    /// <summary>
    /// Denotes what context an object is being constructed in.
    /// </summary>
    public enum ObjectCreationMode
    {
        New,
        Retrieve
    }
    #endregion

    #region content
    public enum ContentFilterType
    {
        Latest,
        MostPopular
    }

    /// <summary>
    /// Denotes what application area a piece of content relates to.
    /// </summary>
    public enum ApplicationContentType
    {
        GalleryCategory,
        GalleryGallery,
        GalleryImage,
        EditorialStory,
        EditorialArticle,
        DirectoryCategory,
        DirectoryItem
    }

    public enum SiteMapContentType
    {
        News,
        Article,
        Gallery,
        DirectoryItem
    }

    /// <summary>
    /// Describes what type of content an object may be, helps to group similar content-lists.
    /// </summary>
    public enum ContentType
    {
        News = 0,
        Article = 1,
        Galleries = 2,
        Generic = 3,
        DirectoryItem = 4
    }

    /// <summary>
    /// Indicates the status of a comment report.
    /// </summary>
    public enum CommentReportStatus
    {
        NoReport = 0,
        Reported = 1,
        Rejected = 2,
        Moderated = 3
    }

    /// <summary>
    /// Defines what the status of a Comment currently is.
    /// </summary>
    public enum CommentStatusType
    {
        Inactive = 0,
        Active = 1,
        Pending = 2
    }

    /// <summary>
    /// Defines what type of object a Comment relates to.
    /// </summary>
    public enum CommentOwnerType
    {
        Editorial = 0,
        Galleries = 1,
        GalleryImages = 2,
        Directory = 3
    }
    #endregion

    #region editorial
    /// <summary>
    /// Describes the types of Content images available.
    /// </summary>
    public enum ContentImageType
    {
        Normal = 0,
        Cover = 1,
        Intro = 2,
        SlideShow = 3
    }

    /// <summary>
    /// Defines what template a Document is using.
    /// </summary>
    public enum DocumentType
    {
        News = 0,
        Article = 1,
        Generic = 2,
        Blog = 3
    }
    #endregion

    #region photographic
    /// <summary>
    /// Defines the orientation of an image.
    /// </summary>
    public enum ImageOrientation
    {
        Landscape,
        Portrait,
        Square
    }

    /// <summary>
    /// Represents the codec used in a video.
    /// </summary>
    public enum Codec
    {
        Unknown = 0,
        WindowsMedia = 1,
        QuickTime = 2,
        RealVideo = 3,
        DivX = 4,
        XviD = 5,
        Mpeg = 6
    }

    /// <summary>
    /// Defines the difference between a gallery that is for a specific event, or one that's part of the general galleries.
    /// </summary>
    public enum GalleryType
    {
        Structured = 0,
        Specific = 1
    }

    /// <summary>
    /// Allows Gallery Images to be created at different qualities to the default (high).
    /// </summary>
    public enum ImageQuality
    {
        UltraHigh = 95,
        High = 60,
        Medium = 45,
        Low = 30
    }

    /// <summary>
    /// Denotes whether or not the Gallery should be put active or not.
    /// </summary>
    public enum GalleryStatus
    {
        Pending = 0,
        Published = 1,
        Inactive = 3
    }

    /// <summary>
    /// Defines what type a Gallery Category is, i.e. public or user-owned.
    /// </summary>
    public enum CategoryType
    {
        Generic = 0,
        User = 1
    }
    #endregion

    #region users
    /// <summary>
	/// Defines what status a User may have assigned to them.
	/// </summary>
	public enum UserStatus 
	{
		Deleted = 0,
		Active = 1,
		Suspended = 2
	}
    #endregion

    #region misc
    /// <summary>
	/// Denotes the status of a Directory Item.
	/// </summary>
	public enum DirectoryStatus 
	{
		Inactive = 0,
		Active = 1,
		Pending = 2
	}

	/// <summary>
	/// Lists the supported Apollo email types.
	/// </summary>
	public enum EmailType 
	{
		RegistrationConfirmation,
		ForgottenDetails,
		DirectoryItemPublished,
		TrackdayBookingRequest,
		DetailsReminder,
        CommentReplyNotification
	}
	
    /// <summary>
	/// Defines how Finder results should be ordered.
	/// </summary>
	public enum FinderOrder 
	{
		Asc,
		Desc
	}
    #endregion
}