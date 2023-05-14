namespace Guint.Test
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuintTest
    {
        [TestMethod]
        public void GeneratedKeyAndVectorAreAlwaysDifferentAndWork()
        {
            // act
            var f = Enumerable
                .Range(0, 1000)
                .Select(i => Guint.GenerateKeyAndInitializationVector());

            // assert
            Assert.AreEqual(1000, f.Select(t => t.Key).Distinct().Count());
            Assert.AreEqual(1000, f.Select(t => t.InitializationVector).Distinct().Count());
        }

        [TestMethod]
        public void Int32MinValueCanBeCorrectlyEncrypted()
        {
            // arrange
            var input = Int32.MinValue;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            // act
            var guid = input.EncryptIntoGuid(key, vector);
            var output = guid.DecryptToInt(key, vector);

            // assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void Int32MaxValueCanBeCorrectlyEncrypted()
        {
            // arrange
            var input = Int32.MaxValue;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            // act
            var guid = input.EncryptIntoGuid(key, vector);
            var output = guid.DecryptToInt(key, vector);

            // assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void RandomValueCanBeCorrectlyEncrypted()
        {
            // arrange
            var input = new Random().Next();
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            // act
            var guid = input.EncryptIntoGuid(key, vector);
            var output = guid.DecryptToInt(key, vector);

            // assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void EncryptIntoGuidIsStable()
        {
            // arrange
            var input = 58008;
            (var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "NSqvP1Acyge252v+8w2HyA==");

            // act
            var guid = input.EncryptIntoGuid(key, vector);

            // assert
            Assert.AreEqual(new Guid("aa870058-6b5b-9a61-f58e-c20bb550bfd3"), guid);
        }

        [TestMethod]
        public void DecryptToIntIsStable()
        {
            // arrange
            var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f85}");
            (var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

            // act
            var id = guid.DecryptToInt(key, vector);

            // assert
            Assert.AreEqual(5318008, id);
        }

        [TestMethod]
        public void DecryptingNormalGuidReturnsNull()
        {
            // arrange
            var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f86}");
            (var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

            // act
            var id = guid.DecryptToInt(key, vector);

            // assert
            Assert.IsNull(id);
        }

        [TestMethod]
        public void WhenSettingInvalidKeyVectorPair_ThrowsException()
        {
            // arrange
            var exception = default(Exception);
            var key = "iEoZxvDg38zjvdUF33lo1A==";
            var vector = "axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=";

            // act
            try
            {
                Guint.Set(key, vector);
            }
            catch (ArgumentException e)
            {
                exception = e;
            }

            // assert
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("invalid"));

            Assert.IsTrue(exception.Data.Contains("Key"));
            Assert.AreEqual(exception.Data["Key"], key);

            Assert.IsTrue(exception.Data.Contains("Vector"));
            Assert.AreEqual(exception.Data["Vector"], vector);

            Assert.IsNotNull(exception.InnerException);
        }

        [TestMethod]
        public void WhenCallingDecrypt_WithoutKeyValuePairConfigured_ThrowsException()
        {
        }

        [TestMethod]
        public void WhenCallingEncrypt_WithoutKeyValuePair_ThrowsException()
        {
        }

        [TestMethod]
        public void WhenSettingValidKeyVectorPair_PairIsUsedForEncryption()
        {
        }

        [TestMethod]
        public void WhenSettingValidKeyVectorPair_PairIsUsedForDecryption()
        {
        }

        [TestMethod]
        public void WhenSettingKeyVectorPairAgain_ThrowsException()
        {
        }
    }
}