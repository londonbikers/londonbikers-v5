using System;
using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IDirectoryItem
    {
        /// <summary>
        /// The unique-identifier for this Directory Item.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// A collection of Categories that this Item belongs to.
        /// </summary>
        DirectoryCategoryCollection DirectoryCategories { get; }

        /// <summary>
        /// A collection of Keyword objects that relate to this Item.
        /// </summary>
        List<string> Keywords { get; }

        /// <summary>
        /// A collection of URL's that relate to this Item.
        /// </summary>
        List<string> Links { get; }

        /// <summary>
        /// A collection of Image URL's that relate to this Item.
        /// </summary>
        List<string> Images { get; }

        /// <summary>
        /// The title of this Item.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The description for this Item.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The telephone number for this Item.
        /// </summary>
        string TelephoneNumber { get; set; }

        /// <summary>
        /// The calculated rating for this Item.
        /// </summary>
        double Rating { get; }

        /// <summary>
        /// The number of ratings that make up the Item rating.
        /// </summary>
        long NumberOfRatings { get; set; }

        /// <summary>
        /// The person that submitted this Item.
        /// </summary>
        User Submiter { get; set; }

        /// <summary>
        /// The status of this Item.
        /// </summary>
        DirectoryStatus Status { get; set; }

        /// <summary>
        /// The date on which this Item was created.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// The date on which this Item was last updated.
        /// </summary>
        DateTime Updated { get; set; }

        /// <summary>
        /// The postal code for the address that may represent this item.
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// The global longitude position of the address for this item.
        /// </summary>
        double Longitude { get; set; }

        /// <summary>
        /// The global latitude position of the address for this item.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Provides access to any user-comments that may relate to this Item.
        /// </summary>
        Comments Comments { get; }

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
        /// Allows for the rating of this Item. Ratings are between 1 and 10.
        /// </summary>
        void AddRating(int rating);

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