using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.ConsoleApp;
using System.Drawing;

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
            IConsoleWrapper consoleWrapper = new ConsoleWrapper();

            //act
            wordSearchProgram.WriteGridToConsole(consoleWrapper, grid, ConsoleColor.Gray, ConsoleColor.Black);

            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());

        }

        ///<summary>This test uses a class, ConsoleWrapperMock, which inherits from ConsoleWrapper and overrides the color setting properties so that
        ///we can "see" the color changes in the output and confirm that the output to the console should appear correctly.
        ///fg: indicates setting the foreground color and bg: indicates setting the background color
        ///</summary>
        [Theory]
        [InlineData("ABC|DEF|GHI", 1, 1, "<fg:Gray><bg:Black>A B C \nD <fg:Black><bg:Gray>E<fg:Gray><bg:Black> F \nG H I \n")]
        [InlineData("ABC|DEF|GHI", 2, 0, "<fg:Gray><bg:Black>A B <fg:Black><bg:Gray>C<fg:Gray><bg:Black> \nD E F \nG H I \n")]
        public void WriteGridToConsole_WhenGridArrayAndHighlightCoordinatesPassedIn_WritesArrayContentsToConsoleWithColorChanges(string gridSource, int xcoord, int ycoord, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);
            WordSearchProgram wordSearchProgram = new WordSearchProgram();
            IConsoleWrapper consoleWrapper = new ConsoleWrapperMock();

            //act
            wordSearchProgram.WriteGridToConsole(consoleWrapper, grid, ConsoleColor.Gray, ConsoleColor.Black, new List<Point>(){ new Point(xcoord, ycoord)});

            var output = _consoleOuput.ToString();

            //assert
            Assert.True(expected == _consoleOuput.ToString());
        }

        private class ConsoleWrapperMock : ConsoleWrapper
        {
            public override ConsoleColor BackgroundColor
            { 
                get 
                {
                    return Console.BackgroundColor;
                }
                set
                {
                    Write($"<bg:{value.ToString()}>");
                    Console.BackgroundColor = value;
                }
            }

            public override ConsoleColor ForegroundColor
            { 
                get 
                {
                    return Console.ForegroundColor;
                }
                set
                {
                    Write($"<fg:{value.ToString()}>");
                    Console.ForegroundColor = value;
                }
            }
        }

        public void Dispose()
        {
            Console.SetOut(_originalConsoleOutput);
        }


    }
}