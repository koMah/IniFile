using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2{ 

        public class IniProperty
        {
            /// <summary>
            /// Property name (key).
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Property value.
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// Set the comment to display above this property.
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// Check if property have multi values.
            /// </summary>
            public bool IsMulti = false;

            public void Reset()
            {
                this.Name = null;
                this.Value = null;
                this.Comment = null;
            }
        }
}
