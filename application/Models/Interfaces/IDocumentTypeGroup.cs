namespace Apollo.Models.Interfaces
{
    /// <summary>
    /// Defines a group of documents that are all of the same type.
    /// </summary>
    public interface IDocumentTypeGroup
    {
        /// <summary>
        /// Defines what type of documents this group holds.
        /// </summary>
        DocumentType Type { get; set; }

        /// <summary>
        /// The collection of documents for the type defined in this group.
        /// </summary>
        ILightCollection<IDocument> Items { get; set; }
    }
}
