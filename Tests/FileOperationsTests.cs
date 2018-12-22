using System;
using Xunit;
using WordSearch.FileLib;

namespace Tests
{
    public class FileOperationsTests
    {
        [Fact]
        public void ReadLines_WhenFileExists_ReturnsStringArrayOfFileLines()
        {
            //arrange
            FileOperations fileOperations = new FileOperations();
            string firstLine = "BONES,KHAN,KIRK,SCOTTY,SPOCK,SULU,UHURA";
            string secondLine = "U,M,K,H,U,L,K,I,N,V,J,O,C,W,E";

            //act
            string[] fileLines = fileOperations.ReadLines(fileOperations.ApplicationBasePath("WordSearch") + "/puzzles/highlyillogical.txt");

            //assert
            Assert.True(fileLines[0] == firstLine);
            Assert.True(fileLines[1] == secondLine);
        }

        [Fact]
        public void ReadLines_WhenFileDoesNotExists_ReturnsEmptyArray()
        {
            //arrange
            FileOperations fileOperations = new FileOperations();

            //act
            string[] fileLines = fileOperations.ReadLines(fileOperations.ApplicationBasePath("WordSearch") + "/puzzles/nofile.txt");

            //assert
            Assert.True(fileLines.Length == 0);
        }

    }
}