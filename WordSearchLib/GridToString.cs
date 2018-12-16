using System;
using System.Text;

namespace WordSearch.WordSearchLib
{
    public class GridToString
    {
        public string Convert(string[,] grid)
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

            Console.WriteLine((columnsCount * rowsCount).ToString());
            Console.WriteLine(gridAsString.Length);

            return gridAsString.ToString();
        }
    }
}
