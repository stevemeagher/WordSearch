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
        private IGridValidator _gridValidator;

        public SearchOrientationManagerTests()
        {
            _testUtilities = new TestUtilities();
            _gridValidator = new GridValidator();
        }

        [Fact]
        public void GetSearchOrientations_WhenGridIsValid_ReturnsListOfISearchOrientations()
        {
            //arrange
            string[,] grid = _testUtilities.StringToGrid("ABC|DEF|GHI");
            ISearchOrientationManager searchOrientationManager = new SearchOrientationManager();

            //act
            var searchOrientations = searchOrientationManager.GetSearchOrientations(_gridValidator, grid);
            var searchOrientation = new SearchOrientation(new GridToLinearLeftRightStrategy(_gridValidator, grid));

            //assert
            Assert.True(searchOrientations is List<ISearchOrientation>);
            Assert.True(searchOrientations.Count == 8);
        }
    }
}