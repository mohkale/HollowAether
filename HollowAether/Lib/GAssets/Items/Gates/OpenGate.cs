using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib.GAssets.Items.Gates {
	public partial class OpenGate : Gate {
		public OpenGate() : this(Vector2.Zero, Vector2.Zero) { }

		public OpenGate(Vector2 position, Vector2 takesToZone) : base(position, takesToZone) { }

		protected override void BuildSequenceLibrary() {
			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = new AnimationSequence(
				0, new Frame(384, 224, 32, 32, 1, 1, 1)
			);
		}

		public override bool CanInteract { get; protected set; } = true;
	}
}
