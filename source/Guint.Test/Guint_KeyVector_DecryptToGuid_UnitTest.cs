namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_KeyVector_DecryptToGuid_UnitTest
    {
		[TestMethod]
		public void Decryption_IsStable()
		{
			// arrange
			var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f85}");
			(var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var id = guid.DecryptToInt(key, vector);

			// assert
			Assert.AreEqual(5318008, id);
		}

		[TestMethod]
		public void Decrypting_RandomGuid_ReturnsNull()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f86");
			(var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var id = guid.DecryptToInt(key, vector);

			// assert
			Assert.IsNull(id);
		}
	}
}