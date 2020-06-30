using System;
using Apollo.Models.Interfaces;
using System.Collections.Generic;

namespace Apollo.Models
{
    /// <summary>
    /// Allows For-Each statements to return the item, not the ID for the LightCollection.
    /// </summary>
    public class LightCollectionEnumerator<T> : IEnumerator<T>, ILightCollectionEnumerator where T : class, ICommonBase
    {
        #region members
        private int _position = -1;
        private readonly LightCollection<T> _items;
        #endregion

        #region constructors
        public LightCollectionEnumerator(LightCollection<T> items) 
        {
            if (items == null)
                throw new ArgumentNullException("items");

            _items = items;
        }
        #endregion

        #region public methods
        public object Current 
        {
            get
            {
                if (_position > _items.Count - 1)
                    throw new InvalidOperationException("The enumerator is past the end of the collection.");

                if (_position == -1)
                    throw new InvalidOperationException("The enumerator is before the beginning of the collection.");

                // retrieve the item for the id stored.
                return _items[_position];
            }
        }
        
        public bool MoveNext() 
        {
            if (_position >= _items.Count - 1)
                return false;

            _position++;
            return true;
        }
        
        public void Reset() 
        {
            _position = -1;
        }

        T IEnumerator<T>.Current
        {
            get
            {
                if (_position > _items.Count - 1)
                    throw new InvalidOperationException("The enumerator is past the end of the collection.");

                if (_position == -1)
                    throw new InvalidOperationException("The enumerator is before the beginning of the collection.");

                // retrieve the item for the id stored.
                return _items[_position];
            }
        }
        #endregion

        public void Dispose()
        {
            //Dispose();
        }
    }
}