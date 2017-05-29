using System;

namespace SimilatiryMeasures
{
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            //TODO adapt CSV reader / KNearestNeighbours to accept 0 values for non rated items, to be able to run CosineSimilarity 
            //TODO write item rating prediction

=======
>>>>>>> c1b94f066d7d278a5875cc1cc040b810b74dc6b2
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
        }
    }
}
