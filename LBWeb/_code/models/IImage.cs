using System;

namespace Tetron.Logic.Models
{
	/// <summary>
	/// Represents an Editorial image within Apollo.
	/// </summary>
    public interface IImage
    {
        DateTime Created { get; set; }
        string Name { get; set; }
        string Filename { get; }
		string Path { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        bool IsCoverImage { get; set; }
        bool IsSlideshowImage { get; set; }
    }
}