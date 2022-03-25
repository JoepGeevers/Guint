namespace Guint.Test.Framework
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuintTestFramework
    {
        [TestMethod]
        public void DecryptingNormalGuidReturnsNullInDotNetFramework()
        {
            // arrange
            var guid = new Guid("{8c6f393e-d06f-ef03-26ae-cd05bf6d7f86}");
            (var key, var vector) = ("axRxUAuCAVDkNzqriQ0j7K/YV02xddjO5wIE1AYKrvY=", "iEoZxvDg38zjvdUF33lo1A==");

            // act
            var id = guid.DecryptToInt(key, vector);

            // assert
            Assert.IsNull(id);
        }
    }
}