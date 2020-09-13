using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GameWindow {
	public sealed class TextButton : Button {
		public TextButton(Vector2 position, String buttonText, int width=SPRITE_WIDTH, int height=SPRITE_HEIGHT, String fontName=DEFAULT_FONT_ID) 
			: base(position, width, height) {
			fontID = fontName;
			SetText(buttonText);
		}

		public void SetText(String text) {
			Vector2 size = font.MeasureString(text);

			textPosition = new Vector2(
				Position.X + (0.5f * (Width - size.X)),
				Position.Y + (0.5f * (Height - size.Y))
			);

			buttonText = text; // Store buttons text
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey);
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);
		}

		public override void Draw() {
			base.Draw();

			GV.MonoGameImplement.SpriteBatch.DrawString(font, buttonText, textPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer+0.001f);
		}
		
		private SpriteFont font { get { return GV.MonoGameImplement.fonts[fontID]; } }
		private String fontID;
		private string buttonText;
		private Vector2 textPosition;
		public const String DEFAULT_FONT_ID = "homescreen";

		public new Vector2 Position { get { return base.Position; } set {
				base.Position = value;
				SetText(buttonText);
			}
		}
	}
}
