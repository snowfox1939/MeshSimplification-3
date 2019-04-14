using System;

namespace Polynano.IO.Ply.Helpers
{
    static class PlyTypeHelper
    {
        /// <summary>
        /// Convert the PLY Type to a C# native type
        /// </summary>
        public static Type ToNative(PlyType type)
        {
            Type t;
            switch (type)
            {
                case PlyType.Char:
                    t = typeof(sbyte);
                    break;
                case PlyType.Uchar:
                    t = typeof(byte);
                    break;
                case PlyType.Short:
                    t = typeof(short);
                    break;
                case PlyType.Ushort:
                    t = typeof(ushort);
                    break;
                case PlyType.Int:
                    t = typeof(int);
                    break;
                case PlyType.Uint:
                    t = typeof(uint);
                    break;
                case PlyType.Float:
                    t = typeof(float);
                    break;
                case PlyType.Double:
                    t = typeof(double);
                    break;
                default:
                    throw new ArgumentException();
            }
            return t;
        }

        /// <summary>
        /// Convert the PLY Type to a string representation
        /// </summary>
        public static string ToStringRepresentation(PlyType type)
        {
            if(!Enum.IsDefined(typeof(PlyType), type))
            {
                throw new ArgumentException();
            }
            return type.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Get raw bytes of a variable 
        /// </summary>
        /// <param name="val">the variable to get the bytes of</param>
        /// <param name="type">PlyType that the variable is store in</param>
        /// <returns>array of bytes representing the value that's stored in val</returns>
        public static byte[] GetBytes<T>(T val, PlyType type) where T : IConvertible
        {
            byte[] bytes;
            switch (type)
            {
                case PlyType.Char:
                    bytes = BitConverter.GetBytes(Convert.ToSByte(val));
                    break;
                case PlyType.Uchar:
                    bytes = BitConverter.GetBytes(Convert.ToByte(val));
                    break;
                case PlyType.Short:
                    bytes = BitConverter.GetBytes(Convert.ToInt16(val));
                    break;
                case PlyType.Ushort:
                    bytes = BitConverter.GetBytes(Convert.ToUInt16(val));
                    break;
                case PlyType.Int:
                    bytes = BitConverter.GetBytes(Convert.ToInt32(val));
                    break;
                case PlyType.Uint:
                    bytes = BitConverter.GetBytes(Convert.ToUInt32(val));
                    break;
                case PlyType.Float:
                    bytes = BitConverter.GetBytes(Convert.ToSingle(val));
                    break;
                case PlyType.Double:
                    bytes = BitConverter.GetBytes(Convert.ToDouble(val));
                    break;
                default:
                    throw new ArgumentException();
            }
            return bytes;
        }

        /// <summary>
        /// Get the number of bytes needed 
        /// to store the PlyType.
        /// </summary>
        /// <param name="type">The type to get the bytes from</param>
        /// <returns>Number of bytes needed as int</returns>
        public static int GetTypeSize(PlyType type)
        {
            int s;
            switch (type)
            {
                case PlyType.Char:
                case PlyType.Uchar:
                    s = 1;
                    break;

                case PlyType.Short:
                case PlyType.Ushort:
                    s = 2;
                    break;
                case PlyType.Int:
                case PlyType.Uint:
                case PlyType.Float:
                    s = 4;
                    break;
                case PlyType.Double:
                    s = 8;
                    break;
                default:
                    throw new ArgumentException();
            }
            return s;
        }

        /// <summary>
        /// Convert a string representation of the type to a enum member
        /// </summary>
        /// <param name="ply">type as string to convert</param>
        /// <returns>enum member representing the type</returns>
        public static PlyType GetNativeType(string ply)
        {

            PlyType t;

            switch (ply)
            {
                case "char":
                case "int8":
                    t = PlyType.Char;
                    break;
                case "uchar":
                case "uint8":
                    t = PlyType.Uchar;
                    break;
                case "short":
                case "int16":
                    t = PlyType.Short;
                    break;
                case "ushort":
                case "uint16":
                    t = PlyType.Ushort;
                    break;
                case "int":
                case "int32":
                    t = PlyType.Int;
                    break;
                case "uint":
                case "uint32":
                    t = PlyType.Uint;
                    break;
                case "float":
                case "float32":
                    t = PlyType.Float;
                    break;
                case "double":
                case "float64":
                    t = PlyType.Double;
                    break;
                default:
                    throw new ArgumentException("Invalid ply type");
            }
            return t;
        }
    }
}
