using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ITag
    {
        /// <summary>
        /// The name of the tag.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A collection for a light group of documents of a specific content-type.
        /// </summary>
        IList<IDocumentTypeGroup> PopularDocuments { get; set; }

        /// <summary>
        /// A collection of documents grouped by sub-type, i.e. stories, articles, blogs, etc.
        /// </summary>
        IList<IDocumentTypeGroup> LatestDocuments { get; set; }

        /// <summary>
        /// The latest published documents to be commented upon.
        /// </summary>
        ILightCollection<IDocument> LatestCommentedDocuments { get; set; }

        /// <summary>
        /// A collection of the latest galleries for this tag.
        /// </summary>
        ILightCollection<IGallery> LatestGalleries { get; }

        // -- this functionality requires proper tagging on galliers/photos
        ///// <summary>
        ///// A collection of the latest galleries for this tag.
        ///// </summary>
        //ILightCollection<IGallery> PopularGalleries { get; set; }

        /// <summary>
        /// Generates a unique ID for the Tag within the context of the application.
        /// </summary>
        string ApplicationUniqueId { get; }

        /// <summary>
        /// Gets a unique ID for this Tag, within the context of a specific Section.
        /// </summary>
        /// <param name="section">The Section to associate with the Tag.</param>
        string GetApplicationUniqueIdForSection(Section section);

        #region retired accessors - look to eliminate
        /// <summary>
        /// Transient use only: Allows Documents to be stored against the tag.
        /// </summary>
        IList<IDocument> Documents { get; }

        /// <summary>
        /// Transient use only: Denotes how many documents are using this tag, for the specific context this tag was instantiated for.
        /// </summary>
        int DocumentCount { get; set; }
        #endregion
    }
}