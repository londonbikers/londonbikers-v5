using System;
using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IDocument : ICommonBase
    {
        /// <summary>
        /// Denotes the original author of the document.
        /// </summary>
        User Author { get; set; }

        /// <summary>
        /// The date on which the document was originally created.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// The collection of content Images that may be associated with the document.
        /// </summary>
        EditorialImages EditorialImages { get; }

        /// <summary>
        /// Denotes when this document was first set to a Published status.
        /// </summary>
        DateTime Published { get; set; }

        /// <summary>
        /// The collection of documents that this document may be related to.
        /// </summary>
        RelatedDocuments RelatedDocuments { get; }

        /// <summary>
        /// The current status of this document.
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// The single-line lead statement for this document.
        /// </summary>
        string LeadStatement { get; set; }

        /// <summary>
        /// The single-paragraph abstract for this document.
        /// </summary>
        string Abstract { get; set; }

        /// <summary>
        /// The content title for this document.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The content body for this document.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Determines whether or not the viewer must be a member or not.
        /// </summary>
        bool RequiresMembershipToBeViewed { get; set; }

        /// <summary>
        /// Provides access to the collection of Sections this Document belongs to.
        /// </summary>
        List<ISection> Sections { get; }

        /// <summary>
        /// Defines what template the Document is using.
        /// </summary>
        DocumentType Type { get; set; }

        /// <summary>
        /// Provides access to the Tags associate with this Document.
        /// </summary>
        TagCollection Tags { get; set; }

        /// <summary>
        /// Provides access to any user-comments that may relate to this Document.
        /// </summary>
        Comments Comments { get; }
    }
}