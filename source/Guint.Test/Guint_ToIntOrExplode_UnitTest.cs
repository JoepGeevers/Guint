﻿namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToIntOrExplode_UnitTest
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ToIntOrExplode_WithoutInitializedSecret_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);
			var guid = Guid.NewGuid();

			// act
			try
			{
				_ = guid.ToIntOrExplode();
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
		public void ToIntOrExplode_IsStable()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f85");
			var secret = "axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvaIShnG8ODfzOO91QXfeWjU";

			Guint.Use(secret);

			// act
			var result = guid.ToIntOrExplode();

			// assert
			Assert.AreEqual(5318008, result);
		}

		[TestMethod]
		public void ToIntOrExplode_RandomGuid_ThrowsException()
		{
			// arrange
			var guid = Guid.NewGuid();
			var secret = Guint.GenerateSecret();
			InvalidOperationException? captivum = null;

			Guint.Use(secret);

			// act
			try
			{
				_ = guid.ToIntOrExplode();
			}
			catch (InvalidOperationException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("could not convert"));
		}
	}
}