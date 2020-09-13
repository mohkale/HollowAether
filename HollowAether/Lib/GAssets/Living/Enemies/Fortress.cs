using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.MapZone;
#endregion

namespace HollowAether.Lib.GAssets {
	public partial class Fortress : Enemy, IPushable {
		public Fortress() : this(Vector2.Zero, 1) { }

		public Fortress(Vector2 position, int level) : base(position, FRAME_WIDTH, FRAME_HEIGHT, level, true) {
			Initialize(@"enemies\badeye");
			defaultPosition = position;
			
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey);

			Killed += (self) => { Animation.SetAnimationSequence("Stationary"); };
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(Position.X, Position.Y, Width, Height));
		}

		protected override void BuildSequenceLibrary() {
			Animation["Stationary"] = GV.MonoGameImplement.importedAnimations[@"fortress\stationary"];
			Animation["Flapping"]   = GV.MonoGameImplement.importedAnimations[@"fortress\flapping"];

			Animation.SetAnimationSequence("Flapping"); // Default animation is stationary
		}


		public void PushTo(Vector2 position, float over = 0.8f) {
			Push(new PushArgs(position, Position, over));
		}

		public void Push(PushArgs args) { if (!BeingPushed) PushPack = args; }

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed time as well

			if (BeingPushed && !PushPack.Update(this, elapsedTime)) {
				PushPack = null; // Delete push pack
			}
		}

		protected override void DoEnemyStuff() {
			Vector2 playerCenter   = GV.MonoGameImplement.Player.SpriteRect.Center.ToVector2();
			float differenceLength = (playerCenter - (defaultPosition + (Size.ToVector2() / 2))).Length(); 

			if (attacking) {
				if (attackTimeout > 0) { attackTimeout -= elapsedMilitime; } else {
					Fireball fire = new Fireball(Position, GV.MonoGameImplement.Player.Position, 45);
					GV.MonoGameImplement.additionBatch.AddNameless(fire); // Add fire to object store
					attackTimeout = 2500; // Don't attack\throw-fireball again for 2.5 seconds
				}

				if (!BeingPushed) {
					if (differenceLength >= DetectionRadius) { attacking = false; Angle = 0f; PushTo(defaultPosition, 2f); } else {
						Angle += AngularSpeed * elapsedTime; // Increment current angle of enemy from center

						Vector2 sinCosRatio = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
						OffsetSpritePosition(sinCosRatio * Radius * elapsedTime); // Make circle around center
					}
				}
			} else {
				if (!BeingPushed && differenceLength <= DetectionRadius) {
					PushTo(defaultPosition - new Vector2(0, Radius), 2f);
					
					if (BeingPushed) { // In case new position not valid
						PushPack.PushCompleted += (self, args) => {
							attacking = true; // Start attacking after pushed
						};
					}
				}
			}
		}

		protected override uint GetHealth() {
			return 100;
			return GV.BasicMath.Clamp<uint>((uint)(3 * GV.Variables.random.Next(Level-5, Level+5)), 1, 100);
		}

		private float GetLengthToPlayerFrom(Vector2 position) {
			return (GV.MonoGameImplement.Player.SpriteRect.Center.ToVector2() - position).Length();
		}

		private bool attacking = false;

		private float attackTimeout = 0f;

		public float DetectionRadius { get; set; } = 150;

		private const float AngularSpeed = (float)(2 * Math.PI / 3);

		private float angle;

		private float Angle { get { return angle; } set {
			angle = value; // Set angle to new value\argument

			if (angle > 2 * Math.PI) angle -= (float)(2 * Math.PI);
		} }

		public PushArgs PushPack { get; private set; } = null;

		public bool BeingPushed { get { return PushPack != null; } }

		public float Radius { get { return Height * 5f; } }

		protected override bool CanGenerateHealth { get; set; } = false;

		protected override bool CausesContactDamage { get; set; } = true;

		private Vector2 defaultPosition;

		public const int FRAME_WIDTH = 31, FRAME_HEIGHT = 24;

		public const int SPRITE_WIDTH = FRAME_WIDTH, SPRITE_HEIGHT = FRAME_HEIGHT;
	}
}
