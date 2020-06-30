using System;

namespace Apollo.Models.Interfaces
{
    public interface ITvListing
    {
        /// <summary>
        /// The unique identifier for this listing in the context of the current listings batch.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// The Date/Time of when the programme begins.
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// The Date/Time of when the programme ends.
        /// </summary>
        DateTime End { get; set; }

        /// <summary>
        /// The name of the programme.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The type, or genre of the programme.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The name of the channel the programme is on.
        /// </summary>
        string Channel { get; set; }

        /// <summary>
        /// The programme description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Sorts by StartDate.
        /// </summary>
        int CompareTo(object obj);
    }
}