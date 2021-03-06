using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.IntegrationTests
{
    public class WordFinderIntegrationTests
    {
        private TestUtilities _testUtilities;
        private ISearchOrientationManager _searchOrientationManager;

        public WordFinderIntegrationTests()
        {
            _testUtilities = new TestUtilities();
            _searchOrientationManager = new SearchOrientationManager();
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "DEF", "(0,1),(1,1),(2,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "JKL", "(1,2),(2,2),(3,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = (wordFinder.GetCoordinatesOfSearchTarget(searchTarget)).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "FED", "(2,1),(1,1),(0,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "LKJ", "(3,2),(2,2),(1,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInRightLeftOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ADG", "(0,0),(0,1),(0,2)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "CGKO", "(2,0),(2,1),(2,2),(2,3)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopBottomOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "GDA", "(0,2),(0,1),(0,0)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "OKGC", "(2,3),(2,2),(2,1),(2,0)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomTopOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "JKL", "Not found")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "QRST", "Search target was not found")]
        public void GetCoordinatesOfSearchTarget_NxNGridDoesNotContainsTarget_NotFoundMessageReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, expected).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "CDE", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "GHIJ", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientationDueToRowEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget,"Not found.").ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "GFE", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "MLKJ", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInRightLeftOrientationDueToRowEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.").ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "DGB", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "GKOD", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopBottomOrientationDueToColumnEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.").ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "EBG", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "KGCN", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomTopOrientationDueToColumnEdgesJoined_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.").ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "AEI", "(0,0),(1,1),(2,2)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "BGL", "(1,0),(2,1),(3,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopLeftBottomRightOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "IEA", "(2,2),(1,1),(0,0)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "LGB", "(3,2),(2,1),(1,0)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomRightTopLeftOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("ABC|DEF|GHI", "CEG", "(2,0),(1,1),(0,2)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "DGJM", "(3,0),(2,1),(1,2),(0,3)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopRightBottomLeftOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "GEC", "(0,2),(1,1),(2,0)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "MJGD", "(0,3),(1,2),(2,1),(3,0)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomLeftTopRightOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            IGridManager gridManager = new GridManager(_testUtilities.StringToGrid(gridSource));

            //act
            IWordFinder wordFinder = new WordFinder(_searchOrientationManager.GetSearchOrientations(gridManager));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget).ToString();

            //assert
            Assert.Equal(expected, actual);
        }
    }
}
