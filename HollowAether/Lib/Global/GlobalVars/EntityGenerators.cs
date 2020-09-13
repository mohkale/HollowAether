#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Encryption;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
#endregion

using System.Reflection;

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class EntityGenerators {
			static EntityGenerators() {
				foreach (Type type in YieldTypesFromAssemblies(typeof(IInitializableEntity))) {
					MethodInfo info = type.GetMethod(
						"GetAdditionalEntityAttributes", // Get The Method That Defines Any Additional EntityAttributes
						BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
					);

					var attributes = (ICollection<KeyValuePair<String, EntityAttribute>>)info.Invoke(null, new object[] { });

					GameEntity value = new GameEntity(type.Name.ToLower(), attributes.ToArray());
					entityTypes[type.Name.ToLower()] = new Tuple<Type, GameEntity>(type, value);
				}
			}

			private static IEnumerable<Type> YieldTypesFromAssemblies(Type desired) {
				Func<Type, bool> IsValid = (type) => Misc.DoesExtend(type, desired) && !type.IsInterface;

				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					foreach (Type type in assembly.GetTypes()) {
						if (type != typeof(object) && IsValid(type))
							yield return type; // Is Valid Type
					}
				}
			}

			public static System.Drawing.Rectangle? GetAnimationFromEntityName(string entityName) {
				try {
					Type type = entityTypes[entityName].Item1; // Get Entity Type

					MethodInfo info = type.GetMethod(
						"GetEntityFrame", // Get The Method That Defines What Frame To Display To Caller
						BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
					);

					return ((Frame)info.Invoke(null, new object[] { })).ToDrawingRect();
				} catch { return null; }
			}

			public static GameEntity StringToGameEntity(String entityName) {
				try { return entityTypes[entityName.ToLower()].Item2.Clone() as GameEntity; } catch {
					throw new HollowAetherException($"Could Not Find Entity '{entityName}'");
				}
			}

			public static IMonoGameObject StringToMonoGameObject(String className) {
				Type entityType; // Type corresponding to desired class name

				try { entityType = entityTypes[className.ToLower()].Item1; } catch {
					throw new Exception($"Couldn't Find Entity Of Type '{className}'");
				}

				IMonoGameObject IMGO = (IMonoGameObject)Activator.CreateInstance(entityType);

				return IMGO; // Made using default parameterless constructor, Should be fine
			}

			public static Dictionary<String, Tuple<Type, GameEntity>> entityTypes = new Dictionary<String, Tuple<Type, GameEntity>>();
		}
	}
}
