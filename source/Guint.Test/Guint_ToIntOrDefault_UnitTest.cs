﻿namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToIntOrDefault_UnitTest
    {
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ToIntOrDefault_WithoutInitializedKeyVector_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);
			var guid = Guid.NewGuid();

			// act
			try
			{
				_ = guid.ToIntOrDefault();
			}
			catch (InvalidOperationException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("not been initialized"));
		}

		[TestMethod]
		public void ToIntOrDefault_IsStable()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f85");
			Guint.Set("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var result = guid.ToIntOrDefault();

			// assert
			Assert.AreEqual(5318008, result);
		}

		[TestMethod]
		public void ToIntOrDefault_RandomGuid_WithConfiguredKeyVector_ReturnsZero()
		{
			// arrange
			var guid = Guid.NewGuid();
			Guint.Set("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// act
			var result = guid.ToIntOrDefault();

			// assert
			Assert.AreEqual(0, result);
		}
	}
}