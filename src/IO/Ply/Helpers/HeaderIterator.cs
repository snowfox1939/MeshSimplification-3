using System;

namespace Polynano.IO.Ply.Helpers
{
    /// <summary>
    /// Helper class for enumerating the PLY File Structure.
    /// The PLY Format is not strictly defined.
    /// therefore the Reader and the Writer need to 
    /// keep track of what properties has been already read 
    /// how many elements have been written
    /// and what kind of values come next. 
    /// </summary>
    internal class HeaderIterator
    {
        /// <summary>
        /// How many elements of the current element type have been written
        /// </summary>
        private int _currentElementInstance;

        /// <summary>
        /// How many properties of the current property type have been written
        /// </summary>
        private int _currentElementPropertyInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="header">The header to iterate</param>
        public HeaderIterator(Header header)
        {
            Header = header;
        }

        /// <summary>
        /// Get the iterated header
        /// </summary>
        public Header Header { get; }

        /// <summary>
        /// Whether the iterator is on the first property
        /// </summary>
        public bool IsOnFirstProperty => _currentElementPropertyInstance == 0;

        /// <summary>
        /// Whether the iterator is on first element
        /// </summary>
        public bool IsOnFirstElement => _currentElementPropertyInstance == 0 && CurrentElement == 0 && _currentElementInstance == 0;

        /// <summary>
        /// Whether the iterator is at the end
        /// </summary>
        public bool IsIterationDone => CurrentElement >= Header.Elements.Count;

        /// <summary>
        /// Get the current element the iterator is at
        /// </summary>
        public int CurrentElement { get; private set; }

        /// <summary>
        /// Move the iterator to the next property
        /// </summary>
        public void NextProperty()
        {
            EnsureNotOutOufBounds();

            _currentElementPropertyInstance++;
            if (_currentElementPropertyInstance >= Header.Elements[CurrentElement].Properties.Count)
            {
                _currentElementPropertyInstance = 0;
                _currentElementInstance++;
            }
            if (_currentElementInstance >= Header.Elements[CurrentElement].Count)
            {
                _currentElementInstance = 0;
                _currentElementPropertyInstance = 0;

                CurrentElement++;
            }
        }

        /// <summary>
        /// Get the current property the iterator is at
        /// </summary>
        public Property CurrentProperty
        {
            get
            {
                EnsureNotOutOufBounds();
                return Header.Elements[CurrentElement].Properties[_currentElementPropertyInstance];
            }
        }

        /// <summary>
        /// Make sure the iterator is not out of bounds.
        /// </summary>
        private void EnsureNotOutOufBounds()
        {
            if (CurrentElement >= Header.Elements.Count)
            {
                throw new InvalidOperationException("Iteration is done");
            }
        }
    }
}