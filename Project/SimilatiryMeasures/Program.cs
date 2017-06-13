using System;
using System.Collections.Generic;
using System.Linq;
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
            var userId = 4;
            var itemId = 101;

            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours();
            var nearestNeigbours = kNearestNeighbours.GetNearestNeighbours(userId, dictionary[userId], dictionary, 3, 0.35);

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
            var orderedIds = uniqueItemIds.OrderBy(x => x).ToArray();

            //Instantiate a 2D array which will contain a matrix of all ratings
            var deviationsMatrix = new double[orderedIds.Length, orderedIds.Length];
            //TODO add place for amount of users have rated item the deviation is calculated for
            //Loop through all 
            //for (int m = 0; m < orderedIds.Length; m++)
            //{
            //    var kIndex = m;
            //    while (kIndex < orderedIds.Length)
            //    {
            //        //If comparing the item to itself, set the deviation to zero
            //        if (kIndex == m)
            //        {
            //            deviationsMatrix[m, kIndex] = 0;
            //            kIndex++;
            //            continue;
            //        }
            //        //Calculate the deviation for the selected Ids
            //        var deviation = calculateSlope.ProcessData(dictionary, orderedIds[m], orderedIds[kIndex]);
            //        deviationsMatrix[m, kIndex] = deviation;
            //        //Invert calculated deviation (positive to negative and vise versa)
            //        deviationsMatrix[kIndex, m] = deviation * -1;

            //        kIndex++;
            //    }
            //}
            for (int m = 0; m < orderedIds.Length; m++)
            {
                for (int p = m; p < orderedIds.Length; p++)
                {
                    //If comparing the item to itself, set the deviation to zero
                    if (p == m)
                    {
                        deviationsMatrix[m, p] = 0;
                        continue;
                    }
                    //Calculate the deviation for the selected Ids
                    var deviation = calculateSlope.ProcessData(dictionary, orderedIds[m], orderedIds[p]);
                    deviationsMatrix[m, p] = deviation;
                    //Invert calculated deviation (positive to negative and vise versa)
                    deviationsMatrix[p, m] = deviation * -1;
                }
            }
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
