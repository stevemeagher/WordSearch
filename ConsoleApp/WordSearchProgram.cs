using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WordSearch.FileLib;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgram
    {
        private IConsoleWrapper _consoleWrapper;
        private IFileOperations _fileOperations;

        public WordSearchProgram(IConsoleWrapper consoleWrapper, IFileOperations fileOperations)
        {
            _consoleWrapper = consoleWrapper;
            _fileOperations = fileOperations;
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

        public (string, string[,]) GetSearchStringsAndGridFromPuzzleFile(string filePath)
        {
            Console.WriteLine(filePath);
            string[] fileRows = _fileOperations.ReadLines(filePath); 

            var searchStrings = fileRows[0];
            var gridRows = fileRows.Skip(1).ToArray();

            string[,] grid = new string[gridRows[0].Count(o => o == ',') + 1, gridRows.Count()];

            int columnCount;
            int rowCount = 0;

            foreach (var row in gridRows)
            {
                string[] rowChars = row.Split(',');
                columnCount = 0;
                foreach (var rowChar in rowChars)
                {
                    grid[rowCount,columnCount] = rowChar;
                    columnCount++;
                }
                rowCount++;
            }

            return (searchStrings, grid);

        }
    }
}