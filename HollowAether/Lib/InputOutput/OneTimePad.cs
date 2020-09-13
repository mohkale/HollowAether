#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace HollowAether.Lib.Encryption {
	/// <summary>Class to securely encrypt a given peice of data</summary>
	public sealed class OneTimePad : Object {
		/// <summary>Storage class for OneTimePad exceptions</summary>
		public static class Exceptions {
			/// <summary>Could not find a valid key</summary>
			public class KeyNotSetException : Exception {
				#region InheritanceClone
				public KeyNotSetException() : base("Key & Alt not set") { }

				public KeyNotSetException(String msg) : base(msg) { }

				public KeyNotSetException(String msg, Exception inner) : base(msg, inner) { }
				#endregion
			}

			/// <summary>Could convert char to int or int to char</summary>
			public class CharConversionException : Exception {
				#region InheritanceClone
				public CharConversionException(String msg) : base(msg) { }

				public CharConversionException(String msg, Exception inner) : base(msg, inner) { }
				#endregion
			}
		}

		/// <summary>Delegate to store methods which convert inbetween encrypted and plain text</summary>
		/// <param name="X">Integer value of 1st value</param> <param name="Y">Integer value of 2nd value</param>
		/// <param name="maxX">The gratest value that can be returned</param>
		/// <returns>Integer position of encrypted or decrypted character</returns>
		private delegate int Cryptifier(int X, int Y, int maxX);

		/// <summary>Initializzer for Onte Time Pad</summary>
		/// <param name="_key">Initial Key for pad to use</param>
		public OneTimePad(String _key = null) { padKey = _key; }

		private String GetKey(String alternate = null) {
			if (alternate != null) return alternate;
			else if (KeySet()) return padKey; // class has key	
			else throw new Exceptions.KeyNotSetException();

			// return KeySet() ? key : alternate; // Unsecure
		}

		/// <summary>Checks wether stored key is valid</summary>
		/// <returns>Boolean indicating wether class-key can be used</returns>
		public Boolean KeySet() { return !(String.IsNullOrWhiteSpace(padKey)); }

		/// <summary>Cryptifies text using given character array</summary>
		/// <param name="text">Plain text to encrypt/decrypt</param>
		/// <param name="key">Key to encrypt/decrypt with</param>
		/// <param name="alphabet">Character array to use</param>
		/// <param name="encrypt">Wether to encrypt or decrypt</param>
		/// <returns>Encrypted or Decrypted version of plaintext</returns>
		private String CryptifyWithAlphabet(String text, String key, Char[] alphabet, bool encrypt) {
			Cryptifier modificationMethod = (encrypt) ? _encrypter : _decrypter;
			StringBuilder container = new StringBuilder(text.Length); // container
			key = GetKey(key); // Ensure key is of correct type and format

			foreach (int X in Enumerable.Range(0, text.Length)) {
				if (!alphabet.Contains(text[X])) {
					throw new Exceptions.CharConversionException(
						$"'{text[X]}' Not Found in Alphabet"
					);
				} else if (!alphabet.Contains(key[X % key.Length])) {
					throw new Exceptions.CharConversionException(
						$"'{key[X % key.Length]}' Not Found in Alphabet"
					);
				}

				int textIndex = Array.FindIndex(alphabet, (Y) => Y == text[X]);
				int keyIndex = Array.FindIndex(alphabet, (Y) => Y == key[X % key.Length]);

				int encryptedTextIndex = modificationMethod(textIndex, keyIndex, alphabet.Length - 1);
				container.Append(alphabet[encryptedTextIndex]); // convert new index to character
			}

			return container.ToString();
		}

		/// <summary>Encrypts or Decrypts given plaintext with given key</summary>
		/// <param name="text">Plain text to encrypt/decrypt</param>
		/// <param name="key">Key to encrypt/decrypt with</param>
		/// <param name="encrypt">Wether to encrypt or decrypt</param>
		/// <returns>Encrypted or Decrypted version of plaintext</returns>
		private String Cryptify(String text, String key, bool encrypt) {
			Cryptifier modificationMethod = (encrypt) ? _encrypter : _decrypter;
			StringBuilder container = new StringBuilder(text.Length); // container
			key = GetKey(key); // Ensure key is of correct type and format

			foreach (int X in Enumerable.Range(0, text.Length)) {
				int plainTextIndex, keyTextIndex, encryptedTextIndex;

				try {
					plainTextIndex = CharToInt(text[X]);
					keyTextIndex = CharToInt(key[X % key.Length]);
				} catch (OverflowException) { // Char outside bounds of index, unlikely but possible
					throw new Exceptions.CharConversionException(
						$"'{text[X]}' Or '{key[X % key.Length]}' Could not be converted"
					);
				}

				encryptedTextIndex = modificationMethod(plainTextIndex, keyTextIndex, maxCharacterIndex);
				container.Append(IntToChar(encryptedTextIndex)); // add encrypted char to text container
			}

			return container.ToString();
		}

		/// <summary>Encrypts given text</summary>
		/// <param name="text">Plain text to encrypt</param>
		/// <param name="key">Optional key to encrypt with</param>
		/// <returns>Encrypted version of plain text</returns>
		public String Encrypt(String text, String key = null) {
			return Cryptify(text, key, true);
		}

		/// <summary>Decrypts given text</summary>
		/// <param name="text">Plain text to decrypt</param>
		/// <param name="key">Optional key to decrypt with</param>
		/// <returns>Decrypted version of plain text</returns>
		public String Decrypt(String text, String key = null) {
			return Cryptify(text, key, false);
		}

		/// <summary>Encrypts given text using character array</summary>
		/// <param name="text">Plain text to Encrypt</param>
		/// <param name="key">Optional key to Encrypt with</param>
		/// <param name="alphabet">Character array to encrypt with</param>
		/// <returns>Encrypted version of plain text</returns>
		public String EncryptWithAlphabet(String text, String key = null, params Char[] alphabet) {
			return CryptifyWithAlphabet(text, key, alphabet, true);
		}


		/// <summary>Decrypts given text</summary>
		/// <param name="text">Plain text to decrypt</param>
		/// <param name="key">Optional key to decrypt with</param>
		/// <param name="alphabet">Character array to decrypt with</param>
		/// <returns>Decrypted version of plain text</returns>
		public String DecryptWithAlphabet(String text, String key = null, params Char[] alphabet) {
			return CryptifyWithAlphabet(text, key, alphabet, false);
		}

		/// <summary>Base key used when no argument key is passed</summary>
		public String key { get { return padKey; } set { padKey = value; } }

		private Cryptifier _encrypter = new Cryptifier((int X, int Y, int Z) => (X + Y < Z) ? X + Y : X + Y - Z);

		private Cryptifier _decrypter = new Cryptifier((int X, int Y, int Z) => (X - Y >= 0) ? X - Y : X - Y + Z);

		public Func<int, char> IntToChar = new Func<int, char>(Convert.ToChar); // Converts integer to character

		public Func<char, int> CharToInt = new Func<char, int>(Convert.ToInt32); // Converts Character to integer

		private static int maxCharacterIndex = Char.MaxValue; private String padKey;
	}
}
