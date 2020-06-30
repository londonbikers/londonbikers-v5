using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IEditorialImages
    {
        /// <summary>
        /// The number of images in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The index of the Cover Image within the collection.
        /// </summary>
        int CoverImage { get; set; }

        /// <summary>
        /// The index of the Intro Image within the collection.
        /// </summary>
        int IntroImage { get; set; }

        /// <summary>
        /// The image collection.
        /// </summary>
        List<EditorialImage> List { get; }

        /// <summary>
        /// The default indexer.
        /// </summary>
        EditorialImage this[int index] { get; }

        /// <summary>
        /// Attempts to retrieve an image of a specific type. Only of use for cover and intro images really.
        /// </summary>
        EditorialImage GetImage(ContentImageType type, bool randomise, bool fallbackToCover);

        /// <summary>
        /// Adds a content image to the collection.
        /// </summary>
        /// <param name="id">The ID of the image to add.</param>
        void Add(long id);

        /// <summary>
        /// Removes a content image from the collection.
        /// </summary>
        /// <param name="id">The ID of the image to remove.</param>
        void Remove(long id);

        /// <summary>
        /// Attempts to retrieve a specific image from the collection.
        /// </summary>
        /// <param name="id">The ID of the image to retrieve.</param>
        EditorialImage GetImage(long id);

        /// <summary>
        /// Returns a sub-collection of Images with a specific Type.
        /// </summary>
        List<EditorialImage> FilterImages(ContentImageType typeToReturn);

        /// <summary>
        /// Counts how many images there are in the collection of a specific type.
        /// </summary>
        /// <param name="imageType">The type of images to count.</param>
        int ImageTypeCount(ContentImageType imageType);
    }
}