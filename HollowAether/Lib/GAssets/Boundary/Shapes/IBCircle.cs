using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowAether.Lib.GAssets {
	public partial struct IBCircle : IBoundary {
		static IBCircle() {
			circleFrame = GlobalVars.ImageManipulation.CreateCircleTexture(frameRadius, Color.White);
		}

		public IBCircle(Vector2 vect, float _radius) : this() {
			origin = vect; radius = _radius;
		}

		public IBCircle(float _X, float _Y, float _radius) {
			origin = new Vector2(_X, _Y); radius = _radius;
		}

		public void Offset(Vector2 vect) {
			origin += vect;
		}

		public void Offset(float X, float Y) {
			Offset(new Vector2(X, Y));
		}

		public void Draw(Color color) {
			GlobalVars.MonoGameImplement.SpriteBatch.Draw(circleFrame, Container, color);
		}

		public IBRectangle GetContainerRect() {
			Vector2 topLeft = new Vector2(X - radius, Y - radius);
			Vector2 bottomRight = new Vector2(X + radius, Y + radius) - topLeft;
			
			return new IBRectangle(topLeft, bottomRight);
		}

		public float X { get { return origin.X; } }

		public float Y { get { return origin.Y; } }

		public IBRectangle Container { get { return GetContainerRect(); } }

		private const int frameRadius = 128;
		private static Texture2D circleFrame;

		public static int counter = 0;

		public Vector2 origin;
		public float radius;
	}
}
