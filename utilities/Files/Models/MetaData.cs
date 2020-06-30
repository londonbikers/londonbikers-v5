using System;
using System.Collections.Generic;

namespace Apollo.Utilities.Files.Models
{
    /// <summary>
    /// Holds common media data from that embedded in files.
    /// </summary>
    public class MetaData
    {
        #region accessors
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Creator { get; set; }
        public List<string> Tags { get; set; }
        public DateTime Captured { get; set; }
        public ExtendedMetaData ExtendedData { get; set; }
        #endregion

        #region constructors
        internal MetaData()
        {
            Captured = DateTime.MinValue;
        }
        #endregion
    }

    /// <summary>
    /// Represents more exhaustive meta-data that can be found embedded within an image file
    /// that comes in the form of various XML schemas.
    /// </summary>
    public class ExtendedMetaData
    {
        #region tiff accessors
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string XResolution { get; set; }
        public string YResolution { get; set; }
        #endregion

        #region exif accessors
        public string ExposureTime { get; set; }
        public string FNumber { get; set; }
        public string FocalLength { get; set; }
        public string Iso { get; set; }
        public string FlashFired { get; set; }
        public string FlashReturn { get; set; }
        public string FlashMode { get; set; }
        public string FlashFunction { get; set; }
        public string FlashRedEyeMode { get; set; }
        #endregion

        #region xap accessors
        public string CreatorTool { get; set; }
        #endregion

        #region aux accessors
        public string Lens { get; set; }
        #endregion

        /* DISABLED -- NOT SURE OF POLITICAL SENSITIVITY
		#region crs accessors
		public string WhiteBalance { get; set; }
		public string Exposure { get; set; }
		public string Shadows { get; set; }
		public string Brightness { get; set; }
		public string Contrast { get; set; }
		public string Saturation { get; set; }
		public string Sharpness { get; set; }
		public string LuminanceSmoothing { get; set; }
		public string ColorNoiseReduction { get; set; }
		public string ChromaticAberrationR { get; set; }
		public string ChromaticAberrationB { get; set; }
		public string VignetteAmount { get; set; }
		public string ShadowTint { get; set; }
		public string RedHue { get; set; }
		public string RedSaturation { get; set; }
		public string GreenHue { get; set; }
		public string GreenSaturation { get; set; }
		public string BlueHue { get; set; }
		public string BlueSaturation { get; set; }
		public string FillLight { get; set; }
		public string Vibrance { get; set; }
		public string HighlightRecovery { get; set; }
		public string Clarity { get; set; }
		public string Defringe { get; set; }
		#endregion
		*/

        #region constructors
        internal ExtendedMetaData()
        {
        }
        #endregion
    }
}