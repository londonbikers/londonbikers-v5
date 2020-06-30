using System;

namespace Apollo.Models.Interfaces
{
    public interface IGalleryCategory
    {
        /// <summary>
        /// Determines whether or not this Category is active.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// The identifier for this Category.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// The textual name for this Category.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The textual description for this Category.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Denotes the type this Category is.
        /// </summary>
        CategoryType Type { get; set; }

        /// <summary>
        /// Denotes the owner of this Category. Categories can be public, or user-owned.
        /// </summary>
        User Owner { get; set; }

        /// <summary>
        /// The public galleries shown in this category.
        /// </summary>
        ILightCollection<IGallery> Galleries { get; }

        /// <summary>
        /// All of the galleries shown in this category.
        /// </summary>
        ILightCollection<IGallery> AllGalleries { get; }

        /// <summary>
        /// The Site to which this Category belongs.
        /// </summary>
        ISite ParentSite { get; set; }

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
        /// Adds a Gallery to the collection.
        /// </summary>
        void AddGallery(Gallery gallery);

        /// <summary>
        /// Removes a specific Gallery from the collection.
        /// </summary>
        void RemoveGallery(Gallery gallery);

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
    }
}