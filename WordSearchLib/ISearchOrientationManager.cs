using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public interface ISearchOrientationManager
    {
        List<ISearchOrientation> GetSearchOrientations(IGridValidator gridValidator, string[,] grid);
    }
}