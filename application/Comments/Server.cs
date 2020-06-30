using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Threading;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;
using Apollo.Caching;

namespace Apollo.Comments
{
    public class MemberCommentsServer
    {
        #region contructors
        /// <summary>
        /// Returns a new MemberCommentsServer object.
        /// </summary>
        internal MemberCommentsServer()
        {
        }
        #endregion

        #region public methods
		/// <summary>
		/// Retrieves a comment from the system.
		/// </summary>
		public Comment GetComment(long commentId)
		{
			var comment = CacheManager.RetrieveItem(CacheManager.GetApplicationUniqueId(typeof(Comment), commentId)) as Comment;
			if (comment == null)
			{
				var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
				SqlDataReader reader;
				var command = new SqlCommand("GetComment", connection) {CommandType = CommandType.StoredProcedure};
			    command.Parameters.Add(new SqlParameter("@CommentID", commentId));

				try
				{
					connection.Open();
					reader = command.ExecuteReader();
				    reader.Read();
				    comment = InitialiseComment(reader);

				    if (comment != null)
						CacheManager.AddItem(comment, comment.ApplicationUniqueId);
				}
				finally
				{
					connection.Close();
				}
			}
			
			return comment;
		}

        /// <summary>
        /// Retrieves a list of the latest comments in the system across all owner types.
        /// </summary>
        /// <param name="maxComments">The maximum number of comments to retrieve.</param>
        /// <returns>A comment collection live from the database.</returns>
        public List<Comment> GetLatestComments(int maxComments)
        {
            var comments = new List<Comment>();
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            SqlDataReader reader;
            var command = new SqlCommand("GetLatestComments", connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(new SqlParameter("@MaxComments", maxComments));

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var comment = GetComment((long)reader["ID"]);
                    comments.Add(comment);
                }
            }
            finally
            {
                connection.Close();
            }

            return comments;
        }

        /// <summary>
        /// Retrieves a list of the latest reported comments for all owner types.
        /// </summary>
        /// <param name="maxComments">The maximum number of comments to retrieve.</param>
        /// <returns>A comment collection live from the database.</returns>
        public List<Comment> GetLatestReportedComments(int maxComments)
        {
			var comments = new List<Comment>();
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			SqlDataReader reader;
			var command = new SqlCommand("GetLatestReportedComments", connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(new SqlParameter("@MaxComments", maxComments));

			try
			{
				connection.Open();
				reader = command.ExecuteReader();
			    while (reader.Read())
			    {
			        var comment = GetComment((long)reader["ID"]);
			        comments.Add(comment);
			    }
			}
			finally
			{
				connection.Close();
			}

			return comments;
        }

        /// <summary>
        /// Returns a new Comment object.
        /// </summary>
        public Comment NewComment()
        {
            return new Comment(ObjectCreationMode.New);
        }

        /// <summary>
        /// Persists any changes to a comment to the database.
        /// </summary>
        /// <returns>A boolean indicating whether or not the update was successful.</returns>
        public bool UpdateComment(Comment comment)
        {
            return UpdateComment(comment, false);
        }

        /// <summary>
        /// Persists any changes to a comment to the database.
        /// </summary>
        /// <returns>A boolean indicating whether or not the update was successful.</returns>
        public bool UpdateComment(Comment comment, bool bypassNotifications)
        {
            if (comment == null || !comment.IsValid())
                throw new ArgumentException("Comment object not valid! Check for missing properties.");

            // ensure the comment text doesn't contain anything it shouldn't.
			comment.Text = WebUtils.FilterOutCensoredWords(comment.Text);
            comment.Text = WebUtils.FilterOutHtml(comment.Text);
            comment.Text = WebUtils.FilterOutScripting(comment.Text);
            comment.Text = WebUtils.FilterOutSqlInjection(comment.Text);

            // activate any links.
            comment.Text = WebUtils.ActivateLinksInText(comment.Text);

            // persist to database.
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("UpdateComment", connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add(new SqlParameter("@ID", comment.Id));
            command.Parameters.Add(new SqlParameter("@AuthorID", comment.Author.Uid));
            command.Parameters.Add(new SqlParameter("@Created", comment.Created));
            command.Parameters.Add(new SqlParameter("@Comment", comment.Text));
            command.Parameters.Add(new SqlParameter("@Status", (int)comment.Status));
            command.Parameters.Add(new SqlParameter("@OwnerID", ((CommonBase) comment.Owner).Id));
            command.Parameters.Add(new SqlParameter("@OwnerType", (int)comment.OwnerType));
            command.Parameters.Add(new SqlParameter("@ReportStatus", (int)comment.ReportStatus));
            command.Parameters.Add(new SqlParameter("@ReceiveNotifications", comment.ReceiveNotifications));
            command.Parameters.Add(new SqlParameter("@RequiresNotification", comment.RequiresNotification));

            try
            {
                connection.Open();
                comment.Id = Convert.ToInt64(command.ExecuteScalar());
                
            }
            finally
            {
                connection.Close();
            }

            Models.Comments comments = null;
            switch (comment.OwnerType)
            {
                case CommentOwnerType.Directory:
                    comments = ((DirectoryItem) comment.Owner).Comments;
                    break;
                case CommentOwnerType.Editorial:
                    comments = ((Document) comment.Owner).Comments;
                    break;
                case CommentOwnerType.Galleries:
                    comments = ((Gallery) comment.Owner).Comments;
                    break;
                case CommentOwnerType.GalleryImages:
                    comments = ((GalleryImage) comment.Owner).Comments;
                    break;
            }

            // remove any notification flags from previous comments by this user.
            if (!bypassNotifications)
            {
                if (comments != null)
                    foreach (var collectionComment in comments.Items.Where(collectionComment => collectionComment.Author.Uid == comment.Author.Uid && collectionComment.RequiresNotification))
                    {
                        collectionComment.RequiresNotification = false;
                        UpdateComment(collectionComment, true);
                    }
            }

			// update comment instances if this is an amendment, not a creation.
            if (comment.IsPersisted)
            {
                // NECESSARY?!
                //comments.ReInitialise();
            }
            else if (!bypassNotifications)
            {
                // new comment, ensure notifications are sent out.
                InitiateNotificationDelivery(comments, comment);
                comment.IsPersisted = true;
            }
			
            return true;
        }

        /// <summary>
        /// Permanently removes a comment from the system.
        /// </summary>
        public void DeleteComment(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            // delete from the database.
            var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
            var command = new SqlCommand("DeleteComment", connection) {CommandType = CommandType.StoredProcedure};
            command.Parameters.Add(new SqlParameter("@ID", comment.Id));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
			}

			// update comment instances.
			switch (comment.OwnerType)
			{
			    case CommentOwnerType.Directory:
			        ((DirectoryItem) comment.Owner).Comments.ReInitialise();
			        break;
			    case CommentOwnerType.Editorial:
			        ((Document) comment.Owner).Comments.ReInitialise();
			        break;
			    case CommentOwnerType.Galleries:
			        ((Gallery) comment.Owner).Comments.ReInitialise();
			        break;
			    case CommentOwnerType.GalleryImages:
			        ((GalleryImage) comment.Owner).Comments.ReInitialise();
			        break;
			}

			// remove from the cache.
			CacheManager.RemoveItem(comment.ApplicationUniqueId);
		}
        #endregion

        #region internal methods
        /// <summary>
        /// Builds a new Comment object from a SqlDataReader with the raw SQL comment data in.
        /// </summary>
        internal Comment InitialiseComment(SqlDataReader reader)
        {
            var comment = new Comment(ObjectCreationMode.Retrieve)
            {
                Id = Convert.ToInt64(reader["ID"]),
                Created = (DateTime) Sql.GetValue(typeof (DateTime), reader["Created"]),
                Author = Server.Instance.UserServer.GetUser((Guid) Sql.GetValue(typeof (Guid), reader["AuthorID"])),
                Text = Sql.GetValue(typeof (string), reader["Comment"]) as string,
                Status = (CommentStatusType) (byte) reader["Status"],
                OwnerType = (CommentOwnerType) (byte) reader["OwnerType"],
                ReportStatus = (CommentReportStatus) (byte) reader["ReportStatus"],
                ReceiveNotifications = (bool) Sql.GetValue(typeof (bool), reader["ReceiveNotification"]),
                RequiresNotification = (bool) Sql.GetValue(typeof (bool), reader["RequiresNotification"])
            };

            // an OwnerID might not be specified.
			if (Sql.DoesColumnExistInResult(reader, "OwnerID"))
			{
				var ownerId = (long)Sql.GetValue(typeof(long), reader["OwnerID"]);

				switch (comment.OwnerType)
				{
				    case CommentOwnerType.Directory:
				        comment.Owner = Server.Instance.DirectoryServer.GetItem(ownerId);
				        break;
				    case CommentOwnerType.Editorial:
				        comment.Owner = Server.Instance.ContentServer.GetDocument(ownerId);
				        break;
				    case CommentOwnerType.Galleries:
				        comment.Owner = Server.Instance.GalleryServer.GetGallery(ownerId);
				        break;
				    case CommentOwnerType.GalleryImages:
				        comment.Owner = Server.Instance.GalleryServer.GetImage(ownerId);
				        break;
				}
			}
            return comment;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Handles the delivery of email notification to members who've subscribed to comment threads.
        /// </summary>
        private static void InitiateNotificationDelivery(Models.Comments comments, Comment comment)
        {
            var deliveryThread = new Thread(DeliverNotifications);
            deliveryThread.Start(new CommentsNotifierContainer {Comments = comments, Comment = comment});
        }

        /// <summary>
        /// Run on a new thread: Handles sending an email notification to all member subscribers.
        /// </summary>
        private static void DeliverNotifications(object data)
        {
            try
            {
                var cnc = data as CommentsNotifierContainer;
                if (cnc == null)
                {
                    Logger.LogWarning("MemberCommentsServer.DeliverNotifications() - Parameter not of type CommentsNotifierContainer.");
                    return;
                }

                var contentUrl = string.Empty;
                var contentType = string.Empty;
                var baseUrl = ConfigurationManager.AppSettings["Global.SiteURL"];

                switch (cnc.Comment.OwnerType)
                {
                    case CommentOwnerType.Directory:
                        {
                            var item = cnc.Comment.Owner as DirectoryItem;
                            contentType = "directory item";
                            contentUrl = string.Format("{0}/directory/{1}/{2}", baseUrl, item.Id, WebUtils.ToUrlString(item.Title));
                        }
                        break;
                    case CommentOwnerType.Editorial:
                        {
                            var doc = cnc.Comment.Owner as Document;
                            contentType = (doc.Type == DocumentType.News) ? "news story" : "article";
                            contentUrl = string.Format("{0}/{1}/{2}/{3}", baseUrl, ((Section) doc.Sections[0]).UrlIdentifier, doc.Id, WebUtils.ToUrlString(doc.Title));
                        }
                        break;
                    case CommentOwnerType.Galleries:
                        {
                            var gallery = cnc.Comment.Owner as Gallery;
                            contentType = "gallery";
                            contentUrl = string.Format("{0}/galleries/{1}/{2}", baseUrl, gallery.Id, WebUtils.ToUrlString(gallery.Title));
                        }
                        break;
                    case CommentOwnerType.GalleryImages:
                        {
                            var img = cnc.Comment.Owner as GalleryImage;
                            contentType = "gallery image";
                            contentUrl = string.Format("{0}/galleries/image/{1}/{2}/{3}", baseUrl, img.ParentGalleryId, img.Id, WebUtils.ToUrlString(img.Name));
                        }
                        break;
                }

                // enumerate possible recipients. get a copy to enable round-about locking.
                foreach (var c in cnc.Comments.Items.ToArray())
                {
                    // with growing comment lists, it's possible the comments collection will be added to and notified upon
                    // after this operation, so just notify from the new comment.
                    // also don't notify for the new comment, or ones which have already been notified, or ones from the same author.
                    if (c.Created > cnc.Comment.Created ||
                        c.Id == cnc.Comment.Id ||
                        c.Author.Uid == cnc.Comment.Author.Uid ||
                        c.ReceiveNotifications == false ||
                        (c.ReceiveNotifications && !c.RequiresNotification))
                        continue;

                    var args = new string[3];
                    args[0] = c.Author.Username;
                    args[1] = contentType;
                    args[2] = contentUrl;

                    Server.Instance.CommunicationServer.SendMail(EmailType.CommentReplyNotification, false, c.Author.Email, args);

                    // update comment's require-notification flag.
                    c.RequiresNotification = false;
                    Server.Instance.MemberCommentsServer.UpdateComment(c);
                }
            }
            catch (Exception ex)
            {
                // try/catch is less than ideal, but I'm not sure exceptions are being caught by conventional means, so this is temporary.
                Logger.LogException(ex, "MemberCommentsServer.DeliverNotifications() - Delivery failed.");
            }
        }
        #endregion
    }

    /// <summary>
    /// Used for passing values to the comment subscriber notification thread.
    /// </summary>
    internal class CommentsNotifierContainer
    {
        public Models.Comments Comments { get; set; }
        public Comment Comment { get; set; }
    }
}