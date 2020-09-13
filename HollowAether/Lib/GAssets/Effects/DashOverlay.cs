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
	public class DashOverlay : VolatileSprite {
		public DashOverlay(Vector2 position, Action UponDeletion=null) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT) {
			InitializeVolatility(VolatilityType.Other, new ImplementVolatility((IMGO) =>
				// Delete sprite when animation for dash sprite has been completed.
				Animation.CurrentSequence.FrameIndex + 1 >= Animation.CurrentSequence.Length
			));

			Initialize("fx\\dashoverlay"); // Initialise with overlay texture

			if (UponDeletion != null) VolatilityManager.Deleting += UponDeletion;
		}

		protected override void BuildSequenceLibrary() {
			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(32, 32,0, 0, 8, runCount: 2);
		}

		private const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
	}
}
