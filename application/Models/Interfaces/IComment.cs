using System;

namespace Apollo.Models.Interfaces
{
    public interface IComment
    {
        /// <summary>
        /// The User who posted this Comment.
        /// </summary>
        User Author { get; set; }

        /// <summary>
        /// When the Comment was first created.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// The full textual comment.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The domain object that this Comment relates to.
        /// </summary>
        ICommonBase Owner { get; set; }

        /// <summary>
        /// Denotes whether or not the user has elected to receive notification of additional comments to their own.
        /// </summary>
        bool ReceiveNotifications { get; set; }

        /// <summary>
        /// Denotes whether or not the user needs to be sent a notification that someone else has commented after them. Only comes into 
        /// effect if the ReceiveNotifications property is true.
        /// </summary>
        bool RequiresNotification { get; set; }

        /// <summary>
        /// The type of domain object that this Comment relates to.
        /// </summary>
        CommentOwnerType OwnerType { get; set; }

        /// <summary>
        /// Designates the current status of the Comment, i.e. published or not.
        /// </summary>
        CommentStatusType Status { get; set; }

        /// <summary>
        /// Indicates the status of any report applied to this comment.
        /// </summary>
        CommentReportStatus ReportStatus { get; set; }

        /// <summary>
        /// Determines whether or not the Comment is valid as a persistable object.
        /// </summary>
        bool IsValid();
    }
}