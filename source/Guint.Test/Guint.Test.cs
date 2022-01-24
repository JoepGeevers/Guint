namespace Guint.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        // static key vi works
        // random key vi works
        // max int value works
        // min int value works

        // generate key vector are always different

        [TestMethod]
        public void GeneratedKeyAndVectorAreAlwaysDifferentAndWork()
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
    }
}