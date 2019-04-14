using System;
using System.IO;
using System.Collections.Generic;

using Polynano.IO.Ply.Helpers;
using Polynano.IO.Ply.Helpers.DataReaders;

namespace Polynano.IO.Ply
{
    /// <summary>
    /// Reader is responsible for the reading of PLY Files.
    /// For more source abstraction generic stream are used,
    /// allowing one to read the file from a database, disk or memory.
    /// The whole class is a state machine because the format has no strict structure.
    /// The class client should read the header first,
    /// decide what data to retrieve and call the appropiate functions in the correct order.
    /// Should the call order not be compatible with the structure definition in the header,
    /// an exception will be thrown.
    /// </summary>
    public class Reader
    {
        /// <summary>
        /// The HeaderIterator used to keep tract of the 
        /// read elements and properties.
        /// </summary>
        private readonly HeaderIterator _iterator;

        /// <summary>
        /// reader used to read file content e.g. ASCII or Binary.
        /// </summary>
        private readonly IDataReader _dataReader;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="source">The Stream object to parse</param>
        public Reader(Stream source)
        {
            var streamReader = new BufferedStreamReader(source);
            _iterator = new HeaderIterator(HeaderParser.Parse(streamReader));

            switch (_iterator.Header.Format)
            {
                case Format.Ascii:
                    _dataReader = new AsciiDataReader(streamReader);
                    break;
                case Format.BinaryBigEndian:
                case Format.BinaryLittleEndian:
                    _dataReader = new BinaryDataReader(streamReader, _iterator.Header.Format == Format.BinaryLittleEndian);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Get the parsed Header
        /// </summary>
        /// <returns>Header object that was parsed from the stream</returns>
        public Header ReadHeader()
        {
            return _iterator.Header;
        }

        /// <summary>
        /// Read a single value from the stream.
        /// </summary>
        /// <returns>Parsed value, type depending on the type defined in the header.</returns>
        public T ReadProperty<T>()
        {
            Property expected = _iterator.CurrentProperty;
            if (expected.IsList)
            {
                throw new InvalidOperationException("Expected property is a list");
            }

            _iterator.NextProperty();

            return _dataReader.ReadProperty<T>(expected);
        }

        /// <summary>
        /// Read a property list from the stream
        /// </summary>
        /// <returns>Parsed array, type depending on the data type defined in the header.</returns>
        public IEnumerable<T> ReadList<T>()
        {
            Property expected = _iterator.CurrentProperty;

            if (!expected.IsList)
            {
                throw new InvalidOperationException("Was not expecting a list");
            }
            _iterator.NextProperty();

            return _dataReader.ReadList<T>(expected);
        }

        /// <summary>
        /// Skip a property that is not wanted.
        /// </summary>
        public void SkipProperty()
        {
            Property expected = _iterator.CurrentProperty;
            _dataReader.SkipProperty(expected);
            _iterator.NextProperty();
        }
    }
}
