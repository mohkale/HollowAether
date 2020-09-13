/*#region SystemImports
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
		/// <summary>Camera behaviour defintions</summary>
		/// Fixed keeps camera in current position until behaviour is changed
		/// Centred keeps camera centred on a given target monogame object
		/// Forward makes camera face directly in front of given target
		/// Smart has it's own life. Don't bother trying to understand it
		/// <remarks>http://yuanworks.com/little-ninja-dev-smart-camera-movement/</remarks>
		public enum CameraType { Fixed, Centred, Forward, Smart }

		/// <summary>Class to hold relevent data for camera transition</summary>
		class TransitionArgs {
			/// <summary>Defualt constructor</summary>
			/// <param name="pos">Position to transition to</param>
			/// <param name="time">Time over wich to transition</param>
			public TransitionArgs(Vector2 pos, float time) {
				target = pos;
				timing = time;
				timeCounter = 0;
			}

			/// <summary>Updates transition and gets new position</summary>
			/// <param name="position">Current position of camera</param>
			/// <returns>New position for camera</returns>
			public Vector2 Update(Vector2 position) {
				timeCounter += GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
				return Vector2.Lerp(position, target, timeCounter / timing);
			}

			/// <summary>Checks whether rounded values for given vectors match</summary>
			/// <param name="position">Vector value to check equality with</param>
			/// <returns>Whether rounded values for two vectors are the same</returns>
			public bool EqualsRoundedTarget(Vector2 position) {
				return Math.Round(position.X) == Math.Round(target.X) && Math.Round(position.Y) == Math.Round(target.Y);
			}

			public Vector2 target;
			float timing;
			float timeCounter;
		}

		/// <summary> Base constructor to create a 2Dimensional camera </summary>
		/// <param name="zoom">Signifies maximum sprite scale zoom, value must be float above 0.1</param>
		/// <param name="type">Type of camera algorithm to follow by default :)</param>
		/// <param name="percentageRotation">Value representing the degrees of rotation to set the camera to</param>
		public Camera2D(CameraType type=CameraType.Centred, float zoom=1.0f, float percentageRotation=0.0f) {
			Rotation = percentageRotation;
			Scale = zoom;
			cameraType = type;
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

		public void Draw() {
			GV.MonoGameImplement.InitializeSpriteBatch();
			//GV.DrawMethods.DrawRectangleFrame(smartCameraRect, Color.Green);
			GV.MonoGameImplement.SpriteBatch.End();
		}

		public void Update() {
			Vector2 newCameraPosition; // Variable to hold new camera position before clamping
			//ZoneBoundaries boundary = GV.MonoGameImplement.map.currentZone.zoneBoundaries;

			if (transitionArgs != null) {
				newCameraPosition = transitionArgs.Update(_position);

				if (transitionArgs.EqualsRoundedTarget(newCameraPosition)) {
					newCameraPosition = transitionArgs.target;
					StopTransition(); // Stop running transition
				}
			} else {
				switch (cameraType) {
					case CameraType.Centred:
						newCameraPosition = GetCentreVect();
						_position = newCameraPosition;
						break;
					case CameraType.Forward:
						newCameraPosition = GetCentreVect() + forwardOffset;
						break;
					case CameraType.Smart:
						newCameraPosition = SmartUpdate();
						break;
					case CameraType.Fixed:
					default:
						return; // Fixed so nothing to do. 
				}
			}

			/*if (!clampToZoneBoundaries) { _position = newCameraPosition; } else {
				_position.X = GV.BasicMath.Clamp(newCameraPosition.X, boundary.minX, boundary.maxX);
				_position.Y = GV.BasicMath.Clamp(newCameraPosition.Y, boundary.minY, boundary.maxY);
			}
		}

		public bool ContainedInCamera(Rectangle rect) {
			Point cameraSize = new Point((int)(GV.Variables.windowWidth / Scale), (int)(GV.Variables.windowHeight / Scale));
			Rectangle cameraRect = new Rectangle(Position.ToPoint(), cameraSize); // Convert camera to viewport rectangle 

			return cameraRect.Intersects(rect); // Use existing rectangle intersection detection method to determine
		}

		private Vector2 GetCentreVect() {
			return new Vector2(target.Position.X - GetScaledOrigin().X, target.Position.Y - GetScaledOrigin().Y);
		}

		private Vector2 SmartUpdate() {
			Vector2 newPosition = smartCameraRect.Location.ToVector2(); // default = current

			if (!smartCameraRect.Contains(target.SpriteRect)) {
				if (target.SpriteRect.Left <= smartCameraRect.Left) {
					smartCameraRect.X = target.SpriteRect.X; 
				}

				if (target.SpriteRect.Right >= smartCameraRect.Right) {
					smartCameraRect.X = target.SpriteRect.X - smartCameraRect.Width + target.SpriteRect.Width;
				}

				if (target.SpriteRect.Top <= smartCameraRect.Top) {
					smartCameraRect.Y = target.SpriteRect.Y;
				}

				if (target.SpriteRect.Bottom >= smartCameraRect.Bottom) {
					smartCameraRect.Y = target.SpriteRect.Y - smartCameraRect.Height + target.SpriteRect.Height;
				}
			}

			return newPosition - SmartCameraOffset;
		}

		public void Offset(Vector2 argVect) {
			_position += new Vector2(argVect.X, argVect.Y);
		}

		public void Offset(int X = 0, int Y = 0) {
			Offset(new Vector2(X, Y));
		}

		public Vector2 GetCenterOfScreen() {
			return new Vector2(
				(_position.X + GV.hollowAether.GraphicsDevice.Viewport.Width) / 2,
				(_position.Y + GV.hollowAether.GraphicsDevice.Viewport.Height) / 2
			);
		}

		public Vector2 GetScaledOrigin() {
			return GetCenterOfScreen() / Scale;
		}

		public void ChangeCameraTarget() {

		}

		public void StartTransition(Vector2 position, float time) {
			transitionArgs = new TransitionArgs(position, time);
		}

		public void StartTransition(float X, float Y, float time) {
			StartTransition(new Vector2(X, Y), time);
		}

		public void StopTransition() {
			transitionArgs = null;
		}

		public event Action ZoomChanged = () => { };

		CameraType cameraType;
		float _scale, _rotation;
		Vector2 _position;
		TransitionArgs transitionArgs;
		Rectangle smartCameraRect;
		public Vector2 forwardOffset = new Vector2(64, 0);

		public bool ClampToZoneBoundaries { get; set; } = true;
		public float Scale { get { return _scale; } set { _scale = value; ZoomChanged(); } }
		public float Rotation { get { return _rotation; } set { _rotation = value; } }
		public Vector2 Position { get { return _position; } private set { _position = value; } }

		private Vector2 SmartCameraOffset {
			get {
				return new Vector2(
					((GV.Variables.windowWidth * 0.5f) - smartCameraRect.Width) / (2 * Scale), 
					(GV.Variables.windowHeight - smartCameraRect.Height) / (2 * Scale)
				);
			}
		}

		IMonoGameObject target { get { return GV.MonoGameImplement.Player; } }
	}
}

*/