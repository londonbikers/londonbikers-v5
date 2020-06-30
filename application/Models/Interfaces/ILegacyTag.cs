using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ILegacyTag
    {
        /// <summary>
        /// The name of the tag.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Generates a unique ID for the Tag within the context of the application.
        /// </summary>
        string ApplicationUniqueId { get; }

        /// <summary>
        /// Transient use only: Allows Documents to be stored against the tag.
        /// </summary>
        List<Document> Documents { get; }

        /// <summary>
        /// Transient use only: Denotes how many documents are using this tag, for the specific context this tag was instantiated for.
        /// </summary>
        int DocumentCount { get; set; }

        /// <summary>
        /// Gets a unique ID for this Tag, within the context of a specific Section.
        /// </summary>
        /// <param name="section">The Section to associate with the Tag.</param>
        string GetApplicationUniqueIdForSection(Section section);
    }
}