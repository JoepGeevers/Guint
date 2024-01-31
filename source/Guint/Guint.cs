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
			using (var algorithm = GetAlgorithm())
			{
				algorithm.GenerateKey();
				algorithm.GenerateIV();

				var key = Convert.ToBase64String(algorithm.Key);
				var vector = Convert.ToBase64String(algorithm.IV);

				return (key, vector);
			}
		}

		internal static Aes GetAlgorithm() => Aes.Create();

		// todo: put DecryptToInt back
		// todo: put all these methods in order maybe
		[Obsolete("Please use ToGuid instead")]
		public static Guid EncryptIntoGuid(this Int32 input, string key, string vector) => input.ToGuid(key, vector);

		public static Guid ToGuid(this Int32 input, string key, string vector)
		{
			using (var algorithm = Guint.GetAlgorithm())
			using (var encryptor = algorithm.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(vector)))
			{
				var bytes = BitConverter.GetBytes(input);

				if (false == BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes);
				}

				return Crypt(bytes, encryptor)
					.Match(
						b => new Guid(b),
						notfound => throw new InvalidOperationException("This error should really, really never happen, because any 32-bit int fits into a 128-bit Guid. Call me!"));
			}
		}

		[Obsolete("Please use ToInt instead")]
		public static int? DecryptToInt(this Guid input, string key, string vector)
			=> input.ToInt(key, vector)
				.Match(
					i => i,
					notfound => default(int?));

		public static OneOf<int, NotFound> ToInt(this Guid guid, string key, string vector)
		{
			using (var algorithm = Guint.GetAlgorithm())
			using (var decryptor = algorithm.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(vector)))
			{
				return Crypt(guid.ToByteArray(), decryptor)
					.Match<OneOf<int, NotFound>>(
						bytes => BitConverter.ToInt32(bytes, 0),
						notfound => notfound);
			}
		}

		public static int ToIntOrDefault(this Guid guid, string key, string vector)
			=> ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => default(int));

		public static int ToIntOrExplode(this Guid guid, string key, string vector)
			=> ToInt(guid, key, vector)
				.Match(
					i => i,
					notfound => throw new Exception()); // what kind of exception do we want to throw here?

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
			if (Guint.key == key && Guint.vector == vector)
			{
				return;
			}

			if (Guint.key != null || Guint.vector != null)
			{
				throw new InvalidOperationException("Key and vector cannot be changed");
			}

			try
			{
				ToGuid(123, key, vector);
			}
			catch (Exception e)
			{
				throw new ArgumentException("Specified key and vector are invalid. See inner exception for more details", e)
				{
					Data = {
						{ "Key", key },
						{ "Vector", vector },
					},
				};
			}

			Guint.key = key;
			Guint.vector = vector;
		}

		public static Guid ToGuid(this int input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToGuid` because key and vector have not been initialized")
				: input.ToGuid(key, vector);

		public static OneOf<int, NotFound> ToInt(this Guid input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `ToInt` because key and vector have not been initialized")
				: input.ToInt(key, vector);

		// todo 4: create ToIntOrDefault the nicely uses other methods for key vector checks 
		public static int ToIntOrDefault(this Guid input) => input.ToIntOrDefault(key, vector);

		// todo 5: create ToIntOrExplode the nicely uses other methods for key vector checks 
		public static int ToIntOrExplode(this Guid input) => input.ToIntOrExplode(key, vector);
	}
}