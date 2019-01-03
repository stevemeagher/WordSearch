using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class SearchOrientationManagerUnitTests
    {
        private TestUtilities _testUtilities;

        public SearchOrientationManagerUnitTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void GetSearchOrientations_ReturnsListOfISearchOrientations()
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid("ABC|DEF|GHI"));
            ISearchOrientationManager searchOrientationManager = new SearchOrientationManager();

            //act
            var searchOrientations = searchOrientationManager.GetSearchOrientations(gridManager);

            //assert
            Assert.True(searchOrientations is List<ISearchOrientation>);
            Assert.True(searchOrientations.Count == 4);
        }
    }
}