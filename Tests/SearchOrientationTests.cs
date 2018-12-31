using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class SearchOrientationTests
    {
        private readonly TestUtilities _testUtilities;

        public SearchOrientationTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void IsSearchTargetFound_LinearViewContainsSearchText_ReturnsTrue()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.True(actual);
        }
        
        [Fact]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoordinatesReturned()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("A");

            //assert
            Assert.Equal(new PointList() {new Point(0,0)}, actual);
        }

        private class GridToLinearStrategyLeftToRightMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRightMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
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
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            bool actual = serachOrientation.IsSearchTargetFound(null);

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_PassEmptySearchTarget_ReturnsFalse()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassNullSearchTarget_ReturnsNull()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget(null);

            //assert
            Assert.True(actual == null);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassEmptySearchTarget_ReturnsEmptyString()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRightMock(gridManager));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("");

            //assert
            Assert.True(actual == null);
        }

        private class GridToLinearStrategyLeftToRighNullLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRighNullLinearViewtMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
            {
                return null;
            }
        }

        private class GridToLinearStrategyLeftToRighValueIsNullInLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearStrategyLeftToRighValueIsNullInLinearViewtMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
            {
                return null;
            }
        }

        [Fact]
        public void IsSearchTargetFound_LinearViewIsNull_ReturnsFalse()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRighNullLinearViewtMock(gridManager));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_ValueOfLinearViewIsNull_ReturnsFalse()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            var serachOrientation = new SearchOrientation(new GridToLinearStrategyLeftToRighNullLinearViewtMock(gridManager));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }
        

    }
}