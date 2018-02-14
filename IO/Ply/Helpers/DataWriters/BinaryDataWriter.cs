using System;
using System.IO;
using System.Text;

namespace Polynano.IO.Ply.Helpers.DataWriters
{
    /// <summary>
    /// Responsible for the writing the data in binary format
    /// supports both litte and big endianess
    /// </summary>
    internal class BinaryDataWriter : BinaryWriter, IDataWriter
    {
        private readonly HeaderIterator _iterator;

        private readonly bool _reverseByteOrder;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="target">Target stream the data will be written into</param>
        /// <param name="iterator">Header iterator to use and to keep track of the written properties</param>
        /// <param name="littleEndian">whethen to write the data in the little endianess</param>
        public BinaryDataWriter(Stream target, HeaderIterator iterator, bool littleEndian)
            : base(target, Encoding.BigEndianUnicode, leaveOpen: true)
        {
            _iterator = iterator;
            _reverseByteOrder = littleEndian != BitConverter.IsLittleEndian;
        }

        /// <summary>
        /// Write an array of values
        /// </summary>
        /// <param name="data">values to write</param>
        public void WriteList<T>(params T[] data) where T : IConvertible
        {
            Write(ToTargetEndianess(GetCountInBytes(data.Length)));
            foreach (T val in data)
            {
                WriteValue(val);
            }
        }

        /// <summary>
        /// Write a single value
        /// </summary>
        /// <param name="val">value to write</param>
        public void WriteValue<T>(T val) where T : IConvertible
        {
            var bytes = ToTargetEndianess(PlyTypeHelper.GetBytes(val, _iterator.CurrentProperty.ValueType));

            Write(bytes);
        }

        /// <summary>
        /// Convert a value from big endian to little endian 
        /// or little endian to big endian
        /// in practice it means simply reversing the array when needed.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private byte[] ToTargetEndianess(byte[] val)
        {
            if (_reverseByteOrder)
            {
                Array.Reverse(val, 0, val.Length);
            }
            return val;
        }

        /// <summary>
        /// Convert an int to the type defined in the header 
        /// and returns it as an array of bytes in the right byte-orderness
        /// </summary>
        /// <param name="arrayLength">number to convert</param>
        /// <returns>arary of bytes representing the number</returns>
        private byte[] GetCountInBytes(int arrayLength)
        {
            byte[] count;
            switch (_iterator.CurrentProperty.ListType)
            {
                case PlyType.Char:
                    if (arrayLength > char.MaxValue)
                        throw new ArgumentException("The array is too big for the pre-defined list indexer type");
                    count = BitConverter.GetBytes(Convert.ToChar(arrayLength));
                    break;
                case PlyType.Uchar:
                    if (arrayLength > byte.MaxValue)
                        throw new ArgumentException("The array is too big for the pre-defined list indexer type");
                    count = BitConverter.GetBytes(Convert.ToByte(arrayLength));
                    break;
                case PlyType.Short:
                    if (arrayLength > short.MaxValue)
                        throw new ArgumentException("The array is too big for the pre-defined list indexer type");
                    count = BitConverter.GetBytes(Convert.ToInt16(arrayLength));
                    break;
                case PlyType.Ushort:
                    if (arrayLength > ushort.MaxValue)
                        throw new ArgumentException("The array is too big for the pre-defined list indexer type");
                    count = BitConverter.GetBytes(Convert.ToUInt16(arrayLength));
                    break;
                case PlyType.Int:
                    count = BitConverter.GetBytes(arrayLength);
                    break;
                case PlyType.Uint:
                    count = BitConverter.GetBytes(Convert.ToUInt32(arrayLength));
                    break;
                default:
                    throw new ArgumentException("Invalid type of the list indexer");
            }
            return count;
        }
    }
}
