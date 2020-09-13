using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Text {
	class ColoredString {
		public ColoredString(String value) {
			_string = value;
		}

		public ColoredString(String value, params Object[] args) {
			_string = String.Format(value, args);
		}

		public static ColoredString Red(String value) {
			return new ColoredString(value) { foreground = ConsoleColor.Red };
		}

		public static ColoredString Blue(String value) {
			return new ColoredString(value) { foreground = ConsoleColor.Blue };
		}

		public static ColoredString Yellow(String value) {
			return new ColoredString(value) { foreground = ConsoleColor.DarkYellow };
		}

		public static ColoredString Green(String value) {
			return new ColoredString(value) { foreground = ConsoleColor.Green };
		}

		public static String Pack(String str) {
			StringBuilder builder = new StringBuilder(); // Container to hold string

			int count = (int)Math.Floor(str.Length / (double)Console.WindowWidth); // Occurence

			foreach (int index in Enumerable.Range(0, count)) {
				builder.Append(str.Substring(Console.WindowWidth * index, Console.WindowWidth));
			}

			String remainder = str.Substring(builder.Length, str.Length - builder.Length);
			builder.Append(remainder.PadLeft((remainder.Length + Console.WindowWidth) / 2));
		
			return builder.ToString(); // Convert given builder to a string
		}

		public static String Pack(String str, params Object[] args) {
			return Pack(String.Format(str, args));
		}

		public void Write() {
			Set(); Console.Write(_string); Reset();
		}

		public void WriteLine() {
			Set(); Console.WriteLine(_string); Reset();
		}

		public void ResetForeground(ConsoleColor? _color = null) {
			Console.ForegroundColor = _color.HasValue ? _color.Value : defForeground;
		}

		public void ResetBackground(ConsoleColor? _color = null) {
			Console.BackgroundColor = _color.HasValue ? _color.Value : defBackground;
		}

		public void Reset(ConsoleColor? fore = null, ConsoleColor? back = null) {
			if (!fore.HasValue) ResetForeground(); else ResetForeground(fore.Value);
			if (!back.HasValue) ResetForeground(); else ResetForeground(back.Value);
		}

		public void SetForeground(ConsoleColor? _color = null) {
			Console.ForegroundColor = _color.HasValue ? _color.Value : foreground;
		}

		public void SetBackground(ConsoleColor? _color = null) {
			Console.BackgroundColor = _color.HasValue ? _color.Value : background;
		}

		public void Set(ConsoleColor? fore = null, ConsoleColor? back = null) {
			if (!fore.HasValue) SetForeground(); else SetForeground(fore.Value);
			if (!back.HasValue) SetForeground(); else SetForeground(back.Value);
		}

		public String _string;

		public ConsoleColor foreground = defForeground, background = defBackground;

		public static ConsoleColor defForeground = Console.ForegroundColor;
		public static ConsoleColor defBackground = Console.BackgroundColor;
	}
}
