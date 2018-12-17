using System;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class WordFinderTests
    {

        [Theory]
        [InlineData("ABC|DEF|GHI", "DEF", "(0,1),(1,1),(2,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "JKL", "(1,2),(2,2),(3,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "FED", "(2,1),(1,1),(0,1)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "LKJ", "(3,2),(2,2),(1,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInRightLeftOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "ADG", "(0,0),(0,1),(0,2)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "CGKO", "(2,0),(2,1),(2,2),(2,3)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopBottomOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "JKL", "Not found")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "QRST", "Search target was not found")]
        public void GetCoordinatesOfSearchTarget_NxNGridDoesNotContainsTarget_NotFoundMessageReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, expected);

            //assert
            Assert.Equal(expected, actual);
        }

        

    }
}
