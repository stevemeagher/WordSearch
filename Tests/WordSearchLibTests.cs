using System;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class WordSearchLibTests
    {
        [Fact]
        public void GridToString_NxNStringGrid_ReturnsLeftToRightString()
        {
            //arrange
            string[,] grid = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            string expected = "ABCDEFGHI";

            //act
            var gridToString = new GridToString();
            string actual = gridToString.ConvertToLeftToRight(grid);

            //assert
            Assert.True(expected == actual);
        }

        /// <summary>Converts formatted string to 2d string array
        /// </summary>
        /// <para>StringToGrid_WellFormedStringInput_Creates2dStringArray converts a specifically formatted source string
        /// into a two-dimensional string array.  This method is used to produce string arrays for testing and enables
        /// the formatted strings to be passed as InLineData attribute parameters.
        /// </para>
        private string[,] StringToGrid(string source)
        {
            if (string.IsNullOrEmpty(source)) throw new ArgumentException("source parameter contains no characters.");
            if (!source.Contains("|")) throw new ArgumentException("source parameter not in correct format: no row separator characters.");

            string[] rows = source.Split('|');
            int rowCount = rows.Length;
            int columnCount = rows[0].Length;

            string[,] grid = new string[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    grid[i,j] = rows[i].Substring(j,1);
                }
            }
            return grid;
        }

        [Fact]
        public void StringToGrid_WellFormedStringInput_Creates2dStringArray()
        {
            //arrange
            string source = "ABC|DEF|GHI";
            string[,] expeceted = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            //act
            string[,] actual = StringToGrid(source);

            //assert
            Assert.Equal(expeceted, actual);

        }

        [Fact]
        public void StringToGrid_NoRowDividers_GeneratesArgumentException()
        {
            //arrange
            string source = "ABCDEFGHI";
            string expectedMessage = "source parameter not in correct format: no row separator characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);

        }

        [Fact]
        public void StringToGrid_EmptyInputParameter_GeneratesArgumentException()
        {
            //arrange
            string source = "";
            string expectedMessage = "source parameter contains no characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);

        }
    }
}
