namespace Guint
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class Extension
    {
        public static Guid EncryptIntoGuid(this Int32 input, string key, string vector)
        {
            using (var algorithm = Guint.GetAlgorithm())
            {
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
        }

        public static Int32? DecryptToInt(this Guid guid, string key, string vector)
        {
            using (var algorithm = Guint.GetAlgorithm())
            {
                using (var decryptor = algorithm.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(vector)))
                {
                    return BitConverter.ToInt32(Crypt(guid.ToByteArray(), decryptor), 0);
                }
            }
        }

        private static byte[] Crypt(byte[] data, ICryptoTransform transform)
        {
            using (var memory = new MemoryStream())
            using (var crypto = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                crypto.Write(data, 0, data.Length);
                crypto.FlushFinalBlock();

                return memory.ToArray();
            }
        }
    }
}