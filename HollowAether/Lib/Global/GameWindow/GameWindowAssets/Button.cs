using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.GAssets;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GameWindow {
	public abstract class Button : Sprite, IPredefinedTexture {
		public Button(Vector2 position, int width=SPRITE_WIDTH, int height=SPRITE_HEIGHT) 
			: base(position, width, height, true) { Initialize(TextureID); }

		protected override void BuildSequenceLibrary() {
			Animation["Default"] = new AnimationSequence(0, new Frame(160, 176, SPRITE_WIDTH, SPRITE_HEIGHT, 1, 1));
			Animation["Dark"]    = new AnimationSequence(0, new Frame(160, 208, SPRITE_WIDTH, SPRITE_HEIGHT, 1, 1));
			Animation["Active"]  = new AnimationSequence(0, new Frame(160, 240, SPRITE_WIDTH, SPRITE_HEIGHT, 1, 1));

			Animation.SetAnimationSequence("Default"); // Set to default animation sequence
		}

		public void Toggle() {
			active = !active; // Switch active to not active

			if (active) Animation.SetAnimationSequence("Active");
			else		Animation.SetAnimationSequence("Default"); 
		}

		public void InvokeClick() { Click(this); }

		public event Action<Button> Click = (self) => { };

		protected bool active = false;

		public bool Active { get { return active; } set { if (active != value) Toggle(); } }

		public virtual String TextureID { get; protected set; } = @"cs\textbox";

		public const int SPRITE_WIDTH = 64, SPRITE_HEIGHT = 32;
	}
}
