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

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GameWindow {
	public class PushableText : IPushable {
		public PushableText(Vector2 position, String text, Color color, String fontKey) {
			Position       = position;
			font           = fontKey;
			this.color     = color;
			stringToOutput = text;
		}

		public void Update(bool updateAnimation=true) {
			int elapsedMilitime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			float elapsedTime     = elapsedMilitime * (float)Math.Pow(10, -3); // To Miliseconds

			if (BeingPushed && !PushPack.Update(this, elapsedTime)) {
				PushPack = null; // Delete push args
			}
		}

		public void Draw() {
			GV.MonoGameImplement.SpriteBatch.DrawString(Font, stringToOutput, Position, color);
		}

		public void PushTo(Vector2 position, float over=0.8f) {
			if (PushArgs.PushValid(position, Position))
				Push(new PushArgs(position, Position, over, true));
		}

		public void Push(PushArgs push) { PushPack = push; }

		public Vector2 MeasureString(String str) {
			return Font.MeasureString(str);
		}

		public PushArgs PushPack { get; private set; } = null;

		public bool BeingPushed { get { return PushPack != null; } }

		public Vector2 Size { get { return MeasureString(stringToOutput); } }

		public SpriteFont Font { get { return GV.MonoGameImplement.fonts[font]; } }

		private String font;

		private Color color;

		private String stringToOutput;

		public Vector2 Position { get; set; }
	}
}
