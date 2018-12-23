using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class SearchOrientationTests
    {
        private TestUtilities _testUtilities;

        public SearchOrientationTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void IsSearchTargetFound_LinearViewContainsSearchText_ReturnsTrue()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
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
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("A");

            //assert
            Assert.Equal(new PointList() {new Point(0,0)}, actual);
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
            string expectedMessage = "gridToLinearStrategy parameter is null.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new SearchOrientation(null));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void IsSearchTargetFound_PassNullSearchTarget_ReturnsFalse()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
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
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassNullSearchTarget_ReturnsNull()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget(null);

            //assert
            Assert.True(actual == null);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassEmptySearchTarget_ReturnsEmptyString()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(grid));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("");

            //assert
            Assert.True(actual == null);
        }

        private class GridToLinearStrategyLeftToRighNullLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRighNullLinearViewtMock(string[,] grid): base(grid)
            {
            }

            public override LinearView GridToLinear()
            {
                return null;
            }
        }

        private class GridToLinearStrategyLeftToRighValueIsNullInLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRighValueIsNullInLinearViewtMock(string[,] grid): base(grid)
            {
            }

            public override LinearView GridToLinear()
            {
                return null;
            }
        }

        [Fact]
        public void IsSearchTargetFound_LinearViewIsNull_ReturnsFalse()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRighNullLinearViewtMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_ValueOfLinearViewIsNull_ReturnsFalse()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRighNullLinearViewtMock(grid));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }
        

    }
}