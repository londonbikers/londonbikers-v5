using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface IBlacklistedReferrersCollection
    {
        /// <summary>
        /// Adds a URL to the collection.
        /// </summary>
        bool Add(string url);

        /// <summary>
        /// Removes a URL from the collection.
        /// </summary>
        bool Remove(string url);

        /// <summary>
        /// Determines whether or not the collection contains a specific URL.
        /// </summary>
        bool Contains(string url);

        /// <summary>
        /// Public default indexer.
        /// </summary>
        string this[int index] { get; set; }

        /// <summary>
        /// Queries the database for a list of the blacklisted referrers.
        /// </summary>
        void RetrieveBlacklistedReferrers();

        /// <summary>
        /// Persists a new referrer back to the database.
        /// </summary>
        void PersistBlacklistedReferrer(string url);

        /// <summary>
        /// Removes a specific referrer from the database.
        /// </summary>
        void UnpersistBlacklistedReferrer(string url);

        void Clear();
        void RemoveAt(int index);
        IEnumerator GetEnumerator();
        int Capacity { get; set; }
        int Count { get; }
    }
}