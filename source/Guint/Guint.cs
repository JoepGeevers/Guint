namespace Guint
{
    using System;
    using System.Security.Cryptography;

    public class Guint
    {
        public static (string Key, string InitializationVector) GenerateKeyAndInitializationVector()
        {
            using (var algorithm = Guint.GetAlgorithm())
            {
                algorithm.GenerateKey();
                algorithm.GenerateIV();

                var key = Convert.ToBase64String(algorithm.Key);
                var initializationVector = Convert.ToBase64String(algorithm.IV);

                return (key, initializationVector);
            }
        }

        internal static Aes GetAlgorithm()
        {
            return Aes.Create();
        }
    }
}