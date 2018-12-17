using System;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class GridToLinearStrategyTests
    {

        

        [Fact]
        public void GridToLinearLeftRightStrategy_NxNStringGrid_ReturnsLeftToRightString()
        {
            //arrange
            string[,] grid = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            string expected = "ABCDEFGHI";

            //act
            var gridToLinearStrategy = new GridToLinearLeftRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ABCDEFGHI")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "ABCDEFGHIJKLMNOP")]
        [InlineData("12345|67890|12345|67890|12345", "1234567890123456789012345")]
        public void GridToLinearLeftRightStrategy_MultipleNxNStringGrid_ReturnsLeftToRightString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearLeftRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IHGFEDCBA")]
        [InlineData("12|34|56", "654321")]
        [InlineData("ABCD|EFGH|IJKL", "LKJIHGFEDCBA")]
        public void GridToLinearRightLeftStrategy_NxNStringGrid_ReturnsRightToLeftString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearRightLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ADGBEHCFI")]
        [InlineData("12|34|56", "135246")]
        public void GridToLinearTopBottomStrategy_NxNStringGrid_ReturnsTopToBottomString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearTopBottomStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IFCHEBGDA")]
        [InlineData("12|34|56", "642531")]
        public void GridToLinearBottomTopStrategy_NxNStringGrid_ReturnsBottomToTopString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearBottomTopStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }
    }
}
