#region SystemImports
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

using GV = HollowAether.Lib.GlobalVars;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace HollowAether.Lib {
	/// <summary> Camera class allowing game to instantiate and use a camera instance
	/// to view game instances. Customised Derivative Camera Class Inspired by source on
	/// : http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame : </summary>
	/// <Note> Note any sprite batches begun with a Camera2D Matrix must have a default
	/// scale value of 0.1 or greater, anything lower will not be visible at all </Note>
	public class Camera2D {
		/// <summary> Base constructor to create a 2Dimensional camera </summary>
		/// <param name="zoom">Signifies maximum sprite scale zoom, value must be float above 0.1</param>
		/// <param name="type">Type of camera algorithm to follow by default :)</param>
		/// <param name="percentageRotation">Value representing the degrees of rotation to set the camera to</param>
		public Camera2D(float zoom=1.0f, float percentageRotation=0.0f) {
			Rotation = percentageRotation;
			Scale = zoom;
			smartCameraRect = new Rectangle(0, 0, target.SpriteRect.Width * 2, target.SpriteRect.Height * 3);
		}

		/// <summary> Function to create and return a ViewMatrix Instance </summary>
		/// <returns> Single Matrix value, showing dimensions of camera </returns>
		public Matrix GetViewMatrix() {
			Matrix[] container = new Matrix[5]; int counter = 0; Vector2 origin = GetScaledOrigin();

			// Constructs a Matrix Container and incrementor, to construct a ViewMatrix
			foreach (Vector2 X in new Vector2[] { -_position, -origin, origin })
				// Loops for three different potential vectors required to influence ViewMatrix
				container[counter++] = Matrix.CreateTranslation(new Vector3(X, 0f));
			// Translates and stores new vectors to Matrix Container for aggregation

			container[counter++] = Matrix.CreateRotationZ(Rotation); // adds rotation
			container[counter++] = Matrix.CreateScale(Scale, Scale, 1.0f);
			// and scale Matrices to simplify implementation of ViewMatrix through aggregation

			return container.Aggregate((Matrix a, Matrix b) => { return a * b; });
			// Agrregates and then returns ViewMatrix using local Lambda Function
		}
	
		public void Update() {
			Vector2 screenSize = new Vector2(GV.Variables.windowWidth, GV.Variables.windowHeight) / Scale;
			Vector2 center     = target.SpriteRect.Center.ToVector2() - screenSize / 2; // Screen Centered

			_position = ClampPositionToZoneBoundaries(new Vector2(center.X, center.Y - GV.Variables.windowHeight * 0.10f));
		}

		private Vector2 ClampPositionToZoneBoundaries(Vector2 pos) {
			Func<float, float, float, float> calculator = (position, scaledScreenMagnitude, zoneMagnitude) => {
				bool exceededRegion = position + scaledScreenMagnitude > zoneMagnitude;

				if (exceededRegion)    return zoneMagnitude - scaledScreenMagnitude;
				else if (position > 0) return position; // When valid positive value
				else                   return 0; // When a -ve value -> isn't allowed
			};

			Vector2 scaledScreenSize = GV.Variables.windowSize / new Vector2(Scale); // Scaled down
			Vector2 zoneSize         = GV.MonoGameImplement.map.CurrentZone.XSize; // XNA Size as V2

			bool xValid = zoneSize.X > scaledScreenSize.X, yValid = zoneSize.Y > scaledScreenSize.Y;

			return (!xValid && !yValid) ? -(scaledScreenSize - zoneSize) / 2 : new Vector2() {
				X = (xValid) ? calculator(pos.X, scaledScreenSize.X, zoneSize.X) : pos.X,
				Y = (yValid) ? calculator(pos.Y, scaledScreenSize.Y, zoneSize.Y) : pos.Y
			};
		}

		public bool ContainedInCamera(Rectangle rect) {
			Point cameraSize = new Point((int)(GV.Variables.windowWidth / Scale), (int)(GV.Variables.windowHeight / Scale));
			Rectangle cameraRect = new Rectangle(Position.ToPoint(), cameraSize); // Convert camera to viewport rectangle 

			return cameraRect.Intersects(rect); // Use existing rectangle intersection detection method to determine
		}

		public void Offset(Vector2 argVect) { _position += new Vector2(argVect.X, argVect.Y); }

		public void Offset(int X = 0, int Y = 0) { Offset(new Vector2(X, Y)); }

		public Vector2 GetCenterOfScreen() {
			return new Vector2(
				(_position.X + GV.hollowAether.GraphicsDevice.Viewport.Width)  / 2,
				(_position.Y + GV.hollowAether.GraphicsDevice.Viewport.Height) / 2
			);
		}

		public Vector2 GetScaledOrigin() { return GetCenterOfScreen() / Scale; }

		float _scale, _rotation;

		Vector2 _position;

		Rectangle smartCameraRect;
		
		public float Scale { get { return _scale; } set { _scale = value; } }

		public float Rotation { get { return _rotation; } set { _rotation = value; } }

		public Vector2 Position { get { return _position; } private set { _position = value; } }

		IMonoGameObject target { get { return GV.MonoGameImplement.Player; } }
	}
}

