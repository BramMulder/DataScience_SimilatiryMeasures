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
            RunItemItemMethods(dictionary);
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

        private static void RunItemItemMethods(Dictionary<int, Dictionary<int, double>> dictionary)
        {
            CalculateSlope calculateSlope = new CalculateSlope();

            //Select all Item Ids per User
            var movieIds = dictionary.Values.Select(x => x.Keys).ToArray();
            var uniqueItemIds = new List<int>();

            //Create a list of unique Item Ids
            for (int j = 0; j < movieIds.Length; j++)
            {
                var id = movieIds[j].ToArray();
                for (int i = 0; i < id.Length; i++)
                {
                    if (!uniqueItemIds.Contains(id[i]))
                    {
                        uniqueItemIds.Add(id[i]);
                    }
                }
            }
            var uniqueItemsOrdered = uniqueItemIds.OrderBy(x => x).ToArray();

            //Instantiate a 2D array which will contain a matrix of all ratings
            var deviationsMatrix = new DeviationObject[uniqueItemsOrdered.Length, uniqueItemsOrdered.Length];
            //var ratingsAmountMatrix = new double[uniqueItemsOrdered.Length, uniqueItemsOrdered.Length];

            for (int m = 0; m < uniqueItemsOrdered.Length; m++)
            {
                for (int p = m; p < uniqueItemsOrdered.Length; p++)
                {
                    //If comparing the item to itself, set the deviation to zero
                    if (p == m)
                    {
                        deviationsMatrix[m, p] = new DeviationObject
                        {
                            Id1 = uniqueItemsOrdered[m],
                            Id2 = uniqueItemsOrdered[p]
                        };
                        continue;
                    }
                    //Calculate the deviation for the selected Ids
                    var deviationResult = calculateSlope.ProcessData(dictionary, uniqueItemsOrdered[m], uniqueItemsOrdered[p]);
                    //Insert deviation
                    deviationsMatrix[m, p] = deviationResult;
                    //Invert calculated deviation (positive to negative and vise versa)
                    var mirroredDeviationResult = new DeviationObject
                    {
                        Id1 = deviationResult.Id2,
                        Id2 = deviationResult.Id1,
                        AmountOfRatings = deviationResult.AmountOfRatings,
                        Deviation = deviationResult.Deviation * -1
                    };
                    deviationsMatrix[p, m] = mirroredDeviationResult;
                }
            }

            //TODO figure out how to toss in the correct values
            var deviationsRow = deviationsMatrix.GetColumnWithObjects(0);
            var predictedRating = CalculatePredictedRating.PredictRating(dictionary, deviationsRow, 7);
            Console.WriteLine("Rating for user {0} is: {1}", 107, predictedRating);
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
