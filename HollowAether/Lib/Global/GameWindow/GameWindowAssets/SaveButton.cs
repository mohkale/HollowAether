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

using GV = HollowAether.Lib.GlobalVars;
using SM = HollowAether.Lib.InputOutput.SaveManager;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;

namespace HollowAether.Lib.GameWindow {
	public sealed class SaveButton : Button {
		private struct SaveFileDetails {
			public SaveFileDetails(int slotIndex) {
				saveIndex = slotIndex; // Store slot index locally
				filePath = SM.GetSaveFilePathFromSlotIndex(slotIndex);
				exists = IOMan.FileExists(filePath); // File Exists :)

				if (exists) {
					made = System.IO.File.GetCreationTime(filePath);
					written = System.IO.File.GetLastWriteTime(filePath);
					Vector2 index = SM.GetSave(slotIndex).currentZone;
					zoneName = IOMan.GetFileTitleFromPath(GV.MonoGameImplement.map.GetZoneArgsFromVector(index).path);
				} else {
					made = DateTime.MinValue;
					written = DateTime.MinValue;
					zoneName = null; // Nothing
				}
			}

			public void Draw(Rectangle containerRect, float layer) {
				SpriteFont font = GV.MonoGameImplement.fonts["homescreen"]; // Get and store sprite font
				Vector2 offset = new Vector2(containerRect.Height * 7 / 40);
				Vector2 textPosition = containerRect.Location.ToVector2() + offset;
				Vector2 containerBottomRight = new Vector2(containerRect.Right, containerRect.Bottom);

				String slotString = $"Slot: {(saveIndex + 1).ToString().PadLeft(3, '0')}";
				Vector2 slotStringSize = font.MeasureString(slotString); // Size of main
				DrawString(slotString, font, textPosition, layer); // Draw slot string to screen

				String bottomCornerString = (Valid) ? zoneName : "New Game!"; // String in bottom right corner
				Vector2 bottomCornerSize = font.MeasureString(bottomCornerString); // Size of said string 
				Vector2 bottomCornerPos = containerBottomRight - offset - bottomCornerSize; // Position

				DrawString(bottomCornerString, font, bottomCornerPos, layer); // Draw corner string to position

				if (Valid) { // Draw some date details
					String madeString = $"Made: {made}"; // When save file was made in string format
					Vector2 madePosition = containerRect.Center.ToVector2() - (font.MeasureString(madeString) / 2);
					DrawString(madeString, font, madePosition, layer); // Draw the date for when the save was made
				}
			}

			private static void DrawString(String value, SpriteFont font, Vector2 position, float layer) {
				GV.MonoGameImplement.SpriteBatch.DrawString(
					font, value, position, Color.White,
					0f, Vector2.Zero, 1f,
					SpriteEffects.None, layer
				);
			}

			public void Delete() {
				exists = false;
			}

			/// <summary>Name of zone</summary>
			private string zoneName;

			/// <summary>Path to save file</summary>
			public String filePath;

			/// <summary>Index of save file</summary>
			public int saveIndex;

			/// <summary>Dates when made and last written</summary>
			public DateTime made, written;

			/// <summary>File exists</summary>
			public bool exists;

			/// <summary>Valid save file</summary>
			public bool Valid { get { return exists; } }
		}

		public SaveButton(Vector2 position, int saveIndex, int width, int height) : base(position, width, height) {
			fileDetails = new SaveFileDetails(saveIndex);
		}

		public override void Draw() {
			base.Draw(); // Draws hold deets

			fileDetails.Draw(SpriteRect, Layer + 0.0005f);
		}

		public void DeleteDetails() {
			fileDetails.Delete();
		}

		protected override void BuildSequenceLibrary() {
			Animation["Default"] = new AnimationSequence(0, new Frame(0, 0, SPRITE_WIDTH, 16, SPRITE_WIDTH, 16));
			Animation["Dark"]    = new AnimationSequence(0, new Frame(0, 1, SPRITE_WIDTH, 16, SPRITE_WIDTH, 16));
			Animation["Active"]  = new AnimationSequence(0, new Frame(0, 2, SPRITE_WIDTH, 16, SPRITE_WIDTH, 16));

			Animation.SetAnimationSequence("Default");
		}

		SaveFileDetails fileDetails;

		public new const int SPRITE_WIDTH = 112;

		public int SaveIndex { get { return fileDetails.saveIndex; } }

		public override String TextureID { get; protected set; } = @"cs\button_stretched";
	}
}
