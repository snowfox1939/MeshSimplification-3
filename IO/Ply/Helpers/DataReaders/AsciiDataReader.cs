using System;
using System.Collections.Generic;
using System.Globalization;

namespace Polynano.IO.Ply.Helpers.DataReaders
{
    /// <summary>
    /// Reader to read values stored in ascii
    /// </summary>
    internal class AsciiDataReader : IDataReader
    {
        private readonly BufferedStreamReader _streamReader;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="streamReader">stream reader to use to read the characters</param>
        public AsciiDataReader(BufferedStreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        /// <summary>
        /// Read an array
        /// </summary>
        /// <param name="expected">description of the property to be read</param>
        /// <returns>iterator over the read values</returns>
        public IEnumerable<T> ReadList<T>(Property expected)
        {
            int count = int.Parse(_streamReader.ConsumeToken());

            for (int i = 0; i < count; ++i)
            {
                yield return (T)ParseToNativeType(_streamReader.ConsumeToken(), expected.ValueType);
            }
        }

        /// <summary>
        /// Read a single value
        /// </summary>
        /// <param name="expected">description of the property to be read</param>
        /// <returns>read value, converted to native type</returns>
        public T ReadProperty<T>(Property expected)
        {
            return (T)ParseToNativeType(_streamReader.ConsumeToken(), expected.ValueType);
        }

        /// <summary>
        /// Skip the next property
        /// </summary>
        /// <param name="expected">The expected property to be skipped</param>
        public void SkipProperty(Property expected)
        {
            if (!expected.IsList)
            {
                _streamReader.ConsumeToken();
            }
            else
            {
                int count = int.Parse(_streamReader.ConsumeToken());
                for (int i = 0; i < count; ++i)
                {
                    _streamReader.ConsumeToken();
                }
            }
        }

        /// <summary>
        /// Parse a string to the expected data type
        /// </summary>
        /// <param name="value">string representation of a value</param>
        /// <param name="type">Type to parse the value to</param>
        /// <returns>numeric value returned as a generic object</returns>
        private object ParseToNativeType(string value, PlyType type)
        {
            object o;
            switch (type)
            {
                case PlyType.Float:
                    o = float.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Int:
                    o = int.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Char:
                    o = sbyte.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Uchar:
                    o = byte.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Short:
                    o = short.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Ushort:
                    o = ushort.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Uint:
                    o = uint.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case PlyType.Double:
                    o = double.Parse(value, CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentException();
            }
            return o;
        }
    }
}
