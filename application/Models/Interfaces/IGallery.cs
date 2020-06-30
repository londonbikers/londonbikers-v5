using System;
using System.Collections.Generic;

namespace Apollo.Models.Interfaces
{
    public interface IGallery : ICommonBase
    {
        /// <summary>
        /// The unique identifier for this Gallery.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// The title of the Gallery.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The description of the Gallery, can be used as an introduction of any length.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The time the gallery was created in Apollo.
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// The collection of Images and Videos being shown in this Gallery.
        /// </summary>
        List<IGalleryImage> Photos { get; }

        /// <summary>
        /// The type of Gallery this is. I.E loose or specific.
        /// </summary>
        GalleryType Type { get; set; }

        /// <summary>
        /// Denotes whether or not this Gallery is public, to be implemented so that only Apollo users can view the Gallery.
        /// </summary>
        bool IsPublic { get; set; }

        /// <summary>
        /// Denotes whether or not a gallery is active or not.
        /// </summary>
        GalleryStatus Status { get; set; }

        /// <summary>
        /// Provides a list of the categories to which this Gallery belongs to.
        /// </summary>
        List<GalleryCategory> Categories { get; }

        /// <summary>
        /// Provides access to any user-comments that may relate to this Gallery.
        /// </summary>
        Comments Comments { get; }

        /// <summary>
        /// Retrieves a specific Exhibit in the gallery.
        /// </summary>
        object this[long id] { get; }

        /// <summary>
        /// If not already present, this will add an Image to the exhibits collection.
        /// </summary>
        void AddPhoto(GalleryImage galleryImage);

        /// <summary>
        /// Removes an Image exhibit from the Gallery.
        /// </summary>
        void RemovePhoto(GalleryImage galleryImage);

        /// <summary>
        /// Determines the position within the collection of a particular image. 1-based.
        /// </summary>
        /// <param name="galleryImage">An Image belonging to the Gallery.</param>
        int GetImagePosition(GalleryImage galleryImage);

        /// <summary>
        /// Retrieves a specific Image from the Gallery.
        /// </summary>
        GalleryImage GetImage(long imageId);

        /// <summary>
        /// Facilitates sequential viewing of Images within the Gallery.
        /// </summary>
        /// <param name="currentGalleryImage">To know what Image to serve next, the current one is required.</param>
        IGalleryImage NextImage(IGalleryImage currentGalleryImage);

        /// <summary>
        /// Facilitates sequential viewing of Images within the Gallery.
        /// </summary>
        /// <param name="currentGalleryImage">To know what Image to serve next, the current one is required.</param>
        IGalleryImage PreviousImage(IGalleryImage currentGalleryImage);
    }
}