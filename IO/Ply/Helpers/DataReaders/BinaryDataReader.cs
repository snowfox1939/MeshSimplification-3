using System;
using System.Collections.Generic;

namespace Polynano.IO.Ply.Helpers.DataReaders
{
    /// <summary>
    /// Reader that can read values in binary big and litte format.
    /// </summary>
    internal class BinaryDataReader : IDataReader
    {
        private readonly BufferedStreamReader _streamReader;

        private readonly bool _reverseByteOrder;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="streamReader">stream reader to use to read the values</param>
        /// <param name="littleEndian">whether the read values are in little endianess</param>
        public BinaryDataReader(BufferedStreamReader streamReader, bool littleEndian)
        {
            _streamReader = streamReader;
            _reverseByteOrder = BitConverter.IsLittleEndian != littleEndian;
        }

        /// <summary>
        /// Read a list of values
        /// </summary>
        /// <param name="expected">The expected property to read</param>
        /// <returns>iterator over the read values</returns>
        public IEnumerable<T> ReadList<T>(Property expected)
        {
            var count = ToTargetEndianess(_streamReader.ConsumeBytes(PlyTypeHelper.GetTypeSize(expected.ListType)));

            int c;
            if(count.Length == 1)
            {
                c = count[0];
            }
            else if(count.Length == 2)
            {
                c = BitConverter.ToInt16(count, 0);
            }
            else if(count.Length == 4)
            {
                c = BitConverter.ToInt32(count, 0);
            }
            else
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < c; ++i)
            {
                yield return ReadProperty<T>(expected);
            }
        }

        /// <summary>
        /// Read a single value
        /// </summary>
        /// <param name="expected">description of the property that will be read</param>
        /// <returns>read property, parsed to a native type</returns>
        public T ReadProperty<T>(Property expected)
        {
            byte[] v = ToTargetEndianess(_streamReader.ConsumeBytes(PlyTypeHelper.GetTypeSize(expected.ValueType)));

            object val = null;

            switch (expected.ValueType)
            {
                case PlyType.Char:
                    val = BitConverter.ToChar(v, 0);
                    break;
                case PlyType.Uchar:
                    val = v[0];
                    break;
                case PlyType.Short:
                    val = BitConverter.ToInt16(v, 0);
                    break;
                case PlyType.Ushort:
                    val = BitConverter.ToUInt16(v, 0);
                    break;
                case PlyType.Int:
                    val = BitConverter.ToInt32(v, 0);
                    break;
                case PlyType.Uint:
                    val = BitConverter.ToUInt32(v, 0);
                    break;
                case PlyType.Float:
                    val = BitConverter.ToSingle(v, 0);
                    break;
                case PlyType.Double:
                    val = BitConverter.ToDouble(v, 0);
                    break;
            }
            return (T)val;
        }

        /// <summary>
        /// Read a property but don't return it.
        /// It's faster this way since there's no need to parse and convert type.
        /// </summary>
        /// <param name="expected"></param>
        public void SkipProperty(Property expected)
        {
            _streamReader.ConsumeBytes(PlyTypeHelper.GetTypeSize(expected.ValueType));
        }

        /// <summary>
        /// Reverse the byte array if needed in order to convert it to big or little endianess
        /// </summary>
        /// <param name="val">array of bytes to reverse</param>
        /// <returns>reversed array of bytes</returns>
        private byte[] ToTargetEndianess(byte[] val)
        {
            if (_reverseByteOrder)
            {
                Array.Reverse(val, 0, val.Length);
            }
            return val;
        }
    }
}
