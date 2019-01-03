using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearDiagonalNESWStrategy : GridToLinearStrategy
    {
        public GridToLinearDiagonalNESWStrategy(IGridManager gridManager) : base(gridManager)
        {
        }

        /// <summary>
        /// Traverse the grid to create a string that enables us to search
        /// for words that have a diagonal orientation from upper right to lower left
        /// </summary>
        public override ILinearView GridToLinear()
        {

            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            int columnNumber = 0;
            int rowNumber = 0;
            int maxColumnNumber = 0;
            int minRowNumber = 0;

            do
            {
                gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                stringPosition++;

                columnNumber--;
                rowNumber++;

                //if we're at the left edge...
                if (columnNumber < 0)
                {
                    gridAsLinear.Append("|");

                    maxColumnNumber++;
                    if (maxColumnNumber > columnsCount - 1)
                    {
                        maxColumnNumber = columnsCount - 1;
                        minRowNumber++;
                    }
                    columnNumber = maxColumnNumber;
                    rowNumber = minRowNumber;
                }

                //if we're at the bottom edge
                if (rowNumber > rowsCount - 1)
                {
                    minRowNumber++;

                    if (minRowNumber < rowsCount)
                    {
                        gridAsLinear.Append("|");
                    }

                    rowNumber = minRowNumber;
                    columnNumber = maxColumnNumber;
                }

            } while (minRowNumber < rowsCount);

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);

        }
    }
}