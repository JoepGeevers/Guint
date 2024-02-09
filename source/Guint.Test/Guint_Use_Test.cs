namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_Use_Test
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}


		[TestMethod]
		public void WhenSettingTheSameKeyVectorPairAgain_DoesNotThrowException()
		{
			// arrange
			var secret = Guint.GenerateSecret();

			// act
			Guint.Use(secret);
			Guint.Use(secret);

			// assert
			{ }
		}

		[TestMethod]
		public void WhenSettingDifferentKeyVectorPair_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);
			var secret1 = Guint.GenerateSecret();
			var secret2 = Guint.GenerateSecret();

			// act
			Guint.Use(secret1);

			try
			{
				Guint.Use(secret2);
			}
			catch (InvalidOperationException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("cannot be changed"));
		}
	}
}