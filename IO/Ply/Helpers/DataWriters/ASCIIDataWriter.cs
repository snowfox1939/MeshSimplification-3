using System;
using System.Globalization;
using System.IO;

namespace Polynano.IO.Ply.Helpers.DataWriters
{
    /// <summary>
    /// Writer to write model data in the ASCII Format.
    /// </summary>
    internal class AsciiDataWriter : IDataWriter
    {
        private readonly HeaderIterator _iterator;

        private readonly StreamWriter _writer;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="writer">writer to use to write the values</param>
        /// <param name="iterator">iterator to keep track of the written properties</param>
        public AsciiDataWriter(StreamWriter writer, HeaderIterator iterator)
        {
            _iterator = iterator;
            _writer = writer;
        }

        public void Close()
        {
            _writer.Close();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        /// <summary>
        /// Write an array of values
        /// </summary>
        /// <param name="data">values to write</param>
        public void WriteList<T>(params T[] data) where T : IConvertible
        {
            WriteSeparator();
            _writer.Write(data.Length.ToString(CultureInfo.InvariantCulture.NumberFormat));
            foreach (T val in data)
            {
                var v = val.ToString(CultureInfo.InvariantCulture.NumberFormat);
                _writer.Write(" " + v);
            }
        }

        /// <summary>
        /// Write a single value.
        /// </summary>
        /// <param name="value">value to write</param>
        public void WriteValue<T>(T value) where T : IConvertible
        {
            WriteSeparator();
            var v = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
            _writer.Write(v);
        }

        private void WriteSeparator()
        {
            // if we're on the first property but not the first element
            // insert a NewLine
            // otherwise insert a space.
            if (_iterator.IsOnFirstProperty)
            {
                if(!_iterator.IsOnFirstElement)
                {
                    _writer.Write(_writer.NewLine);
                }
            }
            else
            {
                _writer.Write(' ');
            }
        }
    }
}
