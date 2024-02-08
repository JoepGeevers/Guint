namespace Guint.Test
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToGuid_UnitTest
    {
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
		public void ToGuid_WithoutInitializedSecret_ThrowsException()
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
			Assert.IsTrue(captivum.Message.Contains("no secret has been initialized"));
		}

		[TestMethod]
		public void ToGuid_IsStable()
		{
			// arrange
			var input = 58008;
			var secret = "wJcb9Q+26p0wdNtNEaA4mkEyT4R56WKPyeSJs25eHtQ1Kq8/UBzKB7bna/7zDYfI";

			Guint.Use(secret);

			// act
			var guid = input.ToGuid();

			// assert
			Assert.AreEqual(new Guid("aa870058-6b5b-9a61-f58e-c20bb550bfd3"), guid);
		}
	}
}