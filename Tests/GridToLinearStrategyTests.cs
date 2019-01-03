using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests
{
    public class GridToLinearStrategyTests
    {
        private readonly TestUtilities _testUtilities;

        public GridToLinearStrategyTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void GridToLinearLeftRightStrategy_NxNStringGrid_ReturnsLeftToRightString()
        {
            //arrange
            string[,] grid = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };
            IGridManager gridManager = new GridManager(grid);
            

            string expected = "ABC|DEF|GHI";

            //act
            var gridToLinearStrategy = new GridToLinearHorizontalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

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
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearHorizontalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IHG|FED|CBA")]
        [InlineData("12|34", "43|21")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "PONM|LKJI|HGFE|DCBA")]
        public void GridToLinearHorizontalStrategy_NxNStringGrid_ReturnsRightToLeftString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearHorizontalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.ReversedValue);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ADG|BEH|CFI")]
        [InlineData("12|34", "13|24")]
        public void GridToLinearVerticalStrategy_NxNStringGrid_ReturnsTopToBottomString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearVerticalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IFC|HEB|GDA")]
        [InlineData("12|34", "42|31")]
        public void GridToLinearVerticalStrategy_NxNStringGrid_ReversedValueReturnsBottomToTopString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearVerticalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.ReversedValue);
        }

        [Fact]
        public void GridToLinearHorizontalStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearHorizontalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearHorizontalStrategy_NxNStringGrid_ReturnsReversedIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearHorizontalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.ReversedIndexToGridPosition);
        }

        [Fact]
        public void GridToLinearVerticalStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearVerticalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearVerticalStrategy_NxNStringGrid_ReturnsReversedIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearVerticalStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.ReversedIndexToGridPosition);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "A|BD|CEG|FH|I")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "A|BE|CFI|DGJM|HKN|LO|P")]
        public void GridToLinearDiagonalNESWStrategy_NxNGrid_ReturnsTopRightToBottomLeftString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNESWStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "I|HF|GEC|DB|A")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "P|OL|NKH|MJGD|IFC|EB|A")]
        public void GridToLinearDiagonalNESWStrategy_NxNGrid_ReversedValueReturnsBottomLeftToTopRightString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNESWStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.ReversedValue);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "C|BF|AEI|DH|G")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "D|CH|BGL|AFKP|EJO|IN|M")]
        public void GridToLinearDiagonalNWSEStrategy_NxNGrid_ReturnsTopLeftToBottomRightString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNWSEStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.Value);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "G|HD|IEA|FB|C")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "M|NI|OJE|PKFA|LGB|HC|D")]
        public void GridToLinearDiagonalNWSEStrategy_NxNGrid_ReversedValueReturnsBottomRightToTopLeftString(string gridSource, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNWSEStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.True(expected == linearView.ReversedValue);
        }

        [Fact]
        public void GridToLinearDiagonalNESWStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(0,0)}, {1, new Point(1,0)}, {2, new Point(0,1)}, {3, new Point(1,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNESWStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearDiagonalNESWStrategy_NxNStringGrid_ReturnsReversedIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(1,1)}, {1, new Point(0,1)}, {2, new Point(1,0)}, {3, new Point(0,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNESWStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.ReversedIndexToGridPosition);
        }

        [Fact]
        public void GridToLinearDiagonalNWSEStrategy_NxNStringGrid_ReturnsIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(1,0)}, {1, new Point(0,0)}, {2, new Point(1,1)}, {3, new Point(0,1)}};

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNWSEStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.IndexToGridPosition);
        }

        [Fact]
        public void GridToLinearDiagonalNWSEStrategy_NxNStringGrid_ReturnsReversedIndexToGridDictionary()
        {
            //arrange
            string[,] grid = {
                {"A","B"},
                {"C","D"}
            };
            IGridManager gridManager = new GridManager(grid);

            var expected = new Dictionary<int, Point>() {{0, new Point(0,1)}, {1, new Point(1,1)}, {2, new Point(0,0)}, {3, new Point(1,0)}};

            //act
            var gridToLinearStrategy = new GridToLinearDiagonalNWSEStrategy(gridManager);
            ILinearView linearView = gridToLinearStrategy.GridToLinear();

            //assert
            Assert.Equal(expected, linearView.ReversedIndexToGridPosition);
        }

    }
}
