namespace Guint
{
    using System;
    using System.IO;
	using System.Security.Cryptography;

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


        public static Guid EncryptIntoGuid(this Int32 input, string key, string vector)
        {
            using (var algorithm = Guint.GetAlgorithm())
            using (var encryptor = algorithm.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(vector)))
            {
                var bytes = BitConverter.GetBytes(input);

                if (false == BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                return new Guid(Crypt(bytes, encryptor));
            }
        }

		public static Int32? DecryptToInt(this Guid guid, string key, string vector)
        {
            using (var algorithm = Guint.GetAlgorithm())
            using (var decryptor = algorithm.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(vector)))
            {
                var bytes = Crypt(guid.ToByteArray(), decryptor);

                if (null == bytes)
                {
                    return default;
                }

                return BitConverter.ToInt32(bytes, 0);
            }
        }

		private static byte[] Crypt(byte[] data, ICryptoTransform transform)
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
                    return default;

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
            if (Guint.key != null || Guint.vector != null)
            {
                throw new InvalidOperationException("Cannot set key and vector more than once");
            }

            try
            {
                EncryptIntoGuid(123, key, vector);
            }
            catch(Exception e)
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

		public static Guid EncryptIntoGuid(this int input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `EncryptIntoGuid` because key and vector have not been set")
				: input.EncryptIntoGuid(key, vector);

		public static int? DecryptToInt(this Guid input)
			=> key == null || vector == null
				? throw new InvalidOperationException("Cannot `DecryptToInt` because key and vector have not been set")
				: input.DecryptToInt(key, vector);
	}
}