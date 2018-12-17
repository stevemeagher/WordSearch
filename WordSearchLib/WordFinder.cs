using System.Text;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class WordFinder
    {
        private List<ISearchOrientation> _searchOrientations;

        public WordFinder(List<ISearchOrientation> searchOrientations)
        {
            _searchOrientations = searchOrientations;
        }

        public string GetCoordinatesOfSearchTarget(string searchTarget, string searchFailedMessage = "")
        {
            foreach (var searchOrientation in _searchOrientations)
            {
                if (searchOrientation.IsSearchTargetFound(searchTarget))
                {
                    return searchOrientation.GetCoordinatesOfSearchTarget(searchTarget);
                }
            }

            return searchFailedMessage;
        }
    }
}