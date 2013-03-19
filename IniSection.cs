using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
        public class IniSection
        {
            private readonly IDictionary<string, IniProperty> _properties;

            /// <summary>
            /// Section name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Set the comment to display above this section.
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// Get the properties in this section.
            /// </summary>
            public IniProperty[] Properties
            {
                get { return _properties.Values.ToArray(); }
            }

            /// <summary>
            /// Create a new IniSection.
            /// </summary>
            /// <param name="name"></param>
            public IniSection(string name)
            {
                Name = name;
                _properties = new Dictionary<string, IniProperty>();
            }

            /// <summary>
            /// Get a property value.
            /// </summary>
            /// <param name="name">Name of the property.</param>
            /// <returns>Value of the property or null if it doesn't exist.</returns>
            public object Get(string name )
            {
                if (_properties.ContainsKey(name))
                {
                    if (_properties[name].IsMulti)
                        return _properties[name].Value;

                    return _properties[name].Value;
                }

                return null;
            }

            /// <summary>
            /// Set a property value.
            /// </summary>
            /// <param name="name">Name of the property.</param>
            /// <param name="value">Value of the property.</param>
            /// <param name="comment">A comment to display above the property.</param>
            public void Set(string name, string value, string comment = null)
            {
                /*if (string.IsNullOrWhiteSpace(value))
                {
                    RemoveProperty(name);
                    return;
                }*/

                if (!_properties.ContainsKey(name))
                    _properties.Add(name, new IniProperty { Name = name, Value = value, Comment = comment });
                else
                {
                    if (!_properties[name].IsMulti)
                    {
                        string tmpName = (string) _properties[name].Name;
                        string tmpValue = (string)_properties[name].Value;
                        string tmpComment = (string)_properties[name].Comment;

                        _properties[name].Reset();

                        List<IniProperty> newValues = new List<IniProperty>();
                        newValues.Add(new IniProperty { Name = tmpName, Value = tmpValue, Comment = tmpComment });
                        _properties[name].Value = newValues;

                        //_properties[name].Value.Add(_properties[name].Value);
                        _properties[name].IsMulti = true;
                    }


                    var l = (List<IniProperty>)_properties[name].Value;
                    l.Add(new IniProperty { Name = name, Value = value, Comment = (string)comment });
                    
                }
            }

            /// <summary>
            /// Remove a property from this section.
            /// </summary>
            /// <param name="propertyName">The property name to remove.</param>
            public void RemoveProperty(string propertyName)
            {
                if (_properties.ContainsKey(propertyName))
                    _properties.Remove(propertyName);
            }
        }
}
