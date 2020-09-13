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
	public class HitSprite : VolatileSprite {
		public enum HitType { Red, Blue }

		public HitSprite(Vector2 position, int width, int height, HitType type) : base(position, width, height, true) {
			hitType = type; // Store hit type
			Initialize(@"fx\hitfx");
			InitializeVolatility(VolatilityType.Other, new ImplementVolatility(ReadyToDelete));
		}

		private bool ReadyToDelete(IMonoGameObject _object) {
			return Animation.CurrentSequence.FrameIndex + 1 >= Animation.CurrentSequence.Length;
		}

		protected override void BuildSequenceLibrary() {
			int blockDimensions = hitType == HitType.Red ? RED_SPRITE_DIMENSIONS : BLUE_SPRITE_DIMENSIONS;

			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(
				blockDimensions, blockDimensions, 0, (int)hitType, 
				(hitType == HitType.Red ? 3 : 4)+1, blockDimensions,
				RED_SPRITE_DIMENSIONS, 5, true, 0
			);
		}

		private HitType hitType;

		public const int RED_SPRITE_DIMENSIONS = 32; // 32 by 32

		public const int BLUE_SPRITE_DIMENSIONS = 48; // 48 by 48
	}
}
