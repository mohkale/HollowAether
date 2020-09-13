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
	public static partial class ContextMenu {
		private class ContextSpriteCharacter : IPushable {
			public class CharaAttributes : ICloneable {
				public CharaAttributes() : this(new Color(255, 255, 255), 0f, SpriteEffects.None) { }

				public CharaAttributes(Color _color, float _rotation, SpriteEffects effects) {
					color = _color;
					rotation = _rotation;
					effect = effects;
				}

				public object Clone() { return new CharaAttributes(color, rotation, effect); }

				public override string ToString() {
					return $"C:{color.ToVector4()}, R:{rotation}, FX:{effect}";
				}

				public Color color;
				public float rotation;
				public SpriteEffects effect;
			}

			public ContextSpriteCharacter(char character, Vector2 _position, CharaAttributes attributes) {
				chara = character;
				Position = _position;
				Attributes = (CharaAttributes)attributes.Clone(); // Create shallow clone
			}

			public ContextSpriteCharacter(char character, String font, Vector2 position, CharaAttributes attributes)
				: this(character, position, attributes) { Font = font; }

			public ContextSpriteCharacter(char character, Vector2 position, Color fontColor, float charRotation = 0f, SpriteEffects fx = SpriteEffects.None)
				: this(character, position, new CharaAttributes(fontColor, charRotation, fx)) { }

			public ContextSpriteCharacter(char character, String font, Vector2 position, Color color, float rotation = 0f, SpriteEffects fx = SpriteEffects.None)
				: this(character, font, position, new CharaAttributes(color, rotation, fx)) { }

			public override string ToString() {
				return $"CharStringSprite : '{chara}'";
			}

			public void PushTo(Vector2 position, float over = 0.8f) {
				if (PushArgs.PushValid(position, Position))
					Push(new PushArgs(position, Position, over));
			}

			public void Push(PushArgs args) { if (!BeingPushed) PushPack = args; }

			public void Update() {
				float elapsedTime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);
				if (BeingPushed && !PushPack.Update(this, elapsedTime)) { PushPack = null; /* Delete push pack */ }
			}

			public void Draw() {
				GV.MonoGameImplement.SpriteBatch.DrawString(
					GV.MonoGameImplement.fonts[Font], chara.ToString(),
					Position, Attributes.color, Attributes.rotation,
					Vector2.Zero, 1, Attributes.effect, 0
				);
			}

			public Vector2 Size() {
				return GV.MonoGameImplement.fonts[Font].MeasureString(chara.ToString());
			}

			public char chara;

			public CharaAttributes Attributes { get; private set; }

			public Vector2 Position { get; set; }

			public String Font { get; set; } = DEFAULT_FONT;

			public PushArgs PushPack { get; private set; } = null;

			public bool BeingPushed { get { return PushPack != null; } }
		}

		private class OKMenuSprite : Sprite {
			public OKMenuSprite() : base(GetPosition(), ScaledSpriteWidth, ScaledSpriteHeight, true) {
				Initialize("cs\\textbox");
				Layer = 0.9f;
			}

			public override void Initialize(string textureKey) {
				base.Initialize(textureKey); // Initialise any other existing values
				
				float yPosition = Position.Y + ((SPRITE_HEIGHT - OKMenuCursorSprite.SPRITE_HEIGHT + 8) * SECONDARY_SCALE * ContextScale) / 2;

				Vector2 yesPosition = new Vector2(Position.X - (16 * SECONDARY_SCALE * ContextScale), yPosition);
				Vector2 noPosition  = new Vector2(Position.X + (68 * SECONDARY_SCALE * ContextScale), yPosition);

				cursor = new OKMenuCursorSprite(yesPosition, noPosition);
			}

			public override void Update(bool updateAnimation) {
				base.Update(updateAnimation);

				cursor.Update(updateAnimation);

				bool leftInput = GV.PeripheralIO.currentControlState.Left;
				bool rightInput = GV.PeripheralIO.currentControlState.Right;

				if (leftInput ^ rightInput) cursor.PointsToYes = leftInput;
				
				if (GV.PeripheralIO.CheckMultipleButtons(true, null, Buttons.A)
				 || GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Enter))
					OkContextMenuComplete(cursor.PointsToYes);
			}

			public override void Draw() { cursor.Draw(); base.Draw(); }

			private static Vector2 GetPosition() {
				return new Vector2(spriteRect.Right - (0.75f * ScaledSpriteWidth), spriteRect.Top - (0.5f * ScaledSpriteHeight));
			}

			protected override void BuildSequenceLibrary() {
				Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = new AnimationSequence(
					0, new Frame(310, 102, SPRITE_WIDTH, SPRITE_HEIGHT, 1, 1, 1)
				);
			}

			private static int ScaledSpriteWidth { get { return (int)(SPRITE_WIDTH * SECONDARY_SCALE * ContextScale); } }

			private static int ScaledSpriteHeight { get { return (int)(SPRITE_HEIGHT * SECONDARY_SCALE * ContextScale); } }

			public const int SPRITE_WIDTH = 154, SPRITE_HEIGHT = 52;

			public static readonly float SECONDARY_SCALE = 0.35f;

			OKMenuCursorSprite cursor;
		}

		private class OKMenuCursorSprite : Sprite {
			public OKMenuCursorSprite(Vector2 yesPosition, Vector2 noPosition)
				: base(yesPosition, ScaledSpriteWidth, ScaledSpriteHeight, true) {
				Initialize("cs\\textbox");
				YesPosition = yesPosition;
				NoPosition  = noPosition;
				Layer = 1f;	
			}

			protected override void BuildSequenceLibrary() {
				Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = new AnimationSequence(
					0, new Frame(224, 176, 32, 32, 1, 1, 1)
				);
			}

			public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;

			public static int ScaledSpriteWidth { get { return (int)(SPRITE_WIDTH * OKMenuSprite.SECONDARY_SCALE * ContextScale); } }

			public static int ScaledSpriteHeight { get { return (int)(SPRITE_HEIGHT * OKMenuSprite.SECONDARY_SCALE * ContextScale); } }

			private bool pointsToYes = true;

			private Vector2 YesPosition, NoPosition;

			public bool PointsToYes { get { return pointsToYes; }
				set {
					pointsToYes = value; // Set private value
					Position = (value) ? YesPosition : NoPosition;
				}
			}
		}
	}
}
