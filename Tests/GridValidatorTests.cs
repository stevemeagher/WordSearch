using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class GridValidatorTests
    {
        private TestUtilities _testUtilities;

        public GridValidatorTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Theory]
        [InlineData("ABC|DEF|GHI")]
        [InlineData("ABCD|EFGH|IJKL|MNOP")]
        public void Validate_WhenGridWithEqualColumnsAndRowsValidated_ReturnTrue(string gridSource)
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid(gridSource);
            IGridValidator gridValidator = new GridValidator();

            //act
            try
            {
                gridValidator.Validate(grid);

            }
            catch (Exception ex)
            {
                Assert.True(false, $"Expected no exception: {ex.Message}");
            }
        }

        [Theory]
        [InlineData("ABCD|EFGH|IJKL")]
        [InlineData("AB|CD|EF|GH")]
        public void Validate_WhenGridWithUnequalColumnsAndRowsValidated_ThrowsArgumentException(string gridSource)
        {
            //arrange
            string expectedMessage = "grid has a mismatch between the number of rows and columns.";
            string[,] grid = _testUtilities.StringToGrid(gridSource);
            IGridValidator gridValidator = new GridValidator();

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridValidator.Validate(grid));
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Validate_WhenGridHasZeroRowsAndColumns_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid has zero rows and/or columns.";
            string[,] grid = new string[0,0];
            IGridValidator gridValidator = new GridValidator();

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridValidator.Validate(grid));
             Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Validate_WhenGridIsNUll_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "grid is null.";
            IGridValidator gridValidator = new GridValidator();

            //act & assert
             var exception = Assert.Throws<ArgumentException>(() => gridValidator.Validate(null));
             Assert.Equal(expectedMessage, exception.Message);
        }
   }
}