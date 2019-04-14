using System;
using System.Collections.Generic;
using System.Linq;
using Polynano.IO.Ply.Extensions;

namespace Polynano.IO.Ply
{
    /// <summary>
    /// Class representing the PLY File Element
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Name of the element
        /// </summary>
        private string _name;

        /// <summary>
        /// Count of the elements
        /// </summary>
        private int _count;

        /// <summary>
        /// List of the properties that this element has.
        /// </summary>
        private IReadOnlyList<Property> _properties;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Element(string name, int count, IReadOnlyList<Property> properties)
        {
            Name = name;
            Count = count;
            Properties = properties;
        }

        /// <summary>
        /// get or set the name of this element
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (!value.IsToken())
                {
                    throw new ArgumentException(@"Invalid property name", nameof(Name));
                }
                _name = value;
            }
        }

        /// <summary>
        /// Get or set the element count
        /// </summary>
        /// <returns>number of elements</returns>
        public int Count 
        {
            get => _count;
            set 
            {
                if(value == 0)
                {
                    throw new ArgumentException(@"Element must define at least one instance", nameof(Count));
                }
                if(value < 0)
                {
                    throw new ArgumentException(@"Invalid negative instances count", nameof(Count));
                }
                _count = value;
            }
        }

        /// <summary>
        /// Get or set the element properties.
        /// </summary>
        /// <returns>list of Properties</returns>
        public IReadOnlyList<Property> Properties 
        {
            get => _properties;
            set 
            {
                if(value == null)
                {
                    throw new ArgumentNullException(nameof(Properties));
                }
                if(value.Count == 0)
                {
                    throw new ArgumentException(@"Element must define at least one property", nameof(Properties));
                }                    

                // make sure there are no name duplicates
                if(value.GroupBy(x => x.Name).Any(g => g.Count() > 1))
                {
                    throw new ArgumentException("property list contains properties with duplicate names");
                }

                _properties = value.ToArray();
            }
        }
    }
}
