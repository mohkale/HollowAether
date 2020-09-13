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
	public sealed partial class Key : BodySprite, IInteractable, IItem, IContextualised {
		public enum keyType { Regular, Dark, White };

		public Key(Vector2 position, keyType _type=keyType.Regular) : base(position, 19, 19) {
			type = _type; // Store type
			Initialize("cs\\npcsym");
			ItemID = "Gray Skull";
			OutputString = $"{GV.MonoGameImplement.PlayerName} Got Key \"{ItemID}\" Now what? #sys:waitforinput;";
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);
			ImplementGravity(); // Needs to fall
		}

		protected override void BuildSequenceLibrary() {
			Frame frame = new Frame(4 + (int)type, 14, 32, 32);

			AnimationSequence sequence = new AnimationSequence(0, frame);

			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = sequence;
		}

		public void Interact() { GameWindow.GameRunning.InvokeGotItem(this); }

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		public bool Interacted { get; private set; } = false;
		public bool CanInteract { get; set; } = true;

		public bool OutputReady() { return false; }

		public String ItemID { get; }

		public String OutputString { get; }

		private keyType type;
	}
}
