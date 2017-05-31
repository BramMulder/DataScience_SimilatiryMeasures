using System;

namespace SimilatiryMeasures.UserItem
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO adapt CSV reader / KNearestNeighbours to accept 0 values for non rated items, to be able to run CosineSimilarity 
            //TODO write item rating prediction

            var dictionary = CsvReader.ReadConnections();

            var dataX = new[] {5.0, 1.0, 3.0};
            var dataY = new[] {2.0, 4.0, 3.0};

            var eDis = SimilarityCalculations.CalculateEculeanDistanceCoefficient(dataX, dataY);
            var mDis = SimilarityCalculations.CalculateManhattanDistanceCoefficient(dataX, dataY);
            var r = SimilarityCalculations.CalculatePearsonCoefficient(dataX, dataY);

            //Take into account values that haven't been rated by both users (which I skipped in the previous calculations (here 0.0))
            var dataCosX = new[] {5.0, 0.0 ,1.0, 3.0, 5.0};
            var dataCosY = new[] {2.0, 5.0, 4.0, 3.0, 0.0};

            var cosSim = SimilarityCalculations.CalculateCosineSimilarityCoefficient(dataCosX, dataCosY);

            KNearestNeighbours kNearestNeighbours = new KNearestNeighbours();
            var nearestNeigbours = kNearestNeighbours.GetNearestNeighbours(7, dictionary[7], dictionary, 3, 0.35);

            foreach (var item in nearestNeigbours)
            {
                Console.WriteLine("Id: {0}  Similarity: {1}" ,item.Key, item.Similarity);
            }

            PredictedRatingCalculations predictedRatingCalculations = new PredictedRatingCalculations();
            var predRating = predictedRatingCalculations.CalculatePredictedRating(101, nearestNeigbours, dictionary);

            Console.WriteLine("Predicted Rating: {0}" , predRating);
        }
    }
}
