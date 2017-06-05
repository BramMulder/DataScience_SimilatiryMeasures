using System;
using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.UserItem
{
    public class PredictedRatingCalculations
    {
        //TODO handle error when a nearest neighbour hasn't rated the item
        public double CalculatePredictedRating(int itemId, KeyValueObject[] nearestNeighbours, Dictionary<int, Dictionary<int, double>> ratings)
        {
            //If not all Nearest Neighbours have rated the item, throw an exception
            //if (!EveryNeighbourHasRatedItem(itemId, nearestNeighbours, ratings))
            //{
            //    throw new Exception("Not all neighbours have rated this item");
            //}

            var nearestNeighboursRatings = new List<RatingObject>(nearestNeighbours.Length);

            //Create objects which also contain a rating for the targeted item
            foreach (var neighbour in nearestNeighbours)
            {
                //If a nearest neighbour has not rated the targeted item, discard it
                if (!ratings[neighbour.Key].ContainsKey(itemId))
                {
                    continue;;
                }
                nearestNeighboursRatings.Add(new RatingObject { Key = neighbour.Key, Rating = ratings[neighbour.Key][itemId], Similarity = neighbour.Similarity });
            }

            var ratingSimilaritySum = 0.0;
            var similaritySum = 0.0;

            //Calculate the predicted rating
            foreach (var neighbourRating in nearestNeighboursRatings)
            {
                ratingSimilaritySum += neighbourRating.Similarity * neighbourRating.Rating;
                similaritySum += neighbourRating.Similarity;
            }

            var predRating = ratingSimilaritySum / similaritySum; 

            return predRating;
        }

        private bool EveryNeighbourHasRatedItem(int itemId, KeyValueObject[] nearestNeighbours, Dictionary<int, Dictionary<int, double>> ratings)
        {
            return nearestNeighbours.All(neighbour => ratings[neighbour.Key].ContainsKey(itemId));
        }

        internal class RatingObject
        {
            public int Key { get; set; }
            public double Rating { get; set; }
            public double Similarity { get; set; }
        }
    }
}
