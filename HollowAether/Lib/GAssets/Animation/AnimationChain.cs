#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
namespace HollowAether.Lib.GAssets {
	public class AnimationChain {
		public AnimationChain(params AnimationSequence[] sequences) {
			chain = sequences.ToList<AnimationSequence>();
			frameCounter = CurrentSequence.Length;
		}

		public bool Update() {
			if (frameCounter != 0) frameCounter -= 1; else {
				chain.RemoveAt(0); // Pop current sequence Entry
				if (ChainAlive) frameCounter = CurrentSequenceLength;
			}

			if (ChainAlive) currentSequenceFrame = CurrentSequence.GetFrame(); // Set Current Sequence
			else ChainFinished();         // Call event handler if animation chain has been completed

			return ChainAlive; // Return Value To Ensure Program Knows Whether Chain Has Ended Or Not
		}

		public Frame GetFrame() { return currentSequenceFrame; }

		public int GetLongLength() { return (from X in chain select X.Length).Aggregate((a, b) => a + b); }

		/// <summary>Current animation sequence length</summary>
		private int CurrentSequenceLength { get { return CurrentSequence.Length; } }

		/// <summary>Current animation sequence in chain</summary>
		AnimationSequence CurrentSequence {
			get {
				try { return chain[0]; } // Top of chain is always the current animation sequence for chain
				catch { throw new HollowAetherException($"Cannot retrieve sequence when chain empty"); }
			}
		}

		/// <summary>Number of animation sequences in chain</summary>
		public int Length { get { return chain.Count; } }

		/// <summary>Whether chain is still running</summary>
		public bool ChainAlive { get { return Length > 0; } }

		/// <summary>Number of frames in chain</summary>
		public int LongLength { get { return GetLongLength(); } }

		private int frameCounter;
		
		private Frame currentSequenceFrame;

		/// <summary>Sequences in chain</summary>
		private List<AnimationSequence> chain;

		public event Action ChainFinished = () => { };
	}
}
