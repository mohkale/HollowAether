using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib.Forms.LevelEditor {
	public class TileBox : PictureBox {
		public TileBox() : base() {
			Paint += PaintTile;
	
			TextureOrRegionChanged  = (t, r) => { texture = t; textureRegion = r; };
			TextureOrRegionChanged += (t, r) => { CropTexture();       		      };
			TextureOrRegionChanged += (t, r) => { BuildCroppedPictureBoxRegion(); };
			TextureOrRegionChanged += (t, r) => { Draw();						  };

			SizeChanged += (s, e) => { TextureOrRegionChanged(texture, textureRegion); };
		}

		~TileBox() { fillBrush.Dispose(); }

		public TileBox(Frame initFrame) : this() { AssignFrame(initFrame); }

		public TileBox(Rectangle initFrame) : this() { AssignRect(initFrame); }

		public void PaintTile(object sender, PaintEventArgs args) {
			if (DrawTile) {
				args.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

				if (croppedTexture != null) {
					if (DisplayBorderRectsForScaledImage && BorderRects != null) {
						args.Graphics.FillRectangle(fillBrush, BorderRects.Item1);
						args.Graphics.FillRectangle(fillBrush, BorderRects.Item2);
					}

					args.Graphics.DrawImage(croppedTexture, CPBR_X, CPBR_Y, CPBR_Width, CPBR_Height);
				} else if (haveErrorIndicator) args.Graphics.DrawImage(ErrorImage, new Rectangle(0, 0, 25, 25));
			}
		}
		
		public void Draw() { Refresh(); /*Invalidates & Updates*/ }

		public void AssignFrame(Frame frame) { AssignRect(frame.ToRect()); }

		public void AssignRect(Microsoft.Xna.Framework.Rectangle rect) {
			AssignRect(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
		}

		public void AssignRect(Rectangle rect) { if (textureRegion != rect) TextureOrRegionChanged(texture, rect); }

		public void AssignTexture(Image image) { if (image != this.texture) TextureOrRegionChanged(image, textureRegion); }

		public void AssignTextureAndRect(Image img, Rectangle rect) {
			if (img != this.texture || rect != TextureRegion) {
				TextureOrRegionChanged(img, rect);
			}
		}

		private void CropTexture() {
			if (texture == null) CropFailureEventHandler(null); else {
				try { croppedTexture = GV.ImageManipulation.CropImage(texture, TextureRegion); } catch (Exception e) {
					Rectangle cropRect = TextureRegion; // Store clone of actual tile crop rectangle for reference
					int width = Texture.Width, height = Texture.Height; // Store Texture Width/Height dimensions
					bool notRangeError = cropRect.Location.X <= width && cropRect.Location.Y <= height; // In Img

					if (notRangeError) CropFailureEventHandler(e); else {
						bool horizontallyOutOfRange = cropRect.Right  > width;
						bool verticallyOutOfRange   = cropRect.Bottom > height;

						if (!horizontallyOutOfRange && !verticallyOutOfRange) CropFailureEventHandler(e); else {
							Size boundedSize = new Size(width, height); // New size for bounding region

							if (horizontallyOutOfRange) boundedSize.Width  -= cropRect.Location.X;
							if (verticallyOutOfRange)   boundedSize.Height -= cropRect.Location.Y;

							cropRect = new Rectangle(cropRect.Location, boundedSize); // New Texture Region

							try   { croppedTexture = GV.ImageManipulation.CropImage(Texture, cropRect); } 
							catch { CropFailureEventHandler(e); /* Unknown error, Image Not Cropped, */ }
						}
					}
				}
			}
		}

		private void CropFailureEventHandler(Exception e) { croppedTexture = null; }
		

		private void BuildCroppedPictureBoxRegion() {
			if (croppedTexture == null || croppedTexture.Width == 0 || croppedTexture.Height == 0) {
				BorderRects = null; CPBR_X = 0; CPBR_Y = 0; CPBR_Width = 0; CPBR_Height = 0;
			} else {
				float horizontalScale = Width  / (float)CroppedTexture.Width, width;
				float verticalScale   = Height / (float)CroppedTexture.Height, height;

				if (horizontalScale == verticalScale) { width = Width; height = Height; /* Perfect Square */ } else {
					if		  (horizontalScale > verticalScale)   { height = Height; width  = CroppedTexture.Width  * verticalScale;   } 
					else /*if (horizontalScale < verticalScale)*/ { width  = Width;  height = CroppedTexture.Height * horizontalScale; }
				}

				CPBR_X = (Width - width) / 2; CPBR_Y = (Height - height) / 2; CPBR_Width = width; CPBR_Height = height;

				#region BorderRectCreation
				RectangleF leftBorder, rightBorder; // Left and right border rectangles

				if (horizontalScale == verticalScale) leftBorder = rightBorder = Rectangle.Empty; else {
					SizeF size =    (horizontalScale > verticalScale) ? new SizeF(CPBR_X,     Height) : new SizeF(Width,       CPBR_Y);
					PointF pointB = (horizontalScale > verticalScale) ? new PointF(CPBR_X + width, 0) : new PointF(0, CPBR_Y + height);

					leftBorder  = new RectangleF(PointF.Empty, size); rightBorder = new RectangleF(pointB, size); // Set border rectangles
				}

				BorderRects = new Tuple<RectangleF, RectangleF>(leftBorder, rightBorder);
				#endregion
			}
		}

		private event Action<Image, Rectangle> TextureOrRegionChanged;

		public Image Texture { get { return texture; } set { AssignTexture(value); } }

		public Image CroppedTexture { get { return croppedTexture; } }

		public Rectangle TextureRegion { get { return textureRegion; } set { AssignRect(value); } }

		private RectangleF CroppedTextureRegion { get {
				return new RectangleF(CPBR_X, CPBR_Y, CPBR_Width, CPBR_Height);
			}
		}

		public bool DisplayBorderRectsForScaledImage { get; set; } = true;

		public int FrameX { get { return textureRegion.X; } set {
			AssignRect(new Rectangle(value, textureRegion.Y, textureRegion.Width, textureRegion.Height));
		} }

		public int FrameY { get { return textureRegion.Y; } set {
			AssignRect(new Rectangle(textureRegion.X, value, textureRegion.Width, textureRegion.Height));
		} }

		public int FrameWidth { get { return textureRegion.Width; } set {
			AssignRect(new Rectangle(textureRegion.X, textureRegion.Y, value, textureRegion.Height));
		} }

		public int FrameHeight { get { return textureRegion.Height; } set {
			AssignRect(new Rectangle(textureRegion.X, textureRegion.Y, textureRegion.Width, value));
		} }

		public Color BorderRectColor { get { return fillBrush.Color; } set { fillBrush.Color = value; } }

		public bool DrawTile { get { return drawTile; } set { drawTile = value; Draw(); } }

		private Image ErrorImage { get { return Properties.Resources.error; } }

		public bool HaveErrorIndicator { get { return haveErrorIndicator; } set { haveErrorIndicator = value; } }

		private Image texture, croppedTexture; // Textures

		private Rectangle textureRegion; // Frame Rect

		private bool drawTile = true, haveErrorIndicator=true;

		private float CPBR_X, CPBR_Y, CPBR_Width, CPBR_Height; // croppedPictureBoxRegion

		private Tuple<RectangleF, RectangleF> BorderRects;

		private SolidBrush fillBrush = new SolidBrush(Color.FromArgb(255, 0, 255));
	}
}
