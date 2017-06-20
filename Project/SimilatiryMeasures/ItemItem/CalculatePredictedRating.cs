using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.ItemItem
{
    public class CalculatePredictedRating
    {
        public static double PredictRating(Dictionary<int, Dictionary<int, double>> usersRatings,  DeviationObject[] deviations, int targetUser)
        {
            var userRatings = usersRatings[targetUser].Values.ToArray(); ;
            var userDeviations = deviations.Where(x => usersRatings[targetUser].Keys.Any(y => x.Id2 == y)).ToArray();

            double numerator = 0;
            double denominator = 0;

            for (int i = 0; i < userRatings.Length; i++)
            {
                var userRating = userRatings[i];
                var deviation = userDeviations[i];

                numerator += (userRating + deviation.Deviation) * deviation.AmountOfRatings;
                denominator += deviation.AmountOfRatings;
            }

            return numerator/denominator;
        }
    }
}