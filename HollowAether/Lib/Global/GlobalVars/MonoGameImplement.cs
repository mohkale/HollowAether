#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Encryption;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
#endregion

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class MonoGameImplement {
			public static void InitializeSpriteBatch(bool useCamera = true, SpriteSortMode mode = SpriteSortMode.FrontToBack) {
				SpriteBatch.Begin(
					samplerState:	 SamplerState.PointClamp,
					sortMode:		 mode,
					blendState:		 BlendState.AlphaBlend,
					transformMatrix: (useCamera) ? camera.GetViewMatrix() : Matrix.Identity
				);
			}

			public static Player Player { get; set; }

			public static bool FullScreen {
				get { return hollowAether.IsFullScreen; } // Return value stored in game instance
				set { if (hollowAether.IsFullScreen != value) hollowAether.ToggleFullScreen(); }
			}

			public static SpriteBatch SpriteBatch { get { return GlobalVars.hollowAether.spriteBatch; } }

			public static float MilisecondsPerFrame { get { return 1000 / framesPerSecond; } }

			public static Dictionary<String, AnimationSequence> importedAnimations = new Dictionary<String, AnimationSequence>();

			public static Dictionary<String, Texture2D> textures = new Dictionary<String, Texture2D>();   // ImportedImages
			public static Dictionary<String, SpriteFont> fonts   = new Dictionary<String, SpriteFont>();  // ImportedFonts
			public static Dictionary<String, Video> videos       = new Dictionary<String, Video>();       // ImportedVideos
			public static Dictionary<String, SoundEffect> sounds = new Dictionary<String, SoundEffect>(); // ImportedSounds

			public static MonoGameObjectStore monogameObjects = new MonoGameObjectStore(); // Main object store
			public static RemovalBatch  removalBatch  = new RemovalBatch();  // Batch to remove existing monogame objects
			public static AdditionBatch additionBatch = new AdditionBatch(); // Batch to add new monogame objects

			public static GameState gameState = GameState.Home; // Stores current game state

			public static Camera2D camera;

			public static float gameZoom = 2.0f;

			public const float gravity = 1.5f * 32f; // gravitational field strength affecting all objects by default

			public static String defaultAnimationSequenceKey = "Idle"; // Sequence run by default on all sprites

			public static Type[] BlockTypes = { typeof(Block), /*typeof(AngledBlock),*/ typeof(Crusher), typeof(GravityBlock) };

			public static GameTime gameTime;

			public static float framesPerSecond = 30; // GT and FPS

			public static Background background;

			public static Dictionary<String, Background> backgrounds = new Dictionary<String, Background>();

			public static Map map { get; set; }

			public static HUD GameHUD;

			public static int money = 0;

			public static int saveIndex = -1; // Default value will throw exception if used as is

			public static int ContactDamage { get { return 3; } }

			public static String PlayerName { get; private set; } = "Quote";
		}
	}
}
