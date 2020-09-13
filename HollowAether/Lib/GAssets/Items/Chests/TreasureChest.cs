using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.Items {
	public sealed partial class TreasureChest : Chest {
		public TreasureChest() : this(Vector2.Zero) { }

		public TreasureChest(Vector2 position) : base(position) { }

		protected override void ReleaseContents() {
			int enumCount = Enum.GetNames(typeof(Coin.CoinType)).Length; // Number of coin types

			Vector2 initCoinPos = SpriteRect.Center.ToVector2() - new Vector2(0, Coin.SPRITE_HEIGHT);

			int coinCount = GV.Variables.random.Next(MIN_COIN_COUNT, MAX_COIN_COUNT);

			foreach (int X in Enumerable.Range(0, coinCount)) {
				int coinType = GV.Variables.random.Next(0, enumCount);

				Coin coin = new Coin(initCoinPos, (Coin.CoinType)coinType) {
					InteractionTimeout = 1000 // Don't interact for 1 secs
				};

				GV.MonoGameImplement.additionBatch.AddNameless(coin);
			}
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		private const int MIN_COIN_COUNT = 4, MAX_COIN_COUNT = 10;
	}
}
