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
using HollowAether.Lib.GAssets; // game assets
using GV = HollowAether.Lib.GlobalVars;
#endregion

using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GameWindow;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Debug;
using HollowAether.Lib.GAssets.Items;
using FC = HollowAether.Lib.GAssets.FX.ValueChangedEffect.FlickerColor;
using HollowAether.Lib.GAssets.FX;

namespace HollowAether.Lib.GameWindow {
	static class GameRunning {
		private static class EventHandlers {
			#region ItemAcquired
			public static void CoinAcquiredFX(Coin coin) {
				Vector2 position = new Vector2(coin.SpriteRect.Right, coin.Position.Y); // Top right edge

				foreach (IMonoGameObject _object in ValueChangedEffect.Create(position, coin.Value, colors: new FC[] { FC.White, FC.Gold }))
					GV.MonoGameImplement.additionBatch.AddNameless(_object); // Store object to global object store
			}

			public static void BurstPointAcquiredFX(BurstPoint point) {
				Vector2 position = new Vector2(point.SpriteRect.Right, point.Position.Y); // Top right edge

				foreach (IMonoGameObject _object in ValueChangedEffect.Create(position, point.Value, colors: FC.Blue))
					GV.MonoGameImplement.additionBatch.AddNameless(_object); // Store object to global object store
			}

			public static void GotItem(IItem item) {
				if (!(item is IMonoGameObject)) return; // Should be impossible, but u never know

				GAssets.HUD.ContextMenu.CreateText($"{item.OutputString}#SYS:WAITFORINPUT;");
				GV.MonoGameImplement.removalBatch.Add((IMonoGameObject)item); // Cast
			}
			#endregion

			#region ZoneChangedEventHandlers
			public static void ZoneTransition(Vector2 oldZone, Vector2 newZone) {
				GAssets.Transition.CreateTransition();

				//GV.MonoGameImplement.monogameObjects.ClearAll();
				GameRunning.MoveZones(newZone); // Move zones
			}
			#endregion

			public static void PlayerDead() {
				GV.MonoGameImplement.gameState = GameState.PlayerDeceased;
				GameWindow.PlayerDeceased.Reset(); // Reset dead game window
			}
		}

		static GameRunning() {
			BuildEventHandlers();
			DebugInit(); // Where map would go later
			// GV.MonoGameImplement.background = new Background();
		}

		private static void BuildEventHandlers() {
			GotItem            = (item)   => {                                                          };
			CoinAcquired       = (coin)   => { GV.MonoGameImplement.money += coin.Value;                };
			BurstPointAcquired = (point)  => { GV.MonoGameImplement.Player.AddBurstPoints(point.Value); };
			GotHealthPickup    = (health) => { GV.MonoGameImplement.GameHUD.TryAddHealth(health.Value); };
			TransitionZone     = (nZ, oZ) => { invokeTransitionZone = false;                            };

			TransitionZone     += EventHandlers.ZoneTransition;
			GotItem            += EventHandlers.GotItem;
			CoinAcquired       += EventHandlers.CoinAcquiredFX;
			BurstPointAcquired += EventHandlers.BurstPointAcquiredFX;

			PlayerDeceased = EventHandlers.PlayerDead;

			PlayerDeceased += () => { Console.WriteLine("Deceased"); };
		}
		
		public static void DebugInit() {
			GV.MonoGameImplement.textures.Add("angledTemp", GV.TextureCreation.GenerateBlankTexture(Color.Red));
			GV.MonoGameImplement.textures.Add("blue",       GV.TextureCreation.GenerateBlankTexture(Color.Blue));
			GV.MonoGameImplement.textures.Add("green",      GV.TextureCreation.GenerateBlankTexture(Color.Green));
			GV.MonoGameImplement.textures.Add("pink",       GV.TextureCreation.GenerateBlankTexture(Color.HotPink));
		}

		public static void Draw() {
			// GV.MonoGameImplement.background.Draw();

			GV.MonoGameImplement.InitializeSpriteBatch();

			GV.MonoGameImplement.Player.Draw();

			foreach (IMonoGameObject mgo in GV.MonoGameImplement.monogameObjects) mgo.Draw();

			GV.MonoGameImplement.SpriteBatch.End();

			GV.MonoGameImplement.GameHUD.Draw();

			if (GAssets.HUD.ContextMenu.Active) GAssets.HUD.ContextMenu.Draw();

			#if DEBUG
			DebugPrinter.Set("Money", GV.MonoGameImplement.money);
			DebugPrinter.Set("Position", GV.MonoGameImplement.Player.Position);
			DebugPrinter.Set("CameraPosition", GV.MonoGameImplement.camera.Position);
			DebugPrinter.Set("BurstPoints", GV.MonoGameImplement.Player.burstPoints);
			DebugPrinter.Set("ObjectsCount", GV.MonoGameImplement.monogameObjects.Length);
			DebugPrinter.Set("BurstPointWait", Math.Round(GV.MonoGameImplement.Player.timeNotMoving, 3));

			IMonoGameObject[] collided = GV.MonoGameImplement.Player.CompoundIntersects();
			DebugPrinter.Set("Intersected", (collided.Length > 0) ? (from X in collided select X.SpriteID).Aggregate((a, b) => $"{a}, {b}") : null);
			Debug.DebugPrinter.Draw(); // Draw
			#endif
		}

		public static void Reset() {

		} // ignore for now.

		public static void Update() {
			if (GV.PeripheralIO.ImplementWaitForInputToBeRemoved(ref WaitForInputToBeRemoved))
				return; // If Still Waiting For Input Retrieval Then Return B4 Update

			if (GV.PeripheralIO.currentControlState.Pause) {
				GV.MonoGameImplement.gameState = GameState.GamePaused;
				GameWindow.GamePaused.WaitForInputToBeRemoved = true;
			} else {
				elapsedGameTime += GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
				bool updateAnimation = elapsedGameTime >= GV.MonoGameImplement.MilisecondsPerFrame;
				if (updateAnimation) elapsedGameTime = 0; // Reset animation update counter

				if (GAssets.HUD.ContextMenu.Active) { GAssets.HUD.ContextMenu.Update(); } else {
					GV.MonoGameImplement.Player.Update(updateAnimation);

					foreach (IMonoGameObject mgo in GV.MonoGameImplement.monogameObjects) {
						mgo.Update(updateAnimation);
					}

					GV.MonoGameImplement.Player.LateUpdate();

					foreach (IMonoGameObject mgo in GV.MonoGameImplement.monogameObjects) {
						mgo.LateUpdate();
					}

					// GV.MonoGameImplement.background.Update();

					GV.MonoGameImplement.removalBatch.Execute(); // Remove any allocated sprites
					GV.MonoGameImplement.additionBatch.Execute(); // Add any designated sprites

					GV.MonoGameImplement.camera.Update(); // camera has to monitor player
					GV.MonoGameImplement.GameHUD.Update(updateAnimation); // HUD has to maintain position
				}

				if (PlayerExitedZoneRegion()) {
					Vector2? adjacent = GV.MonoGameImplement.map.ZoneExistsAdjacentTo(GV.MonoGameImplement.map.Index);

					if (!adjacent.HasValue) { PlayerDeceased(); /*Kill when no where to fall to*/ } else {
						Vector2 playerPos  = GV.MonoGameImplement.Player.Position; // Get player position
						Point   playerSize = GV.MonoGameImplement.Player.Size; // Get player size

						bool wentLeft  = playerPos.X + playerSize.X < 0;
						bool wentRight = playerPos.X                > GV.MonoGameImplement.map.CurrentZone.ZoneWidth;
						bool wentUp    = playerPos.Y + playerSize.Y < 0;
						bool wentDown  = playerPos.Y                > GV.MonoGameImplement.map.CurrentZone.ZoneHeight;

						Vector2 current = GV.MonoGameImplement.map.Index, newZone;

						if      (wentLeft)  newZone = new Vector2(current.X-1,   current.Y);
						else if (wentRight) newZone = new Vector2(current.X+1,   current.Y);
						else if (wentUp)    newZone = new Vector2(current.X,   current.Y-1);
						else if (wentDown)  newZone = new Vector2(current.X,   current.Y+1);
						else                { PlayerDeceased(); return; } // Should be IMPOSSIBLE
						
						if (!GV.MonoGameImplement.map.ContainsZone(newZone)) PlayerDeceased(); else {
							EventHandlers.ZoneTransition(current, newZone);
						}
					}
				}

				if (invokeTransitionZone) InvokeTransitionZone();
			}
		}

		private static bool PlayerExitedZoneRegion() {
			Vector2 position = GV.MonoGameImplement.Player.Position; // Get player position

			if (GV.MonoGameImplement.map.CurrentZone.Contains(position)) return false; else {
				if (position.X >= 0 && position.Y >= 0) return true; else { // 
					Point size = GV.MonoGameImplement.Player.Size;

					Vector2 adjustedPosition = new Vector2() {
						X = position.X + (position.X < 0 ? size.X : 0),
						Y = position.Y + (position.Y < 0 ? size.Y : 0)
					};

					return !GV.MonoGameImplement.map.CurrentZone.Contains(adjustedPosition);
				}
			}
		}

		private static void InvokeTransitionZone() {
			if (!transitionZoneEventArg_New.HasValue)
				throw new HollowAetherException($"Tranisition allocated, but no target set");

			TransitionZone(GV.MonoGameImplement.map.Index, transitionZoneEventArg_New.Value);
		}

		public static void LoadSave(int slotIndex) {
			SaveManager.SaveFile save = SaveManager.GetSave(slotIndex);
			
			GV.MonoGameImplement.saveIndex = slotIndex; // Store save index

			MoveZones(save.currentZone);
			
			GV.MonoGameImplement.Player.Position = save.playerPosition;

			GV.MonoGameImplement.GameHUD.SetHeartCount(save.heartCount);
			GV.MonoGameImplement.GameHUD.SetHealth(save.health);

			GV.MonoGameImplement.Player.ImplementPerks(save.perks);

			foreach (Tuple<String, bool> flagChange in save.flags) {
				FlagAsset flag = GV.MapZone.GetFlagAsset(flagChange.Item1);

				if (!(flag.asset as Flag).ValueChangedFromDefault())
					(flag.asset as Flag).ChangeFlag(flagChange.Item2);
			}
		}

		public static void FullScreenReset(bool isFullScreen) {
			if (GV.MonoGameImplement.gameState == GameState.GameRunning) Reset();
		}

		private static void MoveZones(Vector2 newZone) {
			GV.MapZone.globalAssets["playerStartPosition"].Delete();

			Vector2 zoneDifference = newZone - GV.MonoGameImplement.map.Index;

			GV.MonoGameImplement.map.SetCurrentZone(newZone, false, true); // Could assign new player position

			#region PlayerPositionAssignment
			Vector2 newPlayerPosition = Vector2.Zero; //GV.MonoGameImplement.Player.Position; // By default push to Vector2

			if (GV.MapZone.globalAssets["playerStartPosition"].asset != null) {
				newPlayerPosition = (Vector2)GV.MapZone.globalAssets["playerStartPosition"].GetValue();
			} else if (Math.Abs(zoneDifference.X) == 1 ^ Math.Abs(zoneDifference.Y) == 1) {
				Vector2 zoneDimensions = GV.MonoGameImplement.map.CurrentZone.XSize;

				if      (zoneDifference.X > 1) newPlayerPosition.X = 0; // Default Anyway but leave for lack of better option
				else if (zoneDifference.X < 1) newPlayerPosition.X = zoneDimensions.X - GV.MonoGameImplement.Player.Width;

				/*if      (zoneDifference.Y > 1) newPlayerPosition.Y = 0; // Default anyway, but leave for lack of better option
				else if (zoneDifference.Y < 1) newPlayerPosition.Y = zoneDimensions.Y - GV.MonoGameImplement.Player.Height;*/
			}

			GV.MonoGameImplement.Player.SetPosition(newPlayerPosition);
			#endregion

			GV.MonoGameImplement.Player.interaction_timeout = 500; // Add Timeout
		}

		#region EventStuff
		public static void InvokePlayerDeceased() { PlayerDeceased(); }

		public static void InvokeCoinAcquired(Coin coin) { CoinAcquired(coin); }

		public static void InvokeBurstPointAcquired(BurstPoint point) { BurstPointAcquired(point); }

		public static void InvokeGotHealthPickup(HealthPickup pickup) { GotHealthPickup(pickup); }

		public static void InvokeGotItem(IItem item) { GotItem(item); }

		public static event Action                   PlayerDeceased;
		public static event Action<Coin>             CoinAcquired;
		public static event Action<BurstPoint>       BurstPointAcquired;
		public static event Action<IItem>            GotItem;
		public static event Action<HealthPickup>     GotHealthPickup;
		public static event Action<Vector2, Vector2> TransitionZone;

		public static bool invokeTransitionZone = false;
		public static Vector2? transitionZoneEventArg_New;
		#endregion

		static int elapsedGameTime;

		public static bool WaitForInputToBeRemoved = false;
	}
}
