using System;
using System.IO;
using Xunit;
using WordSearch.FileLib;

namespace Tests
{
    public class FileOperationsTests : IDisposable
    {
        private const string  APPLICATION_DIRECTORY = "WordSearch";
        private string _testFilePath;
        private FileOperations _fileOperations;

        public FileOperationsTests()
        {
            _fileOperations = new FileOperations();
            _testFilePath = _fileOperations.ApplicationBasePath(APPLICATION_DIRECTORY) + "testFile.txt";
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            string[] fileLinesOut = {"First", "Second", "Third", "Fourth", "Fifth"};

            File.WriteAllLines(_testFilePath, fileLinesOut);
        }

        [Fact]
        public void ReadLines_WhenFileExists_ReturnsArrayOfLines()
        {
            //act
            string[] fileLines = _fileOperations.ReadLines(_testFilePath);

            //assert
            Assert.True(fileLines[0] == "First");
            Assert.True(fileLines[1] == "Second");
            Assert.True(fileLines[2] == "Third");
            Assert.True(fileLines[3] == "Fourth");
            Assert.True(fileLines[4] == "Fifth");
        }

        [Fact]
        public void ReadLines_WhenFileExists_ReturnsAllLinesInFile()
        {
            //act
            string[] fileLines = _fileOperations.ReadLines(_testFilePath);

            //assert
            Assert.True(fileLines.Length == 5);
        }

        [Fact]
        public void ReadLines_WhenFileDoesNotExists_ReturnsEmptyArray()
        {
            //arrange
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }

            //act
            string[] fileLines = _fileOperations.ReadLines(_testFilePath);

            //assert
            Assert.True(fileLines.Length == 0);
        }

        public void Dispose()
        {
            //clean-up
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}