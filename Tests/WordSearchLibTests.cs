using System;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class WordSearchLibTests
    {

        #region StringToGrid Test Helper
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

            foreach (var row in rows)
            {
                if (row.Length != columnCount) throw new ArgumentException("source parameter not in correct format: rows must contain the same number of characters.");
            }

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
        
        [Fact]
        public void StringToGrid_InputParameterWouldProduceJaggedArray_GeneratesArgumentException()
        {
            //arrange
            string source = "ABC|DE|FGHI";
            string expectedMessage = "source parameter not in correct format: rows must contain the same number of characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);
        }
        #endregion 

        [Fact]
        public void gridToLinear_NxNStringGrid_ReturnsLeftToRightString()
        {
            //arrange
            string[,] grid = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            string expected = "ABCDEFGHI";

            //act
            var gridToLinear = new GridToLinear();
            LinearView linearView = gridToLinear.ConvertToLeftToRight(grid);

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ABCDEFGHI")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "ABCDEFGHIJKLMNOP")]
        [InlineData("12345|67890|12345|67890|12345", "1234567890123456789012345")]
        public void gridToLinear_MultipleNxNStringGrid_ReturnsLeftToRightString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var gridToLinear = new GridToLinear();
            LinearView linearView = gridToLinear.ConvertToLeftToRight(grid);

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IHGFEDCBA")]
        [InlineData("12|34|56", "654321")]
        [InlineData("ABCD|EFGH|IJKL", "LKJIHGFEDCBA")]
        public void gridToLinear_NxNStringGrid_ReturnsRightToLeftString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var gridToLinear = new GridToLinear();
            LinearView linearView = gridToLinear.ConvertToRightToLeft(grid);

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ADGBEHCFI")]
        [InlineData("12|34|56", "135246")]
        public void gridToLinear_NxNStringGrid_ReturnsTopToBottomString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var gridToLinear = new GridToLinear();
            LinearView linearView = gridToLinear.ConvertToTopToBottom(grid);

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IFCHEBGDA")]
        [InlineData("12|34|56", "642531")]
        public void gridToLinear_NxNStringGrid_ReturnsBottomToTopString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var gridToLinear = new GridToLinear();
            LinearView linearView = gridToLinear.ConvertToBottomToTop(grid);

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "DEF", "(0,1),(1,1),(2,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "JKL", "(1,2),(2,2),(3,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoorinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder();
            string actual = wordFinder.GetCoordinatesOfSearchTarget(grid, searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "FED", "(2,1),(1,1),(0,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "LKJ", "(3,2),(2,2),(1,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInRightLeftOrientation_CoorinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder();
            string actual = wordFinder.GetCoordinatesOfSearchTarget(grid, searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }
    }
}
