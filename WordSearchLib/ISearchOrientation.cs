using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public interface ISearchOrientation
    {
        bool IsSearchTargetFound(string searchTarget);
        //string GetCoordinatesOfSearchTarget(string searchTarget);
        PointList GetCoordinatesOfSearchTarget(string searchTarget);
    }
}