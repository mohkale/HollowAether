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

using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.FX {
	public sealed class ValueChangedEffect : FXSprite {
		public enum FlickerColor { White, Red, Blue, Gold }

		public enum FlickerValue { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Plus, Minus}

		ValueChangedEffect(Vector2 position, FlickerValue _value, bool _changeAlpha, bool _move, int timeout, params FlickerColor[] _colors) 
			: base(position, SPRITE_WIDTH, SPRITE_HEIGHT, timeout) {
			// if (_amount < 0 || _amount > 9) throw new HollowAetherException($"FX only supports amount values between 1 and 9");
			if (_colors.Length == 0) throw new HollowAetherException($"A minimum of at least one colours must be displayed");

			value = _value; colors = _colors; changeAlpha = _changeAlpha; move = _move; // Store used sprite variables

			Initialize($"cs\\incrementeffects"); // Increment effects from Cave Story is the texture used here
		}

		public static IMonoGameObject[] Create(Vector2 position, int _value, bool changeAlpha=false, bool move=true, int timeout = TIME_OUT, params FlickerColor[] colors) {
			String value = (_value < 0) ? _value.ToString().Substring(1) : _value.ToString();
			IMonoGameObject[] objects = new IMonoGameObject[value.Length+1]; // Hold

			int counter = 0; // Counts indexes within value effect objects array
			FlickerValue _operator = _value < 0 ? FlickerValue.Minus : FlickerValue.Plus;
			objects[counter++] = new ValueChangedEffect(position, _operator, changeAlpha, move, timeout, colors);

			foreach (char c in value.ToCharArray()) {
				position.X += SPRITE_WIDTH; // Increment horizontal sprite positioning
				FlickerValue fVal = (FlickerValue)Enum.Parse(typeof(FlickerValue), c.ToString());
				objects[counter++] = new ValueChangedEffect(position, fVal, changeAlpha, move, timeout, colors);
			}

			return objects; // Return all valid effect objects
		}

		public static IMonoGameObject[] CreateStatic(Vector2 position, int value, int timeout, params FlickerColor[] colors) {
			return Create(position, value, false, false, timeout, colors);
		}

		protected override void ImplementEffect() {
			if (changeAlpha) {
				Animation.Opacity -= elapsedTime * 1000 / TIME_OUT;
				if (Animation.Opacity < 0.5) VolatilityManager.Delete(this);
			}

			if (move) {
				velocity += acceleration * elapsedTime;
				OffsetSpritePosition(Y: velocity * elapsedTime);
			}
		}

		protected override void BuildSequenceLibrary() {
			AnimationSequence sequence; // Sequence to assign default animation to

			if (colors.Length == 1) sequence = new AnimationSequence(0, GetFrame(colors[0])); {
				// Otherwise sequence flickers between multiple colours values throughout existence
				sequence = new AnimationSequence(0, (from X in colors select GetFrame(X)).ToArray());
			}

			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = sequence;
		}

		private Frame GetFrame(FlickerColor color, int runCount=4) {
			return new Frame((int)color, (int)value, 16, 16, 16, 16, runCount);
		}

		private bool changeAlpha, move;
		private int amount;
		private FlickerColor[] colors;
		private FlickerValue   value;

		float acceleration = -100;
		float velocity     = -050;

		public const int SPRITE_WIDTH = 12, SPRITE_HEIGHT = 12, TIME_OUT = 5000;
	}
}
