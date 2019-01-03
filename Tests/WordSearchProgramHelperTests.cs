using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.ConsoleApp;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;
using System.Linq;

namespace Tests
{
    [Collection("WordSearchProgram Collection")]
    public class WordSearchProgramHelperHelperTests : IDisposable
    {
        private TestUtilities _testUtilities;
        private readonly StringWriter _consoleOuput;
        private readonly TextWriter _originalConsoleOutput;
        private readonly FileOperations _fileOperations;
        private readonly WordFinder _wordFinder;
        private readonly ISearchOrientationManager _searchOrientationManager;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string TEST_DIRECTORY = "Test_WordSearchProgramHelper";

        public WordSearchProgramHelperHelperTests()
        {
            _testUtilities = new TestUtilities();
            _originalConsoleOutput = Console.Out;
            _consoleOuput = new StringWriter();
            _fileOperations = new FileOperations();
            _wordFinder = new WordFinder();
            _searchOrientationManager = new SearchOrientationManager();
            _consoleWrapper = new ConsoleWrapper();

            Console.SetOut(_consoleOuput);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "A B C \nD E F \nG H I \n")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "A B C D \nE F G H \nI J K L \nM N O P \n")]
        public void WriteGridToConsole_WhenGridArrayPassedIn_WritesArrayContentsToConsole(string gridSource, string expected)
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid(gridSource);
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.WriteGridToConsole(grid, ConsoleColor.Gray, ConsoleColor.Black);

            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());
        }

        ///<summary>This test uses a class, ConsoleWrapperMock, which inherits from ConsoleWrapper and overrides the color setting properties so that
        ///we can "see" the color changes in the output and confirm that the output to the console should appear correctly.
        ///fg: indicates setting the foreground color and bg: indicates setting the background color
        ///</summary>
        [Theory]
        [InlineData("ABC|DEF|GHI", 1, 1, "A B C \nD <fg:Black><bg:Gray>E<fg:Gray><bg:Black> F \nG H I \n")]
        [InlineData("ABC|DEF|GHI", 2, 0, "A B <fg:Black><bg:Gray>C<fg:Gray><bg:Black> \nD E F \nG H I \n")]
        public void WriteGridToConsole_WhenGridArrayAndHighlightCoordinatesPassedIn_WritesArrayContentsToConsoleWithColorChanges(string gridSource, int xcoord, int ycoord, string expected)
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid(gridSource);
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            wordSearchProgramHelper.WriteGridToConsole(grid, ConsoleColor.Gray, ConsoleColor.Black, new PointList(){ new Point(xcoord, ycoord)});

            var output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected,_consoleOuput.ToString());
        }

        [Theory]
        [InlineData("c:/dir/file1.txt|c:/dir/file2.txt|c:/dir/file3.txt", "(1) file1.txt\n(2) file2.txt\n(3) file3.txt\n")]
        [InlineData("c:/dir1/dir2/file1.txt|c:/dir1/dir2/file2.txt|c:/dir1/dir2/file3.txt", "(1) file1.txt\n(2) file2.txt\n(3) file3.txt\n")]
        public void WriteNumberedFileNamesToConsole_WhenWellFormedFilePathsArePassedIn_WritesNumberedFileNamesToConsole(string filePathDelimeteredArray, string expected)
        {
            //arrange
            string[] filePaths = filePathDelimeteredArray.Split('|');
            IFileOperations fileOperations = new FileOperations();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.WriteNumberedFileNamesToConsole(filePaths);
            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());
        }

        [Theory]
        [InlineData("WORD1,WORD2,WORD3", "A,B,C|D,E,F|G,H,I", "puzzle.txt")]
        [InlineData("WORD1,WORD2,WORD3,WORD4", "A,B,C,D|E,F,G,H|I,J,K,L|M,N,O,P", "puzzle.txt")]
        public void GetSearchStringsAndGridFromPuzzleFile_WhenFileExistsInCorrectFormat_ReturnsFirstRowAsSearchStringAndAllOthersAsStringArray(string searchWords, string fileRowsDelimeteredArray, string puzzleFileName)
        {
            //arrange
            string workingDir = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + "/" + TEST_DIRECTORY;
            CreatePuzzleFile(workingDir, searchWords, fileRowsDelimeteredArray, puzzleFileName);

            //need to remove the commas to provide meaningful input to StringToGrid
            string[,] expectedGrid = _testUtilities.StringToGrid(fileRowsDelimeteredArray.Replace(",",""));

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var (searchString, grid) = wordSearchProgramHelper.ConvertPuzzleFileToSearchWordsAndGrid($"{workingDir}/{puzzleFileName}");

            //assert
            Assert.Equal(searchWords, searchString);
            Assert.Equal(expectedGrid, grid);
        }

        private void CreatePuzzleFile(string workingDir, string searchWords, string fileRowsDelimeteredArray, string puzzleFileName)
        {
            _testUtilities.CreateEmptyDirectory(workingDir);

            string[] puzzleRows = fileRowsDelimeteredArray.Split('|');  

            string[] puzzleForFile = new string[puzzleRows.Length + 1];
            puzzleForFile[0] = searchWords;
            for (int i = 0; i < puzzleRows.Length; i++)
            {
                puzzleForFile[i + 1] = puzzleRows[i];
            }

            File.WriteAllLines($"{workingDir}/{puzzleFileName}", puzzleForFile);
        }

        [Theory]
        [InlineData("AB,HEB", "A,B,C|D,E,F|G,H,I", "puzzle.txt","AB: (0,0),(1,0)\nHEB: (1,2),(1,1),(1,0)\n")]
        [InlineData("16AF", "1,2,3,4|5,6,7,8|9,0,A,B|C,D,E,F", "puzzle.txt","16AF: (0,0),(1,1),(2,2),(3,3)\n")]
        public void WriteSolvedPuzzleCoordinatesToConsole_WhenSearchWordFound_CoordinatesWrittenToConsole(string searchWords, string fileRowsDelimeteredArray, string puzzleFileName, string expected)
        {
            //arrange
            string workingDir = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + "/" + TEST_DIRECTORY;
            CreatePuzzleFile(workingDir, searchWords, fileRowsDelimeteredArray, puzzleFileName);

            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            var (searchString, grid) = wordSearchProgramHelper.ConvertPuzzleFileToSearchWordsAndGrid($"{workingDir}/{puzzleFileName}");
            IGridManager gridManager = new GridManager(grid);
            wordSearchProgramHelper.WriteSolvedPuzzleCoordinatesToConsole(searchString, gridManager);
            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());
        }

        [Theory]
        [InlineData("Z")]
        [InlineData("READLINERESULT")]
        [InlineData("Word")]
        public void PromptForSearchWord_WhenUserEntersWord_PromptDisplayedAndWordReturned(string searchWord)
        {
            //arrange
            string expected = $"Enter a search word to find in puzzle or hit <enter> to return to the menu\n\nSearch word: {searchWord}\n";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            ((ConsoleWrapperMock)consoleWrapper).ReadLineResults = new List<string>(){searchWord};
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            string searchWordOut = wordSearchProgramHelper.PromptForSearchWord();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(searchWord, searchWordOut);
            Assert.Equal(expected, output);
        }
        
        [Theory]
        [InlineData(MenuSelection.ShowSolution)]
        [InlineData(MenuSelection.EnterSearchWord)]
        [InlineData(MenuSelection.SelectAnotherFile)]
        [InlineData(MenuSelection.Exit)]
        public void PromptForMenuSelection_WhenUserSelectsNumberedOptionInRange_CorrectNumberReturned(MenuSelection menuSelection)
        {
            //arrange
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChar = ((int)menuSelection).ToString().ToCharArray().First();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            string expected = $"(1) Show solution\n(2) Enter a search word\n(3) Select another file\n(4) Exit\n\nEnter selection: {(int)menuSelection}\n";

            //act
            MenuSelection actualMenuSelection = wordSearchProgramHelper.PromptForMenuSelection();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(menuSelection, actualMenuSelection);
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData("51", "(1) Show solution\n(2) Enter a search word\n(3) Select another file\n(4) Exit\n\nEnter selection: 5\n\nPlease enter a number between 1 and 4\n\nEnter selection: 1\n", MenuSelection.ShowSolution)]
        [InlineData("9A2", "(1) Show solution\n(2) Enter a search word\n(3) Select another file\n(4) Exit\n\nEnter selection: 9\n\nPlease enter a number between 1 and 4\n\nEnter selection: A\n\nPlease enter a number between 1 and 4\n\nEnter selection: 2\n", MenuSelection.EnterSearchWord)]
        public void PromptForMenuSelection_WhenUserSelectsNumberedOptionOutOfRange_RetryMessageDisplayed(string menuSelection, string expectedOutput, MenuSelection expectedMenuSelection)
        {
            //arrange
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = menuSelection.ToCharArray().ToList();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            MenuSelection actualMenuSelection = wordSearchProgramHelper.PromptForMenuSelection();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expectedMenuSelection, actualMenuSelection);
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void WriteGridToConsole_WhenGridIsNull_ThrowArgumentException()
        {
            //arrange
            string expectedMessage = "grid is null.";
            string[,] grid = null;
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgramHelper.WriteGridToConsole(grid, ConsoleColor.Gray, ConsoleColor.Black));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void SetConsoleColors_WhenColorsAreDifferentToCurrentColors_ColorsAreChanged()
        {
            //arrange
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            string expected = "<fg:Cyan><bg:Green>";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Blue;
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.SetConsoleColors(ConsoleColor.Cyan, ConsoleColor.Green);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        [Fact]
        public void SetConsoleColors_WhenColorsAreSameAsCurrentColors_ColorsAreNotChanged()
        {
            //arrange
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            string expected = "";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Blue;
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.SetConsoleColors(ConsoleColor.Blue, ConsoleColor.DarkBlue);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        [Fact]
        public void SetConsoleColors_WhenOnlyBackgroundColorsIsDifferentToCurrentColor_OnlyBackgroundColorChanged()
        {
            //arrange
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            string expected = "<bg:Cyan>";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Blue;
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.SetConsoleColors(ConsoleColor.Blue, ConsoleColor.Cyan);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        [Fact]
        public void SetConsoleColors_WhenOnlyForegroundColorsIsDifferentToCurrentColor_OnlyForegroundColorChanged()
        {
            //arrange
            var fg = Console.ForegroundColor;
            var bg = Console.BackgroundColor;
            string expected = "<fg:Cyan>";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Blue;
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.SetConsoleColors(ConsoleColor.Cyan, ConsoleColor.DarkBlue);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        [Fact]
        public void WriteTitle_ClearsConsoleAndWritesTitle()
        {
            //arrange
            string expected = "<clear>---- Word Search ----\n\n";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock(true);
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgramHelper.WriteTitle();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData(5, "3", "\nSelect file number: 3\n\n")]
        [InlineData(10, "1", "\nSelect file number: 1\n\n")]
        public void ReadFileNumber_WhenUserEntersValidNumber_ThatNumberIsReturned(int numFiles, string userInput, string expected)
        {
            //arrange
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = userInput.ToCharArray().ToList();

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var userSelection = wordSearchProgramHelper.ReadFileNumberFromConsole(numFiles);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
            Assert.Equal(userInput, userSelection.ToString());
        }

        [Theory]
        [InlineData(5, "71", "\nSelect file number: 7\n\n\nSelect file number: 1\n\n")]
        [InlineData(10, "02", "\nSelect file number: 0\n\n\nSelect file number: 2\n\n")]
        public void ReadFileNumber_WhenUserFirstEntersInvalidNumber_UserIsPromptedToEnterAgain(int numFiles, string userInput, string expected)
        {
            //arrange
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = userInput.ToCharArray().ToList();

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var userSelection = wordSearchProgramHelper.ReadFileNumberFromConsole(numFiles);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
            Assert.Equal(userInput.Substring(userInput.Length-1), userSelection.ToString());
        }

        [Fact]
        public void GetPuzzleFilePathsFromPuzzleDirectory_WhenPuzzleDirectoryExistsAndContainsFiles_ListOfFilePathsReturned()
        {
            //arrange
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var filePaths = wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory("tests/testpuzzles");

            //assert
            Assert.True(filePaths.Length == 1);
        }

        [Fact]
        public void GetPuzzleFilePathsFromPuzzleDirectory_WhenPuzzleDirectoryDoesNotExist_ListOfFilePathsReturned()
        {
            //arrange
            string directory = "NODIRECTORY";
            string fullPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}/{directory}";
            string expectedMessage = $"directory does not exist: {fullPath}";            
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory(directory));
            Assert.Equal(expectedMessage, exception.Message);
        }

        public void Dispose()
        {
            Console.SetOut(_originalConsoleOutput);

            string workingDir = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + "/" + TEST_DIRECTORY;
            DirectoryInfo di = new DirectoryInfo(workingDir);
            if (di.Exists)
            {
                di.Delete(true);
            }
        }


    }
}