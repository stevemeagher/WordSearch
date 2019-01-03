using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using WordSearch.ConsoleApp;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    [Collection("WordSearchProgram Collection")]
    public class WordSearchProgramHelperHelperUnitTests
    {
        private readonly TestUtilities _testUtilities;
        private readonly StringWriter _consoleOuput;
        private readonly TextWriter _originalConsoleOutput;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string TEST_DIRECTORY = "Test_WordSearchProgramHelper";

        public WordSearchProgramHelperHelperUnitTests()
        {
            _testUtilities = new TestUtilities();
            _originalConsoleOutput = Console.Out;
            _consoleOuput = new StringWriter();
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, null, null, null);

            //act
            wordSearchProgramHelper.WriteGridToConsole(grid, ConsoleColor.Gray, ConsoleColor.Black);

            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == output);
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            wordSearchProgramHelper.WriteGridToConsole(grid, ConsoleColor.Gray, ConsoleColor.Black, new PointList(){ new Point(xcoord, ycoord)});
            var output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void WriteNumberedFileNamesToConsole_WhenWellFormedFilePathsArePassedIn_WritesNumberedFileNamesToConsole()
        {
            //arrange
            string fileName = "EvilMorty.txt";
            string expected = $"(1) {fileName}\n";
            string[] filePaths = new string[1];
            Mock<IFileOperations> mockFileOperations = new Mock<IFileOperations>();
            mockFileOperations.Setup(m => m.GetFileNameFromPath(It.IsAny<string>())).Returns(() => fileName);

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, mockFileOperations.Object, null, null);

            //act
            wordSearchProgramHelper.WriteNumberedFileNamesToConsole(filePaths);
            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == output);
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);
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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, null, null, null);

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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

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
            Mock<IFileOperations> mockFileOperations = new Mock<IFileOperations>();
            string applicationBasePath = "basePath";
            bool directoryExists = true;
            string[] directoryContents = new string[] {"one.txt", "two.txt", "three.txt"};
            mockFileOperations.Setup(m => m.ApplicationBasePath(It.IsAny<string>())).Returns(() => applicationBasePath);
            mockFileOperations.Setup(m => m.DirectoryExists(It.IsAny<string>())).Returns(() => directoryExists);
            mockFileOperations.Setup(m => m.GetDirectoryContents(It.IsAny<string>())).Returns(() => directoryContents);

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, mockFileOperations.Object, null, null);

            //act
            var filePaths = wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory(TestUtilities.TEST_PUZZLES_DIRECTORY);

            //assert
            Assert.Equal(directoryContents, filePaths);
        }

        [Fact]
        public void GetPuzzleFilePathsFromPuzzleDirectory_WhenPuzzleDirectoryDoesNotExist_ListOfFilePathsReturned()
        {
            //arrange
            Mock<IFileOperations> mockFileOperations = new Mock<IFileOperations>();
            string applicationBasePath = "basePath";
            bool directoryExists = false;
            mockFileOperations.Setup(m => m.ApplicationBasePath(It.IsAny<string>())).Returns(() => applicationBasePath);
            mockFileOperations.Setup(m => m.DirectoryExists(It.IsAny<string>())).Returns(() => directoryExists);

            string directory = "NODIRECTORY";
            string fullPath = $"{applicationBasePath}/{directory}";
            string expectedMessage = $"directory does not exist: {fullPath}";            
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, mockFileOperations.Object, null, null);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory(directory));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}