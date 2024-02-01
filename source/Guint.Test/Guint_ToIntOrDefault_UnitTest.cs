namespace Guint.Test
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
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenKeyIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = (default(string), "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentNullException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("key", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value cannot be null"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenKeyIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("I'm not base64", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("key", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value must be a base 64 encoded byte[32]"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenKeyIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("NSqvP1Acyge252v+8w2HyA==", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("key", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value must be a base 64 encoded byte[32]"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenVectorIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", default(string));

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentNullException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("vector", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value cannot be null"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenVectorIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "I'm not base64");

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("vector", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value must be a base 64 encoded byte[16]"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_WhenVectorIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=");

			// act
			try
			{
				_ = new Guid().ToIntOrDefault(key, vector);
			}
			catch (ArgumentException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.AreEqual("vector", captivum.ParamName);
			Assert.IsTrue(captivum.Message.Contains("Value must be a base 64 encoded byte[16]"));
		}

		[TestMethod]
		public void ToIntOrDefault_WithSpecifiedKeyVector_IsStable()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f85");

			// act
			var result = guid.ToIntOrDefault("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// assert
			Assert.AreEqual(5318008, result);
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
		public void ToIntOrDefault_WithConfiguredKeyVector_IsStable()
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
		public void ToIntOrDefault_RandomGuid_WithSpecifiedKeyVector_ReturnsZero()
		{
			// arrange
			var guid = Guid.NewGuid();

			// act
			var result = guid.ToIntOrDefault("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

			// assert
			Assert.AreEqual(0, result);
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