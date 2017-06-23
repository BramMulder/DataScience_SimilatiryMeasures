using System;
using System.Collections.Generic;
using System.Linq;
using SimilatiryMeasures.UserItem;

namespace SimilatiryMeasures
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = CsvReader.ReadConnections();

            RunUserItemMethods(dictionary);
            //ItemItemLogic.RunItemItemMethods(dictionary, 186, 0);
        }

        private static void RunUserItemMethods(Dictionary<int, Dictionary<int, double>> dictionary)
        {
            var userId = 186;
            var itemId = 514;

            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours();
            var nearestNeigbours = kNearestNeighbours.GetNearestNeighbours(userId, dictionary[userId], dictionary, 25, 0.35);

            foreach (var item in nearestNeigbours)
            {
                Console.WriteLine("Id: {0}  Similarity: {1}", item.Key, item.Similarity);
            }

            //PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
            //var predRating = predictedRatingCalculations.CalculatePredictedRating(itemId, nearestNeigbours, dictionary);
            //Console.WriteLine("Predicted Rating: {0}" , predRating);

            var topRatings = GetTopXAmountOfRatings(8, nearestNeigbours, dictionary);

            foreach (var rating in topRatings)
            {
                Console.WriteLine("Predicted Rating: {0}", rating);
            }
        }

        //TODO fix this
        private static double[] GetTopXAmountOfRatings(int amountOfRatings, KeyValueObject[] nearestNeighbours,
            Dictionary<int, Dictionary<int, double>> dictionary)
        {
            var uniqueItemIds = new List<int>();

            foreach (var neighbour in nearestNeighbours)
            {
                var ids = dictionary[neighbour.Key].Keys.ToArray();
                for (int i = 0; i < ids.Length; i++)
                {
                    if (!uniqueItemIds.Contains(ids[i]))
                    {
                        uniqueItemIds.Add(ids[i]);
                    }
                }
            }
            
            var bestRatings = new List<double>();

            for (int i = 0; i < uniqueItemIds.Count; i++)
            {
                PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
                var predictedRating = predictedRatingCalculations.CalculatePredictedRating(uniqueItemIds[i], nearestNeighbours, dictionary);
                bestRatings.Add(predictedRating);
            }
            return bestRatings.OrderByDescending(x => x).Take(amountOfRatings).ToArray();
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
