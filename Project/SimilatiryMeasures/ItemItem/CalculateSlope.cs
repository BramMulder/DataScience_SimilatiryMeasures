using System;
using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.ItemItem
{
    class CalculateSlope
    {
        //TODO Improve on speed so it doesn't take 25 minutes to complete
        public DeviationObject ProcessData(Dictionary<int, Dictionary<int, double>> ratings, int itemId1, int itemId2)
        {
            var userKeys = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => x.Key).ToArray();
            //var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).ToDictionary(x => x.Key, x => x.Value);
            //var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => new UserRatings { UserId = x.Key, Ratings = x.Value}).ToList();

            if (!userKeys.Any())
                return new DeviationObject() { Id1 = itemId1, Id2 = itemId2};

            return new DeviationObject()
            {
                Id1 = itemId1,
                Id2 = itemId2,
                AmountOfRatings = userKeys.Length,
                Deviation = CalculateDeviation(ratings, userKeys, itemId1, itemId2)
            }; 
        }

        private double CalculateDeviation(Dictionary<int, Dictionary<int, double>> userRatings, int[] userKeys , int itemId1, int itemId2)
        {
            var currDev = userKeys.Sum(key => userRatings[key][itemId1] - userRatings[key][itemId2]);

            return currDev / userKeys.Length;
        }
    }
}
