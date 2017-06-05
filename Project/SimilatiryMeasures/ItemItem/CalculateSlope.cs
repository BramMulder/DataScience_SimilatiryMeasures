using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.ItemItem
{
    class CalculateSlope
    {
        //TODO Improve on speed so it doesn't take 25 minutes to complete
        public double ProcessData(Dictionary<int, Dictionary<int, double>> ratings, int itemId1, int itemId2)
        {
            var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).ToDictionary(x => x.Key, x => x.Value);
            //var filteredRatings = ratings.Where(x => x.Value.ContainsKey(itemId1) && x.Value.ContainsKey(itemId2)).Select(x => new UserRatings { UserId = x.Key, Ratings = x.Value}).ToList();

            if (!filteredRatings.Any())
                return 0.0;

            var deviation = CalculateDeviation(filteredRatings, itemId1, itemId2);

            return deviation;
        }

        public double CalculateDeviation(Dictionary<int, Dictionary<int, double>> userRatings, int itemId1, int itemId2)
        {
            var currDev = 0.0;

            foreach (var userRating in userRatings)
            {
                currDev += userRating.Value[itemId1] - userRating.Value[itemId2];
            }

            currDev = currDev / userRatings.Count;

            return currDev;
        }
    }
}
