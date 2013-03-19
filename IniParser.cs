using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YourNamespace
{
        public class IniParser
        {
            private readonly IDictionary<string, IniSection> _sections;

            /// <summary>
            /// If True, writes extra spacing between the property name and the property value.
            /// (foo=bar) vs (foo = bar)
            /// </summary>
            public bool WriteSpacingBetweenNameAndValue { get; set; }

            /// <summary>
            /// The character a comment line will begin with. Default '#'.
            /// </summary>
            public char CommentChar { get; set; }

            /// <summary>
            /// Get the sections in this IniFile.
            /// </summary>
            public IniSection[] Sections
            {
                get { return _sections.Values.ToArray(); }
            }

            /// <summary>
            /// Create a new IniFile instance.
            /// </summary>
            public IniParser()
            {
                _sections = new Dictionary<string, IniSection>();
                CommentChar = ';';
            }

            /// <summary>
            /// Load an INI file from the file system.
            /// </summary>
            /// <param name="path">Path to the INI file.</param>
            public IniParser(string path)
                : this()
            {
                Load(path);
            }

            /// <summary>
            /// Load an INI file.
            /// </summary>
            /// <param name="reader">A TextReader instance.</param>
            public IniParser(TextReader reader)
                : this()
            {
                Load(reader);
            }

            private void Load(string path)
            {
                using (var file = new StreamReader(path))
                    Load(file);
            }

            private void Load(TextReader reader)
            {
                IniSection section = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    // skip empty lines
                    if (line == string.Empty)
                        continue;

                    // skip comments
                    //if (line.StartsWith(";") || line.StartsWith("#"))
                      //  continue;

                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        var sectionName = line.Substring(1, line.Length - 2);
                        if (!_sections.ContainsKey(sectionName))
                        {
                            section = new IniSection(sectionName);
                            _sections.Add(sectionName, section);
                        }
                        continue;
                    }

                    if (section != null)
                    {
                        string patt = @"^;?(?<key>[^\n\s]+)\s*=\s?(?<value>[^\n]+)?$";
                        Match match = Regex.Match(line, patt, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        /*
                        var keyValue = line.Split(new[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValue.Length != 2)
                            continue;*/
                        if (match.Success)
                        {
                            //var g = match.Groups;

                            // Finally, we get the Group value and display it.
                            string key = match.Groups["key"].Value;
                            string value = match.Groups["value"].Value;
                            Console.WriteLine("{0} = {1}", key, value);
                            //Console.WriteLine(match.Groups[1].Value);
                        }
                        
                    }
                }
            }

            /// <summary>
            /// Get a section by name. If the section doesn't exist, it is created.
            /// </summary>
            /// <param name="sectionName">The name of the section.</param>
            /// <returns>A section. If the section doesn't exist, it is created.</returns>
            public IniSection Section(string sectionName)
            {
                IniSection section;
                if (!_sections.TryGetValue(sectionName, out section))
                {
                    section = new IniSection(sectionName);
                    _sections.Add(sectionName, section);
                }

                return section;
            }

            /// <summary>
            /// Remove a section.
            /// </summary>
            /// <param name="sectionName">Name of the section to remove.</param>
            public void RemoveSection(string sectionName)
            {
                if (_sections.ContainsKey(sectionName))
                    _sections.Remove(sectionName);
            }

            /// <summary>
            /// Create a new INI file.
            /// </summary>
            /// <param name="path">Path to the INI file to create.</param>
            public void Save(string path)
            {
                using (var file = new StreamWriter(path))
                    Save(file);
            }

            /// <summary>
            /// Create a new INI file.
            /// </summary>
            /// <param name="writer">A TextWriter instance.</param>
            public void Save(TextWriter writer)
            {
                foreach (var section in _sections.Values)
                {
                    if (section.Properties.Length == 0)
                        continue;

                    if (section.Comment != null)
                        writer.WriteLine("{0} {1}", CommentChar, section.Comment);

                    writer.WriteLine("[{0}]", section.Name);

                    foreach (var property in section.Properties)
                    {
                        if (property.Comment != null)
                            writer.WriteLine("{0} {1}", CommentChar, property.Comment);

                        var format = WriteSpacingBetweenNameAndValue ? "{0} = {1}" : "{0}={1}";
                        writer.WriteLine(format, property.Name, property.Value);
                    }

                    writer.WriteLine();
                }
            }
        }
}
