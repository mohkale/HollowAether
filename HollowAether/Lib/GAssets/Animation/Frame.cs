#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

using System.Text.RegularExpressions;

using HollowAether.Lib.Exceptions;
using Converters = HollowAether.Lib.InputOutput.Parsers.Converters;
using Parsers = HollowAether.Lib.InputOutput.Parsers.Parser;

namespace HollowAether.Lib.GAssets {
	public struct Frame {
		/// <summary> Holds sprite frame location in a given spritesheet </summary>
		/// <param name="fWidth">Sprite Frame Width</param>
		/// <param name="fHeight">Sprite Frame Height</param>
		/// <param name="xPos">relative X position of sprite</param>
		/// <param name="yPos">relative Y position of sprite</param>
		/// <param name="bWidth">block width</param>
		/// <param name="bHeight">block height</param>
		public Frame(int xPos, int yPos, int fWidth, int fHeight, int bWidth=defaultBlockWidth, int bHeight=defaultBlockHeight, int runCount=1) {
			_frameWidth = fWidth; // Width of sprite in frame
			_frameHeight = fHeight; // Height of sprite in frame
			_blockWidth = bWidth; // Width of sprites leading upto frame sprite
			_blockHeight = bHeight; // Height  of sprites leading upto frame sprite
			_xPos = xPos; // relative X position of sprite in spritesheet
			_yPos = yPos; // relative Y position of sprite in spritesheet
			RunCount = runCount;
		}

		public Frame(System.Drawing.Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height, 1, 1, 1) {

		}

		/// <summary>Sets buildup sprite dimensions</summary>
		/// <param name="X">Width of buildup sprites</param>
		/// <param name="Y">Height of buildup sprites</param>
		public void SetBlockDimensions(int X, int Y) {
			_blockWidth = X;
			_blockHeight = Y;
		}

		public override string ToString() {
			return $"({xPosition}, {yPosition}, {frameWidth}, {frameHeight} : {blockWidth}, {blockHeight})";
		}

		/// <summary>Sets buildup sprite dimensions</summary>
		/// <param name="vect">Vector representing buildup sprite dimensions</param>
		public void SetBlockDimensions(Vector2 vect) {
			SetBlockDimensions((int)vect.X, (int)vect.Y);
		}

		public String ToFileContents() {
			return $"{xPosition}, {yPosition}, {frameWidth}, {frameHeight} : {blockWidth}, {blockHeight} -> {RunCount}";
		}

		public static Frame FromFileContents(String line, bool throwException=false) {
			Regex r = new Regex(@"(\d+[, ]*)+(:[, ]*(\d+[, ]*)+)?->[, ]*\d+");
			
			if (isFrameDefinitionRegexCheck.IsMatch(line)) {
				int[] values = Parsers.IntegerParser(line); // Array of length 6, corresponding to all integer values

				if (!line.Contains(":")) return new Frame(values[0], values[1], values[2], values[3], runCount: values[4]); else {
					return new Frame(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
				}
			} else {
				if (!throwException) return Frame.Zero; else { // Raise new exception
					throw new HollowAetherException($"'{line}' Is Not A Valid Frame Definition");
				}
			}
		}

		public static Regex isFrameDefinitionRegexCheck = new Regex(@"(\d+[, ]*)+(:[, ]*(\d+[, ]*)+)?->[, ]*\d+");

		/// <summary>Sets buildup sprite dimensions</summary>
		/// <param name="tup">Tuple representing buildup sprite dimension</param>
		public void SetBlockDimensions(Tuple<int, int> tup) {
			SetBlockDimensions(tup.Item1, tup.Item2);
		}

		/// <summary>Sets frame dimensions</summary>
		/// <param name="X">Width of frame</param>
		/// <param name="Y">Height of frame</param>
		public void SetFrameDimensions(int X, int Y) {
			_frameWidth = X;
			_frameHeight = Y;
		}

		/// <summary>Sets frame dimensions</summary>
		/// <param name="vect">Vector representing frame dimensions</param>
		public void SetFrameDimensions(Vector2 vect) {
			SetFrameDimensions((int)vect.X, (int)vect.Y);
		}

		/// <summary>Sets frame dimensions</summary>
		/// <param name="tup">Tuple representing frame dimension</param>
		public void SetFrameDimensions(Tuple<int, int> tup) {
			SetFrameDimensions(tup.Item1, tup.Item2);
		}

		/// <summary>Converts frame to source rectangle</summary>
		/// <returns>Rectangle representing source of frame on spritesheet</returns>
		public Rectangle ToRect() {
			return new Rectangle(_blockWidth * _xPos, _blockHeight * _yPos, _frameWidth, _frameHeight);
		}

		public System.Drawing.Rectangle ToDrawingRect() {
			return new System.Drawing.Rectangle(_blockWidth * _xPos, _blockHeight * _yPos, _frameWidth, _frameHeight);
		}

		private int _frameWidth, _frameHeight, _blockWidth, _blockHeight, _xPos, _yPos;

		public const int defaultBlockWidth = 32, defaultBlockHeight = 32;

		public static Frame Zero { get { return new Frame(0, 0, 0, 0, 0, 0, 0); } }

		public int RunCount { get; /*private*/ set; }

		public int frameWidth { get { return _frameWidth; } set { _frameWidth = value; } }

		public int frameHeight { get { return _frameHeight; } set { _frameHeight = value; } }

		public int blockWidth { get { return _blockWidth; } set { _blockWidth = value; } }

		public int blockHeight { get { return _blockHeight; } set { _blockHeight = value; } }

		public int xPosition { get { return _xPos; } set { _xPos = value; } }

		public int yPosition { get { return _yPos; } set { _yPos = value; } }

		public Rectangle frame { get { return ToRect(); } } // Frame to source Rectangle
	}
}
