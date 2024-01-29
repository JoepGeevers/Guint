﻿namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_DecryptToGuid_UnitTest
    {
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void Decryption_WithSpecifiedKeyVector_IsStable()
		{
			// arrange
			var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f85}");

			// act
			var result = guid.DecryptToInt("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// assert
			result.Switch(
				i => Assert.AreEqual(5318008, i),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void Decryption_WithConfiguredKeyVector_IsStable()
		{
			// arrange
			var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f85}");
			Guint.Set("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var result = guid.DecryptToInt();

			// assert
			result.Switch(
				i => Assert.AreEqual(5318008, i),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void Decrypting_RandomGuid_WithSpecifiedKeyVector_ReturnsNotFound()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f86");

			// act
			var result = guid.DecryptToInt("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// assert
			result.Switch(
				i => Assert.Fail(),
				notfound => { });
		}

		[TestMethod]
		public void Decrypting_RandomGuid_WithConfiguredKeyVector_ReturnsNotFound()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f86");
			Guint.Set("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var result = guid.DecryptToInt();

			// assert
			result.Switch(
				i => Assert.Fail(),
				notfound => { });
		}
	}
}