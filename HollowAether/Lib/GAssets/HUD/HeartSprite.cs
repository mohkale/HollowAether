using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.GAssets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.HUD {
	/// <summary>Base class for Hearts displayed on HUD.</summary>
	class HeartSprite : Sprite, IPushable {
		/// <param name="offset">Value with which to offset sprite from top left corner of screen</param>
		/// <param name="start_health">Beginning health assigned to sprite. It's full by default</param>
		public HeartSprite(Vector2 offset, int startHealth=HEART_SPAN) : base(offset, DEFAULT_WIDTH, DEFAULT_HEIGHT, true) {
			health = startHealth; // Set beginning health for heart from constructor, by default is max
			Initialize(@"sprites\heart"); // Initialize from construction with spritesheet & animations
			ResetAnimationSequence(); // ResetAnimationSequence to match corresponding heart health
			heartCount += 1; // Increment the amount of hearts known
			Layer = 0.9f;
		}

		public HeartSprite(int xOffset, int yOffset, int startHealth=HEART_SPAN) : this(new Vector2(xOffset, yOffset), startHealth) { }

		public void PushTo(Vector2 position, float over = 0.8f) {
			if (PushArgs.PushValid(position, Position))
				Push(new PushArgs(position, Position, over));
		}

		public void Push(PushArgs args) { if (!BeingPushed) PushPack = args; }

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed time as well

			if (BeingPushed && !PushPack.Update(this, elapsedTime)) {
				PushPack = null; // Delete push pack
			}
		}

		/// <summary>Builds all animations used by sprite</summary>
		protected override void BuildSequenceLibrary() {
			for (int X = HEART_SPAN; X >= 0; X--) { // Iterative animation generator -> Relies on texture following format
				Animation[$"{X}/{HEART_SPAN}"] = new AnimationSequence(0, new Frame(HEART_SPAN - X, 1, 18, 15, 18, 15));
			}

			ResetAnimationSequence(); // Reset sequence to 
		}

		/// <summary>Set's animation to match health of heart sprite</summary>
		private void ResetAnimationSequence() {
			Animation.SetAnimationSequence($"{health}/{HEART_SPAN}", true);
		}

		/// <summary>Adds health to sprite heart and then updates animation sequence to match</summary>
		/// <param name="value">Value to add to health already contained in heart</param>
		/// <exception cref="HollowAether.Lib.Exceptions.HollowAetherException">New value out of range</exception>
		public void AddHealth(int value=1) {
			if (health + value > HEART_SPAN) throw new HollowAetherException($"Heart health cannot be '{health + value}'");
			health += value; ResetAnimationSequence(); // Subtract from health then re-decide animation-sequence
		}

		/// <summary>Subtracts health from heart sprite and then updates animation sequence to match</summary>
		/// <param name="value">Value to subtract from health already contained in heart</param>
		/// <exception cref="HollowAether.Lib.Exceptions.HollowAetherException">New value out of range</exception>
		public void SubtractHealth(int value=1) {
			if (health - value < 0) throw new HollowAetherException($"Heart health cannot be '{health - value}'");
			health -= value; ResetAnimationSequence(); // Subtract from health then re-decide animation-sequence
		}

		public void RefillHealth() { health = HEART_SPAN; ResetAnimationSequence(); }

		public void EmptyHealth() { health = 0; ResetAnimationSequence(); }

		/// <summary>Sets the health of the heart and then updates animation sequence to match</summary>
		/// <param name="newHealth">New heart health</param>
		/// <exception cref="HollowAether.Lib.Exceptions.HollowAetherException">New value out of range</exception>
		public void SetHealth(int newHealth) {
			if (newHealth < 0 || newHealth > HEART_SPAN) throw new HollowAetherException($"Heart health cannot be '{newHealth}'");
			health = newHealth; ResetAnimationSequence(); // Change stored health to new value, then re-decide animation sequence
		}
		
		/// <param name="heart">Heart to add to</param>
		/// <param name="value">Value to add to heart</param>
		/// <returns>New heart with incremented health</returns>
		public static HeartSprite operator +(HeartSprite heart, int value) {
			heart.health += value; return heart;
		}

		/// <param name="heart">Heart to subtract from heart</param>
		/// <param name="value">Value to subtract from heart</param>
		/// <returns>New heart with decremented health</returns>
		public static HeartSprite operator -(HeartSprite heart, int value) {
			heart.health -= value; return heart;
		}

		/// <summary>Supports implicit casting to integer</summary>
		/// <param name="heart">Heart to cast to integer</param>
		public static implicit operator int(HeartSprite heart) {
			return heart.health;
		}

		/// <summary>Health of heart</summary>
		public int Health { get { return health; } set { SetHealth(value); } }

		public PushArgs PushPack { get; private set; } = null;

		public bool BeingPushed { get { return PushPack != null; } }

		private int health; // Health allocated to sprite/heart

		public const int DEFAULT_WIDTH = 18 * 2, DEFAULT_HEIGHT = 15 * 2;
		public const int HEART_SPAN = 4; // Max amount of heart features
		public static int heartCount = 0;
	}
}
