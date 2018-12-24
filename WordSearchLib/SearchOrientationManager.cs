using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class SearchOrientationManager : ISearchOrientationManager
    {
        public SearchOrientationManager()
        {
        }

        public List<ISearchOrientation> GetSearchOrientations(string[,] grid)
        {
            return new List<ISearchOrientation>() 
            {
                new SearchOrientation(new GridToLinearLeftRightStrategy(grid)),
                new SearchOrientation(new GridToLinearRightLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopBottomStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomTopStrategy(grid)),
                new SearchOrientation(new GridToLinearTopLeftBottomRightStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomRightTopLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopRightBottomLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomLeftTopRightStrategy(grid))
            };
        }

    }
}