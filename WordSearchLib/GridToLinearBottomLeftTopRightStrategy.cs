using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearBottomLeftTopRightStrategy : GridToLinearStrategy
    {
        public GridToLinearBottomLeftTopRightStrategy(string[,] grid) : base(grid)
        {
        }

        /// <summary>
        /// Traverse the grid such that we create a string that enables us to search
        /// for words that have a diagonal orientation from lower left to upper right
        /// </summary>
        public override LinearView GridToLinear()
        {

            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            int columnNumber = columnsCount - 1;
            int rowNumber = rowsCount - 1;
            int minColumnNumber = columnsCount - 1;
            int maxRowNumber = rowsCount - 1;

            do
            {
                gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                stringPosition++;

                columnNumber++;
                rowNumber--;

                if (columnNumber > columnsCount - 1)
                {
                    gridAsLinear.Append("|");
                    minColumnNumber--;
                    if (minColumnNumber < 0)
                    {
                        minColumnNumber = 0;
                        maxRowNumber--;
                    }
                    columnNumber = minColumnNumber;
                    rowNumber = maxRowNumber;
                }

                if (rowNumber < 0)
                {
                    if (maxRowNumber > 0)
                    {
                        gridAsLinear.Append("|");
                    }
                    
                    maxRowNumber--;
                    rowNumber = maxRowNumber;
                    columnNumber = minColumnNumber;
                }

            } while (maxRowNumber >= 0);

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);

        }
    }
}