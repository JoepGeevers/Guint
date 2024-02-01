namespace Guint.Test.Framework
{
    using System;
	using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
	[ExcludeFromCodeCoverage]
	public class Guint_KeyVector_ToInt_UnitTest
    {
        [TestMethod]
        public void ToInt_RandomGuid_DoesNotExplode_InDotNetFramework()
        {
			// arrange
			var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f86}");
            (var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

            // act
            var result = guid.ToInt(key, vector);

            // assert
            result.Switch(
                i => Assert.Fail(),
                notfound => { });
        }
    }
}