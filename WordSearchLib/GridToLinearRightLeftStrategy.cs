using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearRightLeftStrategy : GridToLinearStrategy
    {
        public GridToLinearRightLeftStrategy(string[,] grid) : base(grid)
        {
        }

        /// <summary>
        /// Traverse the grid to create a string that enables us to search
        /// for words that have a horizontal orientation from right to left
        /// </summary>
        public override ILinearView GridToLinear()
        {
            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int rowNumber = rowsCount-1; rowNumber >= 0; rowNumber--)
            {
                for (int columnNumber = columnsCount-1; columnNumber >= 0; columnNumber--)
                {
                    gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }

                if (rowNumber > 0)
                {
                    gridAsLinear.Append("|");
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }
    }
}