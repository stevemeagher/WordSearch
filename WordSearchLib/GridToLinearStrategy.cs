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
            if (!gridManager.IsGridValidated)
            {
                gridManager.ValidateGrid();
            }

            GridManager = gridManager;
        }

        public abstract ILinearView GridToLinear();
    }
}