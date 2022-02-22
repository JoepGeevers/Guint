namespace Guint.Test
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuintTests
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
    }
}