namespace Guint
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;

	using OneOf;
	using OneOf.Types;

	public static class Guint
	{
		internal static byte[] key;
		internal static byte[] vector;
		private const string invalidPaddingMessage = "Padding is invalid and cannot be removed.";

		public static string GenerateSecret()
		{
			using (var algorithm = Guint.GetAlgorithm())
			{
				algorithm.GenerateKey();
				algorithm.GenerateIV();

				return ConvertToSecret(algorithm.Key, algorithm.IV);
			}
		}

		public static string ConvertToSecret(string key, string vector)
		{
			var rgbKey = GetRgbKey(key);
			var rgbVector = GetRgbVector(vector);

			return ConvertToSecret(rgbKey, rgbVector);
		}

		private static string ConvertToSecret(byte[] key, byte[] vector)
		{
			var pair = key
				.Concat(vector)
				.ToArray();

			return Convert.ToBase64String(pair);
		}

		public static void Use(string secret)
		{
			using (var algorithm = Guint.GetAlgorithm())
			{
				var bytes = Convert.FromBase64String(secret);

				if (bytes.Length != (algorithm.KeySize / 8) + (algorithm.BlockSize / 8)) // The size of the IV property must be the same as the BlockSize property divided by 8
				{
					throw new ArgumentException($"Secret is not valid. Please use {nameof(Guint.GenerateSecret)} to generate a valid secret", secret);
				}

				var key = bytes
					.Take(algorithm.KeySize / 8)
					.ToArray();

				var vector = bytes
					.Skip(algorithm.KeySize / 8)
					.ToArray();

				Guint.ValidateInitialization(key, vector);

				Guint.key = key;
				Guint.vector = vector;
			}
		}

		public static Guid ToGuid(this Int32 input)
			=> Guint.key == null || Guint.vector == null
				? throw new InvalidOperationException(Guint.GenerateNotInitializedMessageFor(nameof(Guint.ToGuid)))
				: input.ToGuid(Guint.key, Guint.vector);

		private static string GenerateNotInitializedMessageFor(string method) => $"Cannot `{method}` because no secret has been initialized. Use `{nameof(Guint.Use)}` to initialize your personal secret. If you don't have one yet, use `{nameof(Guint.GenerateSecret)}` to generate one.";

		public static OneOf<Int32, NotFound> ToInt(this Guid input)
			=> Guint.key == null || Guint.vector == null
				? throw new InvalidOperationException(Guint.GenerateNotInitializedMessageFor(nameof(Guint.ToInt)))
				: input.ToInt(Guint.key, Guint.vector);

		public static Int32 ToIntOrDefault(this Guid input)
			=> Guint.key == null || Guint.vector == null
				? throw new InvalidOperationException(Guint.GenerateNotInitializedMessageFor(nameof(Guint.ToIntOrDefault)))
				: input.ToIntOrDefault(Guint.key, Guint.vector);

		public static Int32 ToIntOrExplode(this Guid input)
			=> Guint.key == null || Guint.vector == null
				? throw new InvalidOperationException(Guint.GenerateNotInitializedMessageFor(nameof(Guint.ToIntOrExplode)))
				: input.ToIntOrExplode(Guint.key, Guint.vector);

		private static Guid ToGuid(this Int32 input, byte[] key, byte[] vector)
		{
			using (var algorithm = Guint.GetAlgorithm())
			using (var encryptor = algorithm.CreateEncryptor(key, vector))
			{
				var bytes = BitConverter.GetBytes(input);

				if (false == BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes);
				}

				return Guint.Crypt(bytes, encryptor)
					.Match(
						b => new Guid(b),
						notfound => throw new InvalidOperationException("This error should really, really never happen, because any 32-bit int fits into a 128-bit Guid. Call me!"));
			}
		}

		private static OneOf<Int32, NotFound> ToInt(this Guid guid, byte[] key, byte[] vector)
		{
			using (var algorithm = Guint.GetAlgorithm())
			using (var decryptor = algorithm.CreateDecryptor(key, vector))
			{
				return Guint.Crypt(guid.ToByteArray(), decryptor)
					.Match<OneOf<Int32, NotFound>>(
						bytes => BitConverter.ToInt32(bytes, 0),
						notfound => notfound);
			}
		}

		private static Int32 ToIntOrDefault(this Guid guid, byte[] key, byte[] vector)
			=> Guint.ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => default(Int32));

		private static Int32 ToIntOrExplode(this Guid guid, byte[] key, byte[] vector)
			=> Guint.ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => throw new InvalidOperationException("Could not convert Guid to an Int32 with the specified key and vector"));

		internal static Aes GetAlgorithm()
		{
			var algorithm = Aes.Create();

			algorithm.FeedbackSize = 8;
			algorithm.Mode = CipherMode.CBC;
			algorithm.Padding = PaddingMode.PKCS7;
			algorithm.BlockSize = 16 * 8;
			algorithm.KeySize = 32 * 8;
			
			return algorithm;
		}

		private static void ValidateInitialization(byte[] key, byte[] vector)
		{
			if (Guint.key == null || Guint.vector == null)
			{
				Guint.Validate(key, vector);

				return;
			}

			if (key.SequenceEqual(Guint.key) && vector.SequenceEqual(Guint.vector))
			{
				return;
			}

			throw new InvalidOperationException("Key and vector cannot be changed");
		}

		private static void Validate(byte[] key, byte[] vector) => Guint.ToGuid(123456789, key, vector);

		private static byte[] GetRgbKey(string key) => GetRgb(key, "key", 32);

		private static byte[] GetRgbVector(string vector) => GetRgb(vector, "vector", 16);

		private static byte[] GetRgb(string input, string name, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException(name);
			}

			try
			{
				var bytes = Convert.FromBase64String(input);

				if (bytes.Length == length)
				{
					return bytes;
				}
			}
			catch (FormatException)
			{
			}

			throw new ArgumentException($"Value must be a base 64 encoded byte[{length}]", name);
		}
		private static OneOf<byte[], NotFound> Crypt(byte[] data, ICryptoTransform transform)
		{
			using (var memory = new MemoryStream())
			using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write))
			{
				crypto.Write(data, 0, data.Length);

				try
				{
					crypto.FlushFinalBlock();
				}
				catch (CryptographicException e) when (e.Message == invalidPaddingMessage)
				{
					return new NotFound();
				}
				finally
				{
					try
					{
						crypto.Close();
					}
					catch (CryptographicException e) when (e.Message == invalidPaddingMessage)
					{
						// In .NET Framework, when the CryptoStream is disposed, the stream is flushed, throwing the exact same CryptographicException again
						// Closing the stream ourselves and catching the CryptographicException allows us to return default
					}
				}

				return memory.ToArray();
			}
		}
	}
}