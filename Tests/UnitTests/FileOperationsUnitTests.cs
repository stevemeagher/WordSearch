using System;
using System.IO;
using Xunit;
using WordSearch.FileLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class FileOperationsUnitTests : IDisposable
    {
        private string _testFilePath;
        private FileOperations _fileOperations;
        private TestUtilities _testUtilities;
        private const string TEST_DIRECTORY = "Test_FileOperations";

        public FileOperationsUnitTests()
        {
            _testUtilities = new TestUtilities();
            _fileOperations = new FileOperations();
            _testFilePath = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + $"{Path.DirectorySeparatorChar}fileOperationsTests.txt";
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

        [Fact]
        public void GetDirectoryContents_WhenFilesExistInDirectory_ReturnsSortdedArrayOfFilePaths()
        {
            //arrange
            string workingDir = $"{_fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY)}{Path.DirectorySeparatorChar}{TEST_DIRECTORY}";
            _testUtilities.CreateEmptyDirectory(workingDir);

            File.Create($"{workingDir}{Path.DirectorySeparatorChar}file1.txt");
            File.Create($"{workingDir}{Path.DirectorySeparatorChar}file2.txt");
            File.Create($"{workingDir}{Path.DirectorySeparatorChar}file3.txt");
            File.Create($"{workingDir}{Path.DirectorySeparatorChar}file4.txt");
            File.Create($"{workingDir}{Path.DirectorySeparatorChar}file5.txt");

            //act
            string[] filePaths = _fileOperations.GetDirectoryContents(workingDir);

            //assert
            Assert.True(filePaths[0] == $"{workingDir}{Path.DirectorySeparatorChar}file1.txt");
            Assert.True(filePaths[1] == $"{workingDir}{Path.DirectorySeparatorChar}file2.txt");
            Assert.True(filePaths[2] == $"{workingDir}{Path.DirectorySeparatorChar}file3.txt");
            Assert.True(filePaths[3] == $"{workingDir}{Path.DirectorySeparatorChar}file4.txt");
            Assert.True(filePaths[4] == $"{workingDir}{Path.DirectorySeparatorChar}file5.txt");
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

        public void Dispose()
        {
            //clean-up
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }

            string workingDir = _fileOperations.ApplicationBasePath(TestUtilities.APPLICATION_DIRECTORY) + Path.DirectorySeparatorChar + TEST_DIRECTORY;

            if (Directory.Exists(workingDir))
            {
                Directory.Delete(workingDir, true);
            }
        }
    }
}