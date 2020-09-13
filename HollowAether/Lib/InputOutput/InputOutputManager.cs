#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Encryption;
#endregion

namespace HollowAether.Lib.InputOutput {
	/// <summary>Static class to simplify filestream operations</summary>
	public static class InputOutputManager {
		#region CheckStatemente
		/// <summary>Method to check whether a file exists</summary>
		/// <param name="fpath">Path to a given file</param>
		/// <returns>Boolean indicating existance of given file</returns>
		public static bool FileExists(String fpath) { return File.Exists(fpath); }

		/// <summary>Method to check whether a directory exists</summary>
		/// <param name="dpath">Path to a given directory</param>
		/// <returns>Boolean indicating existence of given dir</returns>
		public static bool DirectoryExists(String dpath) { return Directory.Exists(dpath); }
		#endregion

		#region ReadStatements
		/// <summary>Method to read a file</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="encoding">Encoding to read file with</param>
		/// <returns>String contents of a given file</returns>
		public static String ReadFile(String fpath, Encoding encoding = null) {
			return File.ReadAllText(fpath, (encoding == null) ? defaultEncoding : encoding);
		}

		/// <summary>Reads a file which was encrypted via OneTimePad</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="OTP">Instance of OneTimePad to decrypt with</param>
		/// <param name="encoding">Encoding to read a file with</param>
		/// <returns>Decrypted contents of a given file</returns>
		public static String ReadEncryptedFile(String fpath, OneTimePad OTP, Encoding encoding = null) {
			return OTP.Decrypt(ReadFile(fpath, encoding)); // no need to pass key
		}

		/// <summary>Reads a file which was encrypted via OneTimePad</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="key">Key to encrypt/decrypt with</param>
		/// <param name="encoding">Encoding to read a file with</param>
		/// <returns>Decrypted contents of a given file</returns>
		public static String ReadEncryptedFile(String fpath, String key, Encoding encoding = null) {
			return ReadEncryptedFile(fpath, new OneTimePad(key), encoding);
		}
		#endregion

		#region WriteStatements
		/// <summary>Writes a given string to the path of a file</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="contents">Text to write to file</param>
		/// <param name="encoding">Encoding to interpret file with</param>
		/// <returns>Boolean indicating whether file was sucessfully written to</returns>
		public static bool WriteFile(String fpath, String contents, Encoding encoding = null) {
			try {
				File.WriteAllText(fpath, contents, (encoding == null) ? defaultEncoding : encoding);
				return true; // File was written to sucesfully, return True
			} catch { return false; }
		}

		/// <summary>Encrypts & then writes a given string to the path of a file</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="contents">Text to write to file</param>
		/// <param name="OTP">Instance of OneTimePad to encrypt/decrypt with</param>
		/// <param name="encoding">Encoding to interpret file with</param>
		/// <returns>Boolean indicating whether file was sucessfully written to</returns>
		public static bool WriteEncryptedFile(String fpath, String contents, OneTimePad OTP, Encoding encoding = null) {
			return WriteFile(fpath, OTP.Encrypt(contents), encoding);
		}


		/// <summary>Encrypts & then writes a given string to the path of a file</summary>
		/// <param name="fpath">Path to file</param>
		/// <param name="contents">Text to write to file</param>
		/// <param name="key">Key to encrypt text with</param>
		/// <param name="encoding">Encoding to interpret file with</param>
		/// <returns>Boolean indicating whether file was sucessfully written to</returns>
		public static bool WriteEncryptedFile(String fpath, String contents, String key, Encoding encoding = null) {
			return WriteEncryptedFile(fpath, contents, new OneTimePad(key), encoding);
		}
		#endregion

		#region Miscellaneous
		/// <summary>Combines various paths into a single path</summary>
		/// <param name="fpath">paths which u wish to join</param>
		/// <returns>Joined directory string</returns>
		public static String Join(params String[] fpath) {
			if (fpath.Length >= 1)
				return fpath.Aggregate((A, B) => $"{A}{OSFileSystemSeperator}{B}");
			else
				return String.Empty;
		}

		/// <summary>Strips and returns each sequential directory in a path</summary>
		/// <param name="str">File system path</param>
		/// <param name="_stripChar">Character to strip with</param>
		/// <param name="skipFirst">Whether or not to skip first value</param>
		/// <returns>Enumerable sequenced directory</returns>
		public static IEnumerable<String> SequentialStrip(String str, char? _stripChar = null, bool skipFirst = true) {
			char stripChar = (_stripChar.HasValue) ? _stripChar.Value : OSFileSystemSeperator;
			String[] splitString = str.Split(stripChar); // Split string by given delimeter

			foreach (int X in Enumerable.Range((skipFirst) ? 2 : 1, splitString.Count() - (skipFirst ? 1 : 0))) {
				yield return (from N in Enumerable.Range(0, X) select splitString[N]).Aggregate((a, b) => $"{a}{stripChar}{b}");
			}
		}

		/// <summary>Tries to create a given directory</summary>
		/// <param name="dPath">Path to directory</param>
		/// <returns>Boolean indicating whether path was constructed or not</returns>
		public static bool CheckConstruct(String dPath) {
			try { Directory.CreateDirectory(dPath); return true; } catch { return false; }
		}

		/// <summary>Constructs every directory in a path</summary>
		/// <param name="dpath">Path to directory</param>
		/// <returns>Boolean indicating whether all directories were constructed or not</returns>
		public static bool SequenceConstruct(String dpath) {
			foreach (String path in SequentialStrip(dpath))
				if (!CheckConstruct(path)) return false;

			return true; // If not returned yet then give true
		}

		public static String[] GetFiles(String path) {
			return Directory.GetFiles(path);
		}

		public static String[] GetDirectories(String path) {
			return Directory.GetDirectories(path);
		}

		public static String ChangeExtension(String input, String desiredExtension) {
			return Path.ChangeExtension(input, desiredExtension);
		}

		public static String GetExtension(String input) {
			return Path.GetExtension(input);
		}

		public static String GetDirectoryName(String input) {
			return Path.GetDirectoryName(input);
		}

		public static String GetFileNameFromPath(String input) {
			return Path.GetFileName(input);
		}

		public static String GetFileTitleFromPath(String input) {
			return Path.GetFileNameWithoutExtension(input);
		}

		public static String RemoveExtension(String input) {
			string directoryName = GetDirectoryName(input), fileTitle = GetFileTitleFromPath(input);
			return String.IsNullOrWhiteSpace(directoryName) ? fileTitle : Join(directoryName, fileTitle);
		}

		/// <summary>Returns path from a reference to a given absolute path</summary>
		/// <param name="absolute">Path one wishes to reach</param>
		/// <param name="reference">Path from which one is travelling</param>
		/// <returns>Path relation from given reference path to absolute</returns>
		public static String GetRelativePath(String absolute, String reference) {
			absolute = absolute.ToLower().TrimEnd(OSFileSystemSeperator);
			reference = reference.ToLower().TrimEnd(OSFileSystemSeperator);

			if (absolute.Contains(reference)) // If absolute is within reference
				return absolute.Replace(reference, "").TrimStart(OSFileSystemSeperator);

			foreach (String directory in SequentialStrip(reference, '\\', false).Reverse()) {
				if (!absolute.Contains(directory)) continue; // No related path found yet, there shoud be one at the drive root

				var dotEnumerate = Enumerable.Range(0, reference.Replace(directory, "").Count(X => X == '\\')); // Desired dot count

				return (from X in dotEnumerate select "..").Aggregate((X, Y) => $"{X}\\{Y}") + absolute.Replace(directory, "");
			}

			return absolute; // Couldn't be done, best one can hope for is this
		}

		public static String GetRelativeFilePath(String filePath, String reference) {
			String containingFileDirectory = GetDirectoryName(filePath), fileName = GetFileNameFromPath(filePath);
			return Join(GetRelativePath(containingFileDirectory, reference), fileName);
		}
		#endregion

		#if !LINUX
		/// <summary>Character to seperate paths in fsystem</summary>
		public static char OSFileSystemSeperator = '\\';
		#else
		/// <summary>Character to seperate paths in fsystem</summary>
		public static char OSFileSystemSeperator = '/';
		#endif

		/// <summary>Encoding to read files with by default</summary>
		public static Encoding defaultEncoding = Encoding.Unicode;
	}
}
