using System;
using System.Collections.Generic;

namespace Tetron.Logic.Models
{
	/// <summary>
	/// Represents an editorial document within Apollo.
	/// </summary>
    public interface IDocument
    {
        string Title { get; set; }
        string Abstract { get; set; }
        string Body { get; set; }
        string OriginalMessageBody { get; set; }
        string FileStorePath { get; set; }
        string MessageId { get; set; }
        DateTime Created { get; set; }
        List<string> Tags { get; set; }
        List<Image> Images { get; set; }
    }
}