using System;
using System.Collections.Generic;

namespace Tetron.Logic.Models
{
    /// <summary>
    /// Distilled and modified version of the Apollo Document equivilent used for web-service calls.
    /// </summary>
    public class Document : IDocument
    {
        #region Members
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }
        public string OriginalMessageBody { get; set; }
        public string MessageId { get; set; }
        public DateTime Created { get; set; }
        public List<string> Tags { get; set; }
        public List<Image> Images { get; set; }
        public string FileStorePath { get; set;  }
        #endregion

        #region Constructors
        public Document()
        {
            Created = DateTime.Now;
            Tags = new List<string>();
            Images = new List<Image>();
        }
        #endregion
    }
}