namespace Apollo.Models.Interfaces
{
    public interface IGalleryImages
    {
        /// <summary>
        /// The filename of the 800 x 600 image.
        /// </summary>
        string EightHundred { get; set; }

        /// <summary>
        /// The filename of the 1024 x 768 image.
        /// </summary>
        string OneThousandAndTwentyFour { get; set; }

        /// <summary>
        /// The filename of the 1600 x 1200 image.
        /// </summary>
        string SixteenHundred { get; set; }

        /// <summary>
        /// The filename of the thumbnail image.
        /// </summary>
        string Thumbnail { get; set; }
    }
}