namespace WordSearch.WordSearchLib
{
    public abstract class GridToLinearStrategy
    {
        protected string[,] Grid {get; private set;}
        protected IGridValidator GridValidator {get; set;}

        public GridToLinearStrategy(IGridValidator gridValidator, string [,] grid)
        {
            gridValidator.Validate(grid);

            Grid = grid;
            GridValidator = gridValidator;
        }

        public abstract ILinearView GridToLinear();
    }
}