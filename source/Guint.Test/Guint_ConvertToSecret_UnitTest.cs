namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ConvertToSecret_UnitTest
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ConvertToSecret_WhenKeyIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;

			// act
			try
			{
				var secret = Guint.ConvertToSecret(default(string), "NSqvP1Acyge252v+8w2HyA==");
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
		public void ConvertToSecret_WhenKeyIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;

			// act
			try
			{
				var secret = Guint.ConvertToSecret("I'm not base64", "NSqvP1Acyge252v+8w2HyA==");
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
		public void ConvertToSecret_WhenKeyIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;

			// act
			try
			{
				Guint.ConvertToSecret("NSqvP1Acyge252v+8w2HyA==", "NSqvP1Acyge252v+8w2HyA==");
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
		public void ConvertToSecret_WhenVectorIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;

			// act
			try
			{
				Guint.ConvertToSecret("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", default(string));
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
		public void ConvertToSecret_WhenVectorIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;

			// act
			try
			{
				Guint.ConvertToSecret("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "I'm not base64");
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
		public void ConvertToSecret_WhenVectorIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;

			// act
			try
			{
				Guint.ConvertToSecret("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=");
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
		public void ConvertToSecret_WhenKeyAndVectorAreValid_ReturnsValidSecret()
		{
			// arrange
			var input = 12345;

			var key = "axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=";
			var vector = "iEoZxvDg38zjvdUF33lo1A==";

			// act
			var secret = Guint.ConvertToSecret(key, vector);

			Guint.Use(secret);

			var output = input
				.ToGuid()
				.ToIntOrExplode();

			// assert
			Assert.AreEqual(input, output);
		}

		[TestMethod]
		public void ConvertToSecret_IsStable()
		{
			// arrange
			var expected = "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ1Kq8/UBzKB7bna/7zDYfI";

			// act
			var actual = Guint.ConvertToSecret("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "NSqvP1Acyge252v+8w2HyA==");

			// assert
			Assert.AreEqual(expected, actual);
		}
	}
}