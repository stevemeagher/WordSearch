using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public interface IWordFinder
    {
        PointList GetCoordinatesOfSearchTarget(string searchTarget, string targetNotFoundMessage = "");

        void SetSearchOrientations(List<ISearchOrientation> searchOrientations);
    }
}