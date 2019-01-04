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

namespace WordSearch.Tests.IntegrationTests
{
    [Collection("WordSearchProgram Collection")]
    public class WordSearchProgramIntegrationTests : IDisposable
    {
        private TestUtilities _testUtilities;
        private readonly StringWriter _consoleOuput;
        private readonly TextWriter _originalConsoleOutput;
        private readonly FileOperations _fileOperations;
        private readonly WordFinder _wordFinder;
        private readonly ISearchOrientationManager _searchOrientationManager;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string TEST_DIRECTORY = "Test_WordSearchProgram";

        public WordSearchProgramIntegrationTests()
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

        [Fact]
        public void ProgramLoop_WhenUserSelectsFirstPuzzleAndExit_OuputContainsFileListAndGridAndMenuOptions()
        {
            //arrange
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            string expected = "---- Word Search ----\n\n(1) wordsearch.txt\n\nSelect puzzle number: 1\n\nAD,IE\n\nA B C \nD E F \nG H I \n\n(1) Show solution\n(2) Enter a search word\n(3) Select another puzzle\n(4) Exit\n\nEnter selection: 4\n";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = new List<char>() {'1', '4'};
            WordSearchProgram wordSearchProgram = new WordSearchProgram(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgram.ProgramLoop(TestUtilities.TEST_PUZZLES_DIRECTORY);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void ProgramLoop_WhenUserSelectsFirstPuzzleAndShowSolutionAndExit_OuputContainsFileListAndGridAndMenuOptionsAndSolution()
        {
            //arrange
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            string expected = "---- Word Search ----\n\n(1) wordsearch.txt\n\nSelect puzzle number: 1\n\nAD,IE\n\nA B C \nD E F \nG H I \n\n(1) Show solution\n(2) Enter a search word\n(3) Select another puzzle\n(4) Exit\n\nEnter selection: 1\nAD: (0,0),(0,1)\nIE: (2,2),(1,1)\n\n<fg:Black><bg:Gray>A<fg:Gray><bg:Black> B C \n<fg:Black><bg:Gray>D<fg:Gray><bg:Black> <fg:Black><bg:Gray>E<fg:Gray><bg:Black> F \nG H <fg:Black><bg:Gray>I<fg:Gray><bg:Black> \n\n\n(1) Show solution\n(2) Enter a search word\n(3) Select another puzzle\n(4) Exit\n\nEnter selection: 4\n";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = new List<char>() {'1', '1', '4'};
            WordSearchProgram wordSearchProgram = new WordSearchProgram(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgram.ProgramLoop(TestUtilities.TEST_PUZZLES_DIRECTORY);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void ProgramLoop_WhenUserSelectsFirstPuzzleAndEnterSearchWordOfABAndExit_OuputContainsFileListAndGridAndMenuOptionsAndSolution()
        {
            //arrange
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            string expected = "---- Word Search ----\n\n(1) wordsearch.txt\n\nSelect puzzle number: 1\n\nAD,IE\n\nA B C \nD E F \nG H I \n\n(1) Show solution\n(2) Enter a search word\n(3) Select another puzzle\n(4) Exit\n\nEnter selection: 2\nAD,IE\n\nA B C \nD E F \nG H I \n\nEnter a search word to find in puzzle or hit <enter> to return to the menu\n\nSearch word: AB\nAD,IE\n\nAB: (0,0),(1,0)\n\n<fg:Black><bg:Gray>A<fg:Gray><bg:Black> <fg:Black><bg:Gray>B<fg:Gray><bg:Black> C \nD E F \nG H I \n\nEnter a search word to find in puzzle or hit <enter> to return to the menu\n\nSearch word: \n\n(1) Show solution\n(2) Enter a search word\n(3) Select another puzzle\n(4) Exit\n\nEnter selection: 4\n";
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            ((ConsoleWrapperMock)consoleWrapper).ReadKeyChars = new List<char>() {'1', '2', '4'};
            ((ConsoleWrapperMock)consoleWrapper).ReadLineResults = new List<string>(){"AB", ""};
            WordSearchProgram wordSearchProgram = new WordSearchProgram(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            wordSearchProgram.ProgramLoop(TestUtilities.TEST_PUZZLES_DIRECTORY);
            string output = _consoleOuput.ToString();

            //assert
            Assert.Equal(expected, output);
        }

        [Theory]
        [InlineData("non_existant_directory")]
        [InlineData("nope/noway")]
        public void ProgramLoop_WhenPuzzleDirectoryDoesNotExist_ThrowsArgumentException(string directory)
        {
            //arrange
            string fullPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}/{directory}";
            string expectedMessage = $"directory does not exist: {fullPath}";
            WordSearchProgram wordSearchProgram = new WordSearchProgram(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgram.ProgramLoop(directory));
            Assert.Equal(expectedMessage, exception.Message);
        }
        
        [Fact]
        public void ProgramLoop_WhenPuzzleDirectoryIsEmpty_ThrowsArgumentException()
        {
            //arrange
            string directory = TestUtilities.EMPTY_DIRECTORY;
            string fullPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}/{directory}";
            _testUtilities.CreateEmptyDirectory(fullPath);
            string expectedMessage = $"puzzle directory contains no files: {fullPath}";
            WordSearchProgram wordSearchProgram = new WordSearchProgram(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgram.ProgramLoop(directory));
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

            string directory = TestUtilities.EMPTY_DIRECTORY;
            string fullPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}/{directory}";
            di = new DirectoryInfo(fullPath);
            if (di.Exists)
            {
                di.Delete(true);
            }
        }


    }
}