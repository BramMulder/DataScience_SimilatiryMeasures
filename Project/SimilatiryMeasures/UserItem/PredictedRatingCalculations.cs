using System.Collections.Generic;

namespace SimilatiryMeasures.UserItem
{
    class PredictedRatingCalculations
    {
        //TODO resolve error when a nearest neighbour hasn't rated the item
        public double CalculatePredictedRating(int itemId, KeyValueObject[] nearestNeighbours, Dictionary<int, Dictionary<int, double>> ratings)
        {
            var nearestNeighboursRatings = new List<RatingObject>(nearestNeighbours.Length);

            foreach (var neighbour in nearestNeighbours)
            {
                nearestNeighboursRatings.Add(new RatingObject { Key = neighbour.Key, Rating = ratings[neighbour.Key][itemId], Similarity = neighbour.Similarity });
            }

            var ratingSimilaritySum = 0.0;
            var similaritySum = 0.0;

            foreach (var neighbourRating in nearestNeighboursRatings)
            {
                ratingSimilaritySum += neighbourRating.Similarity * neighbourRating.Rating;
                similaritySum += neighbourRating.Similarity;
            }

            var predRating = ratingSimilaritySum / similaritySum; 

            return predRating;
        }

        private class RatingObject
        {
            public int Key { get; set; }
            public double Rating { get; set; }
            public double Similarity { get; set; }
        }
    }
}
