using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;

namespace WordSearch.ConsoleApp
{
    public enum MenuSelection
    {
        NoSelection = 0,
        ShowSolution,
        EnterSearchWord,
        SelectAnotherFile,
        Exit
    }

    public class WordSearchProgram
    {
        private IConsoleWrapper _consoleWrapper;
        private IFileOperations _fileOperations;
        private IWordFinder _wordFinder;
        private ISearchOrientationManager _searchOrientationManager;
        private IGridValidator _gridValidator;

        public WordSearchProgram(IConsoleWrapper consoleWrapper, IFileOperations fileOperations, IWordFinder wordFinder, ISearchOrientationManager searchOrientationManager, IGridValidator gridValidator)
        {
            _consoleWrapper = consoleWrapper;
            _fileOperations = fileOperations;
            _wordFinder = wordFinder;
            _searchOrientationManager = searchOrientationManager;
            _gridValidator = gridValidator;
        }

        public void WriteGridToConsole(string[,] grid, ConsoleColor foregroundColor, ConsoleColor backgroundColor, PointList coordinatesToHighlight = null)
        {
            if (grid == null) throw new ArgumentException("grid is null.");

            var columnsCount = grid.GetLength(1);
            var rowsCount = grid.GetLength(0);

            SetConsoleColors(foregroundColor, backgroundColor);

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    string letter = grid[rowNumber, columnNumber];

                    if (coordinatesToHighlight != null && coordinatesToHighlight.Contains(new Point(columnNumber, rowNumber)))
                    {
                        SetConsoleColors(backgroundColor, foregroundColor);
                    }

                    _consoleWrapper.Write(letter);
                    
                    SetConsoleColors(foregroundColor, backgroundColor);

                    _consoleWrapper.Write(" ");
                }
                _consoleWrapper.WriteLine();
            }
        }

        private void SetConsoleColors(ConsoleColor fgColor, ConsoleColor bgColor)
        {
            if (_consoleWrapper.ForegroundColor != fgColor)
            {
                _consoleWrapper.ForegroundColor = fgColor;
            }

            if (_consoleWrapper.BackgroundColor != bgColor)
            {
                _consoleWrapper.BackgroundColor = bgColor;
            }
        }

        public void WriteNumberedFileNamesToConsole(string[] filePaths)
        {
            int counter = 1;

            foreach(var filePath in filePaths)
            {
                _consoleWrapper.WriteLine($"({counter}) {_fileOperations.GetFileNameFromPath(filePath)}");
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

            _wordFinder.SetSearchOrientations(_searchOrientationManager.GetSearchOrientations(_gridValidator, grid));

            foreach (var searchWord in searchWords)
            {
                var coordinates = _wordFinder.GetCoordinatesOfSearchTarget(searchWord, $"Did not find {searchWord} in puzzle.");
                if (coordinates != null && coordinates.Count != 0)
                {
                    _consoleWrapper.WriteLine($"{searchWord}: " + $"{coordinates.ToString()}");

                    foreach(var coordinate in coordinates)
                    {
                        if (!points.Contains(coordinate))
                        {
                            points.Add(coordinate);
                        }
                    }
                }
            }

            return points.Count > 0 ? points : null;
        }

        public string PromptForSearchWord()
        {
            _consoleWrapper.WriteLine("Enter a search word to find in puzzle or hit <enter> to return to the menu");
            _consoleWrapper.WriteLine();
            _consoleWrapper.Write("Search word: ");
            return _consoleWrapper.ReadLine();
        }

        public MenuSelection PromptForMenuSelection()
        {
            _consoleWrapper.WriteLine("(1) Show solution");
            _consoleWrapper.WriteLine("(2) Enter a search word");
            _consoleWrapper.WriteLine("(3) Select another file");
            _consoleWrapper.WriteLine("(4) Exit");
            _consoleWrapper.WriteLine();

            string response = "";
            bool responseValid;

            do
            {
                _consoleWrapper.Write("Enter selection: ");
                response = _consoleWrapper.ReadKey().KeyChar.ToString();

                responseValid = "1234".Contains(response);

                if (!responseValid)
                {
                    _consoleWrapper.WriteLine();
                    _consoleWrapper.WriteLine();
                    _consoleWrapper.WriteLine("Please enter a number between 1 and 4");
                    _consoleWrapper.WriteLine();
                }

            } while (!responseValid);

            return (MenuSelection)Convert.ToInt16(response);
        }
    }
}