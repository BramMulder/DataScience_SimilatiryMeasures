using System;
using System.Linq;

namespace SimilatiryMeasures
{
    public static class SimilarityCalculations
    {
        public static double CalculateEculeanDistanceCoefficient (double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Coordinates lengths differ. Please make sure all point have the same amount of coordinates");

            double distance = 0.0;

            //Calculate euclidean distance
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance = distance + Math.Pow(delta, 2);
            }

            //Calculate coefficient
            var coefficient = 1 / (1 + Math.Sqrt(distance));

            return coefficient;
        }

        public static double CalculateManhattanDistanceCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Coordinates lengths differ. Please make sure all point have the same amount of coordinates");

            double distance = 0.0;

            //Sum all (absolute) delta values between Xi and Yi
            for (int i = 0; i < dataX.Length; i++)
            {
                double delta = dataX[i] - dataY[i];
                distance = distance + Math.Abs(delta);
            }

            //Calculate coefficient
            var coefficient = 1 / (1 + distance);

            return coefficient;
        }

        public static double CalculatePearsonCoefficient(double[] dataX, double[] dataY)
        {
            if (dataX.Length != dataY.Length)
                throw new Exception("Coordinates lengths differ. Please make sure all point have the same amount of coordinates");

            // n
            var n = dataX.Length;

            // ∑ x
            var xSum = dataX.Sum();
            // ∑ y
            var ySum = dataY.Sum();
            // ∑ ( x(i) * y(i) )
            var xySum = 0.0;

            // ∑ x(i)²
            var xSquareSum = 0.0;            
            //∑ (x(i))²
            var xSumSquared = Math.Pow(xSum , 2);            
            
            // ∑ y(i)²
            var ySquareSum = 0.0;            
            //∑ (y(i))²
            var ySumSquared = Math.Pow(ySum, 2);


            // ∑ ( x(i) * y(i) )
            for (int i = 0; i < dataX.Length; i++)
            {
                xySum = xySum + (dataX[i] * dataY[i]);
            }

            // ∑ x(i)²
            foreach (var point in dataX)
            {
                xSquareSum = xSquareSum + Math.Pow(point, 2);
            }

            // ∑ y(i)²
            foreach (var point in dataY)
            {
                ySquareSum = ySquareSum + Math.Pow(point, 2);
            }

            double r = (xySum - ((xSum * ySum) / n)) / (Math.Sqrt(xSquareSum - (xSumSquared / n)) * Math.Sqrt(ySquareSum - (ySumSquared / n)));

            return r;
        }

        public static double CalculateCosineSimilarityCoefficient(double[] dataX, double[] dataY)
        {
            // ∑ ( x(i) * y(i) )
            var xySum = 0.0;

            // ||x||
            var dotProductX = 0.0;            
            
            // ||y||
            var dotProductY = 0.0;

            // ∑ ( x(i) * y(i) )
            for (int i = 0; i < dataX.Length; i++)
            {
                xySum = xySum + (dataX[i] * dataY[i]);
            }

            // Calculate ||x||
            foreach (var point in dataX)
            {
                dotProductX = dotProductX + Math.Pow(point, 2);
            }
            dotProductX = Math.Sqrt(dotProductX);

            // Calculate ||y||
            foreach (var point in dataY)
            {
                dotProductY = dotProductY + Math.Pow(point, 2);
            }
            dotProductY = Math.Sqrt(dotProductY);

            var similarityCoefficient = xySum / (dotProductX * dotProductY);

            return similarityCoefficient;
        }
    }
}