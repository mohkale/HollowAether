using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HollowAether.Lib.GAssets;
using System.Windows.Forms;
using System.Drawing;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether {
	public class EntityTile {
		public EntityTile(GameEntity entity) {
			this.Entity = entity;
			BuildDisplayedImage();
		}
		
		public void Paint(PaintEventArgs e) {
			Image drawImage = (displayedImage != null) ? displayedImage : Properties.Resources.error;
			e.Graphics.DrawImage(drawImage, Region);          // Draw cropped image texture to canvas

			if (DrawBorder) {
				using (Pen borderPen = new Pen(BorderColor, 2)) {
					e.Graphics.DrawRectangle(borderPen, Location.X, Location.Y, Width, Height);
				}
			}
		}

		private void BuildDisplayedImage() {
			if (Entity.GetEntityAttributes().Contains("DefaultAnimation") && this["DefaultAnimation"].IsAssigned) {
				frameRect = ((AnimationSequence)this["DefaultAnimation"].GetActualValue()).GetFrame(false).ToDrawingRect();
			} else { frameRect = GV.EntityGenerators.GetAnimationFromEntityName(Entity.EntityType); }

			bool dontBuildTexture = !this["Texture"].IsAssigned || !GV.LevelEditor.textures.ContainsKey(TextureKey) || !frameRect.HasValue;

			if (dontBuildTexture) { displayedImage = null; } else {
				displayedImage = GV.ImageManipulation.CropImage(GV.LevelEditor.textures[TextureKey].Item1, frameRect.Value);
			}
		}

		public bool AttributeExists(string key) { return Entity.AttributeExists(key); }

		public EntityAttribute this[string key] { get { return Entity[key]; } }

		public Vector2 Position { get { return (Vector2)this["Position"].GetActualValue(); } set { this["Position"].Value = value; } }

		public PointF Location { set { Position = new Vector2(value.X, value.Y); } get {
				Vector2 position = Position; return new PointF(position.X, position.Y);
			}
		}

		public int Width { get { return (int)this["Width"].GetActualValue(); } set { this["Width"].Value = value; } }

		public int Height { get { return (int)this["Height"].GetActualValue(); } set { this["Height"].Value = value; } }

		public String TextureKey { get { return this["Texture"].GetActualValue().ToString(); } set {
				this["Texture"].Value = value; BuildDisplayedImage();
			}
		}

		public float Layer { get { return (float)this["Layer"].GetActualValue(); } set { this["Layer"].Value = value; } }

		public RectangleF Region { get { return new RectangleF(Location, Size); } }

		public Size Size { get { return new Size(Width, Height); } }

		private Image displayedImage;

		private System.Drawing.Rectangle? frameRect;

		public GameEntity Entity { get; private set; }

		public String EntityType { get { return Entity.EntityType; } }

		public bool DrawBorder { get; set; } = true;

		public static System.Drawing.Color BorderColor = System.Drawing.Color.BlueViolet;
	}
}
