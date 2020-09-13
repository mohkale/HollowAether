using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.GAssets.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	public class HUD {
		public HUD(int heartCount = 16) {
			InitialiseHearts(heartCount);
			InitialisePointDisplay();
			InitialiseButtons();
		}

		private void InitialiseHearts(int heartCount) {
			Vector2 positionVect = standardCornerOffset + new Vector2(0, PointDisplay.HEIGHT + 15);

			foreach (int X in Enumerable.Range(0, heartCount)) {
				if (X % MAX_X_SPAN == 0 && X != 0) // If maximum amount of hearts on given row has been reached 
					positionVect = new Vector2(standardCornerOffset.X, positionVect.Y + HEART_Y_OFFSET + HeartSprite.DEFAULT_HEIGHT);

				hearts.Add(new HeartSprite(positionVect)); // Create new heart
				positionVect.X += HeartSprite.DEFAULT_WIDTH + HEART_X_OFFSET;
			}
		}

		private void InitialisePointDisplay() {
			Vector2 position; // Position for point display to be set to initially

			if (!GamePad.GetState(PlayerIndex.One).IsConnected) position = standardCornerOffset; //+ new Vector2(0, 30);
			else position = standardCornerOffset /*+ new Vector2(0, 30)*/ + HEART_OFFSET_WITH_BUTTONS;

			pointDisplay = new PointDisplay(position); // Create new display
		}

		private void InitialiseButtons() {
			int iconPush = -3; // Value by which icons should be offset by from there regular arrangement

			topButtonPosition    = standardCornerOffset + new Vector2(GamePadButtonIcon.SPRITE_WIDTH + iconPush, 0);
			bottomButtonPosition = topButtonPosition    + new Vector2(0, 2 * (GamePadButtonIcon.SPRITE_HEIGHT + iconPush));
			leftButtonPosition   = standardCornerOffset + new Vector2(0, GamePadButtonIcon.SPRITE_HEIGHT + iconPush);
			rightButtonPosition  = leftButtonPosition   + new Vector2(2 * (GamePadButtonIcon.SPRITE_WIDTH + iconPush), 0);

			dash     = new GamePadButtonIcon(leftButtonPosition,   GamePadButtonIcon.GamePadButton.X, defaultTheme);
			jump     = new GamePadButtonIcon(bottomButtonPosition, GamePadButtonIcon.GamePadButton.A, defaultTheme);
			interact = new GamePadButtonIcon(topButtonPosition,    GamePadButtonIcon.GamePadButton.Y, defaultTheme);
			other    = new GamePadButtonIcon(rightButtonPosition,  GamePadButtonIcon.GamePadButton.B, defaultTheme);
			
			if (GamePad.GetState(PlayerIndex.One).IsConnected) { // Push initially without transition
				gamepadButtonsDisplayed = true; // Display automatically

				foreach (HeartSprite sprite in hearts)
					sprite.OffsetSpritePosition(HEART_OFFSET_WITH_BUTTONS);
			} else {
				Vector2 center = ButtonCenterPosition; // Store locally

				foreach (GamePadButtonIcon icon in GamePadIcons) {
					icon.SetPosition(center);
				}
			}
		}

		public void Update(bool updateAnimation) {
			foreach (HeartSprite heart in hearts) heart.Update(updateAnimation);

			if (!gamepadButtonsDisplayed && GV.PeripheralIO.gamePadConnected ||
				 gamepadButtonsDisplayed && !GV.PeripheralIO.gamePadConnected)
				ToggleControllerButtons();

			if (gamepadButtonsDisplayed) UpdateControllerIcons(updateAnimation);

			if (GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.R))
				ToggleControllerButtons();

			pointDisplay.Update(updateAnimation); // Update points display
		}

		public void SetHeartCount(int heartCount) {
			if (heartCount != HeartsCount) {
				if (heartCount > HeartsCount) { foreach (int X in Enumerable.Range(0, heartCount  - HeartsCount)) AddHeart();   }
				else                          { foreach (int X in Enumerable.Range(0, HeartsCount - heartCount)) RemoveHeart(); }
			}
		}

		public void SetHealth(int health) {
			int currentHealth = Health; // Store to prevent repeated calculation

			if (health != currentHealth) {
				if (health > currentHealth)		  AddHealth(health - currentHealth);
				else /* health < currentHealth */ TakeDamage(currentHealth - health);
			}
		}

		public void Draw() {
			GV.MonoGameImplement.InitializeSpriteBatch(false, SpriteSortMode.Deferred);

			foreach (HeartSprite heart in hearts) heart.Draw();
			if (gamepadButtonsDisplayed) DrawControllerIcons();
			pointDisplay.Draw(); // Display user points to screen
			DrawPoints(); // Draw monies and burst points to screen

			GV.MonoGameImplement.SpriteBatch.End();
		}

		private void DrawPoints() {
			SpriteFont font = GV.MonoGameImplement.fonts["robotaur"]; // Get sprite font
			String bp = GV.MonoGameImplement.Player.burstPoints.ToString().PadLeft(3, '0');
			String money = GV.MonoGameImplement.money.ToString().PadLeft(3, '0');
			String text = $"BP: {bp}, Money: {money}"; // String to output to screen

			Vector2 pointsStringSize = font.MeasureString(text); // Get dimensions of string
			Vector2 position = new Vector2(hearts[0].Position.X, hearts.Last().Position.Y + pointsStringSize.Y + 15);

			GV.MonoGameImplement.SpriteBatch.DrawString(font, text, position, Color.White);
		}

		private void ToggleControllerButtons() {
			if (gamepadButtonsDisplayed) HideControllerButtons();
			else						 DisplayControllerButtons();
		}

		private void DisplayControllerButtons() {
			if (gamePadTransitionTakingPlace) return; // Block transition access
			else gamePadTransitionTakingPlace = true; // Transition has begun

			Action UponCompletion = () => {
				gamepadButtonsDisplayed = true;

				_PushButtonSpritesToAppropriatePosition();

				jump.PushPack.PushCompleted += (s2, a2) => gamePadTransitionTakingPlace = false;
			}; // Begins Display & Push Of Controller Buttons 

			if (hearts.Count == 0) { /* Hearts don't need to be pushed */ UponCompletion(); } else {
				hearts[0].PushTo(hearts[0].Position + HEART_OFFSET_WITH_BUTTONS, 0.85f);

				hearts[0].PushPack.SpriteOffset += (sprite, pushVect) => {
					foreach (int X in Enumerable.Range(1, hearts.Count - 1)) {
						hearts[X].OffsetSpritePosition(pushVect);
					}

					pointDisplay.Position += pushVect;
				}; // Pushes Hearts Forward

				hearts[0].PushPack.SpriteSet += (sprite, newPos, oldPos) => {
					foreach (int X in Enumerable.Range(1, hearts.Count - 1)) {
						hearts[X].OffsetSpritePosition(newPos - oldPos);
					}

					pointDisplay.Position += newPos - oldPos;
				}; // Accounts For Any Unexpected Heart Displacement

				hearts[0].PushPack.PushCompleted += (sprite, args) => UponCompletion();
			}

			// pointDisplay.PushTo(pointDisplay.Position + HEART_OFFSET_WITH_BUTTONS, 0.85f);
		}

		private void HideControllerButtons() {
			if (gamePadTransitionTakingPlace) return; // Block transition
			else gamePadTransitionTakingPlace = true; // Begin transition

			#region PrimaryAction
			_PushButtonSpritesToCenter(); // Push button sprites back to center/origin
			jump.PushPack.PushCompleted += (sprite, args) => gamepadButtonsDisplayed = false;
			#endregion

			if (hearts.Count == 0) { gamePadTransitionTakingPlace = false; } else {
				jump.PushPack.PushCompleted += (sprite, args) => {
					hearts[0].PushTo(hearts[0].Position - HEART_OFFSET_WITH_BUTTONS);

					hearts[0].PushPack.SpriteOffset += (sprite2, pushVect) => {
						foreach (int X in Enumerable.Range(1, hearts.Count - 1)) {
							hearts[X].OffsetSpritePosition(pushVect);
						}

						pointDisplay.Position += pushVect;
					}; // Pushes Hearts backward

					hearts[0].PushPack.SpriteSet += (sprite3, newPos, oldPos) => {
						foreach (int X in Enumerable.Range(1, hearts.Count - 1)) {
							hearts[X].OffsetSpritePosition(newPos - oldPos);
						}

						pointDisplay.Position += newPos - oldPos;
					}; // Accounts For Any Unexpected Heart Displacement

					hearts[0].PushPack.PushCompleted += (sprite4, args2) => {
						gamePadTransitionTakingPlace = false;
					};
				};
			}

			jump.PushPack.PushCompleted += (sprite, args) => {
				// pointDisplay.PushTo(pointDisplay.Position - HEART_OFFSET_WITH_BUTTONS, 0.85f);
			};
		}

		#region Push Methods
		private void _PushButtonSpritesToAppropriatePosition() {
			foreach (GamePadButtonIcon icon in GamePadIcons) {
				icon.PushTo(icon.initialPosition);
			}
		}

		private void _PushButtonSpritesToCenter() {
			Vector2 center = ButtonCenterPosition;

			foreach (GamePadButtonIcon icon in GamePadIcons)
				icon.PushTo(center); // Initiate Push
		}
		#endregion

		private void UpdateControllerIcons(bool updateAnimation) {
			foreach (GamePadButtonIcon icon in GamePadIcons)
				icon.Update(updateAnimation);

			// Check whether to invalidate here
			dash.Active = GV.MonoGameImplement.Player.CanDash();
			jump.Active = GV.MonoGameImplement.Player.CanJump();
			//interact.Active = GV.MonoGameImplement.player.HasIntersectedTypes(GV.MonoGameImplement.InteractableTypes);
		}

		private void DrawControllerIcons() {
			foreach (GamePadButtonIcon icon in GamePadIcons)
				icon.Draw(); // Draw all game pad icons
		}
		
		public void AddHeart() {
			int active = GetIndexOfLastActiveHeart(); // Get current active heart

			Vector2 newHeartPosition = GetNewHeartPosition(); // new pos
			int lastHeartHealth = hearts[active], newHeartHealth;
			hearts[active].RefillHealth(); // refill

			if (active + 1 == hearts.Count) { // Last heart is active
				newHeartHealth = lastHeartHealth; // don't worry
			} else { // Hearts exist between present and new ones
				hearts[active + 1].Health = lastHeartHealth;
				newHeartHealth = 0; // empty new heart, sorry
			}

			hearts.Add(new HeartSprite(newHeartPosition, newHeartHealth));
		}

		public void RemoveHeart() {
			int active = GetIndexOfLastActiveHeart(), heartsFinal = HeartsCount - 1;

			if (heartsFinal < 0) throw new HollowAetherException($"Cannot Have Negative HEarts");

			int currentHeartHealth = hearts[active].Health; // Store before removal in case needed
			bool resetHealth = active == heartsFinal && hearts[active].Health != HeartSprite.HEART_SPAN;

			hearts.RemoveAt(heartsFinal); // Remove heart at absolute end of hearts list

			if (HeartsCount >= 0 && resetHealth) hearts[HeartsCount - 1].SetHealth(currentHeartHealth);
		}

		private Vector2 GetNewHeartPosition() {
			int xIntoCount = HeartSprite.heartCount % MAX_X_SPAN, yIntoCount = HeartSprite.heartCount / MAX_X_SPAN;

			Vector2 absPos = new Vector2(
				xIntoCount * (HeartSprite.DEFAULT_WIDTH  + HEART_X_OFFSET),
				yIntoCount * (HeartSprite.DEFAULT_HEIGHT + HEART_Y_OFFSET)
			);

			return absPos + hearts[0].Position;
		}

		private int GetIndexOfLastActiveHeart() {
			for (int X = hearts.Count - 1; X >= 0; X--) {
				if (hearts[X] != 0) return X;
			}

			return 0; // Should be unreachable, JIC
		}

		public void AddHealth(int amount) {
			int incrementedHealth = GetDisplayedHealth() + amount, total = TotalHudHealth; // store to lower waste
			if (incrementedHealth > total) throw new HUDHealthAdditionException(total, incrementedHealth); // raise

			int span = HeartSprite.HEART_SPAN; // Store locally to minimise call length

			for (int X = GetIndexOfLastActiveHeart(); X < hearts.Count; X++) {
				if (hearts[X] == span) continue; else {
					if (amount + hearts[X] > span) { amount -= span - hearts[X]; hearts[X].RefillHealth(); } else {
						hearts[X].AddHealth(amount); break; // At this point, amount to add is equivalent to 0
					}
				}
			}
		}

		public void TryAddHealth(int amount) {
			int displayed = GetDisplayedHealth();

			if (displayed != TotalHudHealth) {
				if (displayed + amount <= TotalHudHealth)
					AddHealth(amount); // Adds upto max
				else AddHealth(TotalHudHealth - displayed);
			}
		}

		public void TryTakeDamage(int amount) {
			int displayed = GetDisplayedHealth();

			if (displayed != 0) {
				if (displayed - amount >= 0)
					TakeDamage(amount); // Takes up to 0
				else TakeDamage(displayed);
			}
		}

		public void TakeDamage(int damage) {
			int decrementedHealth = GetDisplayedHealth() - damage; // Store to lower ref memory waste and speed up
			if (decrementedHealth < 0) throw new HUDHealthSubtractionException(TotalHudHealth, decrementedHealth);

			for (int X = GetIndexOfLastActiveHeart(); X >= 0; X--) {
				//if (hearts[X] != HeartSprite.HEART_SPAN) {
					if (damage > hearts[X]) { damage -= hearts[X]; hearts[X].EmptyHealth(); }  else {
						hearts[X].SubtractHealth(damage); // take away health from current heart
						break; // Break when damage has been reduced to nothing / 0
					}
				//}
			}
		}

		public int GetDisplayedHealth() {
			int index = GetIndexOfLastActiveHeart(); // Get chosen index
			return (HeartSprite.HEART_SPAN * index) + hearts[index].Health;
		}

		public void RefillHealth() {
			for (int X = GetIndexOfLastActiveHeart(); X < hearts.Count; X++) {
				hearts[X].RefillHealth(); // Refill health for all hearts
			}
		}

		public int TotalHudHealth { get { return hearts.Count * HeartSprite.HEART_SPAN; } }

		public int Health { get { return GetDisplayedHealth(); } }

		private Vector2 ButtonCenterPosition { get { return new Vector2(topButtonPosition.X, leftButtonPosition.Y); } }

		private GamePadButtonIcon[] GamePadIcons { get { return new GamePadButtonIcon[] { dash, jump, interact, other }; } }

		public int HeartsCount { get { return hearts.Count; } }

		public bool gamepadButtonsDisplayed = false;

		private PointDisplay pointDisplay;
		List<HeartSprite> hearts = new List<HeartSprite>();
		private const int MAX_X_SPAN = 8; // max hearts in row
		private Vector2 previousCameraPosition = Vector2.Zero;
		private GamePadButtonIcon dash, jump, interact, other;
		private const float HEART_X_OFFSET = 2, HEART_Y_OFFSET = 2;
		private static readonly Vector2 standardCornerOffset = new Vector2(25, 25);
		public GamePadButtonIcon.Theme defaultTheme = GamePadButtonIcon.Theme.Regular;
		private static readonly Vector2 HEART_OFFSET_WITH_BUTTONS = new Vector2(125, 0);
		private bool gamePadTransitionTakingPlace = false;
		private Vector2 topButtonPosition, bottomButtonPosition, leftButtonPosition, rightButtonPosition;
	}
}
