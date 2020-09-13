using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HollowAether.Lib.Debug {
	public static class DebugPrinter {
		public static void Draw() {
			if (!DebugPrinter.enabled) return; // Not enabled, return without draw

			float maxX = (from X in debugArgs.Keys select font.MeasureString(KeyToString(X))).Aggregate((a,b) => (a.X >= b.X) ? a : b).X;
			// Run through all debug args and measure their string representations as Vectors. Then select the one with the greatest X value

			GV.MonoGameImplement.SpriteBatch.Begin();
			Vector2 init = new Vector2(5, 5);

			foreach (String key in debugArgs.Keys) {
				String value = (debugArgs[key] == null) ? "None" : debugArgs[key].ToString();

				GV.MonoGameImplement.SpriteBatch.DrawString(font, $"{key}: {value}", init, color);

				init += new Vector2(0, font.MeasureString($"{key}: {value}").Y + 5);
			}

			GV.MonoGameImplement.SpriteBatch.End();
		}

		private static String KeyToString(String key) {
			return $"{key}: {debugArgs[key]}";
		}

		public static void Add(String parameter, Object value = null) {
			debugArgs.Add(parameter, value);
		}

		/// <summary>Sets the value for a debug printer target</summary>
		/// <param name="parameter">Parameter key for variable</param>
		/// <param name="value">New value for target variable</param>
		/// <exception cref="System.FormatException">Parameter doesn't exist</exception>
		public static void Set(String parameter, Object value) {
			if (!debugArgs.ContainsKey(parameter))
				throw new FormatException($"Paramter '{parameter}' Doesn't Exist");

			debugArgs[parameter] = value;
		}

		public static Color color = Color.White;
		public static Boolean enabled = true;

		static Dictionary<String, Object> debugArgs = new Dictionary<String, Object>();
		static SpriteFont font { get { return GV.MonoGameImplement.fonts["debugfont"]; } }
	}
}
