using System;
using System.Collections.Generic;
using System.Linq;

namespace SimilatiryMeasures
{
    public class KNearestNeighbours
    {
        private Dictionary<int, Dictionary<int, double>> _neighbourRatingsList;
        private int _maxRatingListLength;
        private double _threshhold;
        private Dictionary<int, double> _similairtyValues;
        private double _lowestSimilarity;


        //TODO improve speed by replacing excessive code 
        private void GetNearestNeighbours(int individualId, Dictionary<int, double> individual, Dictionary<int, Dictionary<int, double>> neighbours, int neighbourRankingsListLength, int initialThreshold)
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
                //If the similairty is larger or equal to the threshhold  and  the individual has the rated the same items 
                if (similarity >= _threshhold && HasRatedAdditionalItems(individual, neighbour.Value))
                {
                    ProcessNeighbour(neighbour.Key, neighbour.Value, similarity);
                }

            }


        }

        private bool HasRatedAdditionalItems(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {


            return true;
        }

        private double CalculateSimilarity(Dictionary<int, double> individual, Dictionary<int, double> neighbour)
        {
            var ratingsIndividual = individual.Values.Select(i => i).ToArray();
            var ratingsNeighbour = neighbour.Values.Select(i => i).ToArray();
            var result = SimilarityCalculations.CalculateEculeanDistanceCoefficient(ratingsIndividual, ratingsNeighbour);


            return result;
        }

        private void ProcessNeighbour(int neighbourId, Dictionary<int, double> neighbour, double similarity)
        {
            //If the neightbourRatingList isn't 'full' yet, add the neighbour
            if (_neighbourRatingsList.Count < _threshhold)
            {
                _neighbourRatingsList.Add(neighbourId, neighbour);

            }
            //If the list is 'full'
            else if (_neighbourRatingsList.Count == _maxRatingListLength)
            {
                //Check if this neighbour has a better similatrity than the values in the list
                if (_similairtyValues.Last().Value < similarity)
                {
                    //Remove the item with the lowest similarity from the Dictionary
                    _neighbourRatingsList.Remove(_similairtyValues.Last().Key);
                    //Add new individual with a better similarity
                    _neighbourRatingsList.Add(neighbourId, neighbour);
                    _threshhold = Convert.ToDouble(similarity);
                }
            }
        }
    }
}