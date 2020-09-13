using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Converter = HollowAether.Lib.InputOutput.Parsers.Converters;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public partial class Sprite : IMonoGameObject {
		public virtual bool ImplementEntityAttribute(String attrName, Object value, bool throwExcept = true) {
			switch (attrName) {
				case "Position":         SetPosition((Vector2)value);			   return true;
				case "Width":            Width = (int)value;					   return true;
				case "Height":           Height = (int)value;					   return true;
				case "Texture":          animation.TextureID = value.ToString();   return true;
				case "AnimationRunning": animation.animationRunning = (bool)value; return true;
				case "Layer":			 animation.layer = (float)value;           return true;
			
				default: if (throwExcept) throw new HollowAetherException(attrName); return false;
			}
		}

		public void AttatchEntity(GameEntity entity) {
			foreach (String attr in entity.GetEntityAttributes()) {
				if (entity[attr].IsAssigned)
					ImplementEntityAttribute(attr, entity[attr].GetActualValue());
			}
		}

		public static GameEntity GetGameEntity() {
			return new GameEntity(typeof(Player).Name, GetAdditionalEntityAttributes().ToArray());
		}

		public static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[0]; // Return empty entity array for now
		}
	}

	public class GenericSprite : Sprite, IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (base.ImplementEntityAttribute(attrName, value, false)) return true; else {
				switch (attrName) {
					case "DefaultAnimation": Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = (AnimationSequence)value; return true;

					default: if (throwExcept) throw new HollowAetherException(attrName); return false;
				}
			}
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("DefaultAnimation", new EntityAttribute(typeof(AnimationSequence), false))
			};
		}

		protected override void BuildSequenceLibrary() {
			// Not really applicable
		}
	}

	public partial class Player : IInitializableEntity {
		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"cs\main",    typeof(string), false)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,  typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT, typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"player\idleright"].GetFrame(false);
		}
	}

	#region Enemies
	public partial class Bat : IInitializableEntity {
		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"enemies\bat", typeof(string), false)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,   typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,  typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"bat\rest"].GetFrame(false);
		}
	}

	/*public partial class Crawler : IInitializableEntity {
		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"enemies\crawler", typeof(string), true)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,		 typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,		 typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"player\idleRight"].GetFrame(false);
		}
	}*/

	public partial class Crusher : IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (attrName == "Position") initialPosition = (Vector2)value;
			return base.ImplementEntityAttribute(attrName, value, true);
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"enemies\crusher", typeof(string), false)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,		 typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,		 typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"crusher\idle"].GetFrame(false);
		}
	}

	public partial class Fortress : IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (attrName == "Position") defaultPosition = (Vector2)value;
			return base.ImplementEntityAttribute(attrName, value, true);
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"enemies\badeye", typeof(string), false)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,		typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,		typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"fortress\stationary"].GetFrame(false);
		}
	}

	public partial class Jumper : IInitializableEntity {
		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"enemies\jumper", typeof(string), false)),
				new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,		typeof(int),    false)),
				new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,		typeof(int),    false)),
			};
		}

		public static Frame GetEntityFrame() {
			return GV.MonoGameImplement.importedAnimations[@"jumper\idle"].GetFrame(false);
		}
	}
	#endregion

	#region Platforms And Blocks
	public partial class Block : IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (base.ImplementEntityAttribute(attrName, value, false)) return true; else {
				switch (attrName) {
					case "DefaultAnimation": Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = (AnimationSequence)value; return true;

					default: if (throwExcept) throw new HollowAetherException(attrName); return false;
				}
			}
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("DefaultAnimation", new EntityAttribute(typeof(AnimationSequence), false))
			};
		}
	}

	/*public partial class AngledBlock : IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (base.ImplementEntityAttribute(attrName, value, false)) return true; else {
				switch (attrName) {
					case "DefaultAnimation": Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = (AnimationSequence)value; return true;
					
					default: if (throwExcept) throw new HollowAetherException(attrName); return false;
				}
			}
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("DefaultAnimation", new EntityAttribute(typeof(AnimationSequence), false))
			};
		}
	}*/

	public partial class GravityBlock : IInitializableEntity {
		public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
			if (base.ImplementEntityAttribute(attrName, value, false)) return true; else {
				switch (attrName) {
					case "DefaultAnimation": Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = (AnimationSequence)value; return true;

					default: if (throwExcept) throw new HollowAetherException(attrName); return false;
				}
			}
		}

		public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<String, EntityAttribute>("DefaultAnimation", new EntityAttribute(typeof(AnimationSequence), false))
			};
		}
	}
	#endregion

	#region Items And Pickups
	namespace Items {
		public partial class SaveStation : IInitializableEntity {
			public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
				return new KeyValuePair<String, EntityAttribute>[] {
					new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"cs\npcsym",    typeof(string), false)),
					new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(FRAME_WIDTH,  typeof(int),    false)),
					new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(FRAME_HEIGHT, typeof(int),    false)),
				};
			}

			public static Frame GetEntityFrame() {
				return new Frame(6, 1, FRAME_WIDTH, FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT);
			}
		}

		// public partial class Key : IInitializableEntity { }

		// public partial class HeartCanister : IInitializableEntity { }

		public partial class TreasureChest : IInitializableEntity {
			public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
				return new KeyValuePair<String, EntityAttribute>[] {
					new KeyValuePair<String, EntityAttribute>("Texture", new EntityAttribute(@"items\chest", typeof(string), false)),
					new KeyValuePair<String, EntityAttribute>("Width",   new EntityAttribute(SPRITE_WIDTH,   typeof(int),    false)),
					new KeyValuePair<String, EntityAttribute>("Height",  new EntityAttribute(SPRITE_HEIGHT,  typeof(int),    false)),
				};
			}

			public static Frame GetEntityFrame() {
				return new Frame(0, 0, FRAME_WIDTH, FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT);
			}
		}

		namespace Gates {
			public partial class OpenGate : IInitializableEntity {
				public override bool ImplementEntityAttribute(string attrName, object value, bool throwExcept = true) {
					if (base.ImplementEntityAttribute(attrName, value, false)) return true; else {
						switch (attrName) {
							case "TargetZone": TakesToZone = (Vector2)value; return true;
							
							default: if (throwExcept) throw new HollowAetherException(attrName); return false;
						}
					}
				}

				public new static ICollection<KeyValuePair<String, EntityAttribute>> GetAdditionalEntityAttributes() {
					return new KeyValuePair<String, EntityAttribute>[] {
						new KeyValuePair<String, EntityAttribute>("Texture",    new EntityAttribute(@"cs\npcsym",  typeof(string), false)),
						new KeyValuePair<String, EntityAttribute>("Width",      new EntityAttribute(SPRITE_WIDTH,  typeof(int),    false)),
						new KeyValuePair<String, EntityAttribute>("Height",     new EntityAttribute(SPRITE_HEIGHT, typeof(int),    false)),
						new KeyValuePair<String, EntityAttribute>("Layer",      new EntityAttribute(0.5f,          typeof(float),  false)),
						new KeyValuePair<String, EntityAttribute>("TargetZone", new EntityAttribute(typeof(Vector2),               false)),
					};
				}

				public static Frame GetEntityFrame() {
					return new Frame(384, 224, 32, 32, 1, 1, 1);
				}
			}
		}
	}
	#endregion
}
