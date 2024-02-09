namespace Guint.Test
{
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class Guint_GenerateSecret_Test
	{
		[TestMethod]
		public void GeneratedSecretsAreAlwaysDifferent()
		{
			// act
			var secrets = Enumerable
				.Range(0, 1000)
				.Select(i => Guint.GenerateSecret());

			// assert
			Assert.AreEqual(1000, secrets.Distinct().Count());
		}
	}
}