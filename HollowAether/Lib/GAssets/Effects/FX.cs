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

namespace HollowAether.Lib.GAssets.FX {
	public abstract class FXSprite : VolatileSprite {
		public FXSprite(Vector2 position, int width, int height, int timeout) : base(position, width, height) {
			InitializeVolatility(VolatilityType.Timeout, timeout);
			Layer = 0.75f; // Above player, below HUD
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);
			ImplementEffect();

			// Check if FX is in camera here
		}

		protected abstract void ImplementEffect();
	}
}
