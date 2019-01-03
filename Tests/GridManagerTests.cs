using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests
{
    public class GridManagerTests
    {
        private TestUtilities _testUtilities;

        public GridManagerTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Theory]
        [InlineData("ABC|DEF|GHI")]
        [InlineData("ABCD|EFGH|IJKL|MNOP")]
        public void GridManager_WhenGridWithEqualColumnsAndRowsPassedToConstructor_NoExceptionThrown(string gridSource)
        {
            //act
            try
            {
                IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            }
            catch (Exception ex)
            {
                Assert.True(false, $"Expected no exception: {ex.Message}");
            }
        }

        [Theory]
        [InlineData("ABCD|EFGH|IJKL")]
        [InlineData("AB|CD|EF|GH")]
        public void GridManager_WhenGridWithUnequalColumnsAndRowsPassedToConstructor_ThrowsArgumentException(string gridSource)
        {
            //arrange
            string expectedMessage = "grid has a mismatch between the number of rows and columns.";

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => new GridManager(_testUtilities.StringToGrid(gridSource)));
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void GridManager_WhenGridHasZeroRowsAndColumnsPassedToConstructor_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid has zero rows and/or columns.";

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => new GridManager(new string[0,0]));
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void GridManager_WhenGridIsNUll_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid is null.";

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => new GridManager(null));
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void GridManager_WhenGridWithMoreThanOneCharacterInCoordinatePassedToConstructor_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid has more than one character in at least one coordinate.";
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            grid[1,1] = "ABC";

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => new GridManager(grid));
             Assert.Equal(expectedMessage, exception.Message);
        }
   }
}