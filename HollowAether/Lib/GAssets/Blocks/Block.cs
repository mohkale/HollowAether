#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.MapZone;
#endregion

namespace HollowAether.Lib.GAssets {
	public partial class Block : CollideableSprite, IBlock, IInitializableEntity {
		public Block() : base() { }

		public Block(Vector2 position, int width, int height, bool aRunning=true) 
			: base(position, width, height, aRunning) { }

		protected override void BuildSequenceLibrary() {
			if (!Animation.SequenceExists(GlobalVars.MonoGameImplement.defaultAnimationSequenceKey))
				Animation[GlobalVars.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(32, 32, 0, 0, 1, 32, 32);
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}
	}
}
