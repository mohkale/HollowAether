using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;
using GPBI = HollowAether.Lib.GAssets.HUD.GamePadButtonIcon;
using GPB = HollowAether.Lib.GAssets.HUD.GamePadButtonIcon.GamePadButton;
using GPT = HollowAether.Lib.GAssets.HUD.GamePadButtonIcon.Theme;

namespace HollowAether.Lib.GAssets.HUD {
	public class GamePadButtonIconWithText : GPBI {
		public enum TextPosition { BeforeButton, OverButton, AfterButton }

		public GamePadButtonIconWithText(
				Vector2 position, GPB _button, string text,
				TextPosition tp = TextPosition.OverButton, 
				GPT theme=GPT.Regular, int width = SPRITE_WIDTH, 
				int height = SPRITE_HEIGHT, bool active=true
			) : base(position, _button, theme, width, height, active) {
			SetText(text);
			positionArg = tp;
		}

		public override void Draw() {
			base.Draw();

			GV.MonoGameImplement.SpriteBatch.DrawString(Font, Text, GetTextPosition(), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer+0.0005f);
		}

		private Vector2 GetTextPosition() {
			Vector2 horizontalOffset = new Vector2(5, 0);

			switch (positionArg) {
				case TextPosition.AfterButton:
					return Position + horizontalOffset + new Vector2(Width, 0);
				case TextPosition.BeforeButton:
					return Position - new Vector2(stringSize.X, 0) - horizontalOffset;
				case TextPosition.OverButton:
					return SpriteRect.Center.ToVector2() - (stringSize / 2);
				default: throw new HollowAetherException($"Invalid position arg");
			}
		}

		public void SetText(String text) { Text = text; stringSize = Font.MeasureString(text); }


		private Vector2 stringSize;

		public String Text { get; private set; }

		private String fontKey = "homescreen";

		private TextPosition positionArg;

		public SpriteFont Font {
			get { return GV.MonoGameImplement.fonts[fontKey]; } // Gets sprite font from font key index in global store
			set { fontKey = GV.CollectionManipulator.DictionaryGetKeyFromValue(GV.MonoGameImplement.fonts, value); }
		}
	}
}
