using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinear
    {
        public LinearView ConvertToLeftToRight(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    gridAsLinear.Append(grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }

        public LinearView ConvertToRightToLeft(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int rowNumber = rowsCount-1; rowNumber >= 0; rowNumber--)
            {
                for (int columnNumber = columnsCount-1; columnNumber >= 0; columnNumber--)
                {
                    gridAsLinear.Append(grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }

        public LinearView ConvertToTopToBottom(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
                {
                    gridAsLinear.Append(grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }

        public LinearView ConvertToBottomToTop(string[,] grid)
        {
            int rowsCount = grid.GetLength(0);
            int columnsCount = grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int columnNumber = columnsCount - 1; columnNumber >= 0; columnNumber--)
            {
                for (int rowNumber = rowsCount - 1; rowNumber >= 0; rowNumber--)
                {
                    gridAsLinear.Append(grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }
    }
}
