using Apollo.Models.Interfaces;

namespace Apollo.Models
{
    public class DocumentTypeGroup : IDocumentTypeGroup
    {
        public DocumentTypeGroup()
        {
            Items = new LightCollection<IDocument>();
        }

        public DocumentType Type
        {
            get;
            set;
        }

        public ILightCollection<IDocument> Items
        {
            get;
            set;
        }
    }
}