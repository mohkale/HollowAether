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
	public sealed class Coin : TreasureItem {
		/// <summary>The coin types which exist in the game</summary>
		/// <remarks>
		///		Small      - Texture 16_16, Y_Value 01
		///		Medium     - Texture 32_32, Y_Value 01
		///		Large      - Texture 32_32, Y_Value 02
		///		ExtraLarge - Texture 16_16, Y_Value 02
		/// </remarks>
		public enum CoinType { Small, Medium, Large, ExtraLarge }

		public Coin(Vector2 position, CoinType? _type = null, bool randMove = true) 
			: base(position, SPRITE_WIDTH, SPRITE_HEIGHT, 5000, true) {
			type = (_type.HasValue) ? _type.Value : DEFAULT_COIN_TYPE;
			Initialize(TypeToTextureString(type)); // Build sequence lib
		}

		public override void Interact() {
			if (!CanInteract || Interacted) return;
			GameWindow.GameRunning.InvokeCoinAcquired(this);
			base.Interact(); // Allocate to delete & Mark interacted
		}

		private static String TypeToTextureString(CoinType type) {
			return (type == CoinType.Small || type == CoinType.ExtraLarge) ? @"items\coins\coins_16x16" : @"items\coins\coins_32x32";
		}

		private static int TypeToCurrencyValue(CoinType type) {
			switch ((int)type) {
				case 0:  return 01;
				case 1:  return 03;
				case 2:  return 08;
				case 3:  return 12;
				default: return 00;
			}
		}

		private int TypeToCurrencyValue() { return TypeToCurrencyValue(type); }

		protected override void BuildSequenceLibrary() {
			int widthAndHeight = (type == CoinType.Small || type == CoinType.ExtraLarge) ? 16 : 32;
			int frameCount     = (type == CoinType.Small || type == CoinType.ExtraLarge) ? 04 : 06;
			int frameYValue    = (type == CoinType.Small || type == CoinType.Medium)     ? 00 : 01;

			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(
				widthAndHeight, widthAndHeight, 0, frameYValue, frameCount, widthAndHeight, widthAndHeight
			);
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		public CoinType type { get; private set; }

		public int Value { get { return TypeToCurrencyValue(type); } }

		public const CoinType DEFAULT_COIN_TYPE = CoinType.Small;

		public const int SPRITE_WIDTH = 16, SPRITE_HEIGHT = 16;
	}
}
