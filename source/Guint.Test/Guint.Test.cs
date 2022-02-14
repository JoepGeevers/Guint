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
    }
}