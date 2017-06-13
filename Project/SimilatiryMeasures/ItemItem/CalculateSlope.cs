﻿using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.ItemItem
{
    class CalculateSlope
    {
        //TODO Improve on speed so it doesn't take 25 minutes to complete
        public double ProcessData(Dictionary<int, Dictionary<int, double>> ratings, int itemId1, int itemId2)
        {
            var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => x.Key).ToArray();
            //var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).ToDictionary(x => x.Key, x => x.Value);
            //var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => new UserRatings { UserId = x.Key, Ratings = x.Value}).ToList();

            if (!filteredRatings.Any())
                return 0.0;

            return CalculateDeviation(ratings ,filteredRatings, itemId1, itemId2);
        }

        private double CalculateDeviation(Dictionary<int, Dictionary<int, double>> userRatings, int[] keys , int itemId1, int itemId2)
        {
            var currDev = 0.0;

            foreach (var key in keys)
            {
                //currDev += userRating.Value[itemId1] - userRating.Value[itemId2];
                currDev += userRatings[key][itemId1] - userRatings[key][itemId2];
            }

            currDev = currDev / userRatings.Count;

            return currDev;
        }
    }
}
