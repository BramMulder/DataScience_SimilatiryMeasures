using System;
using System.Collections.Generic;
using System.Linq;
using SimilatiryMeasures.ExtensionMethods;

namespace SimilatiryMeasures.ItemItem
{
    class ItemItemLogic
    {
        public static void RunItemItemMethods(Dictionary<int, Dictionary<int, double>> dictionary, int targetUserId, int targetItemId)
        {
            //Select all Item Ids per User
            var movieIds = dictionary.Values.Select(x => x.Keys).ToArray();
            var uniqueIdsOrdered = GetAllUniqueIds(movieIds);

            var deviationsMatrix = CalculateDeviations(dictionary, uniqueIdsOrdered);

            //Generates a dictionary containing the movie Ids and the location of the column/row in the matrix
            var indexDictionary = GenerateIndexDictionary(uniqueIdsOrdered);

            //Retrieve all deviations in a column for the given item Id
            var deviationsRow = deviationsMatrix.GetColumnWithObjects(indexDictionary[targetItemId]);

            //Predict the rating for the given user and item
            var predictedRating = CalculatePredictedRating.PredictRating(dictionary, deviationsRow, targetUserId);
            Console.WriteLine("Rating for user {0} is: {1}", targetUserId, predictedRating);
        }

        private static int[] GetAllUniqueIds(Dictionary<int, double>.KeyCollection[] movieIds)
        {
            var uniqueItemIds = new List<int>();
            //Create a list of unique Item Ids
            for (int j = 0; j < movieIds.Length; j++)
            {
                var id = movieIds[j].ToArray();
                for (int i = 0; i < id.Length; i++)
                {
                    if (!uniqueItemIds.Contains(id[i]))
                    {
                        uniqueItemIds.Add(id[i]);
                    }
                }
            }
            //Order Ids Asscendingly
            var uniqueItemsOrdered = uniqueItemIds.OrderBy(x => x).ToArray();

            return uniqueItemsOrdered;
        }

        private static DeviationObject[,] CalculateDeviations(Dictionary<int, Dictionary<int, double>> dictionary , int[] uniqueIdsOrdered)
        {
            //Instantiate a 2D array which will contain a matrix of all ratings
            var deviationsMatrix = new DeviationObject[uniqueIdsOrdered.Length, uniqueIdsOrdered.Length];

            CalculateSlope calculateSlope = new CalculateSlope();

            for (int m = 0; m < uniqueIdsOrdered.Length; m++)
            {
                for (int p = m; p < uniqueIdsOrdered.Length; p++)
                {
                    //If comparing the item to itself, set the deviation to zero
                    if (p == m)
                    {
                        deviationsMatrix[m, p] = new DeviationObject
                        {
                            Id1 = uniqueIdsOrdered[m],
                            Id2 = uniqueIdsOrdered[p]
                        };
                        continue;
                    }
                    //Calculate the deviation for the selected Ids
                    var deviationResult = calculateSlope.ProcessData(dictionary, uniqueIdsOrdered[m], uniqueIdsOrdered[p]);
                    //Insert deviation
                    deviationsMatrix[m, p] = deviationResult;
                    //Invert calculated deviation (positive to negative and vise versa)
                    var mirroredDeviationResult = new DeviationObject
                    {
                        Id1 = deviationResult.Id2,
                        Id2 = deviationResult.Id1,
                        AmountOfRatings = deviationResult.AmountOfRatings,
                        Deviation = deviationResult.Deviation * -1
                    };
                    deviationsMatrix[p, m] = mirroredDeviationResult;
                }
            }
            return deviationsMatrix;
        }

        private static Dictionary<int, int> GenerateIndexDictionary(int[] uniqueIdsOrdered)
        {
            var idDictionary = new Dictionary<int, int>();
            for (int i = 0; i < uniqueIdsOrdered.Length; i++)
            {
                idDictionary.Add(uniqueIdsOrdered[i], i);
            }
            return idDictionary;
        }
    }
}
