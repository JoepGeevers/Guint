namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_Use_Test
	{
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.secret = null;
		}

		[TestMethod]
		public void WhenSecretIsNull_ThrowsException()
		{
			// arrange
			var captivum = default(ArgumentNullException);

			// act
			try
			{
				Guint.Use(null);
			}
			catch (ArgumentNullException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("Secret cannot be null"));
		}

		[TestMethod]
		public void WhenSecretIsEmpty_ThrowsException() => this.WhenSecret_IsNotValid("");

		[TestMethod]
		public void WhenSecretIsNotBase64_ThrowsException() => this.WhenSecret_IsNotValid("*&^%$");

		[TestMethod]
		public void WhenSecretKeyIsTooShort_ThrowsException() => this.WhenSecret_IsNotValid("MQ==");

		[TestMethod]
		public void WhenSecretVectorIsTooShort_ThrowsException() => this.WhenSecret_IsNotValid("NTQzNWY0M3d0NXdyNDNxcjQzcjM0cXIzYXJhMzU0MzVmNDN3dDV3cjQzcXI0Mw==");

		[TestMethod]
		public void WhenSecretIsTooLong_ThrowsException() => this.WhenSecret_IsNotValid("NTQzNWY0M3d0NXdyNDNxcjQzcjM0cXIzYXJhMzU0MzVmNDN3dDV3cjQzcXI0M3IzNHFyM2FyYTM1NDM1ZjQzd3Q1d3I0M3FyNDNyMzRxcjNhcmEzNTQzNWY0M3d0NXdyNDNxcjQzcjM0cXIzYXJhMw==");

		public void WhenSecret_IsNotValid(string? secret)
		{
			// act
			var captivum = default(ArgumentException);

			try
			{
				Guint.Use(secret);
			}
			catch (ArgumentException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("Secret is not valid"));
		}

		[TestMethod]
		public void WhenSettingTheSameSecretPairAgain_DoesNotThrowException()
		{
			// arrange
			var secret = Guint.GenerateSecret();

			// act
			Guint.Use(secret);
			Guint.Use(secret);

			// assert
			{ }
		}

		[TestMethod]
		public void WhenSettingDifferentSecret_ThrowsException()
		{
			// arrange
			var captivum = default(InvalidOperationException);
			var secret1 = Guint.GenerateSecret();
			var secret2 = Guint.GenerateSecret();

			// act
			Guint.Use(secret1);

			try
			{
				Guint.Use(secret2);
			}
			catch (InvalidOperationException e)
			{
				captivum = e;
			}

			// assert
			Assert.IsNotNull(captivum);
			Assert.IsTrue(captivum.Message.Contains("Secret cannot be changed"));
		}
	}
}