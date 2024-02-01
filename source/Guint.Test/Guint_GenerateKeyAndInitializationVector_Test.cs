namespace Guint.Test
{
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
	[ExcludeFromCodeCoverage]
	public class Guint_GenerateKeyAndInitializationVector_Test
    {
        [TestMethod]
        public void GeneratedKeyAndVectorAreAlwaysDifferentAndWork()
        {
            // act
            var f = Enumerable
                .Range(0, 1000)
                .Select(i => Guint.GenerateKeyAndInitializationVector());

            // assert
            Assert.AreEqual(1000, f.Select(t => t.Key).Distinct().Count());
            Assert.AreEqual(1000, f.Select(t => t.InitializationVector).Distinct().Count());
        }
    }
}