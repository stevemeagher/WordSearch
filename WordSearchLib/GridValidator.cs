using System;
using System.Text.RegularExpressions;

namespace WordSearch.WordSearchLib
{
    public class GridValidator : IGridValidator
    {
        public bool Validate(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);

            if (rowsCount != columnsCount) throw new ArgumentException("grid has a mismatch between the number of rows and columns.");

            return true;
        }
    }
}