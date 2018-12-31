using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearTopLeftBottomRightStrategy : GridToLinearStrategy
    {
        public GridToLinearTopLeftBottomRightStrategy(IGridManager gridManager) : base(gridManager)
        {
        }

        /// <summary>
        /// Traverse the grid to create a string that enables us to search
        /// for words that have a diagonal orientation from upper left to lower right
        /// </summary>
        public override ILinearView GridToLinear()
        {

            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            int columnNumber = columnsCount - 1;
            int rowNumber = 0;
            int minColumnNumber = columnsCount - 1;
            int minRowNumber = 0;

            do
            {
                gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                stringPosition++;

                columnNumber++;
                rowNumber++;

                if (columnNumber > columnsCount - 1)
                {
                    gridAsLinear.Append("|");
                    minColumnNumber--;
                    if (minColumnNumber < 0)
                    {
                        minColumnNumber = 0;
                        minRowNumber++;
                    }
                    columnNumber = minColumnNumber;
                    rowNumber = minRowNumber;
                }

                if (rowNumber > rowsCount - 1)
                {
                    minRowNumber++;

                    if (minRowNumber < rowsCount)
                    {
                        gridAsLinear.Append("|");
                    }
                    
                    rowNumber = minRowNumber;
                    columnNumber = minColumnNumber;
                }

            } while (minRowNumber < rowsCount);

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);

        }
    }
}