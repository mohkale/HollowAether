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

using HollowAether.Lib.Exceptions;

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GameWindow {
	public class MultichoiceButton : Button {
		public MultichoiceButton(Vector2 position, int finHorizontalOffset=32, int width=SPRITE_WIDTH, int height = SPRITE_HEIGHT, String fontName=DEFAULT_FONT_ID, params String[] options) 
			: base(position, width, height) {
			this.options.AddRange(options); // Store passed options
			fontID = fontName;
			this.finHorizontalOffset = finHorizontalOffset;
		}

		public override void Draw() {
			base.Draw();

			Vector2 size = font.MeasureString("<"); // Should be same for > as well
			float finVerticalPos = Position.Y + ((Height - size.Y) / 2); // Y Position

			DrawString("<", new Vector2(Position.X + finHorizontalOffset,				   finVerticalPos));
			DrawString(">", new Vector2(Position.X + Width - size.X - finHorizontalOffset, finVerticalPos));

			string outputString = (selectedOptionIndex != -1) ? options[selectedOptionIndex] : "Null";

			DrawString(outputString, Position + ((new Vector2(Width, Height) - font.MeasureString(outputString)) / 2));
		}

		private void DrawString(String text, Vector2 position) {
			GV.MonoGameImplement.SpriteBatch.DrawString(font, text, position, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer + 0.001f);
		}

		public void SetSelectedOption(int index) {
			if (index > options.Count - 1) throw new HollowAetherException($"Button Select Index Out Of Range"); else {
				selectedOptionIndex = index; Changed(this); // Store new index, then trigger event handler
			}
		}

		public void SelectNextOption() {
			if (options.Count > 1) SetSelectedOption(GV.BasicMath.Clamp(selectedOptionIndex + 1, 0, options.Count - 1));
		}

		public void SelectPreviousOption() {
			if (options.Count > 1) SetSelectedOption(GV.BasicMath.Clamp(selectedOptionIndex - 1, 0, options.Count - 1));
		}

		public event Action<MultichoiceButton> Changed = (self) => { };

		public string ActiveSelection { get { return (selectedOptionIndex == -1) ? null : options[selectedOptionIndex]; } }

		private int selectedOptionIndex = -1;
		public List<String> options = new List<String>();
		public const String DEFAULT_FONT_ID = "homescreen";
		private SpriteFont font { get { return GV.MonoGameImplement.fonts[fontID]; } }
		private String fontID;
		private int finHorizontalOffset;
	}
}
