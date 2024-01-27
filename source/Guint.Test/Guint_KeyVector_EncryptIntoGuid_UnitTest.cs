namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_KeyVector_EncryptIntoGuid_UnitTest
    {
		[TestMethod]
		public void Encryption_IsStable()
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