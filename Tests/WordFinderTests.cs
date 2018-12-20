using System;
using System.Collections.Generic;
using Xunit;
using Moq;
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
        [InlineData("ABC|DEF|GHI", "GDA", "(0,2),(0,1),(0,0)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "OKGC", "(2,3),(2,2),(2,1),(2,0)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomTopOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
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
        
        [Fact]
        public void WordFinder_CreatedWithNullSearchOrientations_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "searchOrientations parameter is null.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(null));
            Assert.Equal(expectedMessage, exception.Message);
        }
        
        [Fact]
        public void WordFinder_CreatedWithEmptySearchOrientations_ThrowsArgumentException()
        {
            //arrange
            string expectedMessage = "searchOrientations list is empty.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new WordFinder(new List<ISearchOrientation>()));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "CDE", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "GHIJ", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInLeftRightOrientationDueToRowEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget,"Not found.");

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "GFE", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "MLKJ", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInRightLeftOrientationDueToRowEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.");

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "DGB", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "GKOD", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopBottomOrientationDueToColumnEdgesJoined_CoordinatesNotReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.");

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "EBG", "Not found.")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "KGCN", "Not found.")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomTopOrientationDueToColumnEdgesJoined_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget, "Not found.");

            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_SearchTargetFound_ReturnsResultOfSearchOrientationGetCoords()
        {
            //arrange
            string expected = "(10,10)";
            Mock<ISearchOrientation> mock = new Mock<ISearchOrientation>();
            mock.Setup(m => m.IsSearchTargetFound(It.IsAny<string>())).Returns(true);
            mock.Setup(m => m.GetCoordinatesOfSearchTarget(It.IsAny<string>())).Returns("(10,10)");

            //act
            var wordFinder = new WordFinder(new List<ISearchOrientation>(){{mock.Object}});
            string actual = wordFinder.GetCoordinatesOfSearchTarget("", "Not found.");

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC|DEF|GHI", "AEI", "(0,0),(1,1),(2,2)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "BGL", "(1,0),(2,1),(3,2)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInTopLeftBottomRightOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
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
        [InlineData("ABC|DEF|GHI", "IEA", "(2,2),(1,1),(0,0)")]
        [InlineData("ABCD|EFGH|IJKL|MNOP", "LGB", "(3,2),(2,1),(1,0)")]
        public void GetCoordinatesOfSearchTarget_NxNGridContainsTargetInBottomRightTopLeftOrientation_CoordinatesReturned(string gridSource, string searchTarget, string expected)
        {
            //arrange
            string[,] grid = TestUtilities.StringToGrid(gridSource);

            //act
            var wordFinder = new WordFinder(TestUtilities.GetSearchOrientations(grid));
            string actual = wordFinder.GetCoordinatesOfSearchTarget(searchTarget);

            //assert
            Assert.Equal(expected, actual);
        }

    }
}
