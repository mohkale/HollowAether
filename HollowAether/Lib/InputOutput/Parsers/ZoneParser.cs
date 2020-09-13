using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
using HollowAether.Lib.MapZone;

namespace HollowAether.Lib.InputOutput.Parsers {
	static partial class ZoneParser {
		public static void Parse(Zone _zone) {
			zone = _zone; // Store reference to zone file
			fileContents = ReadZoneFile();

			for (lineIndex = 0; lineIndex < fileContents.Length; lineIndex++) {
				String line = fileContents[lineIndex];
			
				if (String.IsNullOrWhiteSpace(line)) continue;

				String command = GetCommand(line), args = RemoveCommand(line, command);

				switch (command.ToUpper()) {
					case "IMPORTASSET":
						ImportAsset(args);
						break;
					case "DEFINEASSET":
						DefineAsset(args);
						break;
					case "SET":
						Set(args);
						break;
					case "GAMEENTITY":
						GameEntity(args);
						break;
					case "GAMEENTITYATTRIBUTE":
						GameEntityAttribute(args);
						break;
					default: throw new ZoneSyntaxException(zone.FilePath, $"Command '{command}' Not Found", lineIndex);
				}
			}
		}

		private static String[] ReadZoneFile() {
			String zoneContents; // Pointer to desired map contents

			try   { zoneContents = IOMan.ReadEncryptedFile(zone.FilePath, GV.Encryption.oneTimePad).Replace("\r", ""); } 
			catch { throw new ZoneFileCouldntBeReadException(zone.FilePath); /* Couldn't read map file=>throw exception */ }

			GV.Misc.RemoveComments(ref zoneContents); // remove comment notations from file contents because they serve no revelence

			string[] splitZoneContents = zoneContents.Split('\n'); // Get all lines contained within the zone file

			if (!Parser.HeaderCheck(zoneContents, Zone.ZONE_FILE_HEADER)) throw new ZoneHeaderException(zone.FilePath); // Header failed

			string zoneDimensionsString = splitZoneContents[0].Replace(Zone.ZONE_FILE_HEADER, "").Trim();

			if (zoneDimensionsString.Length != 0) {
				try {
					Vector2 size = Vector2Parser(zoneDimensionsString);
					zone.ZoneWidth = (int)size.X; zone.ZoneHeight = (int)size.Y;
				} catch { /*zone.ZoneWidth = Zone.DEFAULT_ZONE_WIDTH; zone.ZoneHeight = Zone.DEFAULT_ZONE_HEIGHT;*/ }
			}

			return GV.CollectionManipulator.GetSubArray(splitZoneContents, 1, splitZoneContents.Length-1);

			//return zoneContents.Substring(Zone.ZONE_FILE_HEADER.Length, zoneContents.Length - Zone.ZONE_FILE_HEADER.Length).Split('\n');
		}

		#region NewParser
		/// <summary>Extracts command from line in zone file</summary>
		/// <param name="line">Line from which command should be extracted</param>
		private static String GetCommand(String line) {
			Match lineMatch = Regex.Match(line, @"\w+:"); // Match desired word command from line

			if (lineMatch.Success) return lineMatch.Value.Substring(0, lineMatch.Value.Length - 1);

			throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Extract Command From Line '{line}'", lineIndex);
		}

		/// <summary>Removes command from line</summary>
		private static String RemoveCommand(String line, String command) {
			return line.Replace($"{command}:", "").Trim();
		}

		/// <summary>Get's region contained within a code block</summary>
		/// <param name="endStatement">Statement which defines the end of the code block</param>
		/// <returns>Contents of a given code block</returns>
		private static String[] GetCodeBlock(String endStatement) {
			List<String> blockContents = new List<String>();
		
			for (int X = lineIndex + 1; X < fileContents.Length; X++) {
				if (String.IsNullOrWhiteSpace(fileContents[X])) continue;

				if (GetCommand(fileContents[X]).ToUpper() == endStatement.ToUpper()) {
					lineIndex = X + 1; // Account for skipped values
					return blockContents.ToArray(); // Return stored block contents
				}

				blockContents.Add(fileContents[X]);
			}

			throw new ZoneSyntaxException(zone.FilePath, $"End Block Statement '{endStatement}' Not Found", fileContents.Length-1);
		}

		/// <summary>Extracts all asset IDs ina  string</summary>
		/// <param name="line">Line containing [asset IDs]</param>
		/// <returns>Array containing all found asset IDs</returns>
		private static String[] GetAssetIDs(String line) {
			MatchCollection matches = Regex.Matches(line, @"\[[^\r\n ]+\]");

			if (matches.Count != 0)
				return (from Match X in matches select X.Value.Replace("[", "").Replace("]", "")).ToArray();

			throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Extract Asset ID From String {line}", lineIndex);
		}
		#endregion

		#region ParserWrapper
		private static Vector2 Vector2Parser(String line) {
			try { return Parser.Vector2Parser(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Convert Line '{line}' To Vector", lineIndex, e);
			}
		}

		private static Boolean BooleanParser(String line) {
			try { return Parser.BooleanParser(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Convert Line '{line}' To Boolean", lineIndex, e);
			}
		}

		private static int[] IntegerParser(String line) {
			try { return Parser.IntegerParser(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Convert Line '{line}' To Integer", lineIndex, e);
			}
		}

		private static float[] FloatParser(String line) {
			try { return Parser.FloatParser(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, $"Couldn't Convert Line '{line}' To Float", lineIndex, e);
			}
		}

		private static String BracketParser(String line) {
			try { return Parser.GetValueInParentheses(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, e.Message, lineIndex, e);
			}
		}

		private static String SpeechMarkParser(String line) {
			try { return Parser.GetValueInSpeechMarks(line); } catch (Exception e) {
				throw new ZoneSyntaxException(zone.FilePath, e.Message, lineIndex, e);
			}
		}

		private static AnimationSequence AnimationParser(String anim) {
			try { return Parser.AnimationParser(anim); } catch (Exception e) {
				throw new HollowAetherException($"Couldn't Convert Line '{anim}' To Animation", e);
			}
		}
		#endregion

		/// <summary>Reference to zone instance</summary>
		private static Zone zone;

		/// <summary>Contents of zone file</summary>
		private static String[] fileContents;

		/// <summary>Index of line in file contents</summary>
		private static int lineIndex;
	}
}
