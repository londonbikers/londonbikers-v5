namespace Apollo.Models.Interfaces
{
    public interface ITaggedDocumentContainer
    {
        /// <summary>
        /// The Tag to customise the contents for.
        /// </summary>
        ISection Section { get; }

        /// <summary>
        /// The Tag to customise the contents for.
        /// </summary>
        ITag Tag { get; }

        /// <summary>
        /// Provides access to the latest documents in the section with this tag.
        /// </summary>
        ILatestDocuments LatestDocuments { get; }

        /// <summary>
        /// Generates a unique ID within the context of the application. Used for caching purposes primarily. Custom implementation.
        /// </summary>
        string ApplicationUniqueId { get; }
    }
}