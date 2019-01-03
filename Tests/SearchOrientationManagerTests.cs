using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using WordSearch.WordSearchLib;
using System.Linq;

namespace Tests
{
    public class SearchOrientationManagerTests
    {
        private TestUtilities _testUtilities;

        public SearchOrientationManagerTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void GetSearchOrientations_WhenGridIsValid_ReturnsListOfISearchOrientations()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            ISearchOrientationManager searchOrientationManager = new SearchOrientationManager();

            //act
            var searchOrientations = searchOrientationManager.GetSearchOrientations(gridManager);
            var searchOrientation = new SearchOrientation(new GridToLinearHorizontalStrategy(gridManager));

            //assert
            Assert.True(searchOrientations is List<ISearchOrientation>);
            Assert.True(searchOrientations.Count == 4);
        }
    }
}