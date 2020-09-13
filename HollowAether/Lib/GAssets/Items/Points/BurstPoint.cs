using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public sealed class BurstPoint : TreasureItem {
		public enum PointSpan { Small, Medium, Large }

		public BurstPoint(Vector2 position, PointSpan _span) : base(position, spriteWidth, spriteHeight, 10000, true) {
			span = _span; // Store span to help create animation
			Initialize("cs\\npcsym"); // Initialise with necesarry texture
			InitializeVolatility(VolatilityType.Timeout, 50000); // Exists for 5 secs
			Value = GetBurstPointValue(); // Random generate value
		}

		PointSpan span;

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Also updates elapsed time

			if (GV.MonoGameImplement.Player.Intersects(boundary)) {
				Interact(); // When collided with player, interact
			}
		}

		public override void Interact() {
			if (Interacted || !CanInteract) return; // Skip
			GameWindow.GameRunning.InvokeBurstPointAcquired(this);
			base.Interact(); // Allocate to delete & Mark interacted
		}

		private static int GetBurstPointValue() {
			double chance = GV.Variables.random.NextDouble(); // Value between 1.0 & 0.0

			if (chance < 1 / MAX_POINT_VALUE) return 1; else {
				foreach (int X in Enumerable.Range(MIN_POINT_VALUE, MAX_POINT_VALUE))
					if (chance > 1 / X) return X; // Chance becomes less likely as X bigger
			}

			return 1; // Should be impossible, but just in case 
		}

		protected override void VerticalDownwardBoundaryFix() {
			base.VerticalDownwardBoundaryFix();
		}

		protected override void LinearCollisionHandler() {
			base.LinearCollisionHandler();

			/*int currentSequence = Convert.ToInt32(SequenceKey); // Store as frame int
			int next = (currentSequence + 1 < FRAME_COUNT) ? currentSequence + 1 : 0;
			Animation.SetAnimationSequence(next.ToString()); // Next frame in sequence*/
		}

		protected override void BuildSequenceLibrary() {
			/*foreach (int X in Enumerable.Range(0, FRAME_COUNT)) {
				Frame sequenceFrame = new Frame(X, 1 + (int)span, 32, 32);
				Animation[X.ToString()] = new AnimationSequence(0, sequenceFrame);
			}

			Animation.SetAnimationSequence(GV.Variables.random.Next(0, FRAME_COUNT).ToString());*/

			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(
				32, 32, 0, 1 + (int)span, 5, 32, 32, 25, true, 1
			);
		}

		private const int FRAME_COUNT = 5;

		protected override void BuildBoundary() {
			int width; //IBRectangle mainBoundary;

			switch (span) {
				case PointSpan.Large:  width = 26; break;
				case PointSpan.Medium: width = 17; break;
				case PointSpan.Small:  width = 13; break;
				default:               width = 00; break;
			}

			width = (Width - width) / 2;

			boundary = new SequentialBoundaryContainer(SpriteRect,
				new IBRectangle(Position.X + width, Position.Y + width, Width - (2 * width), Height - (2 * width))
			);
		}

		public int Value { get; private set; }

		public const int spriteWidth = 32, spriteHeight = 32;

		private const int MIN_POINT_VALUE = 1, MAX_POINT_VALUE=5;
	}
}
