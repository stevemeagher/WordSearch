using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;

namespace WordSearch.Tests
{
    public class PointListTests
    {
        [Fact]
        public void ToString_WhenPointListContainsListOfPoints_ReturnsPointsAsCommaDelimeteredString()
        {
            //arrange
            string expected = "(0,0),(1,1),(2,2)";
            PointList points = new PointList() {new Point(0,0), new Point(1,1), new Point(2,2)};

            //act
            string pointsAsString = points.ToString();

            //assert
            Assert.Equal(expected, pointsAsString);
        }

        [Fact]
        public void ToString_WhenPointListInitializedWithNoPointMessageAndContainsNoPoints_ReturnsMessage()
        {
            //arrange
            string expected = "NO POINTS!";
            PointList points = new PointList(expected);

            //act
            string pointsAsString = points.ToString();

            //assert
            Assert.Equal(expected, pointsAsString);
        }

        [Fact]
        public void ToString_WhenPointListInitializedWithoutNoPointMessageAndContainsNoPoints_ReturnsEmptyString()
        {
            //arrange
            PointList points = new PointList();

            //act
            string pointsAsString = points.ToString();

            //assert
            Assert.Equal("", pointsAsString);
        }
    }
}