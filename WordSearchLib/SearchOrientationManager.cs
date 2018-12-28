using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class SearchOrientationManager : ISearchOrientationManager
    {
        public SearchOrientationManager()
        {
        }

        public List<ISearchOrientation> GetSearchOrientations(IGridValidator gridValidator, string[,] grid)
        {
            gridValidator.Validate(grid);

            return new List<ISearchOrientation>() 
            {
                new SearchOrientation(new GridToLinearLeftRightStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearRightLeftStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearTopBottomStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearBottomTopStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearTopLeftBottomRightStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearBottomRightTopLeftStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearTopRightBottomLeftStrategy(gridValidator, grid)),
                new SearchOrientation(new GridToLinearBottomLeftTopRightStrategy(gridValidator, grid))
            };
        }

    }
}