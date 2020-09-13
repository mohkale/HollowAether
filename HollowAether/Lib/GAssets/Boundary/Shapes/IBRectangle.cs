#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib.GAssets {
	public partial struct IBRectangle : IEquatable<IBRectangle>, IBoundary {
		#region builtin
		private static IBRectangle emptyRectangle = new IBRectangle();

		public float X;
		public float Y;
		public float Width;
		public float Height;

		public static IBRectangle Empty {
			get { return emptyRectangle; }
		}

		public float Left {
			get { return this.X; }
		}

		public float Right {
			get { return (this.X + this.Width); }
		}

		public float Top {
			get { return this.Y; }
		}

		public float Bottom {
			get { return (this.Y + this.Height); }
		}

		public IBRectangle(float x, float y, float width, float height) {
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public static bool operator ==(IBRectangle a, IBRectangle b) {
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}

		public bool Contains(int x, int y) {
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}

		public bool Contains(Vector2 value) {
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public bool Contains(Point value) {
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}

		public bool Contains(IBRectangle value) {
			return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}

		public static bool operator !=(IBRectangle a, IBRectangle b) {
			return !(a == b);
		}

		private void Offset(Point offset) {
			X += offset.X;
			Y += offset.Y;
		}

		private void Offset(int offsetX, int offsetY) {
			X += offsetX;
			Y += offsetY;
		}

		public Vector2 Center {
			get {
				return new Vector2((this.X + this.Width) / 2, (this.Y + this.Height) / 2);
			}
		}

		public void Inflate(int horizontalValue, int verticalValue) {
			X -= horizontalValue;
			Y -= verticalValue;
			Width += horizontalValue * 2;
			Height += verticalValue * 2;
		}

		public bool IsEmpty {
			get {
				return ((((this.Width == 0) && (this.Height == 0)) && (this.X == 0)) && (this.Y == 0));
			}
		}

		public bool Equals(IBRectangle other) {
			return this == other;
		}

		public override bool Equals(object obj) {
			return (obj is IBRectangle) ? this == ((IBRectangle)obj) : false;
		}

		public override string ToString() {
			return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
		}

		public override int GetHashCode() {
			return ((int)this.X ^ (int)this.Y ^ (int)this.Width ^ (int)this.Height);
		}

		public bool Intersects(IBRectangle r2) {
			return !(r2.Left > Right
					 || r2.Right < Left
					 || r2.Top > Bottom
					 || r2.Bottom < Top
					);

		}

		public void Intersects(ref IBRectangle value, out bool result) {
			result = !(value.Left > Right
					 || value.Right < Left
					 || value.Top > Bottom
					 || value.Bottom < Top
					);

		}

		/// <include file="doc\Rectangle.uex" path="docs/doc[@for="Rectangle.Union"]/*"> 
		/// <devdoc>
		///    <para>
		///       Creates a rectangle that represents the union between a and
		///       b. 
		///    </para>
		/// </devdoc> 
		public static IBRectangle Union(IBRectangle a, IBRectangle b) {
			float x1 = Math.Min(a.X, b.X);
			float x2 = Math.Max(a.X + a.Width, b.X + b.Width);
			float y1 = Math.Min(a.Y, b.Y);
			float y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

			return new IBRectangle(x1, y1, x2 - x1, y2 - y1);
		}
		#endregion

		public IBRectangle(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }

		public IBRectangle(Vector2 topLeft, Vector2 bottomRight) : this(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y) { }

		public void Offset(Vector2 vect) {
			X += vect.X;
			Y += vect.Y;
		}

		public void Offset(float X = 0, float Y = 0) {
			Offset(new Vector2(X, Y));
		}

		public static implicit operator Rectangle(IBRectangle self) {
			return new Rectangle((int)self.X, (int)self.Y, (int)self.Width, (int)self.Height);
		}

		public static implicit operator IBRectangle(Rectangle self) {
			return new IBRectangle(self.X, self.Y, self.Width, self.Height);
		}

		public IBRectangle Container { get { return this; } }
	}
}
