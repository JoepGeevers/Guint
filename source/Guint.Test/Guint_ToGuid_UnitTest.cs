namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToGuid_UnitTest
    {
		// todo: 6 validate key and vector whenever they are used
		// todo: 1 throw a nice exception when the key and vector are null for both toint and toguid
		// todo: 2 throw a nice exception when key and vector are not base64 both toint and toguid
		// todo: 3 throw a nice exception when key and vector are not whatever they are supposed to be both toint and toguid

		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ToGuid_WithSpecifiedKeyVector_WhenKeyIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = (default(string), "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_WhenKeyIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("I'm not base64", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_WhenKeyIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("NSqvP1Acyge252v+8w2HyA==", "NSqvP1Acyge252v+8w2HyA==");

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_WhenVectorIsNull_ThrowsException()
		{
			// arrange
			ArgumentNullException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", default(string));

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_WhenVectorIsNotBase64_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "I'm not base64");

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_WhenVectorIsNotCorrectSize_ThrowsException()
		{
			// arrange
			ArgumentException? captivum = null;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=");

			// act
			try
			{
				var guid = 58008.ToGuid(key, vector);
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
		public void ToGuid_WithSpecifiedKeyVector_IsStable()
		{
			// arrange
			var input = 58008;
			(var key, var vector) = ("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "NSqvP1Acyge252v+8w2HyA==");

			// act
			var guid = input.ToGuid(key, vector);

			// assert
			Assert.AreEqual(new Guid("aa870058-6b5b-9a61-f58e-c20bb550bfd3"), guid);
		}

		[TestMethod]
		public void ToGuid_WithoutInitializedKeyVector_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);

			// act
			try
			{
				123.ToGuid();
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
		public void ToGuid_WithInitializedKeyVector_IsStable()
		{
			// arrange
			var input = 58008;
			Guint.Set("wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ=", "NSqvP1Acyge252v+8w2HyA==");

			// act
			var guid = input.ToGuid();

			// assert
			Assert.AreEqual(new Guid("aa870058-6b5b-9a61-f58e-c20bb550bfd3"), guid);
		}
	}
}