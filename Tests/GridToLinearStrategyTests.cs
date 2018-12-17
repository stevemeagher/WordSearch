using System;
using Xunit;
using WordSearch.WordSearchLib;
using System.Collections.Generic;
using System.Drawing;

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
        public void GridToLinearLeftRightStrategy_NxNGrid_ReturnsLeftToRightString(string gridSource, string expected)
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

        [Fact]
        public void GridToLinearLeftRightStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearLeftRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearRightLeftStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearRightLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearTopBottomStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearTopBottomStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearBottomTopStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearBottomTopStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }
    }
}
