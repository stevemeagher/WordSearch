using System;
using System.Text.RegularExpressions;

namespace WordSearch.WordSearchLib
{
    public class GridManager : IGridManager
    {
        private readonly string[,] _grid;
        
        public GridManager(string[,] grid)
        {
            _grid = grid;
            ValidateGrid();
        }
        
        public string[,] Grid { get => _grid;}

        private void ValidateGrid()
        {
            if (_grid == null) throw new ArgumentException("grid is null.");

            int rowsCount = _grid.GetLength(0);
            int columnsCount = _grid.GetLength(1);

            if (rowsCount == 0 || columnsCount == 0) throw new ArgumentException("grid has zero rows and/or columns.");

            if (rowsCount != columnsCount) throw new ArgumentException("grid has a mismatch between the number of rows and columns.");

            Regex validator = new Regex("[a-zA-Z0-9]");

            for (int i = 0; i < columnsCount; i++)
            {
                for (int j = 0; j < rowsCount; j++)
                {
                    if (_grid[j,i].Length > 1)
                    {
                        throw new ArgumentException("grid has more than one character in at least one coordinate.");
                    }
                    else if (!validator.IsMatch(_grid[j,i]))
                    {
                        throw new ArgumentException($"grid is not valid - at least one element is an invalid character: {_grid[j,i]}");
                    }
                }
            }
        }
    }

    
}