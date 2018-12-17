using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToLinearLeftRightStrategy : GridToLinearStrategy
    {
        public GridToLinearLeftRightStrategy(string[,] grid) : base(grid)
        {
        }

        public override LinearView GridToLinear()
        {
            int rowsCount = Grid.GetLength(0);
            int columnsCount = Grid.GetLength(1);
            int stringPosition = 0;
            StringBuilder gridAsLinear = new StringBuilder("", columnsCount * rowsCount);
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    gridAsLinear.Append(Grid[rowNumber, columnNumber]);
                    indexToGridPosition.Add(stringPosition, new Point(columnNumber, rowNumber));
                    stringPosition++;
                }
            }

            return new LinearView(gridAsLinear.ToString(), indexToGridPosition);
        }
    }
}