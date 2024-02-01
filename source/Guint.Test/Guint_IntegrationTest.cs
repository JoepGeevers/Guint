namespace Guint.Test
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[ExcludeFromCodeCoverage] // todo: make this work for test assemblies
	public class Guint_IntegrationTest
    {
		[TestInitialize]
		public void TestInitialize()
		{
			Guint.key = null;
			Guint.vector = null;
		}

		[TestMethod]
        public void Int32MinValue_CanBeCorrectlyEncrypted_WithSpecifiedKeyVector()
        {
            // arrange
            var input = Int32.MinValue;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

            // act
            var output = input
				.ToGuid(key, vector)
				.ToInt(key, vector);

            // assert
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void Int32MaxValue_CanBeCorrectlyEncrypted_WithSpecifiedKeyVector()
        {
            // arrange
            var input = Int32.MaxValue;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			// act
			var output = input
				.ToGuid(key, vector)
				.ToInt(key, vector);

			// assert
			Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void RandomValue_CanBeCorrectlyEncrypted_WithSpecifiedKeyVector()
        {
            // arrange
            var input = 765437653;
            (var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			// act
			var output = input
				.ToGuid(key, vector)
				.ToInt(key, vector);

			// assert
			Assert.AreEqual(input, output);
        }

		[TestMethod]
		public void Int32MinValue_CanBeCorrectlyEncrypted_WithConfiguredKeyVector()
		{
			// arrange
			var input = Int32.MinValue;
			(var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			Guint.Set(key, vector);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void Int32MaxValue_CanBeCorrectlyEncrypted_WithConfiguredKeyVector()
		{
			// arrange
			var input = Int32.MaxValue;
			(var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			Guint.Set(key, vector);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}

		[TestMethod]
		public void RandomValue_CanBeCorrectlyEncrypted_WithConfiguredKeyVector()
		{
			// arrange
			var input = 765437653;
			(var key, var vector) = Guint.GenerateKeyAndInitializationVector();

			Guint.Set(key, vector);

			// act
			var output = input
				.ToGuid()
				.ToInt();

			// assert
			output.Switch(
				i => Assert.AreEqual(input, output),
				notfound => Assert.Fail());
		}
	}
}