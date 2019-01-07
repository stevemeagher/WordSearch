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
    public class WordSearchProgramHelperUnitTests
    {
        private readonly TestUtilities _testUtilities;
        private readonly StringWriter _consoleOuput;
        private readonly TextWriter _originalConsoleOutput;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string TEST_DIRECTORY = "Test_WordSearchProgramHelper";

        public WordSearchProgramHelperUnitTests()
        {
            _testUtilities = new TestUtilities();
            _originalConsoleOutput = Console.Out;
            _consoleOuput = new StringWriter();
            _consoleWrapper = new ConsoleWrapper();

            Console.SetOut(_consoleOuput);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "A B C {Environment.NewLine}D E F {Environment.NewLine}G H I {Environment.NewLine}")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "A B C D {Environment.NewLine}E F G H {Environment.NewLine}I J K L {Environment.NewLine}M N O P {Environment.NewLine}")]
        public void WriteGridToConsole_WhenGridArrayPassedIn_WritesArrayContentsToConsole(string gridSource, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
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
        [InlineData("ABC|DEF|GHI", 1, 1, "A B C {Environment.NewLine}D <fg:Black><bg:Gray>E<fg:Gray><bg:Black> F {Environment.NewLine}G H I {Environment.NewLine}")]
        [InlineData("ABC|DEF|GHI", 2, 0, "A B <fg:Black><bg:Gray>C<fg:Gray><bg:Black> {Environment.NewLine}D E F {Environment.NewLine}G H I {Environment.NewLine}")]
        public void WriteGridToConsole_WhenGridArrayAndHighlightCoordinatesPassedIn_WritesArrayContentsToConsoleWithColorChanges(string gridSource, int xcoord, int ycoord, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
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
            string expected = $"(1) {fileName}{Environment.NewLine}";
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
            string expected = $"Enter a search word to find in puzzle or hit <enter> to return to the menu{Environment.NewLine}{Environment.NewLine}Search word: {searchWord}{Environment.NewLine}";
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
            string expected = $"(1) Show solution{Environment.NewLine}(2) Enter a search word{Environment.NewLine}(3) Select another puzzle{Environment.NewLine}(4) Exit{Environment.NewLine}{Environment.NewLine}Enter selection: {(int)menuSelection}{Environment.NewLine}";

            //act
            MenuSelection actualMenuSelection = wordSearchProgramHelper.PromptForMenuSelection();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(menuSelection, actualMenuSelection);
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData("51", "(1) Show solution{Environment.NewLine}(2) Enter a search word{Environment.NewLine}(3) Select another puzzle{Environment.NewLine}(4) Exit{Environment.NewLine}{Environment.NewLine}Enter selection: 5{Environment.NewLine}{Environment.NewLine}Please enter a number between 1 and 4{Environment.NewLine}{Environment.NewLine}Enter selection: 1{Environment.NewLine}", MenuSelection.ShowSolution)]
        [InlineData("9A2", "(1) Show solution{Environment.NewLine}(2) Enter a search word{Environment.NewLine}(3) Select another puzzle{Environment.NewLine}(4) Exit{Environment.NewLine}{Environment.NewLine}Enter selection: 9{Environment.NewLine}{Environment.NewLine}Please enter a number between 1 and 4{Environment.NewLine}{Environment.NewLine}Enter selection: A{Environment.NewLine}{Environment.NewLine}Please enter a number between 1 and 4{Environment.NewLine}{Environment.NewLine}Enter selection: 2{Environment.NewLine}", MenuSelection.EnterSearchWord)]
        public void PromptForMenuSelection_WhenUserSelectsNumberedOptionOutOfRange_RetryMessageDisplayed(string menuSelection, string expectedOutput, MenuSelection expectedMenuSelection)
        {
            //arrange
            expectedOutput = expectedOutput.Replace("{Environment.NewLine}", Environment.NewLine);
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
            string expected = $"<clear>---- Word Search ----{Environment.NewLine}{Environment.NewLine}";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock(true);
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, null, null, null);

            //act
            wordSearchProgramHelper.WriteTitle();
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData(5, "3", "{Environment.NewLine}Select puzzle number: 3{Environment.NewLine}{Environment.NewLine}")]
        [InlineData(10, "1", "{Environment.NewLine}Select puzzle number: 1{Environment.NewLine}{Environment.NewLine}")]
        public void ReadFileNumber_WhenUserEntersValidNumber_ThatNumberIsReturned(int numFiles, string userInput, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
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
        [InlineData(5, "71", "{Environment.NewLine}Select puzzle number: 7{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}Select puzzle number: 1{Environment.NewLine}{Environment.NewLine}")]
        [InlineData(10, "02", "{Environment.NewLine}Select puzzle number: 0{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}Select puzzle number: 2{Environment.NewLine}{Environment.NewLine}")]
        public void ReadFileNumber_WhenUserFirstEntersInvalidNumber_UserIsPromptedToEnterAgain(int numFiles, string userInput, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
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