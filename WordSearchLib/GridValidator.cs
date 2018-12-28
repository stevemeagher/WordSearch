using System;
using System.Text.RegularExpressions;

namespace WordSearch.WordSearchLib
{
    public class GridValidator : IGridValidator
    {
        public void Validate(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);

            if (rowsCount != columnsCount) throw new ArgumentException("grid has a mismatch between the number of rows and columns.");

            Regex validator = new Regex("[a-zA-Z0-9]");

            for (int i = 0; i < columnsCount; i++)
            {
                for (int j = 0; j < rowsCount; j++)
                {
                    if (!validator.IsMatch(grid[j,i]))
                    {
                        throw new ArgumentException($"grid is not valid - at least one element is an invalid character: {grid[j,i]}");
                    }
                }
            }
        }
    }
}