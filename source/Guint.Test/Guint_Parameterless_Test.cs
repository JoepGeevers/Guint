namespace Guint.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Guint_Parameterless_Set_Test
    {
        [TestCleanup]
        public void Cleanup()
        {
            Guint.key = null;
            Guint.vector = null;
        }

        [TestMethod]
        public void Setting_InvalidKeyVectorPair_ThrowsException()
        {
            // arrange
            var exception = default(ArgumentException);
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
        public void Decrypting_WithoutKeyValuePairSet_ThrowsException()
        {
            // arrange
            var exception = default(InvalidOperationException);
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            Guint.Set(key, vector);

            // act
            try
            {
                Guint.Set(key, vector);
            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }

            // assert
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("more than once"));
        }

        [TestMethod]
        public void Encrypting_WithoutKeyValueSet_ThrowsException()
        {
            // arrange
            var exception = default(InvalidOperationException);

            // act
            try
            {
                Guint.EncryptIntoGuid(123);
            }
            catch (InvalidOperationException e)
            {
                exception = e;
            }

            // assert
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("not yet been set"));
        }

        [TestMethod]
        public void WhenSettingValidKeyVectorPair_PairIsUsedForEncryption()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void WhenSettingValidKeyVectorPair_PairIsUsedForDecryption()
        {
			Assert.Fail();
		}

		[TestMethod]
        public void WhenSettingKeyVectorPairAgain_ThrowsException()
        {
			Assert.Fail();
		}

		[TestMethod]
		public void All_NonCachedMethodsAreAlsoTestedForCached()
		{
			Assert.Fail();
		}
	}
}