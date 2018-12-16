using System;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToString
    {
        public string ConvertToLeftToRight(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            StringBuilder gridAsString = new StringBuilder("", columnsCount * rowsCount);

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    gridAsString.Append(grid[rowNumber, columnNumber]);
                }
            }

            return gridAsString.ToString();
        }

        public string ConvertToRightToLeft(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            StringBuilder gridAsString = new StringBuilder("", columnsCount * rowsCount);

            for (int rowNumber = rowsCount-1; rowNumber >= 0; rowNumber--)
            {
                for (int columnNumber = columnsCount-1; columnNumber >= 0; columnNumber--)
                {
                    gridAsString.Append(grid[rowNumber, columnNumber]);
                }
            }

            return gridAsString.ToString();
        }

        public string ConvertToTopToBottom(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            StringBuilder gridAsString = new StringBuilder("", columnsCount * rowsCount);

            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
                {
                    gridAsString.Append(grid[rowNumber, columnNumber]);
                }
            }

            return gridAsString.ToString();
        }

        public string ConvertToBottomToTop(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            StringBuilder gridAsString = new StringBuilder("", columnsCount * rowsCount);

            for (int columnNumber = columnsCount - 1; columnNumber >= 0; columnNumber--)
            {
                for (int rowNumber = rowsCount - 1; rowNumber >= 0; rowNumber--)
                {
                    gridAsString.Append(grid[rowNumber, columnNumber]);
                }
            }

            return gridAsString.ToString();
        }
    }
}
