using System;

namespace Apollo.Models.Interfaces
{
    public interface IEditorialImage
    {
        /// <summary>
        /// When this Image object was created.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// The filesystem name for the image file.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// The pixel height of the image file.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// The pixel width of the image file.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// The name given to this Image.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type of Image this is, i.e 'cover', 'full' or 'intro'.
        /// </summary>
        ContentImageType Type { get; set; }

        /// <summary>
        /// Returns the full file-system path of the image, i.e."c:\image.jpg".
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// The unique identifier for this Image object.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// The number of other objects this Image is associated with for content purposes.
        /// </summary>
        int AssociationCount { get; }

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
    }
}