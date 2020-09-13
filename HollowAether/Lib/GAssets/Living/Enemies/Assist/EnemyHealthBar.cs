using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public class EnemyHealthBar {
		static EnemyHealthBar() { healthTexture = GV.TextureCreation.GenerateBlankTexture(Color.Red, 1, 1); }

		public EnemyHealthBar(Vector2 position, int width, int height=DEFAULT_HEIGHT, uint healthSpan=1) {
			Position  = position;
			Width     = width;
			Height    = height;
			Health    = (int)healthSpan;
			MaxHealth = healthSpan;
		}

		public void Draw() {
			GV.MonoGameImplement.SpriteBatch.Draw(
				healthTexture, new Rectangle(Position.ToPoint(), new Point(AdjustedWidth, Height)), Color.White
			);
		}

		public void TakeDamage(int amount) { Health = GV.BasicMath.Clamp<int>(Health-amount, 0, (int)MaxHealth); }

		public void RegainHealth(int amount) { Health = GV.BasicMath.Clamp<int>(Health + amount, 0, (int)MaxHealth); }

		private int GetWidthFromHealthSpan() { return (int)(Width * Health / MaxHealth); }

		public void Offset(Vector2 offset) { Position += offset; }

		public Vector2 Position { get; set; }

		public int Width { get; protected set; }

		public int Height { get; protected set; }
	
		public int Health { get; protected set; }

		public int AdjustedWidth { get { return GetWidthFromHealthSpan(); } }

		public uint MaxHealth { get; private set; }

		private static Texture2D healthTexture;

		public const int DEFAULT_HEIGHT = 5;
	}
}
