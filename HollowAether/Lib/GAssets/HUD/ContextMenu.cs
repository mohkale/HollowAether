using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib.GAssets.HUD {
	/// <summary>Menu class to display text to user</summary>
	/// <remarks>
	///		COMMANDS-LIST:
	///			#COLOR  :             MGColorValue;
	///			#ROTATE :                    Value;
	///			#FX     : (None || FlipX || FlipY);
	///			#SYS    :   (CLS || PauseForInput);
	///			#WAIT   :                    Value;
	/// </remarks>
	public static partial class ContextMenu {
		enum ContextType { OK_Cancel, None };

		public static event Action                          WhenDone;
		public static event Action<bool>                    OkContextMenuComplete;
		private static event Action                         MoveToNextLine;
		private static event Action                         MoveToNextWord;
		private static event Action<ContextSpriteCharacter> AddNewCharacter;
		private static event Action                         RemoveTopLine;
		private static event Action                         ClearScreen;

		static ContextMenu() {
			GV.MonoGameImplement.textures["white"] = GV.TextureCreation.GenerateBlankTexture(Color.White);
			InitialiseEventHandlers(); // Event handlers for events which remain consistent throughout
		}

		/// <summary>Builds events and event handlers</summary>
		private static void InitialiseEventHandlers() {
			MoveToNextWord = () => { contextStringWordIndex++; contextStringCharIndex = 0; };
			MoveToNextWord += ContextMenu_MoveToNextWord_SetNewCharaSpritePosition; // handler

			RemoveTopLine  = ContextMenu_RemoveTopLine_EventHandler; // Default handler

			MoveToNextLine = ContextMenu_MoveToNextLine_SetPreviousCharaPositions; // -> Method

			ClearScreen  = () => { position = defaultPosition; charactersToDraw.Clear(); };
			ClearScreen += () => { invokeClearScreen = false; }; // Prevent handler being called

			AddNewCharacter  = (sprite) => charactersToDraw.Add(sprite); // First add new sprite                  
			AddNewCharacter += (sprite) => contextStringCharIndex++; // Move to next char in word       
			AddNewCharacter += (sprite) => position.X += sprite.Size().X; // Move to next position
			AddNewCharacter += (sprite) => charAppensionTimer = 0; // Reset timer till next chara
		}

		private static void ContextMenu_RemoveTopLine_EventHandler() {
			int currentIndex = lineBreakIndexes[0]; // Store before removal from list
			charactersToDraw.RemoveRange(0, currentIndex+1); // +1 = Ranges Are Weird
			lineBreakIndexes.RemoveAt(0); // No longer line break index for sure, so del

			if (lineBreakIndexes.Count > 0) {
				foreach (int X in Enumerable.Range(0, lineBreakIndexes.Count))
					lineBreakIndexes[X] -= currentIndex + 1; // Push backwards
			}

			if (charactersToDraw.Count > 0) InitiateCharaPushToTop();

			invokeRemoveTopLine = false; // Prevent handler being called again
		}

		private static void ContextMenu_MoveToNextLine_SetPreviousCharaPositions() {
			if (charactersToDraw.Count == 0 || charactersToDraw[0].BeingPushed) return;

			if (charactersToDraw[0].Position == defaultPosition)
				InitiateCharaPushToTop();

			if (position.Y + FontSize >= textContainerBoundary.Bottom)
				invokeRemoveTopLine = true;

			position = new Vector2(defaultPosition.X, position.Y + FontSize);
			lineBreakIndexes.Add(charactersToDraw.Count - 1); // Store index
		}

		private static void InitiateCharaPushToTop() {
			if (charactersToDraw.Count == 0) {
				position = new Vector2(defaultPosition.X, textContainerBoundary.Location.Y);
			} else {
				Vector2 newPosition = new Vector2(charactersToDraw[0].Position.X, textContainerBoundary.Location.Y);
				charactersToDraw[0].PushTo(newPosition, 0.85f); // Push character upwards, while maintaining X-position

				charactersToDraw[0].PushPack.SpriteOffset += (sprite, offset) => {
					foreach (int X in Enumerable.Range(1, charactersToDraw.Count - 1))
						charactersToDraw[X].Position += offset;

					position += offset;
				};

				charactersToDraw[0].PushPack.SpriteSet += (sprite, newPos, oldPos) => {
					foreach (int X in Enumerable.Range(1, charactersToDraw.Count - 1))
						charactersToDraw[X].Position += newPos - oldPos;

					position += newPos - oldPos;
				};
			}
		}

		private static List<int> lineBreakIndexes = new List<int>();

		private static void ContextMenu_MoveToNextWord_SetNewCharaSpritePosition() {
			if (Completed) return; // Checking next word dimensions pointless if menu finished

			String newWord = new string(contextString[contextStringWordIndex]).Split('\n')[0]; // Get new word

			float wordWidth = GV.MonoGameImplement.fonts[currentContextMenuFont].MeasureString(newWord).X;

			if (position.X + wordWidth > textContainerBoundary.Right) MoveToNextLine(); else position.X += 11;
		}

		private static void Create(ContextType contextType, String something) {
			if (Active) throw new HollowAetherException($"Cannot create context menu when it already exists");
			WhenDone = () => { }; // Clear all event handlers stored within completion event. No method, just reset.

			Active = true; // Set to active to prevent update calls to non context menu IMonoGameObject instances

			contextString = (from X in something.Split(' ') select X.ToCharArray()).ToArray();

			charactersToDraw = new List<ContextSpriteCharacter>((int)contextString.LongLength);

			Rectangle sR = spriteRect = GetDrawRect(); // Rectangle holding context menu sprite on the game display

			defaultCharaAttributes = new ContextSpriteCharacter.CharaAttributes(); // reset chara attributes
			contextStringWordIndex = 0; contextStringCharIndex = 0; // Reset index counters to initial value
			currentContextMenuFont = DEFAULT_FONT; // Assign initial font value to be used with characters
			defaultPosition = position = new Vector2(sR.X + xOffset, sR.Center.Y - (int)(0.5 * FontSize));
			charAppensionTimer = outputTimeout = 0; // Remove any leftover timing variables from previous run
			ShowOKCancelWindow = false; // Prevent window from automatically being drawn to the screen or updated
			type = contextType; // Store type of context menu to be displayed to user/player locally
			textContainerBoundary = new Rectangle(sR.X + xOffset, sR.Y + yOffset, sR.Width - (2 * xOffset), sR.Height - (2 * yOffset));
		}

		public static void CreateText(String something) { ContextMenu.Create(ContextType.None, something); }

		public static void CreateOKCancel(String something, params Action<bool>[] eventHandlers) {
			ContextMenu.Create(ContextType.OK_Cancel, something); // Call shared functionality

			OkContextMenuComplete = (ok) => { WhenDone(); Active = false; }; // Erase existing handlers

			foreach (Action<bool> handler in eventHandlers) OkContextMenuComplete += handler;

			MenuSprite = new OKMenuSprite(); // Redefined in case scale changed
		}

		/// <summary>Updates output procedure on context menu</summary>
		public static void Update() {
			if (!Active) throw new HollowAetherException($"Cannot update context menu when it doesn't exists");

			foreach (ContextSpriteCharacter chara in charactersToDraw)
				chara.Update(); // Update chara sprites. Used to implement push features

			if (invokeRemoveTopLine) RemoveTopLine(); // Invoke event handler, while by passing collection modification error

			if (invokeClearScreen) ClearScreen(); // Clears all items in context menu & re-alligns to default chara position

			if (WaitingForInput) { // If command waiting for input has been activated, wait
				if (!GV.PeripheralIO.currentControlState.Jump) // Jump button is used to continue moving context menu
					return; // No input detected from either input device, thus return & containue to wait

				WaitingForInput = false; // Get rid of input waiting command and continue outputting remaining context
			}

			int elapsedTime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;

			if (type == ContextType.OK_Cancel && ShowOKCancelWindow && !WaitForKeyReleaseBeforeAcceptingSkipInput) {
				MenuSprite.Update(true); // If menu exists, draw it
			}

			if (outputTimeout > 0) outputTimeout -= elapsedTime; else {
				if (Completed) {
					if (type != ContextType.OK_Cancel) { WhenDone(); Active = false; }
				} else if (charAppensionTimer >= CHAR_APPENSION_RATE) AppendNewChar(); else {
					charAppensionTimer += elapsedTime;
				}

				bool jumpKeyPressed  = GV.PeripheralIO.currentControlState.Jump;
				bool AButtonPressed  = GV.PeripheralIO.currentKBState.IsKeyDown(Keys.Enter);
				bool enterKeyPressed = GV.PeripheralIO.currentGPState.Buttons.A == ButtonState.Pressed;

				if (jumpKeyPressed || AButtonPressed || enterKeyPressed) {
					if (!WaitForKeyReleaseBeforeAcceptingSkipInput)
						OutputUntilEndOfCurrentSentence();
				} else if (WaitForKeyReleaseBeforeAcceptingSkipInput) {
					WaitForKeyReleaseBeforeAcceptingSkipInput = false;
				}
			}
		}

		private static void AppendNewChar() {
			if (Completed) return; // Nothing to do, just skip any increments or word addition

			char newChar = contextString[contextStringWordIndex][contextStringCharIndex];

			bool moveToNextWord = WordCompleted; // Store before new sprite addition to prevent index error

			switch (newChar) { // Allows handling of unconventional characters
				case '\n': MoveToNextLine(); contextStringCharIndex++; break;
				case '#': InterpretCommand(ReadCommand()); break;
				default:
					var sprite = new ContextSpriteCharacter(newChar, currentContextMenuFont, position, defaultCharaAttributes);
					AddNewCharacter(sprite); // Call event handler for creating and storing a new context menu sprite
					break; // By default, any non system characters are outputted to ContextMenu as sprite.
			}

			if (moveToNextWord) MoveToNextWord(); // If ready to move to next word then call event handler

			if (type == ContextType.OK_Cancel && Completed && !ShowOKCancelWindow) ShowOKCancelWindow = true;
		}

		private static void OutputUntilEndOfCurrentSentence() {
			do { AppendNewChar(); } while (
				   !Completed && !WaitingForInput // In order of most tasking bool check
				&& contextString[contextStringWordIndex][contextStringCharIndex] != '\n'
				&& !(type == ContextType.OK_Cancel && ShowOKCancelWindow)
			);

			outputTimeout = 500; // Skipped to end, take breif pause to let user removee keypress
			WaitForKeyReleaseBeforeAcceptingSkipInput = true;
		}

		public static void Draw() {
			if (!Active) throw new HollowAetherException($"Cannot draw context menu when it doesn't exists");

			GV.MonoGameImplement.InitializeSpriteBatch(false); // Draw without camera

			DrawMenu(); // Draw base sprite for menu to hold text

			if (type == ContextType.OK_Cancel && ShowOKCancelWindow)
				MenuSprite.Draw(); // Draw sprite if valid

			if (WaitingForInput) DrawCursor(); // Indicator drawn
			DrawText(); // Actually tries to draw text to screen

			GV.MonoGameImplement.SpriteBatch.End();
		}

		private static void DrawMenu() {
			Rectangle baseFrame = new Rectangle(6, 6, ContextMenuWidth, ContextMenuHeight); // Frame
			GV.MonoGameImplement.SpriteBatch.Draw(BaseTexture, spriteRect, baseFrame, Color.White);
		}

		private static void DrawCursor() {
			if (cursorTimer > cursorDrawInterval) { cursorBeingDrawn = !cursorBeingDrawn; cursorTimer = 0; } else {
				cursorTimer += GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			}

			if (cursorBeingDrawn) // If cursor is supposed to be drawn to screen draw it, else wait till then
				GV.MonoGameImplement.SpriteBatch.Draw(GV.MonoGameImplement.textures["white"], GetCursorRect(), Color.White);
		}

		private static Rectangle GetCursorRect() {
			return new Rectangle((int)position.X, (int)position.Y, CursorRectWidth, CursorRectHeight);
		}

		private static void DrawText() {
			foreach (ContextSpriteCharacter chara in charactersToDraw) chara.Draw();
		}

		private static Rectangle GetDrawRect() {
			float xOffset = 0.10f * ContextMenuWidth;
			float yOffset = 0.35f * ContextMenuHeight;

			ContextScale = Math.Min(
				GV.Variables.windowWidth / (ContextMenuWidth + (2 * xOffset)),
				GV.Variables.windowHeight / (ContextMenuHeight + yOffset)
			);

			int scaledWidth = (int)(ContextMenuWidth * ContextScale), scaledHeight = (int)(ContextMenuHeight * ContextScale);
			int scaledOffsetHeight = (int)(ContextScale * (ContextMenuHeight + yOffset)); // Accounts for offset as well

			return new Rectangle(
				(GV.Variables.windowWidth - scaledWidth) / 2,
				GV.Variables.windowHeight - scaledOffsetHeight,
				scaledWidth, scaledHeight // 
			);
		}

		/// <summary>Reads command string from context string, with format #NAME:ARG;</summary>
		private static String ReadCommand() {
			StringBuilder builder = new StringBuilder();

			try {
				char commandCurrent; // Store current char

				do {
					commandCurrent = contextString[contextStringWordIndex][++contextStringCharIndex];

					if (commandCurrent == '#') // If system attempts to create a new command regardless of previous one
						throw new HollowAetherException($"Cannot begin new command whilst previous unfinished");

					if (WordCompleted) { contextStringWordIndex++; contextStringCharIndex = 0; }

					builder.Append(commandCurrent);
				} while (commandCurrent != ';');
			} catch (Exception e) { throw new HollowAetherException($"Command begun, not finished", e); }

			contextStringCharIndex++; // Add final increment to skip command terminating character ';'.

			return builder.ToString(); // Cast builder to string and return to caller method
		}

		private static void InterpretCommand(String command) {
			command = command.TrimEnd(';'); // Remove command syntax

			String[] split = command.Split(':'); // Split args from command string

			//if (command.Length == 0) throw new HollowAetherException($"Command with no body given"); else 
			if (split.Length != 2) throw new HollowAetherException($"Invalid command string format '{command}'");

			switch (split[0].Trim().ToLower()) { // Switch with command arg
				case "color":
				case "colour":
					var property = typeof(Color).GetProperty(split[1]); // Get nullable potential color value
					if (property == null) throw new HollowAetherException($"Could not determine color '{split[1]}'");
					defaultCharaAttributes.color = (Color)property.GetValue(null, null); // Cast to appropriate color
					break;
				case "rotation":
				case "rotate":
					float? potentialValue = GV.Misc.StringToFloat(split[1]); // Try cast to floating point value

					if (potentialValue.HasValue) defaultCharaAttributes.rotation = potentialValue.Value;
					else {
						throw new HollowAetherException($"Couldn't convert '{split[1]}' to a float");
					}
					break;
				case "fx":
				case "effect":
					switch (split[1].Trim().ToLower()) {
						case "none":
							defaultCharaAttributes.effect = SpriteEffects.None;
							break;
						case "fliphorizontally":
						case "flipx":
							defaultCharaAttributes.effect = SpriteEffects.FlipHorizontally;
							break;
						case "flipvertically":
						case "flipy":
							defaultCharaAttributes.effect = SpriteEffects.FlipVertically;
							break;
						default:
							throw new HollowAetherException($"Unknown effect command '{split[1]}'");
					}
					break;
				case "system":
				case "sys":
					switch (split[1].Trim().ToLower()) {
						case "clear":
						case "cls":
							invokeClearScreen = true;
							break;
						case "pauseforinput":
						case "waitforinput":
						case "waittillkeypress":
							WaitingForInput = true;
							break;
						default:
							throw new HollowAetherException($"Unknown system command '{split[1]}'");
					}
					break;
				case "wait":
				case "timeout":
					int? span = GV.Misc.StringToInt(split[1]); // Try cast to milisecond integer value

					if (span.HasValue) outputTimeout = span.Value; else {
						throw new HollowAetherException($"Couldn't cast '{span}' to int");
					}
					break;
				case "font":
					break;
				default:
					throw new HollowAetherException($"Unknow Command '{split[0]}'");
			}
		}

		/// <summary>Whether context menu exists</summary>
		public static bool Active { get; private set; } = false;

		/// <summary>Whether all words to be outputted have been outputter</summary>
		private static bool Completed { get { return contextStringWordIndex >= contextString.Length; } }

		/// <summary>Whether the current word has been completed</summary>
		private static bool WordCompleted { get { return contextStringCharIndex + 1 >= contextString[contextStringWordIndex].Length; } }

		private static Texture2D BaseTexture { get { return GV.MonoGameImplement.textures[@"cs\textbox"]; } }

		private static ContextType type;

		private static float ContextScale;

		/// <summary>Structure to hold characters, seperated by words</summary>
		private static char[][] contextString;

		/// <summary>Indexes for values in string</summary>
		private static int contextStringWordIndex, contextStringCharIndex;

		#region TimingVars
		/// <summary>Add character to menu once every 0.05 seconds</summary>
		private const int CHAR_APPENSION_RATE = 50;

		/// <summary>Counts intervals between chara appension</summary>
		private static int charAppensionTimer = 0;

		/// <summary>Whether to draw cursor or not</summary>
		private static bool cursorBeingDrawn = false;

		/// <summary>Time for which cursor is both not drawn & drawn</summary>
		private static int cursorDrawInterval = 150;

		/// <summary>Counts interval between alternations is bool cursorBeingDrawn</summary>
		private static int cursorTimer = 0;

		/// <summary>If greater than 0, is paused until 0</summary>
		private static int outputTimeout = 0;

		/// <summary>Whether waiting for input before continuing menu output</summary>
		private static bool WaitingForInput { get; set; } = false;
		#endregion

		/// <summary>Font at a given point in time</summary>
		private static String currentContextMenuFont;

		/// <summary>Default font used by context menu</summary>
		private static readonly String DEFAULT_FONT = "base";

		private static bool WaitForKeyReleaseBeforeAcceptingSkipInput { get; set; } = false;

		private static bool ShowOKCancelWindow;

		private static bool invokeRemoveTopLine;

		private static bool invokeClearScreen;

		private static List<ContextSpriteCharacter> charactersToDraw;

		private static ContextSpriteCharacter.CharaAttributes defaultCharaAttributes;

		private static Rectangle spriteRect, textContainerBoundary;

		private static Vector2 position, defaultPosition;

		private static OKMenuSprite MenuSprite;

		private const int FontSize = 48;

		private const int CursorRectWidth = (int)(0.66 * FontSize), CursorRectHeight = (int)(0.84 * FontSize);

		private const int xOffset = 35, yOffset = 25;

		private const int ContextMenuWidth = 260, ContextMenuHeight = 36;

		private const int StretchedContextMenuWidth = ContextMenuWidth, StretchedContextMenuHeight = 60;
	}
}