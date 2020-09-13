using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HollowAether.Lib.GAssets {
	public partial class IBRotatedRectangle : IBoundary {
		public IBRotatedRectangle(Rectangle rect, float initialRotation, Vector2 rotationOrigin) {
			origin = rotationOrigin; // Region from which to rotate around
			position = rect.Location.ToVector2(); // Actual unrotated rectangle
			size = rect.Size;
			Rotation = initialRotation; // Value by which to rotate rectangle

			texture = GlobalVars.TextureCreation.GenerateBlankTexture(Color.White, rect.Width, rect.Height);
		}

		public IBRotatedRectangle(Rectangle rect, float rotation) 
			: this(rect, rotation, new Vector2(rect.Width/2, rect.Height/2)) { }

		public IBRotatedRectangle(int X, int Y, int width, int height, float rotation)
			: this(new Rectangle(X, Y, width, height), rotation) { }


		public IBRotatedRectangle(int X, int Y, int width, int height, float rotation, Vector2 origin)
			: this(new Rectangle(X, Y, width, height), rotation, origin) { }

		public void Offset(float X=0, float Y=0) { position += new Vector2(X, Y); Container.Offset(X, Y); }

		public void Offset(Vector2 offset) { position += offset; Container.Offset(X, Y); }

		private static Vector2 RotateVector(Vector2 point, Vector2 origin, float rotation) {
			float originDeltaX = point.X - origin.X, originDeltaY = point.Y - origin.Y;

			return new Vector2() { // Create and return new Vector2 representing rotated point
				X = (float)(origin.X + originDeltaX * Math.Cos(rotation) - originDeltaY * Math.Sin(rotation)),
				Y = (float)(origin.Y + originDeltaY * Math.Cos(rotation) + originDeltaX * Math.Sin(rotation))
			};
		}

		public void Draw(Color color) {
			GlobalVars.MonoGameImplement.SpriteBatch.Draw(texture, position, origin: origin, rotation: rotation, color: color, layerDepth: 0.01f);
		}

		private Texture2D texture;

		public bool IsAxisCollision(IBRotatedRectangle rect, Vector2 axis) {
			int[] selfScalars = (from X in this.GenerateRotatedPoints() select GetScalar(X, axis)).ToArray(); 
			int[] rectScalars = (from X in rect.GenerateRotatedPoints() select GetScalar(X, axis)).ToArray();

			int selfMinimum = selfScalars.Min(), selfMaximum = selfScalars.Max();
			int rectMinimum = rectScalars.Min(), rectMaximum = rectScalars.Max();

			return (selfMinimum <= rectMaximum && selfMaximum >= rectMaximum)
				|| (rectMinimum <= selfMaximum && rectMaximum >= selfMaximum);
		}

		private int GetScalar(Vector2 rectCorner, Vector2 axis) {
			float numerator   = (rectCorner.X * axis.X)    + (rectCorner.Y * axis.Y);
			float denominator = (float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2);

			float quotient = numerator / denominator; // Division of numerator and denominator
			Vector2 projectedCorner = new Vector2(quotient) * axis; // Scale axis by quotient

			return (int)((axis.X * projectedCorner.X) + (axis.Y * projectedCorner.Y));
		}

		public Vector2 GetRotatedUpperLeftCorner() {
			return RotateVector(AxisAllignedTopLeftPoint, AxisAllignedTopLeftPoint + origin, rotation);
		}

		public Vector2 GetRotatedUpperRightCorner() {
			return RotateVector(AxisAllignedTopRightPoint, AxisAllignedTopRightPoint + new Vector2(-origin.X, origin.Y), rotation);
		}

		public Vector2 GetRotatedLowerLeftCorner() {
			return RotateVector(AxisAllignedBottomLeftPoint, AxisAllignedBottomLeftPoint + new Vector2(origin.X, -origin.Y), rotation);
		}

		public Vector2 GetRotatedLowerRightCorner() {
			return RotateVector(AxisAllignedBottomRightPoint, AxisAllignedBottomRightPoint + new Vector2(-origin.X, -origin.Y), rotation);
		}

		public IEnumerable<Vector2> GenerateRotatedPoints() {
			yield return RotatedTopLeftPoint;
			yield return RotatedTopRightPoint;
			yield return RotatedBottomLeftPoint;
			yield return RotatedBottomRightPoint;
		}

		public IEnumerable<Vector2> GeneratePoints() {
			yield return AxisAllignedTopLeftPoint;
			yield return AxisAllignedTopRightPoint;
			yield return AxisAllignedBottomLeftPoint;
			yield return AxisAllignedBottomRightPoint;
		}

		private Rectangle GetContainer() {
			return GlobalVars.Physics.GetContainerRectFromVectors(RotatedTopLeftPoint, RotatedTopRightPoint, RotatedBottomLeftPoint, RotatedBottomRightPoint);
		}
		
		public Vector2 position;

		public Point size;

		private float rotation;

		public readonly Vector2 origin;

		public float Rotation { get { return rotation; } set {
			rotation = value; Container = GetContainer(); // Re set container rect as well

			if (rotation > 2 * Math.PI) Rotation -= (float)(2 * Math.PI);
		} }

		public IBRectangle Container { get; private set; }

		#region AxisAllignedProperties
		public float X { get { return position.X; } }

		public float Y { get { return position.Y; } }

		public float Width { get { return position.X + size.X; } }

		public float Height { get { return position.Y + size.Y; } }

		public Vector2 AxisAllignedTopLeftPoint { get { return position; } }

		public Vector2 AxisAllignedTopRightPoint { get { return position + new Vector2(size.X, 0); } }

		public Vector2 AxisAllignedBottomLeftPoint { get { return position + new Vector2(0, size.Y); } }

		public Vector2 AxisAllignedBottomRightPoint { get { return position + new Vector2(size.X, size.Y); } }
		#endregion

		#region RotatedProperties
		public Vector2 RotatedTopLeftPoint { get { return GetRotatedUpperLeftCorner(); } }

		public Vector2 RotatedTopRightPoint { get { return GetRotatedUpperRightCorner(); } }

		public Vector2 RotatedBottomLeftPoint { get { return GetRotatedLowerLeftCorner(); } }

		public Vector2 RotatedBottomRightPoint { get { return GetRotatedLowerRightCorner(); } }
		#endregion
	}
}
