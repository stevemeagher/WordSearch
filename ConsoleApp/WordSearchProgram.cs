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
        private WordSearchProgramHelper _programHelper;

        public WordSearchProgram(IConsoleWrapper consoleWrapper, IFileOperations fileOperations, IWordFinder wordFinder, ISearchOrientationManager searchOrientationManager)
        {
            _programHelper = new WordSearchProgramHelper(consoleWrapper, fileOperations, wordFinder, searchOrientationManager);
            _consoleWrapper = consoleWrapper;
        }

        public void ProgramLoop(string puzzleDirectory)
        {
            ConsoleColor foregroundColor = ConsoleColor.Gray;
            ConsoleColor backgroundColor = ConsoleColor.Black;

            _programHelper.SetConsoleColors(foregroundColor, backgroundColor);

            string[] puzzleFilePaths = _programHelper.GetPuzzleFilePathsFromPuzzleDirectory(puzzleDirectory);
            
            MenuSelection menuSelection = MenuSelection.NoSelection;

            //main loop for selecting puzzle files
            do
            {
                _programHelper.WriteTitle();

                _programHelper.WriteNumberedFileNamesToConsole(puzzleFilePaths);

                short fileListNumber = _programHelper.ReadFileNumber(puzzleFilePaths.Count());

                _consoleWrapper.Clear();

                var (searchWords, grid) = _programHelper.ConvertPuzzleFileToSearchWordsAndGrid(puzzleFilePaths[fileListNumber - 1]);

                IGridManager gridManager = new GridManager(grid);
                gridManager.ValidateGrid();

                _consoleWrapper.WriteLine(searchWords);

                _consoleWrapper.WriteLine();

                _programHelper.WriteGridToConsole(gridManager.Grid, foregroundColor, backgroundColor);

                //loop for selecting menu actions (solve puzzle, search for a word, go back to main loop, exit program)
                do
                {
                    _consoleWrapper.WriteLine();

                    menuSelection = _programHelper.PromptForMenuSelection();

                    switch(menuSelection)
                    {
                        case MenuSelection.ShowSolution:  
                            _programHelper.ShowPuzzleSolution(searchWords, gridManager, foregroundColor, backgroundColor);
                            break;
                        case MenuSelection.EnterSearchWord:   
                            _programHelper.ShowSolutionForEnteredWords(searchWords, gridManager, foregroundColor, backgroundColor);
                            break;
                        default:
                            break;
                    }
                } while (menuSelection != MenuSelection.SelectAnotherFile && menuSelection != MenuSelection.Exit);
            } while (menuSelection != MenuSelection.Exit);
        }
    }
}
