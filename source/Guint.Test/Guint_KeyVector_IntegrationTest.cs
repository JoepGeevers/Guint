namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
    public class Guint_KeyVector_IntegrationTest
    {
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
            var input = 765437653;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            // act
            var guid = input.EncryptIntoGuid(key, vector);
            var output = guid.DecryptToInt(key, vector);

            // assert
            Assert.AreEqual(input, output);
        }
    }
}