using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var dataX = new[] { 5.0, 1.0, 3.0 };
            var dataY = new[] { 2.0, 4.0, 3.0 };

            var eDis = SimilarityCalculations.CalculateEculeanDistanceCoefficient(dataX, dataY);
            var mDis = SimilarityCalculations.CalculateManhattanDistanceCoefficient(dataX, dataY);
            var r = SimilarityCalculations.CalculatePearsonCoefficient(dataX, dataY);

            //Take into account values that haven't been rated by both users (which I skipped in the previous calculations (here 0.0))
            var dataCosX = new[] { 5.0, 0.0, 1.0, 3.0, 5.0 };
            var dataCosY = new[] { 2.0, 5.0, 4.0, 3.0, 0.0 };

            var cosSim = SimilarityCalculations.CalculateCosineSimilarityCoefficient(dataCosX, dataCosY);

            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours();
            var nearestNeigbours = kNearestNeighbours.GetNearestNeighbours(7, dictionary[7], dictionary, 3, 0.35);

            foreach (var item in nearestNeigbours)
            {
                Console.WriteLine("Id: {0}  Similarity: {1}", item.Key, item.Similarity);
            }

            //PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
            //var predRating = predictedRatingCalculations.CalculatePredictedRating(101, nearestNeigbours, dictionary);
            //Console.WriteLine("Predicted Rating: {0}" , predRating);
        }

        //TODO skip m == k
        //TODO 
        private static void RunItemItemMethods(Dictionary<int, Dictionary<int, double>> dictionary)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            CalculateSlope calculateSlope = new CalculateSlope();

            var movieIds = dictionary.Values.Select(x => x.Keys).ToArray();
            var keysets = new List<int>();

            for (int j = 0; j < movieIds.Length; j++)
            {
                var id = movieIds[j].ToArray();
                for (int i = 0; i < id.Length; i++)
                {
                    if (!keysets.Contains(id[i]))
                    {
                        keysets.Add(id[i]);
                    }
                }
            }
            var orderedKeysets = keysets.OrderBy(x => x).ToArray();

            var deviationsMatrix = new double[orderedKeysets.Length, orderedKeysets.Length];

            for (int m = 0; m < orderedKeysets.Length; m++)
            {
                var kIndex = m;
                while (kIndex < orderedKeysets.Length)
                {
                    var deviation = calculateSlope.ProcessData(dictionary, orderedKeysets[m], orderedKeysets[kIndex]);
                    deviationsMatrix[m, kIndex] = deviation;
                    //Invert calculated deviation (positive to negative and vise versa)
                    deviationsMatrix[kIndex, m] = deviation * -1;

                    kIndex++;
                }
            }


            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

        }
    }
}
