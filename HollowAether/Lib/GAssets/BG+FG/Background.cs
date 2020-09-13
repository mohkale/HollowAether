using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HollowAether.Lib.GAssets {
	public class Background : List<BackgroundLayer>, IGeneralUpdateable, IDrawable {
		public Background() : base() { }

		public Background(int capacity) : base(capacity) { }

		public Background(IEnumerable<BackgroundLayer> collection) : base(collection) { }

		public void Update() {
			foreach (BackgroundLayer layer in this) {
				if (layer is IGeneralUpdateable)
					(layer as IGeneralUpdateable).Update();
			}
		}

		public void Draw() {
			GV.MonoGameImplement.InitializeSpriteBatch(false);

			foreach (BackgroundLayer layer in this)
				layer.Draw(generalLayer);

			GV.MonoGameImplement.SpriteBatch.End();
		}

		public List<BackgroundLayer> layers;
		public static float generalLayer = 0.15f;
	}

	public class BackgroundLayer {
		public BackgroundLayer(String textureKey, Frame frame, float relativeLayer = 1f) {
			this.textureKey = textureKey;
			this.relativeLayer = relativeLayer;
			this.frame = frame;

			Texture2D texture = Texture; // Store
			size = frame.ToRect().Size;
		}

		public BackgroundLayer(String textureKey, Frame frame, int width, int height, float relativeLayer = 1f) 
			: this(textureKey, frame, relativeLayer) { size = new Point(width, height);	}

		public virtual void Draw(float rootLayer) {
			Rectangle region = ScreenRegion; // Store screen region instance locally
			Texture2D backgroundTexture = GV.MonoGameImplement.textures[textureKey];

			DrawFromPosition(offset, rootLayer, backgroundTexture); // Draw from initial position

			if (!RepeatHorizontally && !RepeatVertically) DrawFromPosition(offset, rootLayer, backgroundTexture); else {
				if (RepeatHorizontally && RepeatVertically) {
					foreach (int direction in new int[] { +1, -1 }) {
						foreach (Vector2 yPos in YieldVerticalSpritePositions(offset, size.Y, direction)) {
							DrawRow(backgroundTexture, yPos.Y, rootLayer); // For each vertical layer segment
						}
					}
				} else if (RepeatHorizontally) {
					DrawRow(backgroundTexture, offset.Y, rootLayer);
				} else if (RepeatVertically) {
					foreach (int direction in new int[] { +1, -1 }) {
						foreach (Vector2 newPos in YieldVerticalSpritePositions(offset, size.X, direction)) {
							DrawFromPosition(newPos, rootLayer, backgroundTexture);
						}
					}
				}
			}
		}

		private void DrawFromPosition(Vector2 pos, float rootLayer, Texture2D texture) {
			GV.MonoGameImplement.SpriteBatch.Draw(
				texture,
				//position			 : pos,
				layerDepth			 : GetAbsoluteLayer(this, rootLayer),
				sourceRectangle		 : frame.ToRect(),
				destinationRectangle : new Rectangle((pos).ToPoint(), size),
				color				 : Color.White
			);
		}

		#region RepeatDrawers
		private void DrawRow(Texture2D texture, float yPosition, float rootLayer) {
			Vector2 position = new Vector2(offset.X, yPosition); // start point

			foreach (int direction in new int[] { +1, -1 }) {
				foreach (Vector2 newPos in YieldHorizontalSpritePositions(position, size.X, direction)) {
					DrawFromPosition(newPos, rootLayer, texture);
				}
			}
		}

		#region PositionCalculators
		protected static IEnumerable<float> YieldSpritePositionVariables(float current, int increment, int direction, int maxVal) {
			while ((direction > 0 && current < maxVal) || (direction < 0 && current > 0)) {
				yield return current; current += direction * increment;  // Yield sequential layer positiona
			}

			yield return current; // Final yield to account for loop ending before completion
		}

		protected static IEnumerable<Vector2> YieldHorizontalSpritePositions(Vector2 current, int width, int direction) {
			IEnumerable<float> iter = YieldSpritePositionVariables(current.X, width, direction, GV.Variables.windowWidth);
			return (from X in iter select new Vector2(X, current.Y)); // Constant Y Value, yield alternating horizontal X values
		}

		protected static IEnumerable<Vector2> YieldVerticalSpritePositions(Vector2 current, int height, int direction) {
			IEnumerable<float> iter = YieldSpritePositionVariables(current.Y, height, direction, GV.Variables.windowHeight);
			return (from Y in iter select new Vector2(current.X, Y)); // Constant X Value, yield alternating vertical Y values
		}
		#endregion

		#endregion

		private Rectangle GetEncompassingScreenRegion() {
			return new Rectangle(offset.ToPoint(), new Point((int)(GV.Variables.windowWidth), (int)(GV.Variables.windowHeight)));
		}

		public static float GetAbsoluteLayer(BackgroundLayer self, float rootLayer) {
			return (float)(rootLayer + (Math.Pow(10, -3) * self.relativeLayer));
		}

		protected Rectangle ScreenRegion { get { return GetEncompassingScreenRegion(); } }

		public Texture2D Texture { get { return GV.MonoGameImplement.textures[textureKey]; } }

		public bool RepeatHorizontally { get; set; } = false;

		public bool RepeatVertically { get; set; } = false;

		protected Frame frame;

		protected String textureKey;

		protected Vector2 offset;

		public Vector2 Offset { get { return offset; } set { offset = value; } }

		public Point size;

		public float relativeLayer;
	}

	public class MovingBackgroundLayer : BackgroundLayer, IGeneralUpdateable {
		public MovingBackgroundLayer(String textureKey, Frame frame, float relativeLayer) : base(textureKey, frame, relativeLayer) { }

		public MovingBackgroundLayer(String textureKey, Frame frame, int width, int height, float relativeLayer)
			: base(textureKey, frame, width, height, relativeLayer) { }

		public void Update() {
			if (Velocity.X != 0 && (offset.X > GV.Variables.windowWidth || offset.X < 0))
				offset.X = size.X - Math.Abs(YieldSpritePositionVariables(offset.X, size.X, -1, GV.Variables.windowWidth).Last());

			if (Velocity.Y != 0 && (offset.Y > GV.Variables.windowHeight || offset.Y < 0))
				offset.Y = size.Y - Math.Abs(YieldSpritePositionVariables(offset.Y, size.Y, -1, GV.Variables.windowHeight).Last());

			offset += Velocity * GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);
		}

		public Vector2 Velocity { get; set; } = Vector2.Zero;
	}

	public class StretchedBackgroundLayer : BackgroundLayer {
		public StretchedBackgroundLayer(String textureKey, Frame frame, float relativeLayer) : base(textureKey, frame, relativeLayer) { }

		public override void Draw(float rootLayer) {
			GV.MonoGameImplement.SpriteBatch.Draw(
				Texture,
				//position:			   offset + GV.MonoGameImplement.camera.Position,
				layerDepth			 : GetAbsoluteLayer(this, rootLayer),
				sourceRectangle		 : frame.ToRect(),
				destinationRectangle : new Rectangle(Point.Zero, ScreenRegion.Size),
				color				 : Color.White
			);
		}
	}

	public class ScaledBackgroundLayer : BackgroundLayer {
		public ScaledBackgroundLayer(String textureKey, Frame frame, float relativeLayer) : base(textureKey, frame, relativeLayer) {
			unscaledSize = size;
		}

		public float Scale { get { return scale; } set { scale = value; size = (unscaledSize.ToVector2() * value).ToPoint(); } }

		private float scale = 1;

		private Point unscaledSize;
	}

	public class MovingScaledBackgroundLayer : ScaledBackgroundLayer, IGeneralUpdateable {
		public MovingScaledBackgroundLayer(String textureKey, Frame frame, float relativeLayer) : base(textureKey, frame, relativeLayer) { }

		public void Update() {
			if (Velocity.X != 0 && (offset.X > GV.Variables.windowWidth || offset.X < 0))
				offset.X = size.X - Math.Abs(YieldSpritePositionVariables(offset.X, size.X, -1, GV.Variables.windowWidth).Last());

			if (Velocity.Y != 0 && (offset.Y > GV.Variables.windowHeight || offset.Y < 0))
				offset.Y = size.Y - Math.Abs(YieldSpritePositionVariables(offset.Y, size.Y, -1, GV.Variables.windowHeight).Last());

			offset += Velocity * GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);
		}

		public Vector2 Velocity { get; set; } = Vector2.Zero;
	}

	/*public class ParallaxingBackgroundLayer : BackgroundLayer {

	}*/
}