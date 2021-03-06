using System;
using System.IO;
using Xunit;
using WordSearch.FileLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class FileOperationsUnitTests
    {
        private string _testFilePath;
        private string _testDirectoryPath;
        private FileOperations _fileOperations;
        private TestUtilities _testUtilities;

        public FileOperationsUnitTests()
        {
            _testUtilities = new TestUtilities();
            _fileOperations = new FileOperations();
            _testDirectoryPath = $"{_fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY)}{Path.DirectorySeparatorChar}{TestUtilities.TEST_PUZZLES_DIRECTORY}";
            _testFilePath = $"{_testDirectoryPath}{Path.DirectorySeparatorChar}wordsearch.txt";
        }

        [Fact]
        public void ReadLines_WhenFileExists_ReturnsArrayOfLines()
        {
            //act
            string[] fileLines = _fileOperations.ReadLines(_testFilePath);

            //assert
            Assert.True(fileLines[0] == "AD,IE");
            Assert.True(fileLines[1] == "A,B,C");
            Assert.True(fileLines[2] == "D,E,F");
            Assert.True(fileLines[3] == "G,H,I");
        }

        [Fact]
        public void ReadLines_WhenFileExists_ReturnsAllLinesInFile()
        {
            //act
            string[] fileLines = _fileOperations.ReadLines(_testFilePath);

            //assert
            Assert.True(fileLines.Length == 4);
        }

        [Fact]
        public void ReadLines_WhenFileDoesNotExists_ReturnsEmptyArray()
        {
            //act
            string[] fileLines = _fileOperations.ReadLines($"{_testDirectoryPath}{Path.DirectorySeparatorChar}does_not_exist.txt");

            //assert
            Assert.True(fileLines.Length == 0);
        }

        [Fact]
        public void GetDirectoryContents_WhenFilesExistInDirectory_ReturnsSortdedArrayOfFilePaths()
        {
            //act
            string[] filePaths = _fileOperations.GetSortedDirectoryContents(_testDirectoryPath);

            //assert
            Assert.Equal($"{_testDirectoryPath}{Path.DirectorySeparatorChar}wordsearch.txt", filePaths[0]);
            Assert.Equal($"{_testDirectoryPath}{Path.DirectorySeparatorChar}wordsearch2.txt", filePaths[1]);
        }

        [Fact]
        public void DirectoryExists_WhenDirectoryExists_ReturnsTrue()
        {
            //arrange
            string directory = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY);

            //act
            bool directoryExists = _fileOperations.DirectoryExists(directory);

            //assert
            Assert.True(directoryExists);
        }

        [Fact]
        public void DirectoryExists_WhenDirectoryDoesNotExist_ReturnsFalse()
        {
            //arrange
            string directory = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + "NOPE";

            //act
            bool directoryExists = _fileOperations.DirectoryExists(directory);

            //assert
            Assert.False(directoryExists);
        }

        [Fact]
        public void GetApplicationBasePath_WhenPathHasMultipleInstancesOfApplicationName_ReturnsFullBasePath()
        {
            //arrange
            string duplicateDirectoryNameOnly = "MyApplicationName";
            string duplicateDirectory = $"{Path.DirectorySeparatorChar}MyApplicationName";
            string baseDirectory = _fileOperations.ApplicationBasePath("WordSearch");
            string expected = baseDirectory + $"{duplicateDirectory}{duplicateDirectory}";
            if (!Directory.Exists(baseDirectory + $"{duplicateDirectory}"))
            {
                Directory.CreateDirectory(baseDirectory + $"{duplicateDirectory}");
                if (!Directory.Exists($"{baseDirectory}{duplicateDirectory}{duplicateDirectory}"))
                {
                    Directory.CreateDirectory($"{baseDirectory}{duplicateDirectory}{duplicateDirectory}");
                }   
            }

            //act
            string actual = _fileOperations.ApplicationBasePath(duplicateDirectoryNameOnly, baseDirectory + $"{duplicateDirectory}{duplicateDirectory}{Path.DirectorySeparatorChar}Directory1{Path.DirectorySeparatorChar}Directory2");

            //assert
            Assert.Equal(expected, actual);
        }

    }
}