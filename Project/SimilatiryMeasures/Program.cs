using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimilatiryMeasures.ExtensionMethods;
using SimilatiryMeasures.ItemItem;
using SimilatiryMeasures.UserItem;

namespace SimilatiryMeasures
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO update item-rating calculation and similarRatings
            var dictionary = CsvReader.ReadConnections();

            //RunUserItemMethods(dictionary);
            ItemItemLogic.RunItemItemMethods(dictionary, 3, 105);
        }

        private static void RunUserItemMethods(Dictionary<int, Dictionary<int, double>> dictionary)
        {
            //TODO figure out why this input doesn't return 2,63284325835078 as predicted rating
            var userId = 186;
            var itemId = 778;

            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours();
            var nearestNeigbours = kNearestNeighbours.GetNearestNeighbours(userId, dictionary[userId], dictionary, 8, 0.35);

            foreach (var item in nearestNeigbours)
            {
                Console.WriteLine("Id: {0}  Similarity: {1}", item.Key, item.Similarity);
            }

            PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
            var predRating = predictedRatingCalculations.CalculatePredictedRating(itemId, nearestNeigbours, dictionary);
            Console.WriteLine("Predicted Rating: {0}" , predRating);
        }

        

        private static void RunCalculationTestMethods()
        {
            var dataX = new[] { 5.0, 1.0, 3.0 };
            var dataY = new[] { 2.0, 4.0, 3.0 };

            var eDis = SimilarityCalculations.CalculateEculeanDistanceCoefficient(dataX, dataY);
            var mDis = SimilarityCalculations.CalculateManhattanDistanceCoefficient(dataX, dataY);
            var r = SimilarityCalculations.CalculatePearsonCoefficient(dataX, dataY);

            //Take into account values that haven't been rated by both users (which I skipped in the previous calculations (here 0.0))
            var dataCosX = new[] { 5.0, 0.0, 1.0, 3.0, 5.0 };
            var dataCosY = new[] { 2.0, 5.0, 4.0, 3.0, 0.0 };

            var cosSim = SimilarityCalculations.CalculateCosineSimilarityCoefficient(dataCosX, dataCosY);
        }
    }
}
