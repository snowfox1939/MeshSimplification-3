using System.IO;
using System;

namespace Polynano.IO.Ply.Helpers
{
    /// <summary>
    /// The loading performence was not satisfactory
    /// so that a custom stream reader was developed.
    /// It is about 40% faster than the standard StreamReader although 
    /// some compromises had to be made. 
    /// </summary>
    internal class BufferedStreamReader
    {
        /// <summary>
        /// The stream to read data from
        /// </summary>
        private readonly Stream _source;

        /// <summary>
        /// The buffer of read data chunk.
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        /// position in the internal buffer
        /// </summary>
        private int _bufferPosition;

        /// <summary>
        /// size of the stream
        /// </summary>
        private readonly long _cachedStreamSize;

        /// <summary>
        /// Size of the buffer
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// Default constructor. 
        /// </summary>
        /// <param name="source">Stream to be read</param>
        /// <param name="bufferSize">the size of the buffer to use</param>
        public BufferedStreamReader(Stream source, int bufferSize = 4096)
        {
            if (bufferSize < 1024 || bufferSize > 4096)
            {
                  throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }
            if (!source.CanSeek)
            {
                throw new ArgumentException("Stream must be seekable");
            }
            if (!source.CanRead)
            {
                throw new ArgumentException("Stream must be writeable");
            }
            _source = source;
            _buffer = new byte[bufferSize];
            _bufferSize = bufferSize;
            _cachedStreamSize = source.Length;
            _bufferPosition = -1;
        }

        /// <summary>
        /// Whether the stream has been complemently read
        /// </summary>
        /// <returns>false if the stream was not fully read, true otherwise</returns>
        private bool EndOfStream => _source.Position == _cachedStreamSize;

        /// <summary>
        /// The last read character
        /// </summary>
        private char CurrentChar => (char)_buffer[_bufferPosition];

        /// <summary>
        /// Whether we're out of buffer
        /// </summary>
        private bool EndOfBuffer => (_bufferPosition == -1 || _bufferPosition >= _bufferSize);

        /// <summary>
        /// Read a line from the stream
        /// </summary>
        /// <returns>read line</returns>
        public string ConsumeLine()
        {
            return ConsumeUntilBlank(spaceTerminates: false);
        }

        /// <summary>
        /// Read a token from the stream
        /// </summary>
        /// <returns>read token</returns>
        public string ConsumeToken()
        {
            return ConsumeUntilBlank(spaceTerminates: true);
        }

        /// <summary>
        /// Read the stream until a space or a newline character
        /// </summary>
        /// <param name="spaceTerminates">whether the newline character should terminate</param>
        /// <returns>read token</returns>
        private string ConsumeUntilBlank(bool spaceTerminates)
        {
            if (EndOfStream && EndOfBuffer)
            {
                return null;
            }
            if (EndOfBuffer)
            {
                UpdateBuffer();
            }

            // Jump to the first non whitespace and non-newline
            // character, go through multiple buffers if needed.
            JumpFirstNotBlank();

            // set the space to \0 character if the client
            // does not want the space to terminate.
            char space = spaceTerminates ? ' ' : char.MinValue;

            // Remember the token begin position
            // and move the bufferPosition to the end of this token.
            int begin = _bufferPosition;
            while (_bufferPosition < _bufferSize
                && CurrentChar != '\n'
                && CurrentChar != '\r'
                && CurrentChar != space)
            {
                _bufferPosition++;
            }

            // If we've reached the end of the buffer before 
            // reaching the end of the token, we need to update the buffer
            // and combine the two read tokens

            string str;
            if (EndOfBuffer
                && (char)_buffer[_bufferPosition - 1] != '\n'
                && (char)_buffer[_bufferPosition - 1] != '\r'
                && (char)_buffer[_bufferPosition - 1] != space)
            {
                string temp = ReadBufferAsString(begin, _bufferPosition - begin);

                while (!EndOfStream)
                {
                    UpdateBuffer();
                    while (_bufferPosition < _bufferSize
                           && CurrentChar != '\n'
                           && CurrentChar != '\r'
                           && CurrentChar != space)
                    {
                        _bufferPosition++;
                    }
                    temp += ReadBufferAsString(0, _bufferPosition);
                    if (!EndOfBuffer)
                    {
                        break;
                    }
                }
                str = temp;
            }
            else
            {
                str = ReadBufferAsString(begin, _bufferPosition - begin);
            }

            // We need to make sure we're not leaving the buffer on whitespace
            // should the binary read follow.
            JumpFirstNotBlank();

            return str;
        }

        /// <summary>
        /// Consume the count bytes from the stream
        /// </summary>
        /// <param name="count">how many bytes to read</param>
        /// <returns>read bytes</returns>
        public byte[] ConsumeBytes(int count)
        {
            if(count > _bufferSize)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (EndOfStream && EndOfBuffer)
            {
                return null;
            }
            if (EndOfBuffer)
            {
                UpdateBuffer();
            }

            byte[] val = new byte[count];
            // If the count is bigger than the unread part of the buffer
            // we need to consume the current buffer, load next part and combine them.
            // this only supports one buffer update. So that it is neccessary to throw ArgumentOutOfRange
            // if the requested size is too big. There's not really a need to implement multiple buffer updates.
            if(count > _bufferSize - _bufferPosition)
            {
                Array.Copy(_buffer, _bufferPosition, val, 0, _bufferSize - _bufferPosition);
                int remaining = count - (_bufferSize - _bufferPosition);
                UpdateBuffer();
                Array.Copy(_buffer, _bufferPosition, val, count - remaining, remaining);
                _bufferPosition += remaining;
            }
            else
            {
                Array.Copy(_buffer, _bufferPosition, val, 0, count);
                _bufferPosition += count;
            }

            return val;
        }

        /// <summary>
        /// Update the buffer, reset the counters
        /// </summary>
        private void UpdateBuffer()
        {
            long sizeToRead = _buffer.Length;

            if (_cachedStreamSize - _source.Position <= sizeToRead)
            {
                sizeToRead = _source.Length - _source.Position;
            }

            _source.Read(_buffer, 0, (int)sizeToRead);
            _bufferSize = (int)sizeToRead;
            _bufferPosition = 0;
        }

        /// <summary>
        /// Go through the buffer,
        /// and find the first not whitespace and not newline character
        /// leave the bufferPosition reference at the first non blank character
        /// </summary>
        private void JumpFirstNotBlank()
        {
            while (CurrentChar == ' '
                    || CurrentChar == '\r'
                    || CurrentChar == '\n')
            {
                _bufferPosition++;
                if (EndOfBuffer && !EndOfStream)
                {
                    UpdateBuffer();
                }
            }
        }

        /// <summary>
        /// Convert the byte array buffer into an string
        /// </summary>
        /// <param name="startIndex">start index in the buffer to begin converting</param>
        /// <param name="length">how many bytes to convert</param>
        /// <returns>converted bytes to ascii</returns>
        private string ReadBufferAsString(int startIndex, int length)
        {
            //  Do not use Encoding.Default.GetString(_buffer, startIndex, length);
            //  as it's slow.
            var tmp = new char[length];
            for (int i = 0; i < length; ++i)
                tmp[i] = (char)_buffer[startIndex + i];
            var str = new string(tmp);
            return str;
        }
    }
}