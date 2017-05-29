using System;

namespace SimilatiryMeasures
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
            Console.WriteLine(eDis);

            var mDis = SimilarityCalculations.CalculateManhattanDistanceCoefficient(dataX, dataY);
            Console.WriteLine(mDis);

            var r = SimilarityCalculations.CalculatePearsonCoefficient(dataX, dataY);
            Console.WriteLine(r);

            //Take into account values that haven't been rated by both users (which I skipped in the previous calculations (here 0.0))
            var dataCosX = new[] {5.0, 0.0 ,1.0, 3.0, 5.0};
            var dataCosY = new[] {2.0, 5.0, 4.0, 3.0, 0.0};

            var cosSim = SimilarityCalculations.CalculateCosineSimilarityCoefficient(dataCosX, dataCosY);
            Console.WriteLine(cosSim);

            var individualId = 186;
            var amountOfNeighbours = 8;
            var initialThreshhold = 0.35;

            //Run KNearestNeighbours
            var kNearestNeighbours = new KNearestNeighbours();
            var result = kNearestNeighbours.GetNearestNeighbours(individualId, dictionary[individualId], dictionary, amountOfNeighbours, initialThreshhold);

            foreach (var item in result)
            {
                Console.WriteLine("Id: {0}, Similarity: {1}", item.Key, item.Similarity );
            }
        }
    }
}
