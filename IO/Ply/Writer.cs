using System;
using System.IO;
using Polynano.IO.Ply.Helpers;
using Polynano.IO.Ply.Helpers.DataWriters;
using System.Text;

namespace Polynano.IO.Ply
{
    /// <summary>
    /// Use to create a proper PLY file
    /// from the model data.
    /// </summary>
    public class Writer
    {
        /// <summary>
        /// The HeaderIterator used to keep tract of the 
        /// written elements and properties.
        /// </summary>
        private readonly HeaderIterator _iterator;

        /// <summary>
        /// The DataWriter used to write the model data.
        /// </summary>
        private readonly IDataWriter _dataWriter;

        /// <summary>
        /// The Stream to write data to.
        /// </summary>
        private Stream _stream;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="target">The Target Stream for the data to be written into</param>
        /// <param name="header">The Header defining the type and order of the data to be written</param>
        public Writer(Stream target, Header header)
        {
            if (!target.CanWrite)
            {
                throw new ArgumentException(@"Stream must be writeable", nameof(target));
            }

            _stream = target;
            _iterator = new HeaderIterator(header);

            switch (header.Format)
            {
                case Format.Ascii:
                    var writer = new StreamWriter(target, Encoding.Default, 1024, leaveOpen: true);
                    writer.Write(HeaderGenerator.GetHeader(_iterator.Header));
                    _dataWriter = new AsciiDataWriter(writer, _iterator);
                    break;
                case Format.BinaryLittleEndian:
                case Format.BinaryBigEndian:
                    _dataWriter = new BinaryDataWriter(_stream, _iterator, header.Format == Format.BinaryLittleEndian);
                    ((BinaryWriter)_dataWriter).Write(Encoding.ASCII.GetBytes(HeaderGenerator.GetHeader(_iterator.Header)));
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Default class Destructor. 
        /// Close the stream.
        /// </summary>
        ~Writer()
        {
            Close();
        }

        /// <summary>
        /// Write an array of values to the stream.
        /// Should the data not follow the predefined structure in the 
        /// header, an InvalidOperationException will be thrown.
        /// </summary>
        /// <param name="values">The value to be written</param>
        /// <returns>The Writer instance allowing for method chaining</returns>
        public Writer WriteValues<T>(params T[] values) where T : IConvertible
        {
            foreach (var val in values)
            {
                WriteValue(val);
            }
            return this;
        }

        /// <summary>
        /// Write a single value to the Stream.
        /// Should the data not follow the predefined structure in the 
        /// header, an InvalidOperationException will be thrown.
        /// </summary>
        /// <param name="value">The value to be written</param>
        /// <returns>The Writer instance allowing for method chaining</returns>
        public Writer WriteValue<T>(T value) where T :IConvertible
        {    
            EnsureNotClosed();
            EnsureWriteCallIsValid<T>(false);

            _dataWriter.WriteValue(value);

            _iterator.NextProperty();
            return this;
        }

        /// <summary>
        /// Write an array of values to the Stream.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Writer WriteList<T>(params T[] data) where T : IConvertible
        {
            EnsureNotClosed();
            EnsureWriteCallIsValid<T>(true);

            _dataWriter.WriteList(data);

            _iterator.NextProperty();
            return this;
        }

        /// <summary>
        /// Finish writing data and close the stream.
        /// One should always call the Close method
        /// to prevent data loss.
        /// </summary>
        public void Close()
        {
            if (_stream == null)
                return;

            if (!_iterator.IsIterationDone)
            {
                throw new InvalidOperationException("Not all defined elements have been written!");
            }

            // shutdown the writer and stream.
            _dataWriter.Close();
            _stream.Dispose();
            _stream = null;
        }

        private void EnsureNotClosed()
        {
            if (_stream == null)
            {
                throw new InvalidOperationException("This writer has been already closed");
            }
        }

        /// <summary>
        /// Helper method to check whether the Generic Parameter T 
        /// can be converted into the data type defined in the header 
        /// of the current property.
        /// </summary>
        /// <param name="writingList">Whether a write list call was made</param>
        private void EnsureWriteCallIsValid<T>(bool writingList)
        {
            var expected = _iterator.CurrentProperty;

            if (writingList != expected.IsList)
            {
                throw new ArgumentException("you passed a list/single while the opposite was expected");
            }

            if (PlyTypeHelper.ToNative(expected.ValueType) != typeof(T))
            {
                throw new ArgumentException("Unexpected value type");
            }
        }
    }
}

