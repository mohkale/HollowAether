#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#endregion

using HollowAether.Lib.InputOutput;
using HollowAether.Lib.Exceptions.CE;

namespace HollowAether.Lib.GAssets {
	/// <summary>Class to store a series of frames and cycle through them one at a time</summary>
	public class AnimationSequence {
		/// <summary>Sets default base values for use by class</summary>
		/// <param name="initIndex">index of current frame in sequence</param>
		AnimationSequence(int initIndex) { FrameIndex = initIndex; }
		
		/// <summary>Creates animation sequence from passed frame arguments</summary>
		/// <param name="initIndex">Index of frame to begin animtion with</param>
		/// <param name="_frames">Frames to add to animation sequence</param>
		public AnimationSequence(int initIndex = 0, params Frame[] _frames) : this(initIndex) { frames.AddRange(_frames); }

		/// <summary>Builds animation sequence from XY frame tuples</summary>
		/// <param name="initIndex">Index of frame to begin animtion with</param>
		/// <param name="width">Width of frames</param> <param name="height">Height of frames</param>
		/// <param name="blockWidth">Width of blocks</param> <param name="blockHeight">Height of blocks</param>
		/// <param name="args">XY pairs for values in animation in multiples of block width and height</param>
		public static AnimationSequence FromTuples(int width, int height, int blockWidth = 32, int blockHeight = 32, int runCount=1, int initIndex = 0, params Tuple<int, int>[] args) {
			return new AnimationSequence(initIndex, GenerateFramesFromSequence(width, height, blockWidth, blockHeight, runCount, args).ToArray());
		}

		/// <summary>Builds animation sequence from straight strip in texture</summary>
		/// <param name="numOfFrames">Number of frames in strip</param>
		/// <param name="initIndex">Index of frame to begin animtion with</param>
		/// <param name="keepY">Whether the strip is horizontal or vertical</param>
		/// <param name="startPositionX">Beginning position on X axis to build frame</param>
		/// <param name="startPositionY">Beginning position on Y axis to build frame</param>
		/// <param name="width">Width of frames</param> <param name="height">Height of frames</param>
		/// <param name="blockWidth">Width of blocks</param> <param name="blockHeight">Height of blocks</param>
		public static AnimationSequence FromRange(int width, int height, int startPositionX, int startPositionY, int numOfFrames, int blockWidth = 32, int blockHeight = 32, int runCount=1, bool keepY = true, int initIndex = 0) {
			Func<int, Frame> GetFrame = (N) => {
				// Gets frame accounting for whether X or Y changes
				var pos = new Tuple<int, int>( // relativePosition
					(keepY) ? startPositionX + N : startPositionX,
					(keepY) ? startPositionY : startPositionY + N
				);
				
				return new Frame(pos.Item1, pos.Item2, width, height, blockWidth, blockHeight, runCount);
			};

			return new AnimationSequence(0, (from X in Enumerable.Range(0, numOfFrames) select GetFrame(X)).ToArray());
		}

		/// <summary>Yields framesfrom XY value combinations</summary>
		/// <param name="width">Width of frames</param> <param name="height">Height of frames</param>
		/// <param name="blockWidth">Width of blocks</param> <param name="blockHeight">Height of blocks</param>
		/// <param name="positions">XY pairs for values in animation in multiples of block width and height</param>
		private static IEnumerable<Frame> GenerateFramesFromSequence(int width, int height, int blockWidth, int blockHeight, int runCount, params Tuple<int, int>[] positions) {
			foreach (Tuple<int, int> position in positions) // Yield new frame for each in tuple collection
				yield return new Frame(position.Item1, position.Item2, width, height, blockWidth, blockHeight, runCount);
		}

		/// <summary>Converts animation to animation file contents</summary>
		/// <param name="animationName">Name of animation to store</param>
		/// <param name="sequence">Sequence to convert to animation</param>
		public static String ToFileContents(String animationName, AnimationSequence sequence) {
			var builder = new StringBuilder("\"" + animationName + "\" {\n");

			foreach (Frame frame in sequence.frames) {
				builder.Append($"    {frame.ToFileContents()}\n");
			}

			builder.Append("}\n"); // Closing curly brace for animation

			return builder.ToString(); // Convert to string and return
		}

		/// <summary>Reverses all frames in current sequence</summary>
		public void Reverse() { frames.Reverse(); }

		/// <summary>Adds frame to animation</summary>
		/// <param name="frame">Frame to add</param>
		public void AddFrame(Frame frame) { frames.Add(frame); }

		/// <summary>Resets animation sequence to beginning</summary>
		/// <param name="value">Index to return to, default is 0</param>
		public void ResetSequence(int value = 0) { FrameIndex = value; }

		/// <summary>Converts this animation to how it appears in an animation file</summary>
		/// <param name="animationName">Name of animation to store</param>
		public String ToFileContents(String animationName) {
			return ToFileContents(animationName, this);
		}

		/// <summary>Gets current frame in animation sequence</summary>
		/// <param name="increment">Whether to allow sequence to move on to next frame</param>
		/// <returns>Current animation frame before increment if incremented</returns>
		public Frame GetFrame(bool increment = true) {
			int returnIndex = FrameIndex; // stores current frame index before modification
			if (increment) FrameIndex = (FrameIndex + 1 < Length) ? FrameIndex + 1 : 0;
			return frames[returnIndex]; // Increments and then returns previous frame in animation
		}

		/// <summary>Gets frame of sequence at known index</summary>
		/// <param name="index">Index to retrieve frame from</param>
		private Frame GetFrameFromIndex(int index) {
			try { return frames[index]; } // Try to return frame of sequence from given index. If doesn't exist then throw exception
			catch { throw new AnimationException($"Sequence of length {Length} doesn't have value at index {index + 1}"); }
		}

		/// <summary>Returns a given frame in sequence</summary>
		/// <param name="index">Index of frame to return</param>
		public Frame this[int index] { get { return GetFrameFromIndex(index); } }

		/// <summary>Public accessor for frames in sequence</summary>
		public List<Frame> Frames { get { return frames; } }

		/// <summary>Index of current frame</summary>
		public int FrameIndex { get; private set; } = 0;
		
		/// <summary>Whether animation is from file stream</summary>
		public bool IsImported { get; set; } = false;

		/// <summary>Length of frames in given animation sequence</summary>
		public int Length { get { return frames.Count; } }

		/// <summary>Current frame in animation sequence, ignores increment</summary>
		public Frame CurrentFrame { get { return GetFrame(false); } }

		/// <summary>Frames in given sequence</summary>
		private List<Frame> frames = new List<Frame>();
	}
}
