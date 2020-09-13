#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region MicrosoftImports
using Microsoft.Scripting.Hosting;
#endregion

#region IPythonImports
using IronPython.Hosting;
using IronPython.Runtime;
#endregion

namespace HollowAether {
	/// <summary>Methods to only be run on startup, because of *drastic* effects on run speed</summary>
	public static class StartUpMethods {
		/// <summary>Runs a given python script and returns whatever the script prints</summary>
		/// <param name="script">String containing script to execute by py2.7 engine</param>
		/// <returns>Out And Error streams written to by the py2.7 engine</returns>
		public static Dictionary<String, String> ReadPyScriptExecution(String script) {
			ScriptEngine pyEngine = Python.CreateEngine(); // Create Python RunTime Engine
			ScriptScope pyScope = pyEngine.CreateScope(); // Create scope to access vars
			System.IO.MemoryStream outStream, errStream; // Add management streams to memory
			outStream = errStream = new System.IO.MemoryStream(); // Create instance of stream
			pyEngine.Runtime.IO.SetOutput(outStream, Encoding.Default); // set encoding
			pyEngine.Runtime.IO.SetErrorOutput(errStream, Encoding.Default); // set encoding

			pyEngine.Execute(script, pyScope); // execute given script in desired scope

			return new Dictionary<string, string>() {
				{ "out", Encoding.Default.GetString(outStream.ToArray()) },
				{ "error", Encoding.Default.GetString(errStream.ToArray()) }
			};
		}

		/// <summary>Uses InputParser python script to interperate command line arguments</summary>
		/// <param name="commandLineString">String as it would appear in the user command line</param>
		/// <returns>Dictionary (Flag->Args). Default args with no flag is /0 or break char</returns>
		public static Dictionary<String, String[]> InputParserScriptInterpreter(String commandLineString) {
			ScriptEngine pyEngine = Python.CreateEngine(); // Create Python RunTime Engine
			ScriptSource source = pyEngine.CreateScriptSourceFromFile(@"PyScripts\input_parser.py");
			ScriptScope scope = pyEngine.CreateScope(); // Create scope for python runtime
			source.Execute(scope); // Execute script to build python script objects

			dynamic InputParser = scope.GetVariable("InputParser"); // grab class reference
			dynamic InputParserInstance = InputParser(); // Create new instance from reference

			PythonTuple parsedResults = InputParserInstance.parse_input(commandLineString);

			Dictionary<String, String[]> parsed = new Dictionary<String, String[]>(); // Flag->Args

			foreach (PythonTuple sub in parsedResults) { // For Found Flags
				parsed[(sub[0] == null) ? "\0" : sub[0].ToString()] =
					(from X in sub[1] as List select X.ToString()).ToArray();
			}

			return parsed; // Returns found flags and arguments to caller
		}

		/// <summary>Uses SystemParser python script to recursively parse directories</summary>
		/// <param name="rootPath">Path to retrieve all sub-directories from</param>
		/// <returns>All file found in a given directory including subdirectories</returns>
		public static String[] SystemParserScriptInterpreter(String rootPath) {
			ScriptEngine pyEngine = Python.CreateEngine(); // Create Python RunTime Engine
			ScriptSource source = pyEngine.CreateScriptSourceFromFile(@"PyScripts\system_parser.py");
			ScriptScope scope = pyEngine.CreateScope(); // Create scope for python runtime
			source.Execute(scope); // Execute script to build python script objects

			dynamic SystemParser = scope.GetVariable("SystemParser"); // Grab Class Reference
			dynamic ScriptParserInstance = SystemParser(rootPath); // create new instance from ref

			List parsedSystemFiles = ScriptParserInstance.get_files();
			return (from X in parsedSystemFiles select X.ToString()).ToArray();
		}

		/// <summary>Uses SystemParser python script to recursively parse directories for files of a specific type</summary>
		/// <param name="rootPath">Path to retrieve all sub-files of a given file type from</param>
		/// <param name="types">Desired file types to parse directories for</param>
		/// <returns>All files found in a given directory including subdirectories of desired types</returns>
		public static String[] SystemParserTypeSpecificScriptInterpreter(String rootPath, params String[] types) {
			ScriptEngine pyEngine = Python.CreateEngine(); // Create Python RunTime Engine
			ScriptSource source = pyEngine.CreateScriptSourceFromFile(@"PyScripts\system_parser.py");
			ScriptScope scope = pyEngine.CreateScope(); // Create scope for python runtime
			source.Execute(scope); // Execute script to build python script objects

			dynamic SystemParser = scope.GetVariable("SystemParser"); // Grab Class Reference
			dynamic ScriptParserInstance = SystemParser(rootPath); // Create new instance from ref

			List parsedSystemFiles = ScriptParserInstance.get_files_of_type(types);
			return (from X in parsedSystemFiles select X.ToString()).ToArray();
		}

		public static String[] SystemParserTruncatedScriptInterpreter(String rootPath) {
			ScriptEngine pyEngine = Python.CreateEngine(); // Create Python RunTime Engine
			ScriptSource source = pyEngine.CreateScriptSourceFromFile(@"PyScripts\system_parser.py");
			ScriptScope scope = pyEngine.CreateScope(); // Create scope for python runtime
			source.Execute(scope); // Execute script to build python script objects

			dynamic SystemParser = scope.GetVariable("SystemParser");
			dynamic ScriptParserInstance = SystemParser(rootPath);

			List parsedSystemFiles = ScriptParserInstance.get_truncated_files();
			return (from X in parsedSystemFiles select X.ToString()).ToArray();
		}
	}
}
