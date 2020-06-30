using System;
using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface ISite
    {
        /// <summary>
        /// The friendly name for this Site.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The fully-qualified URL for this site.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The collection of Channels associated with this Site.
        /// </summary>
        List<IChannel> Channels { get; }

        /// <summary>
        /// The collection of Section objects associated with this Channel.
        /// </summary>
        List<ISection> Sections { get; }

        /// <summary>
        /// The collection of root categories for all channels.
        /// </summary>
        List<IGalleryCategory> GalleryCategories { get; }

        /// <summary>
        /// Provides access to the latest galleries for the whole Site.
        /// </summary>
        LatestGalleries RecentGalleries { get; }

        /// <summary>
        /// The identifier for the derived object.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// The number of times the content has been viewed.
        /// </summary>
        long Views { get; }

        /// <summary>
        /// For GUID based consumers, the GUID ID should be passed through to enable correct ApplicationUniqueID's.
        /// </summary>
        Guid LegacyId { get; set; }

        /// <summary>
        /// Generates a unique ID for the derived object within the context of the application. Used for objects that implement numeric ID's.
        /// </summary>
        string ApplicationUniqueId { get; }

        /// <summary>
        /// The current mode the object is operating under.
        /// </summary>
        ObjectMode ObjectMode { get; set; }

        /// <summary>
        /// A User is able to vote for a piece of content to show their approval or interest. There can only be
        /// one voting per User, though if a user has already voted, no vote will be made an no response will be given.
        /// </summary>
        /// <param name="user">The User who is voting for the piece of content.</param>
        void MarkVoted(User user);

        /// <summary>
        /// Marks the content as has being viewed. Members of staff do not have views counted.
        /// </summary>
        /// <param name="user">
        /// If present (null otherwise), this allows the application to determine whether or not the
        /// person is a member of staff.
        /// </param>
        void MarkViewed(User user);

        /// <summary>
        /// Causes the gallery category cache for this object to be refreshed.
        /// </summary>
        void RefreshGalleryCategories();
    }
}