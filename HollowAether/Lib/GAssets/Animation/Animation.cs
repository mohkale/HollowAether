#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.InputOutput;
#endregion

namespace HollowAether.Lib.GAssets {
	public class Animation {
		/// <summary>Assignment constructor</summary>
		/// <param name="_position">Position of animation on screen</param>
		/// <param name="_width">Width of animation on screen</param>
		/// <param name="_height">Height of animation on screen</param>
		/// <param name="aRunning">Whether animation is running</param>
		/// <param name="_textureID">Reference to texture stored</param>
		Animation(Vector2 _position, int _width, int _height, bool aRunning, String _textureID) {
			if (!String.IsNullOrWhiteSpace(_textureID))
				TextureID = _textureID;

			position         = _position;
			animationRunning = aRunning;
			width            = _width;
			height           = _height;
		}

		/// <summary>Animation from stored sequences passed as tuples</summary>
		/// <param name="_position">Position of animation on screen</param>
		/// <param name="_width">Width of animation on screen</param>
		/// <param name="_height">Height of animation on screen</param>
		/// <param name="aRunning">Whether animation is running</param>
		/// <param name="_textureID">Reference to texture stored</param>
		/// <param name="sequences">Key = sequence name, Value = actual sequence</param>
		public Animation(Vector2 _position, int _width=32, int _height=32, bool aRunning=true, String _textureID=null, params Tuple<String, AnimationSequence>[] sequences) 
			: this(_position, _width, _height, aRunning, _textureID) {

			foreach (Tuple<String, AnimationSequence> sequence in sequences) {
				sequenceLibrary[sequence.Item1] = sequence.Item2;
			}
		}

		/// <summary>Reads all sequences known from an animation file</summary>
		/// <param name="fPath">Path to an animation file to read from</param>
		public static Dictionary<String, AnimationSequence> FromFile(String fPath) {
			if (!InputOutputManager.FileExists(fPath)) throw new AnimationFileNotFoundException(fPath);

			String _aniContents; // String to store the contents of the animation file once opened
			Dictionary<String, AnimationSequence> container = new Dictionary<String, AnimationSequence>();

			try {
				_aniContents = InputOutputManager.ReadEncryptedFile(fPath, GlobalVars.Encryption.oneTimePad).Replace("\r", "");
			} catch {
				throw new AnimationFileNotFoundException(fPath); // Couldn't read animation file, therefore throw exception
			}

			if (!InputOutput.Parsers.Parser.HeaderCheck(_aniContents, FILE_HEADER))
				throw new AnimationIncorrectHeaderException(fPath); // Map file doesn't have the correct header

			GV.Misc.RemoveComments(ref _aniContents); // remove comment notations from contents
			MatchCollection animations = Regex.Matches(_aniContents, "\\\"[^ ]+\\\"[ {]+[^}]+\\}");

			foreach (Match match in animations) {
				String regMatch = Regex.Match(match.Value, "\\\"[^ ]+\\\"").Value;
				String animName = regMatch.Substring(1, regMatch.Length - 2).Trim();

				container.Add(animName, new AnimationSequence(0) { IsImported = true });

				String contMatch = Regex.Match(match.Value,           "\\{[^}]+\\}").Value;
				String contents  = contMatch.Substring(1, contMatch.Length - 2).Trim();

				foreach (String frameLine in contents.Split('\n')) { // Frame Line
					if (!string.IsNullOrWhiteSpace(frameLine)) {
						try { container[animName].AddFrame(Frame.FromFileContents(frameLine.Trim(' ', '\t', '\r'))); }
						catch { Console.WriteLine($"Warning, Couldn't Read Animation {fPath}\\{animName}"); }
					}
				}
			}

			return container;
		}

		private static IEnumerable<Match> YieldRegexMatchCollection(MatchCollection matched) {
			foreach (Match m in matched) yield return m;
		}

		/// <summary>Initialises used animation variables</summary>
		/// <param name="_textureID">Reference to texture stored</param>
		/// <exception cref="HollowAether.Lib.Exceptions.AnimationException">If texture not set or if sequence doesn't exist</exception>
		public void Initialize(String _textureID) {
			if (String.IsNullOrWhiteSpace(_textureID))
				throw new AnimationException("Texture not set");
			else TextureID = _textureID; // Set texture ID

			if (!sequenceLibrary.Keys.Contains(sequenceKey)) // Not texture exists
				throw new AnimationException($"Animation sequence '{sequenceKey}' doesn't exist");

			currentFrame = CurrentSequence.GetFrame(false); // Set current frame, without increment
		}

		public String ToAnimationFile(String prefix="") {
			StringBuilder builder = new StringBuilder($"{FILE_HEADER}\n\n"); // Holds animation details

			foreach (var tuple in sequenceLibrary) // Key value pair
				builder.Append(tuple.Value.ToFileContents(prefix+tuple.Key)+"\n");

			return builder.ToString();
		}

		/// <summary>Updates animation by moving to next sequence</summary>
		public void Update() {
			if (!animationRunning) return; // Animation isn't running, don't update

			if (frameRunCount < currentFrame.RunCount) { frameRunCount += 1; } else {
				if (ChainAttatched) {
					if (chain.Update()) currentFrame = chain.GetFrame(); else DeleteChain();
					// Chain no longer active, so set to null so can be deleted by GC
				} else currentFrame = CurrentSequence.GetFrame();

				frameRunCount = 1; // reset run count to minimum run count
			}
		}

		/// <summary>Deletes animation chain</summary>
		public void DeleteChain() {
			ChainFinished(chain); // Event to call when chain has been completed
			chain = null; // Allocate chain for deletion by garbage collecter
			currentFrame = CurrentSequence.GetFrame(false); // Without update
		}

		/// <summary>Draws animation to canvas</summary>
		public void Draw() {
			GV.MonoGameImplement.SpriteBatch.Draw(
				texture:			  Texture, 
				destinationRectangle: SpriteRect, 
				sourceRectangle:	  GetFrame().frame, 
				color:				  Color.White * Opacity,
				layerDepth:			  layer,
				scale: new Vector2(scale),
				rotation: rotation, 
				origin: origin
			);

		}

		/// <summary>Get current frame</summary>
		/// <returns>Current frame</returns>
		private Frame GetFrame() { return currentFrame; }

		/// <summary>Add sequence to current animation instance</summary>
		/// <param name="key">Key for stored animation</param>
		/// <param name="sequence">Sequence of animation</param>
		public void AddAnimationSequence(String key, AnimationSequence sequence) {
			sequenceLibrary[key] = sequence;
		}

		/// <summary>Overlays chain above animation</summary>
		/// <param name="_chain">Chain to attatch</param>
		public void AttatchAnimationChain(AnimationChain _chain) {
			chain = _chain;
			ChainLinked(chain);
		}

		/// <summary>Checks if sequence exists</summary>
		/// <param name="sequence">Key of sequence</param>
		public bool SequenceExists(String sequence) {
			return sequenceLibrary.ContainsKey(sequence);
		}

		/// /// <summary>Checks if sequence exists</summary>
		/// <param name="sequence">Sequence instance</param>
		public bool SequenceExists(AnimationSequence sequence) {
			return sequenceLibrary.ContainsValue(sequence);
		}

		/// <summary>Sets animation sequence using sequence key from library</summary>
		/// <param name="key">Key of animation to set sequence to. Can skip check using next arg.</param>
		/// <param name="skipCheck">Doesnt bother to check if animation sequence exists, dangerous :(</param>
		/// <exception cref="HollowAether.Lib.Exceptions.AnimationException">When sequence doesn't exist</exception>
		public void SetAnimationSequence(String key, bool skipCheck=false) {
			if (sequenceKey == key) return; // use reset when setting same

			if (!skipCheck && !sequenceLibrary.Keys.Contains(key))
				throw new AnimationException($"Key: \"{key}\" not found");

			SequenceChanged(sequenceKey, key); // Event for when sequence has changed.
			
			if (CurrentSequenceExists) CurrentSequence.ResetSequence(); // Resets previous sequence
			sequenceKey = key; // Set new sequence key to over-write previous animation sequence
			currentFrame = CurrentSequence.GetFrame(false); // Assign new frame after assignment
		}

		/// <summary>Changes animation sequence</summary>
		/// <param name="sequence">Sequence to set animation to</param>
		private void SetAnimationSequence(AnimationSequence sequence) {
			String newSequenceKey = GV.CollectionManipulator.DictionaryGetKeyFromValue(sequenceLibrary, sequence);
			SequenceChanged(sequenceKey, newSequenceKey); // Event for when sequence has changed.

			if (CurrentSequenceExists) CurrentSequence.ResetSequence(); // Resets previous sequence
			sequenceKey = newSequenceKey; // Assign new animation sequnece key
			currentFrame = CurrentSequence.GetFrame(false); // Assign new frame after assignment
		}

		/// <summary>Square bracket notation for adding sequences</summary>
		/// <param name="name">Name of sequence to add to library</param>
		public AnimationSequence this[String name] {
			get { return sequenceLibrary[name]; }
			set { AddAnimationSequence(name, value); }
		}

		/// <summary>Whether chain is attatched to animation</summary>
		public bool ChainAttatched { get { return chain != null; } }

		/// <summary>Tries and returns animation texture if texture exists, else throws new exception</summary>
		/// <exception cref="HollowAether.Lib.Exceptions.AnimationException">When texture doesn't exist</exception>
		private Texture2D Texture {
			get {
				try { return GV.MonoGameImplement.textures[TextureID]; } // When exists return
				catch { throw new AnimationException($"Texture Key \"{TextureID}\" not found"); }
			}
		}

		/// <summary>Gets current animation sequence using current sequence key from sequence library</summary>
		/// <exception cref="HollowAether.Lib.Exceptions.AnimationException">When sequence doesn't exist</exception>
		public AnimationSequence CurrentSequence {
			get {
				try { return sequenceLibrary[sequenceKey]; } // When sequence exists return it to user
				catch { throw new AnimationException($"Animation with Key:'{sequenceKey}' Not Found"); }
			}
		}

		private bool CurrentSequenceExists {
			get { return sequenceLibrary.ContainsKey(sequenceKey); }
		}

		/// <summary>ID of texture used by animation</summary>
		public String TextureID { get; set; }

		/// <summary>Rectangle holding sprite when drawn onto screen</summary>
		public Rectangle SpriteRect { get { return new Rectangle(position.ToPoint(), new Point(width, height)); } }

		/// <summary>Keys of animation sequences in animation</summary>
		public String[] SequenceKeys { get { return sequenceLibrary.Keys.ToArray(); } }

		/// <summary>All sequences in animation</summary>
		public AnimationSequence[] Sequences { get { return sequenceLibrary.Values.ToArray(); } }

		/// <summary>Event to call when a sequence change has occured</summary>
		public event Action<String, String> SequenceChanged = (prev, next) => { };

		/// <summary>Event to call when chain has been added to animation</summary>
		public event Action<AnimationChain> ChainLinked     = (chain)      => { };

		/// <summary>Event to call when chain has been removed from animation</summary>
		public event Action<AnimationChain> ChainFinished   = (chain)      => { };

		public float Opacity { get; set; } = 1.0f;

		/// <summary>Chain which overlaps sequence</summary>
		private AnimationChain chain;

		/// <summary>Current frame for animation</summary>
		public Frame currentFrame;

		private int frameRunCount = 1;

		/// <summary>Position of frame on screen</summary>
		public Vector2 position;

		/// <summary>With and height of frame on screen</summary>
		public int width, height;

		/// <summary>Layer for animation</summary>
		public float layer = 0f;

		public float scale = 1f;

		public float rotation = 0f;

		public Vector2 origin = Vector2.Zero;

		/// <summary>Whether animation should update frames</summary>
		public bool animationRunning;

		/// <summary>Sequence key at start of animation</summary>
		public String sequenceKey = GV.MonoGameImplement.defaultAnimationSequenceKey;

		/// <summary>Sequences stored in animation. Accessible with String keys per sequence</summary>
		private Dictionary<String, AnimationSequence> sequenceLibrary = new Dictionary<String, AnimationSequence>();

		public const String FILE_HEADER = "ANI";
	}
}
