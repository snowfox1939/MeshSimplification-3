using System;
using System.Collections.Generic;
using System.Linq;

namespace Polynano.IO.Ply 
{
    /// <summary>
    /// A class representing the PLY File Header.
    /// </summary>
    public class Header 
    {
        /// <summary>
        /// The Format of this header/file
        /// </summary>
        private Format _format;

        /// <summary>
        /// Comments of the file, seperated by unix newlines breakers.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Object infos of the header
        /// </summary>
        private IReadOnlyList<string> _objectInfo;

        /// <summary>
        /// Elements that the header defines.
        /// </summary>
        private IReadOnlyList<Element> _elements;

        public Header (Format format, IReadOnlyList<Element> elements) 
        {
            Format = format;
            Elements = elements;
        }

        public Header (Format format, string comment, IReadOnlyList<string> objectInfo, IReadOnlyList<Element> elements) 
        {
            Format = format;
            Comment = comment;
            ObjectInfo = objectInfo;
            Elements = elements;
        }

        public Format Format 
        {
            get => _format;
            set 
            {
                if (!Enum.IsDefined (typeof (Format), value)) 
                {
                    throw new ArgumentException (@"Invalid PlyFormat", nameof (Format));
                }
                _format = value;
            }
        }

        public IReadOnlyList<string> ObjectInfo 
        {
            get => _objectInfo;
            set => _objectInfo = value?.ToArray ();
        }

        public IReadOnlyList<Element> Elements 
        {
            get => _elements;
            set 
            {
                if (value == null) 
                {
                    throw new ArgumentNullException (nameof (Elements));
                }
                if (value.Count == 0) 
                {
                    throw new ArgumentException ("Header must define at least one element");
                }
                if (value.GroupBy (x => x.Name).Any (g => g.Count () > 1)) 
                {
                    throw new ArgumentException ("property list contains elements with duplicate names");
                }
                _elements = value.ToArray ();
            }
        }
    }
}