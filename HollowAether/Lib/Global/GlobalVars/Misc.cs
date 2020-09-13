#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Encryption;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
#endregion

namespace HollowAether.Lib {
	public static partial class GlobalVars {

		public static class Misc {
			public static string GenerateRandomString(int length) {
				const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // store alpha
				return new string(Enumerable.Repeat(chars, length).Select(s => s[Variables.random.Next(s.Length)]).ToArray());
			}

			/// <summary>Method to remove comments from Map & Zone Files</summary>
			/// <param name="contents">Contents of Map/Zone files</param>
			/// <param name="commentString">String which acts at comment</param>
			public static void RemoveComments(ref String contents, String commentString = "#") {
				foreach (Match comment in Regex.Matches(contents, $"{commentString}(.*)")) {
					contents = contents.Replace(comment.Value, ""); // remove instance of comment
				}
			}

			public static bool TypesContainsGivenType(Type current, Type[] desired) {
				foreach (Type type in desired) {
					if (DoesExtend(current, type))
						return true; // Valid type
				}

				return false; // Not in valid type array
			}

			public static bool DoesExtend(Type current, Type desired) {
				return current == desired || current.IsAssignableFrom(desired) || current.IsSubclassOf(desired) || current.GetInterface(desired.FullName) != null;
			}

			/// <summary>Method to try to turn a string to an int and return it</summary>
			/// <param name="arg">String value to return as an int</param>
			/// <returns>Nullable which may have integer value of arg</returns>
			public static int? StringToInt(String arg) {
				try { return int.Parse(arg); } catch { return null; }
			}

			public static float? StringToFloat(String arg) {
				try { return float.Parse(arg); } catch { return null; }
			}

			public static bool SignChangedFromAddition(float A, float B) {
				return (A < 0 && A + B >= 0) || (A >= 0 && A + B < 0);
				// (A is -ive && A + B is +ive) || (A is +ive && A + B is -ve) 
			}

			public static bool SignChangedFromSubtraction(float A, float B) {
				return !SignChangedFromAddition(A, -B);
			}
		}
	}
}
