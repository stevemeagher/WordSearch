using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgram
    {
        private IConsoleWrapper _consoleWrapper;

        public WordSearchProgram(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void WriteGridToConsole(string[,] grid, ConsoleColor foregroundColor, ConsoleColor backgroundColor, List<Point> coordinatesToHighlight = null)
        {
            var columnsCount = grid.GetLength(1);
            var rowsCount = grid.GetLength(0);

            _consoleWrapper.ForegroundColor = foregroundColor;
            _consoleWrapper.BackgroundColor = backgroundColor;

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    string letter = grid[rowNumber, columnNumber];

                    if (coordinatesToHighlight != null && coordinatesToHighlight.Contains(new Point(columnNumber, rowNumber)))
                    {
                        _consoleWrapper.ForegroundColor = backgroundColor;
                        _consoleWrapper.BackgroundColor = foregroundColor;
                    }

                    _consoleWrapper.Write(letter);
                    
                    if (_consoleWrapper.ForegroundColor != foregroundColor)
                        _consoleWrapper.ForegroundColor = foregroundColor;

                    if (_consoleWrapper.BackgroundColor != backgroundColor)
                        _consoleWrapper.BackgroundColor = backgroundColor;

                    _consoleWrapper.Write(" ");
                }
                _consoleWrapper.WriteLine();
            }
        }

        public void WriteNumberedFileNamesToConsole(string[] filePaths)
        {
            int counter = 1;

            foreach(var filePath in filePaths)
            {
                _consoleWrapper.WriteLine($"({counter}) {Path.GetFileName(filePath)}");
                counter++;
            }

        }
    }
}