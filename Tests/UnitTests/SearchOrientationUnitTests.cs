using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class SearchOrientationUnitTests
    {
        private readonly TestUtilities _testUtilities;

        public SearchOrientationUnitTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoordinatesReturned()
        {
            //arrange
            var serachOrientation = new SearchOrientation(new GridToLinearHorizontalStrategyMock(null));

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("A");

            //assert
            Assert.Equal(new PointList() {new Point(0,0)}, actual);
        }

        
        [Fact]
        public void IsSearchTargetFound_LinearViewContainsSearchText_ReturnsTrue()
        {
            //arrange
            var serachOrientation = new SearchOrientation(new GridToLinearHorizontalStrategyMock(null));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.True(actual);
        }

        
        [Fact]
        public void SearchOrientation_CreatedWithNullGridToLinearStrategy_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "gridToLinearStrategy parameter is null.";
            GridToLinearStrategy strategy = null;

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new SearchOrientation(strategy));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void IsSearchTargetFound_PassNullSearchTarget_ReturnsFalse()
        {
            //arrange
            ILinearView linearView = null;
            var serachOrientation = new SearchOrientation(linearView);

            //act
            bool actual = serachOrientation.IsSearchTargetFound(null);

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_PassEmptySearchTarget_ReturnsFalse()
        {
            //arrange
            ILinearView linearView = null;
            var serachOrientation = new SearchOrientation(linearView);

            //act
            bool actual = serachOrientation.IsSearchTargetFound("");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassNullSearchTarget_ReturnsNull()
        {
            //arrange
            ILinearView linearView = null;
            var serachOrientation = new SearchOrientation(linearView);

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget(null);

            //assert
            Assert.True(actual == null);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_PassEmptySearchTarget_ReturnsEmptyString()
        {
            //arrange
            ILinearView linearView = null;
            var serachOrientation = new SearchOrientation(linearView);

            //act
            PointList actual = serachOrientation.GetCoordinatesOfSearchTarget("");

            //assert
            Assert.True(actual == null);
        }


        [Fact]
        public void IsSearchTargetFound_LinearViewIsNull_ReturnsFalse()
        {
            //arrange
            var serachOrientation = new SearchOrientation(new GridToLinearHorizontalStrategyNullLinearViewtMock(null));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }

        [Fact]
        public void IsSearchTargetFound_ValueOfLinearViewIsNull_ReturnsFalse()
        {
            //arrange
            var serachOrientation = new SearchOrientation(new GridToLinearHorizontalStrategyNullLinearViewtMock(null));

            //act
            bool actual = serachOrientation.IsSearchTargetFound("BC");

            //assert
            Assert.False(actual);
        }
        
        private class GridToLinearHorizontalStrategyMock : GridToLinearStrategy
        {

            public GridToLinearHorizontalStrategyMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
            {
                return new LinearView("ABC", new Dictionary<int, Point>() {{0, new Point(0,0)}});
            }
        }

        private class GridToLinearHorizontalStrategyNullLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearHorizontalStrategyNullLinearViewtMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
            {
                return null;
            }
        }

        private class GridToLinearHorizontalStrategyValueIsNullInLinearViewtMock : GridToLinearStrategy
        {

            public GridToLinearHorizontalStrategyValueIsNullInLinearViewtMock(IGridManager gridManager) : base(gridManager)
            {
            }

            public override ILinearView GridToLinear()
            {
                return null;
            }
        }


    }
}