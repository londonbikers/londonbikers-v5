using System;

namespace Apollo.Models.Interfaces
{
    public interface IGalleryImage : ICommonBase, IComparable
    {
        /// <summary>
        /// The unique-identifer for this Image.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// The name of the image.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The comment for this Image.
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Any additional credits for this Image.
        /// </summary>
        string Credit { get; set; }

        /// <summary>
        /// The date this Image was taken on.
        /// </summary>
        DateTime CaptureDate { get; set; }

        /// <summary>
        /// The date this Image was created in the Gallery.
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// If this Image is stored on an third-party server, this is the base-url to prefix the filenames with.
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// The collection of filenames to the specific-size Images.
        /// </summary>
        GalleryImages GalleryImages { get; }

        /// <summary>
        /// Provides an indirect reference to the Gallery containing this Image.
        /// </summary>
        long ParentGalleryId { get; set; }

        /// <summary>
        /// Provides access to any user-comments that may relate to this Document.
        /// </summary>
        Comments Comments { get; }

        /// <summary>
        /// Used to re/define the source image to use for the size-specific Images. Use of this method triggers the downsampling process immediately.
        /// </summary>
        /// <param name="sourceImageFilename">The filename to the source image. This should be located in the gallery's contentstore root.</param>
        void MakeImages(string sourceImageFilename);

        /// <summary>
        /// Invariably, not all images are available at larger sizes. This determines what the best-match size would be for a given one.
        /// </summary>
        /// <param name="desiredSize">The desired primary dimension size.</param>
        /// <returns>An integer representing the best-match primary dimension size for the size sought. This may be the actual desired size, or one smaller.</returns>
        int GetActualSizeForDesiredSize(int desiredSize);
    }
}