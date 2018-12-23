using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgram
    {
        private IConsoleWrapper _consoleWrapper;
        private IFileOperations _fileOperations;
        private IWordFinder _wordFinder;

        public WordSearchProgram(IConsoleWrapper consoleWrapper, IFileOperations fileOperations, IWordFinder wordFinder)
        {
            _consoleWrapper = consoleWrapper;
            _fileOperations = fileOperations;
            _wordFinder = wordFinder;
        }

        public void WriteGridToConsole(string[,] grid, ConsoleColor foregroundColor, ConsoleColor backgroundColor, PointList coordinatesToHighlight = null)
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

        public (string, string[,]) ConvertPuzzleFileToSearchWordsAndGrid(string filePath)
        {
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

        public PointList WriteSolvedPuzzleCoordinatesToConsole(string searchString, string[,] grid)
        {
            string[] searchWords = searchString.Split(',');

            PointList points = new PointList();

            _wordFinder.SetSearchOrientations(GetSearchOrientations(grid));

            foreach (var searchWord in searchWords)
            {
                var coordinates = _wordFinder.GetCoordinatesOfSearchTarget(searchWord, "");
                if (coordinates != null && coordinates.Count != 0)
                {
                    _consoleWrapper.WriteLine($"{searchWord}: " + $"{coordinates.ToString()}");
                }
            }

            return points;
        }

        private List<ISearchOrientation> GetSearchOrientations(string[,] grid)
        {
            return new List<ISearchOrientation>() 
            {
                new SearchOrientation(new GridToLinearLeftRightStrategy(grid)),
                new SearchOrientation(new GridToLinearRightLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopBottomStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomTopStrategy(grid)),
                new SearchOrientation(new GridToLinearTopLeftBottomRightStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomRightTopLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopRightBottomLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomLeftTopRightStrategy(grid))
            };
        }
    }
}