using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgram
    {
        public void WriteGridToConsole(IConsoleWrapper consoleWrapper, string[,] grid, ConsoleColor foregroundColor, ConsoleColor backgroundColor, List<Point> coordinatesToHighlight = null)
        {
            var columnsCount = grid.GetLength(1);
            var rowsCount = grid.GetLength(0);

            consoleWrapper.ForegroundColor = foregroundColor;
            consoleWrapper.BackgroundColor = backgroundColor;

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    string letter = grid[rowNumber, columnNumber];

                    if (coordinatesToHighlight != null && coordinatesToHighlight.Contains(new Point(columnNumber, rowNumber)))
                    {
                        consoleWrapper.ForegroundColor = backgroundColor;
                        consoleWrapper.BackgroundColor = foregroundColor;
                    }

                    consoleWrapper.Write(letter);
                    
                    if (consoleWrapper.ForegroundColor != foregroundColor)
                        consoleWrapper.ForegroundColor = foregroundColor;

                    if (consoleWrapper.BackgroundColor != backgroundColor)
                        consoleWrapper.BackgroundColor = backgroundColor;

                    consoleWrapper.Write(" ");
                }
                consoleWrapper.WriteLine();
            }
        }
    }
}