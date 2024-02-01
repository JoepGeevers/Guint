namespace Guint.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Guint_Set_Test
    {
        [TestInitialize]
        public void TestInitialize()
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
		public void ToInt_WithoutKeyValuePairSet_ThrowsException()
		{
			// arrange
			var exception = default(InvalidOperationException);

			// act
			try
			{
				Guid.NewGuid().ToInt();
			}
			catch (InvalidOperationException e)
			{
				exception = e;
			}

			// assert
			Assert.IsNotNull(exception);
			Assert.IsTrue(exception.Message.Contains("not been initialized"));
		}

		[TestMethod]
        public void WhenSettingTheSameKeyVectorPairAgain_DoesNotThrowException()
        {
			// arrange
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			Guint.Set(key, vector);

			// act
			Guint.Set(key, vector);

            // assert
            { }
		}

		[TestMethod]
		public void WhenSettingDifferentKeyVectorPair_ThrowsException()
		{
			// arrange
			var exception = default(InvalidOperationException);
			(var key1, var vector1) = Guint.GenerateKeyAndInitializationVector();
			(var key2, var vector2) = Guint.GenerateKeyAndInitializationVector();

			Guint.Set(key1, vector1);

			// act
			try
			{
				Guint.Set(key2, vector2);
			}
			catch (InvalidOperationException e)
			{
				exception = e;
			}

			// assert
			Assert.IsNotNull(exception);
			Assert.IsTrue(exception.Message.Contains("cannot be changed"));
		}
	}
}