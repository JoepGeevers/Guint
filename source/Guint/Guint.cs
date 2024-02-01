namespace Guint
{
	using System;
	using System.IO;
	using System.Security.Cryptography;

	using OneOf;
	using OneOf.Types;

	public static class Guint
	{
		internal static string key;
		internal static string vector;

		private const string invalidPaddingMessage = "Padding is invalid and cannot be removed.";

		public static (string Key, string InitializationVector) GenerateKeyAndInitializationVector()
		{
			using (var algorithm = Guint.GetAlgorithm())
			{
				algorithm.GenerateKey();
				algorithm.GenerateIV();

				var key = Convert.ToBase64String(algorithm.Key);
				var vector = Convert.ToBase64String(algorithm.IV);

				return (key, vector);
			}
		}

		internal static Aes GetAlgorithm() => Aes.Create();

		// todo: 7 put DecryptToInt back and put both under test after we have put the underlaying methods completely under test
		// todo: 8 put all these methods in order maybe
		[Obsolete("Use `ToGuid` instead")]
		public static Guid EncryptIntoGuid(this Int32 input, string key, string vector) => input.ToGuid(key, vector);

		public static Guid ToGuid(this Int32 input, string key, string vector)
		{
			var rgbKey = Guint.GetRgbKey(key);
			var rgbVector = Guint.GetRgbVector(vector);

			using (var algorithm = Guint.GetAlgorithm())
			using (var encryptor = algorithm.CreateEncryptor(rgbKey, rgbVector))
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

		[Obsolete("Use `ToInt`, `ToIntOrDefault` or `ToIntOrExplode` instead")]
		public static Int32? DecryptToInt(this Guid input, string key, string vector)
			=> input.ToInt(key, vector)
				.Match(
					i => i,
					notfound => default(Int32?));

		public static OneOf<Int32, NotFound> ToInt(this Guid guid, string key, string vector) // todo: do we actually need to expose this method? should't Guintv2 always work with pre-initialization?
		{
			var rgbKey = Guint.GetRgbKey(key);
			var rgbVector = Guint.GetRgbVector(vector);

			using (var algorithm = Guint.GetAlgorithm())
			using (var decryptor = algorithm.CreateDecryptor(rgbKey, rgbVector))
			{
				return Guint.Crypt(guid.ToByteArray(), decryptor)
					.Match<OneOf<Int32, NotFound>>(
						bytes => BitConverter.ToInt32(bytes, 0),
						notfound => notfound);
			}
		}

		public static Int32 ToIntOrDefault(this Guid guid, string key, string vector)
			=> Guint.ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => default(Int32));

		public static Int32 ToIntOrExplode(this Guid guid, string key, string vector)
			=> Guint.ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => throw new InvalidOperationException("Could not convert Guid to an Int32 with the specified key and vector"));

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

		public static void Set(string key, string vector)
		{
			// todo: extract methods to make intent more clear
			if (Guint.key == key && Guint.vector == vector)
			{
				return;
			}

			if (Guint.key != null || Guint.vector != null)
			{
				throw new InvalidOperationException("Key and vector cannot be changed");
			}

			Guint.ToGuid(123, key, vector);

			Guint.key = key;
			Guint.vector = vector;
		}

		public static Guid ToGuid(this Int32 input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToGuid` because key and vector have not been initialized")
				: input.ToGuid(key, vector);

		public static OneOf<Int32, NotFound> ToInt(this Guid input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToInt` because key and vector have not been initialized")
				: input.ToInt(key, vector);

		public static Int32 ToIntOrDefault(this Guid input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToInt` because key and vector have not been initialized")
				: input.ToIntOrDefault(key, vector);

		public static Int32 ToIntOrExplode(this Guid input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToInt` because key and vector have not been initialized")
				: input.ToIntOrExplode(key, vector);
	}
}