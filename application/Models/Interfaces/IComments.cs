using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IComments
    {
        /// <summary>
        /// Provides access to the actual collection of Comments. Ordered by date-descending. Do NOT use add/remove methods, use Comments.Add() or Comments.Remove().
        /// </summary>
        List<Comment> Items { get; }

        /// <summary>
        /// Denotes the type of owner object this collection is related to.
        /// </summary>
        CommentOwnerType OwnerType { get; }

        /// <summary>
        /// Retrieves a specific Comment object from the collection by its index position.
        /// </summary>
        Comment this[int index] { get; }

        /// <summary>
        /// Clears the current comments and refreshes them from the database.
        /// </summary>
        void ReInitialise();

        /// <summary>
        /// Checks to see if the collection contains the comment already.
        /// </summary>
        bool Contains(Comment comment);

        /// <summary>
        /// Returns a new Comment object associated with this collection.
        /// </summary>
        Comment New();

        /// <summary>
        /// Adds a new Comment to the collection and persists it. An invalid Comment object will throw an ArgumentException.
        /// </summary>
        void Add(Comment comment);

        /// <summary>
        /// Removes a Comment from the collection and un-persists it. An invalid Comment object will throw an ArgumentException.
        /// </summary>
        void Remove(Comment comment);

        /// <summary>
        /// Retrieves a Comment from the collection by its ID.
        /// </summary>
        Comment GetComment(long commentId);
    }
}