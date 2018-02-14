using System.Collections.Generic;
using System.Text;
using System;

namespace Polynano.IO.Ply.Helpers
{
    /// <summary>
    /// Parser of the PLY File Header
    /// </summary>
    internal class HeaderParser
    {
        /// <summary>
        ///  Stream reader to read data from the stream
        /// </summary>
        private readonly BufferedStreamReader _reader;

        /// <summary>
        /// The last line not parsed
        /// </summary>
        private string _line;

        /// <summary>
        /// Default construcotr.
        /// </summary>
        /// <param name="reader">SteramReader to use</param>
        private HeaderParser(BufferedStreamReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Parse the data in the buffer using the reader given in parameter and return the header.
        /// </summary>
        /// <param name="reader">Stream reader to use</param>
        /// <returns>Parsed header</returns>
        public static Header Parse(BufferedStreamReader reader)
        {
            return new HeaderParser(reader).Parse();
        }

        /// <summary>
        /// Parse the header from the stream
        /// </summary>
        /// <returns>Parsed header</returns>
        private Header Parse()
        {
            ConsumeMagicNumber();
            var format = ConsumeFormat();
            var comment = ConsumeComments();
            var info = ConsumeObjectInfo();
            var elements = ConsumeElements();

            // when all the Consume functions are done 
            // make sure the line remaining is the end_header
            // command
            if (_line != "end_header")
            {
                throw new ArgumentException();
            }

            return new Header(format, comment, info, elements);
        }

        /// <summary>
        /// The first line of the file is the magic number.
        /// make sure it is there.
        /// </summary>
        private void ConsumeMagicNumber()
        {
            _line = _reader.ConsumeLine();

            if (_line == null || _line.Trim() != "ply")
            {
                throw new ArgumentException();
            }

            _line = _reader.ConsumeLine();
        }

        /// <summary>
        /// The second line is the content format definition.
        /// </summary>
        /// <returns>The parsed format as enum</returns>
        private Format ConsumeFormat()
        {
            string vs = _line.Trim();

            Format v = vs == "format ascii 1.0" ? Format.Ascii
                        : vs == "format binary_big_endian 1.0" ? Format.BinaryBigEndian
                        : vs == "format binary_little_endian 1.0" ? Format.BinaryLittleEndian
                        : throw new ArgumentException("Invalid format");

            _line = _reader.ConsumeLine();
            return v;
        }

        /// <summary>
        /// After the format definition, comments may follow.
        /// Parse all the comments in the file.
        /// </summary>
        /// <returns>Parsed comments, seperated by the unix newline breaker</returns>
        private string ConsumeComments()
        {
            var buffer = new StringBuilder();

            while (_line != null && _line.StartsWith("comment "))
            {
                if (buffer.Length != 0)
                {
                    buffer.AppendLine();
                }

                buffer.Append(_line, 8, _line.Length - 8);

                _line = _reader.ConsumeLine();
            }
            return buffer.ToString();
        }

        /// <summary>
        /// After comments, additional object info may follow.
        /// parse all the info and return it as a string list.
        /// </summary>
        /// <returns>list of parsed model informations</returns>
        private IReadOnlyList<string> ConsumeObjectInfo()
        {
            List<string> info = new List<string>();

            while (_line != null && _line.StartsWith("obj_info "))
            {
                if (_line.Length <= 9)
                {
                    throw new ArgumentException();
                }

                info.Add(_line.Substring(9));

                _line = _reader.ConsumeLine();
            }
            return info;
        }

        /// <summary>
        /// Consume the elements from the stream.
        /// </summary>
        /// <returns>list of parsed elements</returns>
        private IReadOnlyList<Element> ConsumeElements()
        {
            List<Element> elements = new List<Element>();

            while (_line != null && _line.StartsWith("element "))
            {
                string[] t = _line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (t.Length != 3)
                {
                    throw new ArgumentException();
                }

                _line = _reader.ConsumeLine();
                int index;
                try
                {
                    index = int.Parse(t[2]);
                }
                catch (FormatException)
                {
                    throw new ArgumentException();
                }
                // add the element to the element list, parse the properties in between
                elements.Add(new Element(t[1], index, ConsumeProperties()));
            }

            if (elements.Count == 0)
                throw new ArgumentException();

            return elements;
        }

        /// <summary>
        /// Read the stream as long as there are properties to parse
        /// </summary>
        /// <returns>List of parsed properties</returns>
        private IReadOnlyList<Property> ConsumeProperties()
        {
            List<Property> properties = new List<Property>();

            while (_line != null && _line.StartsWith("property "))
            {
                string[] t = _line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (t.Length == 3)
                {
                    properties.Add(new Property(t[2], PlyTypeHelper.GetNativeType(t[1])));
                }
                else if (t.Length == 5 && t[1] == "list")
                {
                    properties.Add(new Property(t[4], PlyTypeHelper.GetNativeType(t[2]), PlyTypeHelper.GetNativeType(t[3])));
                }
                else
                {
                    throw new ArgumentException();
                }
                _line = _reader.ConsumeLine();
            }

            if (properties.Count == 0)
            {
                throw new ArgumentException();
            }

            return properties;
        }
    }
}

