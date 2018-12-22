using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.ConsoleApp;

namespace Tests
{
    public class WordSearchProgramTests : IDisposable
    {
        private StringWriter _consoleOuput;
        public TextWriter _originalConsoleOutput;

        public WordSearchProgramTests()
        {
            _originalConsoleOutput = Console.Out;
            _consoleOuput = new StringWriter();
            Console.SetOut(_consoleOuput);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "A B C \nD E F \nG H I \n")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "A B C D \nE F G H \nI J K L \nM N O P \n")]
        public void WriteGridToConsole_WhenGridArrayPassedIn_WritesArrayContentsToConsole(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);
            WordSearchProgram wordSearchProgram = new WordSearchProgram();

            //act
            wordSearchProgram.WriteGridToConsole(grid);

            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());

        }

        public void Dispose()
        {
            Console.SetOut(_originalConsoleOutput);
        }
    }
}