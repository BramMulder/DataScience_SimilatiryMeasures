using System;
using NUnit.Framework;
using Shouldly;

namespace SimilatiryMeasuresUnitTests
{
    //TODO add NSubstitute to check for method calls
    [TestFixture]
    public class CalculatePearsonCoefficientUnitTest
    {
        private double _expected;
        private double _result;
        private double[] _dataX;
        private double[] _dataY;

        [SetUp]
        public void Arrange()
        {
            // Arrange
            _expected = -1;

            //Test data
            _dataX = new[] { 5.0, 1.0, 3.0 };
            _dataY = new[] { 2.0, 4.0, 3.0 };

            Act();
        }

        private void Act()
        {
            // Act
            var result = SimilatiryMeasures.UserItem.SimilarityCalculations.CalculatePearsonCoefficient(_dataX, _dataY);
            _result = Math.Round(result, 5);
        }

        [Test]
        public void Then_Result_Succeeded_Should_Be_Minus_One()
        {
            // Assert
            _result.ShouldBe(_expected);
        }
    }
}