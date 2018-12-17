using System;
using Xunit;
using WordSearch.WordSearchLib;
using System.Collections.Generic;
using System.Drawing;

namespace Tests
{
    public class SearchOrientationTests
    {
        [Fact]
        public void IsSearchTargetFound_LinearViewContainsSearchText_ReturnsTrue()
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.True(actual);
        }
        
        [Fact]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoordinatesReturned()
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            string actual = serachOrientation.GetCoordinatesOfSearchTarget("A");

            //assert
            Assert.Equal("(0,0)", actual);
        }

        private class GridToLinearStrategyLeftToRightMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRightMock(string[,] grid): base(grid)
            {
            }

            public override LinearView GridToLinear()
            {
                return new LinearView("ABC", new Dictionary<int, Point>() {{0, new Point(0,0)}});
            }
        }

        [Fact]
        public void SearchOrientation_CreatedWithNullGridToLinearStrategy_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "gridToLinearStragtey parameter is null.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new SearchOrientation(null));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void IsSearchTargetFound_PassNullSearchTarget_ReturnsFalse()
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound(null);

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_PassEmptySearchTarget_ReturnsFalse()
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("");

            //assert
            Assert.False(actual);
        }
    }
}