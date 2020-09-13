using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public partial class Bat : Enemy {
		public Bat() : this(Vector2.Zero, 1, -1) { }

		public Bat(Vector2 position, int level, int direction) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT, level, true) {
			/*Initialize(@"enemies\bat");*/ travellingDirection = direction;
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey); // Sets animation and texture for sprite

			Killed += (self) => { self.Animation.SetAnimationSequence("Rest"); };
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed time as well
		}

		protected override void BuildSequenceLibrary() {
			Animation["Rest"]     =     GV.MonoGameImplement.importedAnimations[@"bat\rest"];
			Animation["Flapping"] = GV.MonoGameImplement.importedAnimations[@"bat\flapping"];

			Animation.SetAnimationSequence("Flapping"); // Default animation is at rest
		}

		protected override void BuildBoundary() { boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(SpriteRect)); }

		protected override void DoEnemyStuff() {
			if (!active) {
				OffsetSpritePosition(LINEAR_VELOCITY * travellingDirection * elapsedTime);

				Vector2 playerPos = GV.MonoGameImplement.Player.Position;
				float heightDifferential = Math.Abs(Position.Y - playerPos.Y);

				if (playerPos != Position && (playerPos - Position).Length() < MAX_CHECK_RADIUS) {
					bool passesMin = heightDifferential > MIN_RADIAL_HEIGHT_DIFFERENTIAL;
					bool passesMax = heightDifferential < MAX_RADIAL_HEIGHT_DIFFERENTIAL;

					if (passesMin && passesMax) { // Valid enemy position to spin around
						active = true; // Player can now initialise attack motion
						rotationPivot = Position; // Rotates from current position
						angle = 0; // Reset rotational angle to 0. I.E. No rotation

						Vector2 playerCenter = GV.MonoGameImplement.Player.SpriteRect.Center.ToVector2();

						angularRadius = (playerCenter - SpriteRect.Center.ToVector2()).Length(); // Radius
						angularVelocity = LINEAR_VELOCITY / angularRadius; // Convert linear to angular

						rotationalDirection = new Vector2(
							playerPos.X > Position.X ? +1 : -1,
							playerPos.Y > Position.Y ? +1 : -1
						);
					}
				}
			} else {
				angle += angularVelocity * elapsedTime; // Increment angle of rotation of enemy

				Vector2 sinCosRatio = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

				OffsetSpritePosition(sinCosRatio * angularRadius * elapsedTime * rotationalDirection);

				if (angle > 2 * Math.PI) { // Has completed single revolution
					active = false; // Stop enemy attack after complete revolution
					SetPosition(rotationPivot); // Set to position before rotation
				}
			}
		}

		protected override bool ReadyToDelete(IMonoGameObject self) {
			return base.ReadyToDelete(self) || !GV.MonoGameImplement.camera.ContainedInCamera(SpriteRect);
		}

		private float angle = 0, angularVelocity = 0, angularRadius = 0;
		private Vector2 rotationPivot, rotationalDirection;

		#region 
		protected override bool CanGenerateHealth { get; set; } = false;

		protected override bool CausesContactDamage { get; set; } = true;

		private bool active = false; // Started to attack player

		private int travellingDirection;

		private const int LINEAR_VELOCITY = 95, MAX_CHECK_RADIUS = 80;

		private const int MAX_RADIAL_HEIGHT_DIFFERENTIAL = 250, MIN_RADIAL_HEIGHT_DIFFERENTIAL = 8;

		public const int FRAME_WIDTH = 16, FRAME_HEIGHT = 12;

		public const int SPRITE_WIDTH = FRAME_WIDTH * 2, SPRITE_HEIGHT = FRAME_HEIGHT * 2; 
		#endregion
	}
}
