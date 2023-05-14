namespace Guint.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Guint_CachedKeyVector_Test
    {
        [TestCleanup]
        public void Cleanup()
        {
            Guint.key = null;
            Guint.vector = null;
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