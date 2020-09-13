using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
using System.Text.RegularExpressions;
using HollowAether.Lib.MapZone;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.InputOutput.Parsers {
	static class MapParser {
		public static void Parse(Map _map) {
			map = _map; // Store reference to map
			String[] fContents = ReadMapFile();
			
			for (lineIndex = 0; lineIndex < fContents.Length; lineIndex++) {
				String line = fContents[lineIndex]; // store to simplify access

				if (String.IsNullOrWhiteSpace(line)) continue; // skip when empty

				if      (IsZoneDefinition(line))     ExecuteZoneDefintion(line);
				else if (IsStartZoneDefintion(line)) ExecuteStartZoneDefintion(line);
				else if (IsFlagDefintion(line))      ExecuteFlagDefintion(line);
				else throw new MapSyntaxException(map.FilePath, $"Couldn't Interpret Line '{line}'", lineIndex);
			}
		}

		/// <summary>Opens Map File and reads contents + Performs necessary initial checks</summary>
		/// <returns>Stripped (by linebreak) contents of zone file with removed comments</returns>
		private static String[] ReadMapFile() {
			String mapContents; // Pointer to desired map contents

			try { mapContents = IOMan.ReadEncryptedFile(map.FilePath, GV.Encryption.oneTimePad).Replace("\r", ""); } 
			catch { throw new MapFileCouldntBeReadException(map.FilePath); /* Couldn't read map file => throw exception */ }

			if (!Parser.HeaderCheck(mapContents, Map.MAP_FILE_HEADER)) throw new MapHeaderException(map.FilePath); // Header failed

			GV.Misc.RemoveComments(ref mapContents); // remove comment notations from file contents because they serve no revelence

			return mapContents.Substring(Map.MAP_FILE_HEADER.Length, mapContents.Length - Map.MAP_FILE_HEADER.Length).Split('\n');
		}

		#region LineCheckerMethods
		private static void ExecuteZoneDefintion(String line) {
			Vector2 position = Vector2Parser(line); // Store new zone definition

			if (map.ContainsZone(position)) // If map already has a zone at the given zone index
				throw new MapFileDefinesZoneAtSamePositionTwiceException(map.FilePath, position, lineIndex);

			char visibilityIndicator = line.Substring(0, 1).ToUpper().ToCharArray()[0];

			if (!(new char[] { 'N', 'V', 'H' }).Contains(visibilityIndicator))
				throw new MapUnknownVisibilityException(map.FilePath, visibilityIndicator.ToString(), lineIndex);
			
			map.AddZone(position, SystemPathExtracter(line), visibilityIndicator != 'H');
		}

		private static void ExecuteStartZoneDefintion(String line) {
			map.SetCurrentZone(Vector2Parser(line), true, false);
		}

		private static void ExecuteFlagDefintion(String line) {
			String flagName = BracketParser(line); // Extract contents of parentheses

			if (GV.MapZone.globalAssets.Keys.Contains(flagName)) // Flag exists already
				throw new MapRepeatedFlagCreationException(map.FilePath, flagName, lineIndex);

			Match hasValue = Regex.Match(line, @"=([Ff]alse|[Tt]rue)"); // If definition includes default value

			if (!hasValue.Success) GV.MapZone.globalAssets.Add(flagName, new FlagAsset(flagName));
			else GV.MapZone.globalAssets.Add(flagName, new FlagAsset(flagName, BooleanParser(hasValue.Value)));
		}
		#endregion

		#region LineCheckerMethods
		/// <summary>Checks if line is a zone definition</summary>
		private static bool IsZoneDefinition(String line) { // N-(Position) C:\Path\To\Zone
			return Regex.IsMatch(line, @"\w-\(.+\) [^ \n\r]+\\[^ \n\r]+");
		}

		/// <summary>Checks if line is a Start Zone definition</summary>
		private static bool IsStartZoneDefintion(String line) { // [StartZone(X: 000, Y: 000)]
			return Regex.IsMatch(line, @"\[[Ss]tart[Zz]one.+\]");
		}

		/// <summary>Checks if line is a Flag definition</summary>
		private static bool IsFlagDefintion(String line) { // [DefineFlag(FlagName)] || [DefineFlag(FlagName)=True]
			return Regex.IsMatch(line, @"\[[Dd]efine[Ff]lag.+\]");
		}
		#endregion

		#region ParserWrapper
		private static Vector2 Vector2Parser(String line) {
			try { return Parser.Vector2Parser(line); } catch (Exception e) {
				throw new MapLineConversionException(map.FilePath, line, "Vector2", lineIndex, e);
			}
		}

		private static Boolean BooleanParser(String line) {
			try { return Parser.BooleanParser(line); } catch (Exception e) {
				throw new MapLineConversionException(map.FilePath, line, "Boolean", lineIndex, e);
			}
		}

		private static String BracketParser(String line) {
			try { return Parser.GetValueInParentheses(line); } catch (Exception e) {
				String message = $"Couldn't Extract Contents of Parentheses In Line '{line}'";
				throw new MapSyntaxException(map.FilePath, message, lineIndex, e); // throw
			}
		}

		private static String SystemPathExtracter(String line) {
			try { return Parser.SystemPathExtracter(line); } catch (Exception e) {
				String message = $"Couldn't Find Path In Line '{line}'";
				throw new MapSyntaxException(map.FilePath, message, lineIndex, e);
			}
		}
		#endregion

		/// <summary>Reference to map file</summary>
		private static Map map;

		/// <summary>Reference to line index</summary>
		private static int lineIndex;
	}
}