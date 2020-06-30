using System;

namespace Apollo.Models.Interfaces
{
    public interface ICommonBase
    {
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
        /// The Type of the derived class. Used for generating unique id's.
        /// </summary>
        Type DerivedType { get; set; }

        /// <summary>
        /// For application efficiency, this allows the controller to determine if the derived object needs updating or not.
        /// </summary>
        bool HasChanged { get; set; }

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
        /// determines if the object has been persisted to the database.
        /// </summary>
        bool IsPersisted { get; set; }

        DomainObjectType GetDomainObjectType();
    }
}