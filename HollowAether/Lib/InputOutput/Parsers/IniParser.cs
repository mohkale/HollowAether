using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HollowAether.Lib.InputOutput.Parsers {
	public static class INIParser { // Also used for save files
		public class SectionContainer : Dictionary<String, Dictionary<String, String>> {
			public SectionContainer() : base() { } // Type definition, other contructors unnecessary

			public void AddSection(String sectionHeader) {
				if (!Keys.Contains(sectionHeader)) // Just in case doesn't exist
					this[sectionHeader] = new Dictionary<String, String>();
			}

			public String ToFileContents() {
				StringBuilder builder = new StringBuilder(); // Builder more efficient than string

				foreach (String sectionHeader in Keys) {
					builder.AppendLine($"[{sectionHeader}]");

					foreach (String key in this[sectionHeader].Keys)
						builder.AppendLine($"{key}={this[sectionHeader][key]}");

					builder.AppendLine(""); // Leave trailing line break
				}

				return builder.ToString(); // Cast to string and return to sender
			}
		}

		/// <summary>Parses INI file type contents</summary>
		/// <param name="fContents">Contents of file</param>
		/// <param name="fpath">Path to file, used for exception notification</param>
		/// <param name="throwExceptionOnInvalidLine">Whether to ignore or let throw</param>
		/// <param name="throwExceptionOnValueWithoutSection">Whether to ignore or let throw</param>
		/// <remarks>
		///		!, #      => Used for Commenting
		///		[TEXT]    => Used for section headers
		///		Key=Value => Used for key value definitions
		/// </remarks>
		/// <returns>Complex data type consiting of key=section, key=key, value=value</returns>
		public static SectionContainer Parse(String fContents, String fpath, bool throwExceptionOnInvalidLine=false, bool throwExceptionOnValueWithoutSection=false) {
			String sectionHeader = null; // Header for current section

			SectionContainer sections = new SectionContainer(); // Dictionary<String, Dictionary<String, String>>();

			var lines = (from X in fContents.Split('\n') select X.Trim());
			// Gets rid of leading and trailing whitespace + makes into lines

			foreach (String line in lines) {
				if (line.Length == 0 || (new char[] { '!', '#' }).Contains(line[0]))
					continue; // Skip, blank line or comment line. Doesn't matter to me

				if (Regex.IsMatch(line, @"\[.+\]")) { // Section definition
					sectionHeader = line.Substring(1, line.Length - 2);
					sections.AddSection(sectionHeader); // Create new container
				} else if (Regex.IsMatch(line, @".+=.*")) { // Is key value definition
					if (String.IsNullOrEmpty(sectionHeader)) { // No section for var
						if (throwExceptionOnValueWithoutSection)
							throw new Exception("");
						// Else continue on to next line
					} else {
						String[] splitLine = line.Split('='); // Python wouldn't need this line :(
						sections[sectionHeader][splitLine[0]] = splitLine[1]; // Store key/value pair 
					}
				} else if (throwExceptionOnInvalidLine) {
					throw new Exception($"Unknown line {line}");
				}
			}

			return sections; // All sections within argument file
		}

		
	}
}
