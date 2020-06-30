using System;
using System.Collections;

namespace Apollo.Models
{
    /// <summary>
    /// Allows For-Each statements to return the Document, not the UID for the LightDocumentCollection.
    /// </summary>
    public class LightDocumentCollectionEnumerator : IEnumerator
    {
        #region members
        private int _position = -1;
        private readonly LightDocumentCollection _documents;
        #endregion

        #region constructors
        public LightDocumentCollectionEnumerator(LightDocumentCollection documents) 
        {
            if (documents == null)
                throw new ArgumentNullException("documents");

            _documents = documents;
        }
        #endregion

        #region public methods
        public object Current 
        {
            get
            {
                if (_position > _documents.Count - 1)
                    throw new InvalidOperationException("The enumerator is past the end of the collection.");

                if (_position == -1)
                    throw new InvalidOperationException("The enumerator is before the beginning of the collection.");

                // retrieve the document for the uid stored.
                return _documents[_position];
            }
        }
        
        public bool MoveNext() 
        {
            if (_position >= _documents.Count - 1)
                return false;

            _position++;
            return true;
        }
        
        public void Reset() 
        {
            _position = -1;
        }
        #endregion
    }
}