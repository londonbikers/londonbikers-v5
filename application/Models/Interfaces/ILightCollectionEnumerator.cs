namespace Apollo.Models.Interfaces
{
    public interface ILightCollectionEnumerator
    {
        object Current { get; }
        bool MoveNext();
        void Reset();
        void Dispose();
    }
}