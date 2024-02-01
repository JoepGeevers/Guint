namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_EncryptIntoGuid_UnitTest
    {
		[TestMethod]
		public void EncryptIntoGuid_WhenKeyIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = (default(string), "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_WhenKeyIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("I'm not base64", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_WhenKeyIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("NSqvP1Acyge252v+8w2HyA==", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_WhenVectorIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", default(string));

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_WhenVectorIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "I'm not base64");

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_WhenVectorIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=");

			// act
			try
			{
				_ = 58008.EncryptIntoGuid(key, vector);
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
		public void EncryptIntoGuid_IsStable()
		{
			// arrange
			var input = 58008;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "NSqvP1Acyge252v+8w2HyA==");

			// act
			var guid = input.EncryptIntoGuid(key, vector);

			// assert
			Assert.AreEqual(new Guid("aa870058-6b5b-9a61-f58e-c20bb550bfd3"), guid);
		}
	}
}