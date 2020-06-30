using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Apollo.Models.Interfaces;
using Apollo.Utilities.DataManipulation;

namespace Apollo.Models
{
    /// <summary>
    /// Represents a collection of Comments relating to a domain object.
    /// </summary>
    public class Comments : IComments
    {
        #region members
        private List<Comment> _list;
        private readonly ICommonBase _owner;
        private readonly CommentOwnerType _ownerType;
        readonly DynamicComparer<Comment> _comparer;
        #endregion

        #region accessors
        /// <summary>
        /// Provides access to the actual collection of Comments. Ordered by date-descending. Do NOT use add/remove methods, use Comments.Add() or Comments.Remove().
        /// </summary>
        public List<Comment> Items 
        {
            get 
            {
                if (_list == null)
                    ReInitialise();

                return _list;
            } 
        }

        /// <summary>
        /// Denotes the type of owner object this collection is related to.
        /// </summary>
        public CommentOwnerType OwnerType { get { return _ownerType; } }

        /// <summary>
        /// Retrieves a specific Comment object from the collection by its index position.
        /// </summary>
        public Comment this[int index]
        {
            get
            {
                if (index > _list.Count - 1)
                    throw new IndexOutOfRangeException();

                return _list[index];
            }
         }
        #endregion

        #region constructors
        /// <summary>
        /// Returns a new Comments object for a domain-object.
        /// </summary>
        /// <param name="owner">The domain object this collection relates to.</param>
        internal Comments(ICommonBase owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;

            // determine the owner type.
            if (_owner is Document)
                _ownerType = CommentOwnerType.Editorial;
            else if (_owner is Gallery)
                _ownerType = CommentOwnerType.Galleries;
            else if (_owner is GalleryImage)
                _ownerType = CommentOwnerType.GalleryImages;
            else if (_owner is DirectoryItem)
                _ownerType = CommentOwnerType.Directory;

            _comparer = new DynamicComparer<Comment>("Created");
        }
        #endregion

        #region internal methods
        /// <summary>
        /// Clears the current comments and refreshes them from the database.
        /// </summary>
        public void ReInitialise()
        {
            _list = new List<Comment>();
            lock (Items)
            {
                SqlDataReader reader = null;
                var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
                var command = new SqlCommand("GetComments", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@OwnerType", (byte)OwnerType));

                if (_owner is DirectoryItem)
                    command.Parameters.Add(new SqlParameter("@OwnerID", _owner.Id));
                else if (_owner is Document)
                    command.Parameters.Add(new SqlParameter("@OwnerID", _owner.Id));
                else if (_owner is Gallery)
                    command.Parameters.Add(new SqlParameter("@OwnerID", _owner.Id));
                else if (_owner is GalleryImage)
                    command.Parameters.Add(new SqlParameter("@OwnerID", _owner.Id));

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var comment = Server.Instance.MemberCommentsServer.GetComment(reader.GetInt64(reader.GetOrdinal("ID")));
                        comment.Owner = _owner;
                        Items.Add(comment);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();

                    connection.Close();
                }
            }
        }
        #endregion

        #region public methods
		/// <summary>
		/// Checks to see if the collection contains the comment already.
		/// </summary>
		public bool Contains(Comment comment)
		{
			lock (Items)
			{
				if (Items.Any(listComment => listComment.Id == comment.Id))
				    return true;
			}

			return false;
		}

        /// <summary>
        /// Returns a new Comment object associated with this collection.
        /// </summary>
        public Comment New()
        {
            var comment = Server.Instance.MemberCommentsServer.NewComment();
            comment.Owner = _owner;
            comment.OwnerType = OwnerType;
            return comment;
        }

        /// <summary>
        /// Adds a new Comment to the collection and persists it. An invalid Comment object will throw an ArgumentException.
        /// </summary>
        public void Add(Comment comment)
        {
            // ensure this isn't a duplicate entry.
            if (comment.Id > 0 && Contains(comment))
				return;

            // persist the changes.
            if (comment.IsPersisted) return;
            Server.Instance.MemberCommentsServer.UpdateComment(comment);
            lock (Items)
            {
                Items.Add(comment);
                Items.Sort(_comparer.Compare);
            }
        }

        /// <summary>
        /// Removes a Comment from the collection and un-persists it. An invalid Comment object will throw an ArgumentException.
        /// </summary>
        public void Remove(Comment comment)
        {
            if (comment == null)
                throw new ArgumentException("Comment object not valid!");

			lock (Items)
			{
				// find the comment in the collection.
				var indexPosition = -1;
				for (var i = 0; i < Items.Count; i++)
				{
				    if (Items[i].Id != comment.Id) continue;
				    indexPosition = i;
				    break;
				}

				// comment not found in the collection?
				if (indexPosition == -1)
					throw new ArgumentException("Comment not found in collection!");

				Server.Instance.MemberCommentsServer.DeleteComment(comment);
				Items.RemoveAt(indexPosition);
				Items.Sort(_comparer.Compare);
			}
        }

        /// <summary>
        /// Retrieves a Comment from the collection by its ID.
        /// </summary>
        public Comment GetComment(long commentId)
        {
			lock (Items)
			{
				foreach (var comment in Items.Where(comment => comment.Id == commentId))
				    return comment;
			}

            return null;
        }
        #endregion
    }
}