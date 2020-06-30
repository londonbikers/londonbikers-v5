namespace Apollo.Models.Interfaces
{
    public interface ILightDocumentCollectionEnumerator
    {
        object Current { get; }
        bool MoveNext();
        void Reset();
    }
}