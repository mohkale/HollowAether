using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public static class Transition {
		private sealed class FadeSprite : VolatileSprite {
			public FadeSprite(Vector2 position, AnimationSequence _default) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT, true) {
				Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = _default;
				Initialize(textureKey); // Initialise from construction
			}

			public override void Initialize(string textureKey) {
				base.Initialize(textureKey); // Call base constructor to 

				InitializeVolatility(VolatilityType.Other, new ImplementVolatility(VolatilityImplement));
				// Initialise volatility to check when it is best to remove sprite from sprite batch
			}

			public override void Update(bool updateAnimation) {
				base.Update(updateAnimation);
			}

			public override void Draw() {
				GV.MonoGameImplement.SpriteBatch.End(); // End already running with camera

				GV.MonoGameImplement.InitializeSpriteBatch(false); // Start without camera

				GV.MonoGameImplement.SpriteBatch.Draw(
					Texture, position: Position,
					sourceRectangle: Animation.currentFrame.ToRect(),
					scale: new Vector2(transitionZoom),
					color: Color.White
				);

				GV.MonoGameImplement.SpriteBatch.End(); // End non camera sprite batch

				GV.MonoGameImplement.InitializeSpriteBatch(); // Init with camera for next sprites
			}

			private bool VolatilityImplement(IMonoGameObject self) {
				// When animation has reached end, ensure sprite is added to removal batch and ready to be deleted
				return Animation.CurrentSequence.FrameIndex + 1 >= Animation.CurrentSequence.Length;
			}

			protected override void BuildSequenceLibrary() {
				//throw new NotImplementedException();
			}

			public static String textureKey = @"fx\fadewhite";

			public static readonly int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
		}

		public enum Transitions {
			Uniform, // Equal transition view
			LeftHeavy, // More transition on the left
			RightHeavy, // more transition on the right
			UpHeavy, // more transition above
			BottomHeavy // more transition below
		}

		public enum TransitionShape {
			Diamond = 0,
			Circle = 1
		}

		public static void CreateTransition(Transitions transition = Transitions.Uniform, TransitionShape shape=TransitionShape.Diamond) {
			if (transitionRunning) return; // Already running, don't run again

			Frame[] frames = GetFrames((int)shape); // Store all frames for desired shape animation

			int width = GV.Variables.windowWidth, height = GV.Variables.windowHeight;

			int horizontalSpriteSpan = (int)Math.Ceiling(width  / FadeSprite.SPRITE_WIDTH  / transitionZoom);
			int verticalSpriteSpan   = (int)Math.Ceiling(height / FadeSprite.SPRITE_HEIGHT / transitionZoom);

			switch ((int)transition) {
				case 0: CreateUniformTransition     ((int)shape, frames, horizontalSpriteSpan, verticalSpriteSpan); break;
				case 1: CreateLeftHeavyTransition   ((int)shape, frames, horizontalSpriteSpan, verticalSpriteSpan); break;
				case 2: CreateRightHeavyTransition  ((int)shape, frames, horizontalSpriteSpan, verticalSpriteSpan); break;
				case 3: CreateUpHeavyTransition     ((int)shape, frames, horizontalSpriteSpan, verticalSpriteSpan); break;
				case 4: CreateBottomHeavyTransition ((int)shape, frames, horizontalSpriteSpan, verticalSpriteSpan); break;
			}

			transitionRunning = true;
		}

		private static void CreateUniformTransition(int shape, Frame[] frames, int xSpan, int ySpan) {
			foreach (int Y in Enumerable.Range(0, ySpan)) {
				foreach (int X in Enumerable.Range(0, xSpan)) {
					Vector2 position = new Vector2(X*FadeSprite.SPRITE_WIDTH*transitionZoom, Y*FadeSprite.SPRITE_HEIGHT*transitionZoom);

					GV.MonoGameImplement.additionBatch.AddNameless(new FadeSprite(position, new AnimationSequence(0, frames)));
				}
			}

			//((IVolatile)GV.MonoGameImplement.monogameObjects[spriteID]).VolatileManager.Deleting += () => transitionRunning = false;
			// Add event to final volatile sprite so new transitions can be made after it's deletion/removal-from-SpriteBatch.
		}

		private static void CreateLeftHeavyTransition(int shape, Frame[] frames, int xSpan, int ySpan) {
			throw new NotImplementedException($"Left heavy not yet done");

			String spriteID = String.Empty; // Var to store any new sprite IDs

			foreach (int Y in Enumerable.Range(0, ySpan)) {
				foreach (int X in Enumerable.Range(0, xSpan)) {

				}
			}

			//((IVolatile)GV.MonoGameImplement.monogameObjects[spriteID]).VolatileManager.Deleting += () => transitionRunning = false;
			// Add event to final volatile sprite so new transitions can be made after it's deletion/removal-from-SpriteBatch.
		}

		private static void CreateRightHeavyTransition(int shape, Frame[] frames, int xSpan, int ySpan) {
			throw new NotImplementedException($"Right heavy not yet done");

			String spriteID = String.Empty; // Var to store any new sprite IDs

			((IVolatile)GV.MonoGameImplement.monogameObjects[spriteID]).VolatilityManager.Deleting += () => transitionRunning = false;
			// Add event to final volatile sprite so new transitions can be made after it's deletion/removal-from-SpriteBatch.
		}

		private static void CreateUpHeavyTransition(int shape, Frame[] frames, int xSpan, int ySpan) {
			throw new NotImplementedException($"Up heavy not yet done");

			String spriteID = String.Empty; // Var to store any new sprite IDs

			((IVolatile)GV.MonoGameImplement.monogameObjects[spriteID]).VolatilityManager.Deleting += () => transitionRunning = false;
			// Add event to final volatile sprite so new transitions can be made after it's deletion/removal-from-SpriteBatch.
		}

		private static void CreateBottomHeavyTransition(int shape, Frame[] frames, int xSpan, int ySpan) {
			throw new NotImplementedException($"Down heavy not yet done");

			String spriteID = String.Empty; // Var to store any new sprite IDs

			((IVolatile)GV.MonoGameImplement.monogameObjects[spriteID]).VolatilityManager.Deleting += () => transitionRunning = false;
			// Add event to final volatile sprite so new transitions can be made after it's deletion/removal-from-SpriteBatch.
		}

		/// <summary>Get Sequeunce for animation of desired shape</summary>
		/// <param name="shape">Shape (Y-Value) for frames in animation</param>
		/// <returns>Sequence containing desired animation</returns>
		private static AnimationSequence GetFullSequence(int shape) {
			return AnimationSequence.FromRange(32, 32, 0, shape, 16, 32, 32, 1, true, 0);
		}

		/// <summary>Get all frames in sequence for given transition</summary>
		/// <param name="shape">Shape (Y-Value) for frames in animation</param>
		/// <returns>All frames for the desired animation sequence</returns>
		private static Frame[] GetFrames(int shape) {
			return GetFullSequence(shape).Frames.ToArray();
		}

		/// <summary>Value by which to zoom transitions in</summary>
		public static readonly float transitionZoom = 1.875f;

		/// <summary>Used to prevent simultaneous transition creations</summary>
		private static bool transitionRunning = false;
	}
}
