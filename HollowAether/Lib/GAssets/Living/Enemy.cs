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
	public abstract class Enemy : VolatileBodySprite {
		public Enemy(Vector2 position, int width, int height, int level, bool animationRunning) 
			: base(position, width, height, animationRunning) {
			Level = level; // Store level to instance
			Layer = 0.45f;

			InitializeVolatility(VolatilityType.Other, new ImplementVolatility(ReadyToDelete));

			Killed += Enemy_Killed;

			VolatilityManager.Deleting += () => { Console.WriteLine($"Deleted {SpriteID}"); };
		}

		private void Enemy_Killed(Enemy obj) {
			foreach (int X in Enumerable.Range(1, GV.Variables.random.Next(1, 5))) {
				int spanIndex = GV.Variables.random.Next(0, 2); // Point Span Enum Index
				BurstPoint.PointSpan span = (BurstPoint.PointSpan)spanIndex;

				GV.MonoGameImplement.additionBatch.AddNameless(new BurstPoint(Position, span));
			}

			//throw new NotImplementedException();
		}

		private void VolatilityManager_Deleting() {
			foreach (int X in Enumerable.Range(1, GV.Variables.random.Next(1, 5))) {
				int spanIndex = GV.Variables.random.Next(0, 2); // Point Span Enum Index
				BurstPoint.PointSpan span = (BurstPoint.PointSpan)spanIndex;

				GV.MonoGameImplement.additionBatch.AddNameless(new BurstPoint(Position, span));
			}

			GV.MonoGameImplement.additionBatch.AddNameless(new BurstPoint(Position, BurstPoint.PointSpan.Medium));

			//throw new NotImplementedException();
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey); // Initialise base sprite features. Ensures any base logic is properly implemented

			Damaged = (self, amount) => {
				if (self.Alive) {
					int damage = GV.BasicMath.Clamp(amount, 0, Health); // Clamp damage to ensure it's always less then the health of enemy

					#region EffectCreation
					GV.MonoGameImplement.additionBatch.AddNameless(new HitSprite(Position, 16, 16, HitSprite.HitType.Red)); // Make hit sprite
					FX.ValueChangedEffect.FlickerColor fxColor = FX.ValueChangedEffect.FlickerColor.White; // Color for effect sprite to draw
					GV.MonoGameImplement.additionBatch.AddRangeNameless(FX.ValueChangedEffect.CreateStatic(Position, -damage, 1000, fxColor));
					#endregion

					#region DamageTaking
					self.healthBar.TakeDamage(damage); // Take health from current enemy
					if (Health == 0) Killed(this); // If was killed, execute event handler
					invincibilityTime = 750; // Make unable to be hurt for 0.75 seconds
					#endregion
				}
			};

			Killed += (self) => { RecursiveKilledHitEffectDisplayer(GV.Variables.random.Next(5, 15)); };

			healthBar = new EnemyHealthBar(GetHealthBarInitialPosition(), Width, EnemyHealthBar.DEFAULT_HEIGHT, GetHealth());
		}

		public override void Draw() {
			base.Draw(); // Draw actual sprite instance

			if (Alive && Health != healthBar.MaxHealth) {
				healthBar.Draw(); // Draw health bar to game
			}
		}


		/// <summary>Basically sets it up so a set number of sprites are generated one after the other recursively</summary>
		/// <param name="count">Number of FX sprites to generate. By default it is 5 but can accept any value</param>
		private void RecursiveKilledHitEffectDisplayer(int count=5) {
			if (count >= 0) { // So long as number of sprites to display is not -ve
				int hsDimensions = 18; // Size of hit sprite which is about to be displayed by method

				Vector2 position = Position  /*- new Vector2(hsDimensions)*/ + new Vector2(
					GV.Variables.random.Next(Width), GV.Variables.random.Next(Height)
				); // Position of newly defined hit sprite, randomised to enemy sprite rect

				HitSprite hit = new HitSprite(position, hsDimensions, hsDimensions, HitSprite.HitType.Blue);
				// Creates new hit sprite with desired dimensions and of type HitSprite.HitType.Blue

				hit.VolatilityManager.Deleting += () => { RecursiveKilledHitEffectDisplayer(count - 1); };
				// When the volatile hit sprite is being removed/deleted, call this method again to create new

				GV.MonoGameImplement.additionBatch.AddNameless(hit); // Add effect sprite to store
			} else deathFXSpritesComplete = true; // Enemy ready to delete now
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Also takes care of volatility

			if (CanGenerateHealth && Alive && Health != healthBar.MaxHealth && false) {
				/*if (timeNotDamaged >= 5000) {
					generatedHealthStore += 3f * elapsedTime;

					if (generatedHealthStore > 1) { // If generated at least 1 point
						healthBar.RegainHealth((int)generatedHealthStore);
						generatedHealthStore -= 1; // Reset to close to 0
					}
				} else { timeNotDamaged += elapsedMilitime; }*/
			}

			if (Alive) {
				DoEnemyStuff(); // Implement enemy algorithm defined in any child classes

				if (CausesContactDamage && GV.MonoGameImplement.Player.Intersects(this)) {
					if (!GV.MonoGameImplement.Player.CurrentWeapon.Attacking)
						GV.MonoGameImplement.Player.Attack(GV.MonoGameImplement.ContactDamage);
				}

				if (invincibilityTime > 0) { invincibilityTime -= elapsedMilitime; } else {
					bool weaponAttacking = GV.MonoGameImplement.Player.CurrentWeapon.Attacking
									|| GV.MonoGameImplement.Player.CurrentWeapon.Thrown;

					if (weaponAttacking && GV.MonoGameImplement.Player.CurrentWeapon.Intersects(this))
						Attack(GV.MonoGameImplement.Player.CurrentWeapon.GetDamage());

					IMonoGameObject[] damagingIntersects = CompoundIntersects<IDamagingToEnemies>();

					if (damagingIntersects.Length > 0) {
						int damage = (from X in damagingIntersects select (X as IDamaging).GetDamage()).Aggregate((a, b) => a + b);
						Attack(damage);
					}
				}
			} else Animation.Opacity -= 0.075f * elapsedTime; /* Lose by 7.5% every second */ 
		}

		public override void OffsetSpritePosition(Vector2 XY) {
			base.OffsetSpritePosition(XY); // Offsets enemy
			if (healthBar != null) healthBar.Offset(XY); // Offsets health bar for enemy
		}

		public override void SetPosition(Vector2 nPos) {
			base.SetPosition(nPos); // Sets position of actual enemy sprite
			if (healthBar != null) healthBar.Offset(GetHealthBarInitialPosition() - healthBar.Position);
		}

		protected Vector2 GetHealthBarInitialPosition() { return Position - new Vector2(0, 3 * EnemyHealthBar.DEFAULT_HEIGHT); }

		public virtual void Attack(int damage) { if (Alive) Damaged(this, damage); }

		protected virtual uint GetHealth() {
			return 1; // Default returns 1
		}

		/// <summary>When enemy can be deleted from game</summary>
		/// <param name="self">Current instance, use if needed</param>
		/// <returns>Bool indicating sprite can be deleted</returns>
		protected virtual bool ReadyToDelete(IMonoGameObject self) {
			return !Alive && deathFXSpritesComplete;
		}

		/// <summary>Implementation of actual enemy logic</summary>
		protected abstract void DoEnemyStuff();

		private EnemyHealthBar healthBar;

		public int Level { get; protected set; }

		public int Health { get { return healthBar.Health; } }

		public bool Alive { get; protected set; } = true;

		protected abstract bool CanGenerateHealth { get; set; }

		protected abstract bool CausesContactDamage { get; set; }

		protected int timeNotDamaged = 0; // Time when not damaged

		private float generatedHealthStore = 0;

		private bool deathFXSpritesComplete = false;

		private int invincibilityTime = 0;

		public event Action<Enemy, int> Damaged;

		public event Action<Enemy> Killed = (self) => { self.Alive = false; };
	}
}
