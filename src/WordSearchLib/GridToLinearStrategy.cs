namespace WordSearch.WordSearchLib
{
    public abstract class GridToLinearStrategy
    {
        protected IGridManager GridManager {get; set;}

        protected string[,] Grid
        {
            get
            {
                return GridManager.Grid;
            }
        }

        public GridToLinearStrategy(IGridManager gridManager)
        {
            GridManager = gridManager;
        }

        public abstract ILinearView GridToLinear();
    }
}