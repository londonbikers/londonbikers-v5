using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
    /// <summary>
    /// Represents a member-written comment in response to a domain object.
    /// </summary>
    public class Comment : CommonBase, IComment
    {
        #region accessors
        /// <summary>
        /// The User who posted this Comment.
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// When the Comment was first created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The full textual comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The domain object that this Comment relates to.
        /// </summary>
        public ICommonBase Owner { get; set; }

        /// <summary>
        /// Denotes whether or not the user has elected to receive notification of additional comments to their own.
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        /// <summary>
        /// Denotes whether or not the user needs to be sent a notification that someone else has commented after them. Only comes into 
        /// effect if the ReceiveNotifications property is true.
        /// </summary>
        public bool RequiresNotification { get; set; }

        /// <summary>
        /// The type of domain object that this Comment relates to.
        /// </summary>
        public CommentOwnerType OwnerType { get; set; }

        /// <summary>
        /// Designates the current status of the Comment, i.e. published or not.
        /// </summary>
        public CommentStatusType Status { get; set; }

        /// <summary>
        /// Indicates the status of any report applied to this comment.
        /// </summary>
        public CommentReportStatus ReportStatus { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Returns a new Comment object.
        /// </summary>
        /// <param name="mode">If 'New' then an empty Comment object is prepared, else a simple shell is returned to be populated.</param>
        internal Comment(ObjectCreationMode mode)
        {
            if (mode == ObjectCreationMode.New)
                IsPersisted = false;

            ReceiveNotifications = true;
            RequiresNotification = true;
            Created = DateTime.Now;
            Status = CommentStatusType.Active;
            ReportStatus = CommentReportStatus.NoReport;
            DerivedType = GetType();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Determines whether or not the Comment is valid as a persistable object.
        /// </summary>
        public bool IsValid()
        {
            return (Author != null && Owner != null) && !string.IsNullOrEmpty(Text);
        }
        #endregion
    }
}