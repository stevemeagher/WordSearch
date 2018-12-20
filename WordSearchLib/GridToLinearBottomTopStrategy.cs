using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearBottomTopStrategy : GridToLinearStrategy
    {
        public GridToLinearBottomTopStrategy(string[,] grid) : base(grid)
        {
        }

        /// <summary>
        /// Traverse the grid to create a string that enables us to search
        /// for words that have a vertical orientation from bottom to top
        /// </summary>
        public override LinearView GridToLinear()
        {
            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int columnNumber = columnsCount - 1; columnNumber >= 0; columnNumber--)
            {
                for (int rowNumber = rowsCount - 1; rowNumber >= 0; rowNumber--)
                {
                    gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }

                if (columnNumber > 0)
                {
                    gridAsLinear.Append("|");
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }
    }
}