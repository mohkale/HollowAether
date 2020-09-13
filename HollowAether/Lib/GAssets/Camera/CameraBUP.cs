#region SystemImports
using System.Linq;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

/*namespace HollowAether.Lib {
	/// <summary> Camera class allowing game to instantiate and use a camera instance
	/// to view game instances. Customised Derivative Camera Class Inspired by source on
	/// : http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame : </summary>
	/// <Note> Note any sprite batches begun with a Camera2D Matrix must have a default
	/// scale value of 0.1 or greater, anything lower will not be visible at all </Note>
	public class Camera2D {
		public enum CameraType {
			Fixed, // Camera stays in a given position
			Centered, // Attatches to player and follows him
			Forward, // Looks slightly forward in the players facing direction
			Smart // Specific following algorithm
		}

		/// <summary> Base constructor to create a 2Dimensional camera </summary>
		/// <param name="vp">Viewport base to construct camera onto, and define camera base values</param>
		/// <param name="zoom">Signifies maximum sprite scale zoom, value must be float above 0.1</param>
		/// <param name="rotation">Value representing the degrees of rotation to set the camera to</param>
		public Camera2D(CameraType type = CameraType.Centered, float zoom = 1.0f, float rotation = 0.0f) {
			this.rotationFactor = rotation;
			this.scale = zoom;
		}

		/// <summary> Function to create and return a ViewMatrix Instance </summary>
		/// <returns> Single Matrix value, showing dimensions of camera </returns>
		public Matrix GetViewMatrix() {
			Matrix[] container = new Matrix[5]; int counter = 0;
			// Constructs a Matrix Container and incrementor, to construct a ViewMatrix
			foreach (Vector2 X in new Vector2[] { -this.position, -this.origin, this.origin })
				// Loops for three different potential vectors required to influence ViewMatrix
				container[counter++] = Matrix.CreateTranslation(new Vector3(X, 0f));
			// Translates and stores new vectors to Matrix Container for aggregation

			container[counter++] = Matrix.CreateRotationZ(this.rotationFactor); // adds rotation
			container[counter++] = Matrix.CreateScale(this.scale, this.scale, 1.0f);
			// and scale Matrices to simplify implementation of ViewMatrix through aggregation

			return container.Aggregate((Matrix a, Matrix b) => { return a * b; });
			// Agrregates and then returns ViewMatrix using local Lambda Function
		}

		public void Update() {
			int displacement = 5;

			if (GlobalVars.PeripheralIO.currentKBState.IsKeyDown(Keys.D)) {
				Offset(displacement);
			} if (GlobalVars.PeripheralIO.currentKBState.IsKeyDown(Keys.W)) {
				Offset(Y: -displacement);
			} if (GlobalVars.PeripheralIO.currentKBState.IsKeyDown(Keys.A)) {
				Offset(-displacement);
			} if (GlobalVars.PeripheralIO.currentKBState.IsKeyDown(Keys.S)) {
				Offset(Y: displacement);
			}
		} // Lerp to player position here

		/// <summary> Recommended way to alter camera position
		/// a positive X value moves camera to the right, a negative X value moves to the left
		/// a positive Y value moves camera upwards, a negative Y value camera downwards </summary>
		/// <param name="argVect">Vector representing what value to offset camera by</param>
		public void Offset(Vector2 argVect) {
			this.position += new Vector2(argVect.X, argVect.Y);
			//this.origin += new Vector2(-argVect.X, -argVect.Y);
			// <Note>X Axis flipped to help resemble math graphs</Note>
		}

		/// <summary> Recommended way to alter camera position </summary>
		/// <param name="X">a positive X value moves camera to the right, a negative X value moves to the left</param>
		/// <param name="Y">a positive Y value moves camera upwards, a negative Y value camera downwards</param>
		public void Offset(int X = 0, int Y = 0) {
			this.Offset(new Vector2(X, Y));
			// Calls Vector based Offset Overload
		}

		#region Attributes
		private Vector2 _position = new Vector2();
		#endregion

		#region PublicAttributes
		public float rotationFactor, scale;
		#endregion

		#region Properties
		public Matrix Transform { get { return GetViewMatrix(); } }
		public Vector2 origin { get { return screenCenter / scale; } }
		private float viewportWidth { get { return viewport.Width; } }
		private float viewportHeight { get { return viewport.Height; } }
		public Vector2 position { get { return _position; } set { _position = value; } }
		private Viewport viewport { get { return GlobalVars.hollowAether.GraphicsDevice.Viewport; } }
		public Vector2 screenCenter { get { return new Vector2((position.X + viewport.Width) / 2, (position.Y + viewport.Height) / 2); } }
		public Vector2 target { get { return GlobalVars.MonoGameImplement.player.boundary.Container.Center.ToVector2(); } } // attatch to player position
		#endregion
	}
}
*/
