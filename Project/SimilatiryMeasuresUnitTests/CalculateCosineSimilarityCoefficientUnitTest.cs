using System;
using NUnit.Framework;
using Shouldly;

namespace SimilatiryMeasuresUnitTests
{
    [TestFixture]
    class CalculateCosineSimilarityCoefficientUnitTest
    {
        private double _expected;
        private double _result;
        private double[] _dataX;
        private double[] _dataY;

        [SetUp]
        public void Arrange()
        {
            // Arrange
            _expected = 0.40407;

            //Test data
            _dataX = new[] { 5.0, 0.0 ,1.0, 3.0, 5.0 };
            _dataY = new[] { 2.0, 5.0, 4.0, 3.0, 0.0 };

            Act();
        }

        private void Act()
        {
            // Act
            var result = SimilatiryMeasures.SimilarityCalculations.CalculateCosineSimilarityCoefficient(_dataX, _dataY);
            _result = Math.Round(result, 5);
        }

        [Test]
        public void Then_Result_Succeeded_Should_Be_Zero_Point_Four()
        {
            // Assert
            _result.ShouldBe(_expected);
        }
    }
}

