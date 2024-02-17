namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_IntegrationTest
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.secret = null;
		}

		[TestMethod]
		public void Int32MinValue_CanBeCorrectlyEncrypted()
		{
			// arrange
			var input = Int32.MinValue;
			var secret = Guint.GenerateSecret();

			Guint.Use(secret);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void Int32MaxValue_CanBeCorrectlyEncrypted()
		{
			// arrange
			var input = Int32.MaxValue;
			var secret = Guint.GenerateSecret();

			Guint.Use(secret);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void RandomValue_CanBeCorrectlyEncrypted()
		{
			// arrange
			var input = 765437653;
			var secret = Guint.GenerateSecret();

			Guint.Use(secret);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}
	}
}