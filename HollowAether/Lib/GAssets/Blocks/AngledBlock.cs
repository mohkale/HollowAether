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
	/*public partial class AngledBlock : CollideableSprite, IBlock, IInitializableEntity { // Only builds right angled blocks, Other implementation comes later if possible
		public AngledBlock() : this(new Vector2(), 0, 0, IBRightAngledTriangle.BlockType.A) { }

		public AngledBlock(Vector2 position, int width, int height, IBRightAngledTriangle.BlockType _type) 
			: base(position, width, height) { type = _type; } 

		public AngledBlock(Vector2 position, int width, int height, bool _rightAllign=true, bool topAllign=false) 
			: base(position, width, height) {
			type = IBRightAngledTriangle.AllignmentsToBlockType(_rightAllign, topAllign);
		}

		protected override void BuildSequenceLibrary() {
			//animation[GlobalVars.MonoGameImplement.defaultAnimationSequenceKey] = new AnimationSequence(0, new Frame(0, 0, 32, 32));
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRightAngledTriangle(Position, Width, Height, type));
		}

		//public static void AngledBlockFrom3Vects(Vector2 a, Vector2 b, Vector2 c) { }

		public IBRightAngledTriangle.BlockType type;
	}*/
}
