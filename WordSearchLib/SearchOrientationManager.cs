using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class SearchOrientationManager : ISearchOrientationManager
    {
        public SearchOrientationManager()
        {
        }

        public List<ISearchOrientation> GetSearchOrientations(IGridManager gridManager)
        {
            if (!gridManager.IsGridValidated)
            {
                gridManager.ValidateGrid();
            }

            return new List<ISearchOrientation>() 
            {
                new SearchOrientation(new GridToLinearLeftRightStrategy(gridManager)),
                new SearchOrientation(new GridToLinearRightLeftStrategy(gridManager)),
                new SearchOrientation(new GridToLinearTopBottomStrategy(gridManager)),
                new SearchOrientation(new GridToLinearBottomTopStrategy(gridManager)),
                new SearchOrientation(new GridToLinearTopLeftBottomRightStrategy(gridManager)),
                new SearchOrientation(new GridToLinearBottomRightTopLeftStrategy(gridManager)),
                new SearchOrientation(new GridToLinearTopRightBottomLeftStrategy(gridManager)),
                new SearchOrientation(new GridToLinearBottomLeftTopRightStrategy(gridManager))
            };
        }

    }
}