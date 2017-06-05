using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using SimilatiryMeasures.UserItem;

namespace SimilatiryMeasuresUnitTests
{
    class CalculatePredictRatingUnitTest
    {
        private double _expected;
        private double _result;
        private Dictionary<int, Dictionary<int, double>> _ratings = new Dictionary<int, Dictionary<int, double>>();
        private KeyValueObject[] _nearestNeigbours;

        [SetUp]
        public void Arrange()
        {
            // Arrange
            _expected = 3.839;

            //Test data
            _ratings.Add(1, new Dictionary<int, double>());
            _ratings[1] = new Dictionary<int, double>
            {
                {1, 1.5},
                {2, 4.5},
                {3, 3}
            };
            _ratings[2] = new Dictionary<int, double>
            {
                {1, 5},
                {3, 1},
                {4, 1}
            };
            _ratings[3] = new Dictionary<int, double>
            {
                {1, 1},
                {2, 4},
                {3, 5},
                {4, 5}
            };
            _ratings[4] = new Dictionary<int, double>
            {
                {1, 2},
                {2, 5},
                {4, 4}
            };
            _ratings[5] = new Dictionary<int, double>
            {
                {1, 3},
                {2, 4},
                {3, 4}
            };

            _nearestNeigbours = new KeyValueObject[]
            {
                //3 nearest neighbours - euclidean distance
                new KeyValueObject {Key = 1, Similarity = 1 / ( 1 + Math.Sqrt(0.5))},
                new KeyValueObject {Key = 3, Similarity = 1 / ( 1 + Math.Sqrt(3))},
                new KeyValueObject {Key = 5, Similarity = 1 / ( 1 + Math.Sqrt(2))}
            };

            Act();
        }

        private void Act()
        {
            // Act
            PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
            var result = predictedRatingCalculations.CalculatePredictedRating(3, _nearestNeigbours, _ratings);
            _result = Math.Round(result, 3);
        }

        [Test]
        public void Then_Result_Succeeded_Should_Be_Expected()
        {
            // Assert
            _result.ShouldBe(_expected);
        }
    }
}
