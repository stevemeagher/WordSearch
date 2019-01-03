using System.Collections.Generic;
using System.Drawing;

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
                new SearchOrientation(new GridToLinearHorizontalStrategy(gridManager)),
                new SearchOrientation(new GridToLinearVerticalStrategy(gridManager)),
                new SearchOrientation(new GridToLinearDiagonalNWSEStrategy(gridManager)),
                new SearchOrientation(new GridToLinearDiagonalNESWStrategy(gridManager)),
            };

        }
        

    }
}