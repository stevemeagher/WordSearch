namespace WordSearch.WordSearchLib
{
    public abstract class GridToLinearStrategy
    {
        protected string[,] Grid {get; private set;}

        public GridToLinearStrategy(string [,] grid)
        {
            Grid = grid;
        }

        public abstract ILinearView GridToLinear();
    }
}