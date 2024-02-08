namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToInt_UnitTest
    {
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ToInt_WithoutInitializedSecret_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);
			var guid = Guid.NewGuid();

			// act
			try
			{
				_ = guid.ToInt();
			}
			catch (InvalidOperationException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("no secret has been initialized"));
		}

		[TestMethod]
		public void ToInt_IsStable()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f85");
			var secret = "axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvaIShnG8ODfzOO91QXfeWjU";

			Guint.Use(secret);

			// act
			var result = guid.ToInt();

			// assert
			result.Switch(
				i => Assert.AreEqual(5318008, i),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void ToInt_RandomGuid_ReturnsNotFound()
		{
			// arrange
			var guid = Guid.NewGuid();
			var secret = Guint.GenerateSecret();

			Guint.Use(secret);

			// act
			var result = guid.ToInt();

			// assert
			result.Switch(
				i => Assert.Fail(),
				notfound => { });
		}
	}
}