namespace Guint.Test.Framework
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_ToInt_UnitTest
	{
		[TestMethod]
		public void ToInt_RandomGuid_DoesNotExplode_InDotNetFramework()
		{
			// arrange
			var guid = new Guid("8c6f393e-d06f-ef03-26ae-cd05bf6d7f86");
			var secret = Guint.GenerateSecret();

			Guint.Use(secret);

			// act
			var result = guid.ToInt();

			// assert
			result.Switch(
				i => Assert.Fail(),
				notfound => { });
		}
	}
}