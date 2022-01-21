namespace Guint
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class Extension
    {
        public static Guid EncryptIntoGuid(this Int32 input, string key, string vector)
        {
            using (var algorithm = new RijndaelManaged())
            {
                algorithm.Key = Convert.FromBase64String(key);
                algorithm.IV = Convert.FromBase64String(vector);

                using (var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
                using (var memory = new MemoryStream())
                using (var crypto = new CryptoStream(memory, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(crypto))
                        writer.Write(input.ToString());

                    return new Guid(memory.ToArray());
                }
            }
        }

        public static Int32? DecryptToInt(this Guid guid, string key, string vector)
        {
            using (var algorithm = new RijndaelManaged())
            {
                algorithm.Key = Convert.FromBase64String(key);
                algorithm.IV = Convert.FromBase64String(vector);

                using (var decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
                using (var memory = new MemoryStream(guid.ToByteArray()))
                using (var crypto = new CryptoStream(memory, decryptor, CryptoStreamMode.Read))
                {
                    try
                    {
                        using (var reader = new StreamReader(crypto))
                        {
                            return int.Parse(reader.ReadToEnd());
                        }
                    }
                    catch
                    {
                        return default;
                    }
                }
            }
        }
    }
}