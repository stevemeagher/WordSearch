using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
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
        public void ValidateGrid_WhenGridWithEqualColumnsAndRowsValidated_ReturnTrue(string gridSource)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            try
            {
                gridManager.ValidateGrid();

            }
            catch (Exception ex)
            {
                Assert.True(false, $"Expected no exception: {ex.Message}");
            }
        }

        [Theory]
        [InlineData("ABCD|EFGH|IJKL")]
        [InlineData("AB|CD|EF|GH")]
        public void ValidateGrid_WhenGridWithUnequalColumnsAndRowsValidated_ThrowsArgumentException(string gridSource)
        {
            //arrange
            string expectedMessage = "grid has a mismatch between the number of rows and columns.";
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridManager.ValidateGrid());
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ValidateGrid_WhenGridHasZeroRowsAndColumns_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid has zero rows and/or columns.";
            IGridManager gridManager = new GridManager(new string[0,0]);

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridManager.ValidateGrid());
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ValidateGrid_WhenGridIsNUll_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid is null.";
            IGridManager gridManager = new GridManager(null);

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridManager.ValidateGrid());
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ValidateGrid_WhenGridWithMoreThanOneCharacterInCoordinateValidated_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid has more than one character in at least one coordinate.";
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            grid[1,1] = "ABC";
            IGridManager gridManager = new GridManager(grid);

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridManager.ValidateGrid());
             Assert.Equal(expectedMessage, exception.Message);
        }
   }
}