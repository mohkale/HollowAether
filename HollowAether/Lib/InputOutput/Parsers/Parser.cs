#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.InputOutput;
#endregion
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.MapZone;

namespace HollowAether.Lib.InputOutput.Parsers {
	static class Converters {
		#region StringToValues

		#endregion

		public static Object StringToAssetValue(Type assetType, String value) {
			return StringToAssetValue(TypeToDesiredString(assetType), value);
		}

		public static Object StringToAssetValue(String assetType, String value) {
			switch (assetType.ToLower()) {
				case "string":
				case "str":
				case "texture2d":
				case "texture":
				case "sound":
					return value;
				case "animation":
				case "animationsequence":
					return Parser.AnimationParser(value);
				case "vector":
				case "vector2":
					return Parser.Vector2Parser(value);
				case "integer":
				case "int":
					return Parser.IntegerParser(value)[0];
				case "float":
					return Parser.FloatParser(value)[0];
				case "bool":
				case "boolean":
				case "flag":
					return Parser.BooleanParser(value);
			}

			throw new HollowAetherException($"Couldn't Convert '{value}' To Value Of Type '{assetType}'");
		}

		public static String TypeToDesiredString(Type type) {
			switch (type.Name.ToLower()) {
				case "int64": case "int32": case "int16":	return "Integer";
				case "single":								return "Float";
				case "string":								return "String";
				case "vector2":								return "Vector";
				case "boolean":								return "Bool";
				case "animation": case "animationsequence": return "Animation";
				case "flag":								return "Flag";
			}

			throw new HollowAetherException($"Couldn't Interpret '{type}' As Asset Type");
		}

		public static Asset StringToAsset(String assetType, String assetID, Object value) {
			switch (assetType.ToLower()) {
				case "string":
					return new Asset(typeof(String), assetID, value);
				case "texture2d": case "texture":
					return new TextureAsset(assetID, value);
				case "animation": case "animationsequence":
					try {
						var sequence = (value is String) ? Parser.AnimationParser(value.ToString()) : (AnimationSequence)value;
						return new AnimationAsset(assetID, sequence) { IsImportedAsset = sequence.IsImported };
					} catch (KeyNotFoundException) { throw new Exception($"Animation {value.ToString()} Not Found"); }
				case "vector": case "vector2":
					return new PositionAsset(assetID, value);
				case "integer": case "int": case "int16": case "int32": case "int64":
					return new IntegerAsset(assetID, value);
				case "float": case "single":
					return new FloatAsset(assetID, value);
				case "bool": case "boolean":
					return new BooleanAsset(assetID, value);
				case "flag":
					return new FlagAsset(assetID, value);
			}

			throw new HollowAetherException($"Couldn't Convert '{value.ToString()}' To Asset of Type '{assetType}'");
		}
		
		public static String ValueToString(Type type, Object value) {
			if (value is Asset) return $"[{(value as Asset).assetID}]"; // return assets as they should be

			switch (type.Name.ToLower()) {
				case "int64": case "int32": case "int16": case "single": case "string":
					return value.ToString();
				case "animationsequence":
					AnimationSequence sequence = value as AnimationSequence;

					if (!sequence.IsImported) { return "{"+sequence.Frames[0].ToFileContents()+"}"; } else {
						return GV.CollectionManipulator.DictionaryGetKeyFromValue(GV.MonoGameImplement.importedAnimations, sequence);
					}
				case "vector2": case "vector":
					return $"(X:{((Vector2)value).X.ToString().PadLeft(3, '0')}, Y:{((Vector2)value).Y.ToString().PadLeft(3, '0')})";
				case "boolean":
					return (bool)value ? "True" : "False";
				case "flag":
					return (value as Flag).Value ? "True" : "False";
			}

			throw new HollowAetherException($"Couldn't Convert String '{value.ToString()}' into object of type {type}");
		}

		public static bool IsValidConversion(string type, string value) {
			try { StringToAssetValue(type, value); return true; } catch { return false; }
		}
	}

	static class Parser {
		/// <summary>Checks whether the first line of a string matches a desired header</summary>
		/// <param name="line">String representing first line of a given </param>
		/// <param name="header">Header which line should match for correct return</param>
		/// <returns>Boolean indicating whether a line matches a desired header</returns>
		public static bool HeaderCheck(String line, String header) {
			return line.Length >= header.Length && line.Substring(0, header.Length) == header;
		}

		public static String SystemPathExtracter(String line) {
			//Match pathMatch = Regex.Match(line, @"[^ \n\r]+\\.+[^ \n\r]+");
			Match pathMatch = Regex.Match(line, @"(([\w]:\\)|\\)([^\n\r]+)");

			if (pathMatch.Success) return pathMatch.Value.TrimStart('\\'); else {
				throw new HollowAetherException($"Couldn't Find A System Path in Line '{line}'");
			}
		}

		public static AnimationSequence AnimationParser(Object value) {
			if (value is AnimationSequence) return (AnimationSequence)value; // when default is sequence

			string valString = value.ToString();
			bool isSingle = valString.Length > 2 && Frame.isFrameDefinitionRegexCheck.IsMatch(valString.Substring(1, valString.Length - 1));
			
			if (isSingle) {
				try { return new AnimationSequence(0, Frame.FromFileContents(value.ToString(), true)); } 
				catch (Exception e) { throw new HollowAetherException($"Animation Couldn't Be Extracted", e); }
			} else {
				try { return GV.MonoGameImplement.importedAnimations[value.ToString().ToLower()]; } 
				catch (Exception e) { throw new HollowAetherException($"Animation Couldn't Be Extracted", e); }
			}
		}

		/// <summary>Extracts value in a given string containing brackets</summary>
		/// <param name="line">String containing parentheses/brackets</param>
		/// <returns>The contents of the given parentheses/brackets</returns>
		public static String GetValueInParentheses(String line) {
			if (Regex.Match(line, @"(\(.+\))").Success) {
				String brackets = Regex.Match(line, @"\((.*?)\)").Value; // Value & ()
				return brackets.Substring(1, brackets.Length - 2); // Value Without ()
			} else { // Throw exception
				if (Regex.Match(line, @"\(\)").Success) {
					throw new HollowAetherException($"Line {line} Has Empty Parentheses Values");
				} else throw new HollowAetherException($"Line {line} Doesn't Contains Parentheses");
			}
		}

		/// <summary>Extracts value in a given string containing speech marks</summary>
		/// <param name="line">String containing speech marks</param>
		/// <returns>The contents of the given speech marks</returns>
		public static String GetValueInSpeechMarks(String line) {
			Match match = Regex.Match(line, "([\"'])(?:(?=(\\\\?))\\2.)*?\\1");

			if (!match.Success) // If no speech marks are in the given string
				throw new HollowAetherException($"Could Not Extract Value in Speech Marks For Line '{line}'");

			if (match.Value.Length == 2) // When just empty speech marks like ""
				throw new HollowAetherException($"No Value Found in Speech Marks For Line '{line}'");

			return match.Value.Substring(1, match.Value.Length - 2);
		}

		/// <summary>Constructs a Vector from an input string</summary>
		/// <param name="line">String containing vector in format (X:00, Y:00)</param>
		/// <returns>Vectorised value of vector in input string</returns>
		public static Vector2 Vector2Parser(String line) {
			String bracketContents = GetValueInParentheses(line); // Contents of (parenthesese)
			MatchCollection xyMatches = Regex.Matches(bracketContents, @"([Xx||Yy]:-?\d+)");

			if (xyMatches.Count != 2) // Lines has more than just X or Y in the axis
				throw new HollowAetherException($"Line {line} doesn't Have Vector Values Formatted Correctly");

			float X=float.NaN, Y=float.NaN;

			//Vector2 returnValue = new Vector2(); // Store vector which will be returned

			foreach (String xyMatch in (from Match N in xyMatches select N.Value)) {
				String[] splitMatch = xyMatch.Split(':'); // Index 0=Axis, 1=Value

				switch (splitMatch[0]) {
					case "x": case "X": X = FloatParser(splitMatch[1])[0]; break;
					case "y": case "Y": Y = FloatParser(splitMatch[1])[0]; break;
					default: throw new HollowAetherException($"Vector value has invalid axis '{splitMatch[0]}'");
				}
			}

			if (X == float.NaN) throw new HollowAetherException($"X axis unassigned in float definition");
			if (Y == float.NaN) throw new HollowAetherException($"Y axis unassigned in float definition");

			return new Vector2(X, Y); // Return new vector corresponding to desired values
		}

		/// <summary>Method to convert a string to a boolean value</summary>
		/// <param name="line">Line to extract boolean value from</param>
		/// <remarks>Acceptable boolean values=true, True, false, False</remarks>
		public static bool BooleanParser(String line) {
			if (Regex.Match(line, @"[Tt]rue").Success) return true;
			if (Regex.Match(line, @"[Ff]alse").Success) return false;

			throw new HollowAetherException($"Couldn't Convert Line '{line}' To A Boolean");

		}

		/// <summary>Converts String to intger</summary>
		/// <param name="line">Line containing single integer</param>
		/// <returns>All Integers found in string</returns>
		public static Int32[] IntegerParser(String line) {
			MatchCollection matches = Regex.Matches(line, @"-?\d+"); // All int matches, +ve || -ve

			if (matches.Count == 0) throw new HollowAetherException($"No Integer Found In Line '{line}'"); else {
				try { return (from X in Enumerable.Range(0, matches.Count) select Int32.Parse(matches[X].Value)).ToArray(); }
				catch { throw new HollowAetherException($"Couldn't Convert '{line}' To Int"); /*Because of casting error*/ }
			}
		}

		/// <summary>Converts string to float</summary>
		/// <param name="line">Line to convert to float</param>
		/// <returns>Float version of input string</returns>
		public static float[] FloatParser(String line) {
			MatchCollection matches = Regex.Matches(line, @"[-+]?[0-9]*\.?[0-9]+"); // All float matches, +ve || -ve

			if (matches.Count == 0) throw new HollowAetherException($"No Integer Found In Line '{line}'"); else {
				try { return (from X in Enumerable.Range(0, matches.Count) select float.Parse(matches[X].Value)).ToArray(); } 
				catch { throw new HollowAetherException($"Couldn't Convert '{line}' To float"); /*Because of casting error*/ }
			}
		}
	}
}

