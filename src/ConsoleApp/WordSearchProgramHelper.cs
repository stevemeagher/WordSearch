using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgramHelper
    {
        private IConsoleWrapper _consoleWrapper;
        private IFileOperations _fileOperations;
        private IWordFinder _wordFinder;
        private ISearchOrientationManager _searchOrientationManager;

        public WordSearchProgramHelper(IConsoleWrapper consoleWrapper, IFileOperations fileOperations, IWordFinder wordFinder, ISearchOrientationManager searchOrientationManager)
        {
            _consoleWrapper = consoleWrapper;
            _fileOperations = fileOperations;
            _wordFinder = wordFinder;
            _searchOrientationManager = searchOrientationManager;
        }

        public void SetConsoleColors(ConsoleColor fgColor, ConsoleColor bgColor)
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

        public void WriteTitle()
        {
            _consoleWrapper.Clear();
            _consoleWrapper.WriteLine("---- Word Search ----");
            _consoleWrapper.WriteLine();
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

        public short ReadFileNumberFromConsole(int numFiles)
        {
            short selectedFileNumber;
            char selectedKey;

            do
            {
                _consoleWrapper.WriteLine();
                _consoleWrapper.Write("Select file number: ");
                selectedKey = _consoleWrapper.ReadKey().KeyChar;
                _consoleWrapper.WriteLine();
                _consoleWrapper.WriteLine();


            } while (!Int16.TryParse(selectedKey.ToString(), out selectedFileNumber) || selectedFileNumber < 1 || selectedFileNumber > numFiles);

            return selectedFileNumber;
        }

        public string[] GetPuzzleFilePathsFromPuzzleDirectory(string puzzleDirectory)
        {
            string puzzleDirectoryPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}/{puzzleDirectory}";

            if (!_fileOperations.DirectoryExists(puzzleDirectoryPath)) throw new ArgumentException($"directory does not exist: {puzzleDirectoryPath}");

            string[] puzzleFilePaths = _fileOperations.GetDirectoryContents(puzzleDirectoryPath);

            if (puzzleFilePaths.Length == 0) throw new ArgumentException($"puzzle directory contains no files: {puzzleDirectoryPath}");

            return puzzleFilePaths;
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

            _consoleWrapper.WriteLine();

            return (MenuSelection)Convert.ToInt16(response);
        }

        public void ShowPuzzleSolution(string searchWords, IGridManager gridManager, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            _consoleWrapper.Clear();
            PointList solutionCoordinates = WriteSolvedPuzzleCoordinatesToConsole(searchWords, gridManager);
            _consoleWrapper.WriteLine();
            WriteGridToConsole(gridManager.Grid, foregroundColor, backgroundColor, solutionCoordinates);
            _consoleWrapper.WriteLine();
        }

        public void ShowSolutionForEnteredWords(string searchWords, IGridManager gridManager, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            string searchWord = "";
            _consoleWrapper.Clear();
            _consoleWrapper.WriteLine(searchWords);
            _consoleWrapper.WriteLine();
            WriteGridToConsole(gridManager.Grid, foregroundColor, backgroundColor);

            //search for individual words in puzzle and view solution, or press enter to jump back to menu
            do
            {
                _consoleWrapper.WriteLine();
                searchWord = PromptForSearchWord();
                if (!String.IsNullOrEmpty(searchWord))
                {
                    _consoleWrapper.Clear();
                    _consoleWrapper.WriteLine(searchWords);
                    _consoleWrapper.WriteLine();
                    var coordinatesOfSearchTarget = WriteSolvedPuzzleCoordinatesToConsole(searchWord, gridManager);
                    _consoleWrapper.WriteLine();
                    WriteGridToConsole(gridManager.Grid, foregroundColor, backgroundColor, coordinatesOfSearchTarget);
                }
            } while (searchWord != "");
        }

        public string PromptForSearchWord()
        {
            _consoleWrapper.WriteLine("Enter a search word to find in puzzle or hit <enter> to return to the menu");
            _consoleWrapper.WriteLine();
            _consoleWrapper.Write("Search word: ");
            return _consoleWrapper.ReadLine();
        }

        public PointList WriteSolvedPuzzleCoordinatesToConsole(string searchString, IGridManager gridManager)
        {
            string[] searchWords = searchString.Split(',');

            PointList points = new PointList();

            _wordFinder.SetSearchOrientations(_searchOrientationManager.GetSearchOrientations(gridManager));

            foreach (var searchWord in searchWords)
            {
                var coordinatesOfSearchTarget = _wordFinder.GetCoordinatesOfSearchTarget(searchWord, $"Did not find {searchWord} in puzzle.");
                if (coordinatesOfSearchTarget != null && coordinatesOfSearchTarget.Count > 0)
                {
                    _consoleWrapper.WriteLine($"{searchWord}: " + $"{coordinatesOfSearchTarget.ToString()}");

                    //create list of all coordinates of grid that are part of the puzzle solution
                    foreach(var coordinate in coordinatesOfSearchTarget)
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
    }
}
