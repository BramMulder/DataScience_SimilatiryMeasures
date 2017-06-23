using System;
using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures.UserItem
{
    public class KNearestNeighbours
    {
        private Dictionary<int, Dictionary<int, double>> _neighbourRatingsList;
        private int _maxRatingListLength;
        private double _threshhold;
        private Dictionary<int, double> _similairtyValues;

        //TODO improve speed by refactoring code 
        public KeyValueObject[] GetNearestNeighbours(int individualId, Dictionary<int, double> individual, Dictionary<int, Dictionary<int, double>> neighbours, int neighbourRankingsListLength, double initialThreshold)
        {
            //Initialize
            _neighbourRatingsList = new Dictionary<int, Dictionary<int, double>>(neighbourRankingsListLength);
            _maxRatingListLength = neighbourRankingsListLength;
            _similairtyValues = new Dictionary<int, double>(neighbours.Count);
            _threshhold = initialThreshold;

            foreach (var neighbour in neighbours)
            {
                //Skip the all the actions on the "neighbour" if the neighbour is the individual
                if (neighbour.Key == individualId)
                {
                    continue;
                }

                //Calculate the similarity between the individual and the neighbour
                var similarity = CalculateSimilarity(individual, neighbour.Value);
                //Add the similarity value to a dictornary for later usage (Neighbour key, similarity)
                _similairtyValues.Add(neighbour.Key, similarity);
                //If the similairty is larger or equal to the threshhold and the individual has the rated the same items 
                if (similarity >= _threshhold && HasRatedAdditionalItems(individual, neighbour.Value))
                {
                    ProcessNeighbour(neighbour.Key, neighbour.Value, similarity);
                }
            }

            return (from i in _similairtyValues
                    join n in _neighbourRatingsList
                    on i.Key equals n.Key
                    select new KeyValueObject { Key = i.Key, Similarity = i.Value }).ToArray();
        }

        private bool HasRatedAdditionalItems(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            var similarRatedItems = individual.Keys.Where(x => neighbour.Keys.Any(z => z != x));

            return similarRatedItems.Count() > 2;
        }

        private double CalculateSimilarity(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            //var cosDistance = SimilarityCalculations.RunCosineSimilarity(individual, neighbour);

            //Select the items that both users have rated
            var ratingsIndividual = (from i in individual
                                     join n in neighbour
                                     on i.Key equals n.Key
                                     select Convert.ToDouble(i.Value)).ToArray();
            var ratingsNeighbour = (from i in individual
                                    join n in neighbour
                                    on i.Key equals n.Key
                                    select Convert.ToDouble(n.Value)).ToArray();

            //var eDistance = SimilarityCalculations.CalculateEculeanDistanceCoefficient(ratingsIndividual, ratingsNeighbour);
            var eDistance = SimilarityCalculations.CalculatePearsonCoefficient(ratingsIndividual, ratingsNeighbour);

            return eDistance;
        }

        private void ProcessNeighbour(int neighbourId, Dictionary<int, double> neighbour, double similarity)
        {
            //If the neightbourRatingList isn't 'full' yet, add the neighbour
            if (_neighbourRatingsList.Count < _maxRatingListLength)
            {
                _neighbourRatingsList.Add(neighbourId, neighbour);

            }
            //If the list is 'full'
            else if (_neighbourRatingsList.Count == _maxRatingListLength)
            {
                //Find the key of the neighbour with the least similarity in the nearest neighbour list
                var lastKey = _similairtyValues.OrderByDescending(x => x.Value).Last(x => _neighbourRatingsList.ContainsKey(x.Key)).Key;
                //Check if this neighbour has a better similatrity than the values in the list
                if (_similairtyValues[lastKey] < similarity)
                {
                    //Remove the item with the lowest similarity from the Dictionary
                    _neighbourRatingsList.Remove(lastKey);
                    //Add new individual with a better similarity
                    _neighbourRatingsList.Add(neighbourId, neighbour);
                    //Update the threshhold by taking the similarity value of the last neighbour in the nearest neighbour list
                    _threshhold = _similairtyValues.OrderByDescending(x => x.Value).Last(x => _neighbourRatingsList.ContainsKey(x.Key)).Value;
                }
            }
        }
    }

    public class KeyValueObject
    {
        public int Key { get; set; }
        public double Similarity { get; set; }
    }
}