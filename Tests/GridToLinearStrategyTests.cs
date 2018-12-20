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

            string expected = "ABC|DEF|GHI";

            //act
            var gridToLinearStrategy = new GridToLinearLeftRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ABC|DEF|GHI")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "ABCD|EFGH|IJKL|MNOP")]
        [InlineData("12345|67890|12345|67890|12345", "12345|67890|12345|67890|12345")]
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
        [InlineData("ABC|DEF|GHI", "IHG|FED|CBA")]
        [InlineData("12|34|56", "65|43|21")]
        [InlineData("ABCD|EFGH|IJKL", "LKJI|HGFE|DCBA")]
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
        [InlineData("ABC|DEF|GHI", "ADG|BEH|CFI")]
        [InlineData("12|34|56", "135|246")]
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
        [InlineData("ABC|DEF|GHI", "IFC|HEB|GDA")]
        [InlineData("12|34|56", "642|531")]
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

        [Theory]
        [InlineData("ABC|DEF|GHI", "A|BD|CEG|FH|I")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "A|BE|CFI|DGJM|HKN|LO|P")]
        public void GridToLinearTopRightBottomLeftStrategy_NxNGrid_ReturnsTopRightToBottomLeftString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearTopRightBottomLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "I|HF|GEC|DB|A")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "P|OL|NKH|MJGD|IFC|EB|A")]
        public void GridToLinearBottomLeftTopRightStrategy_NxNGrid_ReturnsBottomLeftToTopRightString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearBottomLeftTopRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "C|BF|AEI|DH|G")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "D|CH|BGL|AFKP|EJO|IN|M")]
        public void GridToLinearTopLeftBottomRightStrategy_NxNGrid_ReturnsTopLeftToBottomRightString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearTopLeftBottomRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "G|HD|IEA|FB|C")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "M|NI|OJE|PKFA|LGB|HC|D")]
        public void GridToLinearBottomRightTopLeftStrategy_NxNGrid_ReturnsBottomRightToTopLeftString(string gridSource, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var gridToLinearStrategy = new GridToLinearBottomRightTopLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Fact]
        public void GridToLinearTopRightBottomLeftStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearTopRightBottomLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearBottomLeftTopRightStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearBottomLeftTopRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearTopLeftBottomRightStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(1,0)}, {1, new Point(0,0)}, {2, new Point(1,1)}, {3, new Point(0,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearTopLeftBottomRightStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearBottomRightTopLeftStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };

            var expected = new Dictionary<int, Point>() {{0, new Point(0,1)}, {1, new Point(1,1)}, {2, new Point(0,0)}, {3, new Point(1,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearBottomRightTopLeftStrategy(grid);
            LinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

    }
}
