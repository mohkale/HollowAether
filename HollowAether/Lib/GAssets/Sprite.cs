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
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GAssets {
	/// <summary>Object which can be drawn to the screen and holds an animation</summary>
	public abstract partial class Sprite : IMonoGameObject {
		public Sprite() : this(new Vector2(), 0, 0, true) { }

		public Sprite(Vector2 position, int width, int height, bool animationRunning) {
			animation = new Animation(position, width, height, animationRunning);
		}

		/// <summary>Initializes sprite with base properties</summary>
		/// <param name="textureKey">Key of texture used alongside sprite</param>
		public virtual void Initialize(String textureKey) {
			BuildSequenceLibrary(); // Builds all animations used by sprite instance

			try { animation.Initialize(textureKey); } catch (Exception e) {
				throw new HollowAetherException($"Entity '{SpriteID}' Animation Initialisation Exception", e);
			}
		}

		/// <summary>Updates sprite</summary>
		public virtual void Update(bool updateAnimation) {
			elapsedTime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);
			elapsedMilitime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds; // Elapsed milleseconds

			if (updateAnimation) animation.Update(); // Updates animation when it should be, otherwise ignore
		}

		/// <summary>Update run immeadiately after first update</summary>
		public virtual void LateUpdate() { }

		/// <summary>Builds all animations used with sprite</summary>
		/// <remarks>Cannot be abstract because most work for sprite</remarks>
		protected abstract void BuildSequenceLibrary();

		/// <summary>Draws Sprite to screen</summary>
		public virtual void Draw() { animation.Draw(); }

		/// <summary>Adds animation sequence to sprite</summary>
		/// <param name="key">Key/ID of new animation sequence</param>
		/// <param name="sequence">Sequence to add to sprite-store</param>
		public void AddAnimationSequence(String key, AnimationSequence sequence) {
			animation.AddAnimationSequence(key, sequence);
		}

		/// <summary>Pushes sprite by a given vector</summary>
		/// <param name="offset">offset</param>
		public virtual void OffsetSpritePosition(Vector2 offset) {
			animation.position += offset;
		}

		public void OffsetSpritePosition(float X = 0, float Y = 0) {
			OffsetSpritePosition(new Vector2(X, Y));
		}

		public virtual void SetPosition(Vector2 nPos) {
			SetPosition(nPos.X, nPos.Y);
		}

		/// <summary>Sets position to given new position</summary>
		/// <param name="nPos">New position of sprite instance</param>
		public virtual void SetPosition(float? X = null, float? Y = null) {
			Vector2 offset = new Vector2(
				(X.HasValue) ? X.Value - Position.X : 0,
				(Y.HasValue) ? Y.Value - Position.Y : 0
			);

			OffsetSpritePosition(offset);
		}

		public Point Size { get { return new Point(Width, Height); } }

		/// <summary>Attribute holding width of sprite</summary>
		public int Width {
			get { return animation.width; }
			set { animation.width = value; }
		}

		/// <summary>Attribute holding height of sprite</summary>
		public int Height {
			get { return animation.height; }
			set { animation.height = value; }
		}

		/// <summary>Layer of sprite within game</summary>
		public float Layer {
			get { return animation.layer; }
			set { animation.layer = value; }
		}

		public float Rotation {
			get { return animation.rotation; }
			set { animation.rotation = value; }
		}

		public float Scale {
			get { return animation.scale; }
			set { animation.scale = value; }
		}

		public Vector2 Origin {
			get { return animation.origin; }
			set { animation.origin = value; }
		}

		/// <summary>Positional Rectangle of sprite</summary>
		public Rectangle SpriteRect {
			get { return animation.SpriteRect; }
		}

		/// <summary>Sprites position in the game field</summary>
		public Vector2 Position {
			get { return animation.position; }
			set { SetPosition(value); }
		}

		/// <summary>Retrieves key of current animation sequence </summary>
		protected String SequenceKey {
			get { return animation.sequenceKey; }
			set { animation.SetAnimationSequence(value); }
		}

		/// <summary>Name of texture for sprite</summary>
		public String TextureKey {
			get { return animation.TextureID; }
			private set { animation.TextureID = value; }
		}
		
		protected Texture2D Texture {
			get { return GV.MonoGameImplement.textures[TextureKey]; }
		}

		/// <summary>Animation of given sprite</summary>
		private Animation animation;

		/// <summary>Public accessor, with private modification priviliges for animation</summary>
		public Animation Animation {
			get         { return animation; }
			private set { animation = value; }
		}

		/// <summary>Time elapsed since last update</summary>
		protected float elapsedTime;

		protected int elapsedMilitime;

		/// <summary>ID of given sprite</summary>
		public String SpriteID { get; set; }
	}
}
