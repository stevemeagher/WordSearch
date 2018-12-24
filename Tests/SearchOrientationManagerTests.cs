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
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            ISearchOrientationManager searchOrientationManager = new SearchOrientationManager();

            //act
            var searchOrientations = searchOrientationManager.GetSearchOrientations(grid);
            var searchOrientation = new SearchOrientation(new GridToLinearLeftRightStrategy(grid));

            //assert
            Assert.True(searchOrientations is List<ISearchOrientation>);
            Assert.True(searchOrientations.Count == 8);
        }
    }
}