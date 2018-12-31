using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearBottomRightTopLeftStrategy : GridToLinearStrategy
    {
        public GridToLinearBottomRightTopLeftStrategy(IGridManager gridManager) : base(gridManager)
        {
        }

        /// <summary>
        /// Traverse the grid to create a string that enables us to search
        /// for words that have a diagonal orientation from lower right to upper left
        /// </summary>
        public override ILinearView GridToLinear()
        {

            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            int columnNumber = 0;
            int rowNumber = rowsCount - 1;
            int maxColumnNumber = 0;
            int maxRowNumber = rowsCount - 1;

            do
            {
                gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                stringPosition++;

                columnNumber--;
                rowNumber--;

                if (columnNumber < 0)
                {
                    gridAsLinear.Append("|");
                    maxColumnNumber++;
                    if (maxColumnNumber > columnsCount - 1)
                    {
                        maxColumnNumber = columnsCount - 1;
                        maxRowNumber--;
                    }
                    columnNumber = maxColumnNumber;
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
                    columnNumber = maxColumnNumber;
                }

            } while (maxRowNumber >= 0);

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);

        }
    }
}