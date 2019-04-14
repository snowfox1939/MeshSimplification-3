using System.Collections.Generic;

namespace Polynano.IO.Ply.Helpers.DataReaders
{
    /// <summary>
    /// Common interface for the data readers
    /// </summary>
    internal interface IDataReader
    {
        /// <summary>
        /// Skip the current property, e.g. read it without returning.
        /// </summary>
        /// <param name="expected">expected property</param>
        void SkipProperty(Property expected);

        /// <summary>
        /// Read a single property
        /// </summary>
        /// <param name="expected">Definition of the property to read</param>
        /// <returns>the read property already converted to a c sharp type</returns>
        T ReadProperty<T>(Property expected);

        /// <summary>
        /// Read an array
        /// </summary>
        /// <param name="expected">Expected property describing the array</param>
        /// <returns>iterator over the read values</returns>
        IEnumerable<T> ReadList<T>(Property expected);
    }
}
