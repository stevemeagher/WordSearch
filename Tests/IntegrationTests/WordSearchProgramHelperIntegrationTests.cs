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
    public class WordSearchProgramHelperHelperIntegrationTests : IDisposable
    {
        private TestUtilities _testUtilities;
        private readonly StringWriter _consoleOuput;
        private readonly TextWriter _originalConsoleOutput;
        private readonly FileOperations _fileOperations;
        private readonly WordFinder _wordFinder;
        private readonly ISearchOrientationManager _searchOrientationManager;
        private readonly IConsoleWrapper _consoleWrapper;
        private const string TEST_DIRECTORY = "Test_WordSearchProgramHelper";

        public WordSearchProgramHelperHelperIntegrationTests()
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
        [InlineData("c:/dir/file1.txt|c:/dir/file2.txt|c:/dir/file3.txt", "(1) file1.txt{Environment.NewLine}(2) file2.txt{Environment.NewLine}(3) file3.txt{Environment.NewLine}")]
        [InlineData("c:/dir1/dir2/file1.txt|c:/dir1/dir2/file2.txt|c:/dir1/dir2/file3.txt", "(1) file1.txt{Environment.NewLine}(2) file2.txt{Environment.NewLine}(3) file3.txt{Environment.NewLine}")]
        public void WriteNumberedFileNamesToConsole_WhenWellFormedFilePathsArePassedIn_WritesNumberedFileNamesToConsole(string filePathDelimeteredArray, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
            string[] filePaths = filePathDelimeteredArray.Split('|');
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

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
            string workingDir = $"{_fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY)}{Path.DirectorySeparatorChar}{TEST_DIRECTORY}";
            CreatePuzzleFile(workingDir, searchWords, fileRowsDelimeteredArray, puzzleFileName);

            //need to remove the commas to provide meaningful input to StringToGrid
            string[,] expectedGrid = _testUtilities.StringToGrid(fileRowsDelimeteredArray.Replace(",",""));

            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var (searchString, grid) = wordSearchProgramHelper.ConvertPuzzleFileToSearchWordsAndGrid($"{workingDir}{Path.DirectorySeparatorChar}{puzzleFileName}");

            //assert
            Assert.Equal(searchWords, searchString);
            Assert.Equal(expectedGrid, grid);
        }

        [Theory]
        [InlineData("AB,HEB", "A,B,C|D,E,F|G,H,I", "puzzle.txt","AB: (0,0),(1,0){Environment.NewLine}HEB: (1,2),(1,1),(1,0){Environment.NewLine}")]
        [InlineData("16AF", "1,2,3,4|5,6,7,8|9,0,A,B|C,D,E,F", "puzzle.txt","16AF: (0,0),(1,1),(2,2),(3,3){Environment.NewLine}")]
        public void WriteSolvedPuzzleCoordinatesToConsole_WhenSearchWordFound_CoordinatesWrittenToConsole(string searchWords, string fileRowsDelimeteredArray, string puzzleFileName, string expected)
        {
            //arrange
            expected = expected.Replace("{Environment.NewLine}", Environment.NewLine);
            string workingDir = $"{_fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY)}{Path.DirectorySeparatorChar}{TEST_DIRECTORY}";
            CreatePuzzleFile(workingDir, searchWords, fileRowsDelimeteredArray, puzzleFileName);

            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            //act
            var (searchString, grid) = wordSearchProgramHelper.ConvertPuzzleFileToSearchWordsAndGrid($"{workingDir}{Path.DirectorySeparatorChar}{puzzleFileName}");
            IGridManager gridManager = new GridManager(grid);
            wordSearchProgramHelper.WriteSolvedPuzzleCoordinatesToConsole(searchString, gridManager);
            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());
        }
        
        [Fact]
        public void GetPuzzleFilePathsFromPuzzleDirectory_WhenPuzzleDirectoryExistsAndContainsFiles_ListOfFilePathsReturned()
        {
            //arrange
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act
            var filePaths = wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory(TestUtilities.TEST_PUZZLES_DIRECTORY);

            //assert
            Assert.True(filePaths.Length == 1);
        }

        [Fact]
        public void GetPuzzleFilePathsFromPuzzleDirectory_WhenPuzzleDirectoryDoesNotExist_ListOfFilePathsReturned()
        {
            //arrange
            string directory = "NODIRECTORY";
            string fullPath = $"{_fileOperations.ApplicationBasePath("WordSearch")}{Path.DirectorySeparatorChar}{directory}";
            string expectedMessage = $"directory does not exist: {fullPath}";            
            WordSearchProgramHelper wordSearchProgramHelper = new WordSearchProgramHelper(_consoleWrapper, _fileOperations, _wordFinder, _searchOrientationManager);

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => wordSearchProgramHelper.GetPuzzleFilePathsFromPuzzleDirectory(directory));
            Assert.Equal(expectedMessage, exception.Message);
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

            File.WriteAllLines($"{workingDir}{Path.DirectorySeparatorChar}{puzzleFileName}", puzzleForFile);
        }

        public void Dispose()
        {
            Console.SetOut(_originalConsoleOutput);

            string workingDir = $"{_fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY)}{Path.DirectorySeparatorChar}{TEST_DIRECTORY}";
            DirectoryInfo di = new DirectoryInfo(workingDir);
            if (di.Exists)
            {
                di.Delete(true);
            }
        }


    }
}