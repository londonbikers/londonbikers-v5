using System;
using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IDirectoryCategory
    {
        /// <summary>
        /// The name for this Category.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The description for this Category.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Determines whether or not site membership is required to access this Category and it's items.
        /// </summary>
        bool RequiresMembership { get; set; }

        /// <summary>
        /// The collection of Directory Items associated with this Category.
        /// </summary>
        DirectoryItemCollection DirectoryItems { get; }

        /// <summary>
        /// A collection of keywords associated with this Category.
        /// </summary>
        List<string> Keywords { get; }

        /// <summary>
        /// The collection of sub-Categories associated with this Category.
        /// </summary>
        DirectoryCategoryCollection SubDirectoryCategories { get; }

        /// <summary>
        /// Returns any applicable parent Category for this Category.
        /// </summary>
        DirectoryCategory ParentDirectoryCategory { get; set; }

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
        /// Returns a filtered version of the Category Items collection.
        /// </summary>
        /// <param name="status">Determines which items of the relevant DirectoryStatus should be returned.</param>
        DirectoryItemCollection FilteredItems(DirectoryStatus status);

        int CompareTo(object obj);

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