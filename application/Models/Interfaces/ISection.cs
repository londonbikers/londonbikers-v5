using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ISection : ICommonBase
    {
        /// <summary>
        /// The public name of the Section.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A short description that can be used for leading information.
        /// </summary>
        string ShortDescription { get; set; }

        /// <summary>
        /// The part to be used in the construction of url's for the Section.
        /// </summary>
        string UrlIdentifier { get; set; }

        /// <summary>
        /// Any document id's associated with this section at the root level. If a structure approach is required, use the categories.
        /// </summary>
        List<long> Documents { get; }

        /// <summary>
        /// Sections have favourite tags, those which are most important, i.e. more for generic concepts.
        /// </summary>
        TagCollection FavouriteTags { get; set; }

        /// <summary>
        /// Provides access to the Channel which this Section is associated with.
        /// </summary>
        IChannel ParentChannel { get; set; }

        /// <summary>
        /// If this Section is directly related to a Site then it is available here, otherwise you can navigate to the parent site through the parent channel.
        /// </summary>
        ISite ParentSite { get; set; }

        /// <summary>
        /// Describes what type of content is listed in this Section.
        /// </summary>
        ContentType ContentType { get; set; }

        /// <summary>
        /// Provides convinient access to the newest documents within this Section.
        /// </summary>
        ILatestDocuments LatestDocuments { get; }

        /// <summary>
        /// If this Section is of type Generic, then it will need a default document to act as the index.
        /// </summary>
        Document DefaultDocument { get; set; }

        /// <summary>
        /// Indicates the status of the Section.
        /// </summary>
        DomainObjectStatus Status { get; set; }

        /// <summary>
        /// Any featured-documents for this section.
        /// </summary>
        IFeaturedDocumentsCollection FeaturedDocuments { get; }

        /// <summary>
        /// An alphabetically-sorted collection of the most popular tags assigned to content within this section.
        /// </summary>
        ITagCollection PopularTags { get; }
    }
}