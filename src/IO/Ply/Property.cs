using System;
using Polynano.IO.Ply.Extensions;

namespace Polynano.IO.Ply
{
    /// <summary>
    /// Class representing the PLY File Header Property
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Name of the property
        /// </summary>
        private string _name;

        /// <summary>
        /// Data type of the property
        /// </summary>
        private PlyType _valueType;

        /// <summary>
        /// Data type of the list indexer
        /// Should this property not be a list,
        /// defaults to PlyType.None
        /// </summary>
        private PlyType _listType;

        public Property(string name, PlyType valueType)
        {
            Name = name;
            ValueType = valueType;
        }

        public Property(string name, PlyType listType, PlyType valueType)
            : this(name, valueType)
        {
            ListType = listType;
        }

        /// <summary>
        /// Get or set the property name
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Name));
                }

                // make sure the value contains only valid characters e.g. A-Z 0-9-_
                if (!value.IsToken())
                {
                    throw new ArgumentException(@"Invalid property name", nameof(Name));
                }
                _name = value;
            }
        }

        /// <summary>
        /// Get or set the value data type
        /// </summary>
        public PlyType ValueType
        {
            get => _valueType;
            set
            {
                if(!Enum.IsDefined(typeof(PlyType), value))
                {
                    throw new ArgumentException("Invalid type");
                }
                _valueType = value;
            }
        }

        /// <summary>
        /// Get or set the property indexer type
        /// </summary>
        public PlyType ListType
        {
            get => _listType;
            set
            {
                if(!Enum.IsDefined(typeof(PlyType), value))
                {
                    throw new ArgumentException("Invalid type");
                }
                _listType = value;
            }
        }

        /// <summary>
        /// Whether this property is an array
        /// </summary>
        public bool IsList => _listType != PlyType.None;
    }
}
