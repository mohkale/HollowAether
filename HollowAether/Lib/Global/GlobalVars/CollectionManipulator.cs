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

namespace HollowAether.Lib {
	public static partial class GlobalVars {

		public static class CollectionManipulator {
			public static T[] GetSubArray<T>(T[] array, int index, int length) {
				T[] result = new T[length];
				Array.Copy(array, index, result, 0, length);
				return result;
			}

			public static K DictionaryGetKeyFromValue<K, V>(Dictionary<K, V> dict, V value) {
				foreach (K key in dict.Keys) {
					if (dict[key].Equals(value)) // If value found
						return key; // Then return found key
				}

				throw new Exception($"Value {value.ToString()} Not Found In Dictionary");
			}

			public static void SwapListValues<T>(IList<T> list, int indexA, int indexB) {
				T tmp		 = list[indexA];
				list[indexA] = list[indexB];
				list[indexB] = tmp;
			}
		}
	}
}
