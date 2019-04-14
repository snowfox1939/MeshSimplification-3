using System.Text;

namespace Polynano.IO.Ply.Helpers
{
    internal static class HeaderGenerator
    {
        /// <summary>
        /// Generate a valid string defining the ply header
        /// from the header class.
        /// </summary>
        /// <param name="header">The header to generate string from</param>
        /// <returns>A valid string to be written into the PLY file</returns>
        public static string GetHeader(Header header)
        {
            var sb = new StringBuilder();

            sb.AppendLine("ply");
            sb.AppendLine(header.Format == Format.Ascii ? "format ascii 1.0"
                       : header.Format == Format.BinaryBigEndian ? "format binary_big_endian 1.0"
                       : "format binary_little_endian 1.0");

            if (header.Comment != null)
            {
                foreach (string comment in header.Comment.Split('\n'))
                {
                    sb.AppendLine($"comment {comment}");
                }
            }

            if (header.ObjectInfo != null)
            {
                foreach (string info in header.ObjectInfo)
                {
                    sb.AppendLine($"obj_info {info}");
                }
            }

            foreach (Element element in header.Elements)
            {
                sb.AppendLine($"element {element.Name} {element.Count}");
                foreach (Property p in element.Properties)
                {
                    sb.AppendLine(p.IsList
                        ? $"property list {PlyTypeHelper.ToStringRepresentation(p.ListType)} {PlyTypeHelper.ToStringRepresentation(p.ValueType)} {p.Name}"
                        : $"property {PlyTypeHelper.ToStringRepresentation(p.ValueType)} {p.Name}");
                }
            }

            sb.AppendLine("end_header");
            return sb.ToString();
        }
    }
}