using System;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class WordFinderUnitTests
    {
        private TestUtilities _testUtilities;
        private ISearchOrientationManager _searchOrientationManager;

        public WordFinderUnitTests()
        {
            _testUtilities = new TestUtilities();
            _searchOrientationManager = new SearchOrientationManager();
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

        
        [Fact]
        public void GetCoordinatesOfSearchTarget_SearchTargetFound_ReturnsResultOfSearchOrientationGetCoords()
        {
            //arrange
            string expected = "(10,10)";
            Mock<ISearchOrientation> mock = new Mock<ISearchOrientation>();
            mock.Setup(m => m.IsSearchTargetFound(It.IsAny<string>())).Returns(true);
            mock.Setup(m => m.GetCoordinatesOfSearchTarget(It.IsAny<string>())).Returns(new PointList(){new Point(10,10)});

            //act
            IWordFinder wordFinder = new WordFinder(new List<ISearchOrientation>(){{mock.Object}});
            string actual = wordFinder.GetCoordinatesOfSearchTarget("","Not found.").ToString();

            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetCoordinatesOfSearchTarget_WhenSearchOrientationsNull_ThrowsException()
        {
            //arrange
            string expectedMessage = "no SearchOrientations have been set in WordFinder.";
            IWordFinder wordFinder = new WordFinder();

            //act & assert
            var exception = Assert.Throws<Exception>(() => wordFinder.GetCoordinatesOfSearchTarget("target"));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
