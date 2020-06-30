using System;

namespace Tetron.Logic.Models
{
	/// <summary>
	/// Represents an Editorial image within Apollo.
	/// </summary>
    public class Image : IImage
    {
        #region Members
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Filename {  get { return System.IO.Path.GetFileName(Path); } }
		public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsCoverImage { get; set; }
        public bool IsSlideshowImage { get; set; }
        #endregion

        #region Constructors
        public Image()
        {
            Created = DateTime.Now;
        }
        #endregion
    }
}