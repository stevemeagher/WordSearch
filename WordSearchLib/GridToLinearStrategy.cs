namespace WordSearch.WordSearchLib
{
    public abstract class GridToLinearStrategy : IGridToLinearStrategy
    {
        protected string[,] Grid {get; private set;}

        public GridToLinearStrategy(string [,] grid)
        {
            Grid = grid;
        }

        public abstract LinearView GridToLinear();
    }
}