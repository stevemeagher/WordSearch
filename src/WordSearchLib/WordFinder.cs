using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;

namespace WordSearch.WordSearchLib
{
    public class WordFinder : IWordFinder
    {
        private List<ISearchOrientation> _searchOrientations;

        public WordFinder()
        {
        }

        public WordFinder(List<ISearchOrientation> searchOrientations)
        {
            SetSearchOrientations(searchOrientations);
        }

        public PointList GetCoordinatesOfSearchTarget(string searchTarget, string targetNotFoundMessage = "")
        {
            foreach (var searchOrientation in _searchOrientations)
            {
                if (searchOrientation.IsSearchTargetFound(searchTarget))
                {
                    return searchOrientation.GetCoordinatesOfSearchTarget(searchTarget);
                }
            }

            return new PointList(targetNotFoundMessage);
        }

        public void SetSearchOrientations(List<ISearchOrientation> searchOrientations)
        {
            if (searchOrientations is null) throw new ArgumentException("searchOrientations parameter is null.");
            if (searchOrientations.Count == 0) throw new ArgumentException("searchOrientations list is empty.");

            _searchOrientations = searchOrientations;
        }
    }
}