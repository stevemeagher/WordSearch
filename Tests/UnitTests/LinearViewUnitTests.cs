using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests.UnitTests
{
    public class LinearViewUnitTests
    {
        private TestUtilities _testUtilities;

        public LinearViewUnitTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void LinearView_WhenInstantiatedWithValidParameterValues_CanAccessPropertyValues()
        {
            //arrange
            string expectedValue = "Value";
            Dictionary<int, Point> expecetdIndexToGridPosition = new Dictionary<int, Point>();
            expecetdIndexToGridPosition.Add(0, new Point(1,1));

            //act
            ILinearView linearView = new LinearView(expectedValue, expecetdIndexToGridPosition);

            //assert
            Assert.Equal(expectedValue, linearView.Value);
            Assert.Equal(expecetdIndexToGridPosition, linearView.IndexToGridPosition);
        }

        [Fact]
        public void LinearView_WhenInstantiatedWithEmptyValueString_ThrowArgumentException()
        {
            //arrange
            string expectedMessage = "value parameter is empty or null.";
            string value = String.Empty;
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();
            indexToGridPosition.Add(1, new Point(1,1));

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new LinearView(value, indexToGridPosition));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void LinearView_WhenInstantiatedWithNullIndexToGridPosition_ThrowArgumentException()
        {
            //arrange
            string expectedMessage = "indexToGridPosition parameter is null.";
            string value = "Value";
            Dictionary<int, Point> indexToGridPosition = null;

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new LinearView(value, indexToGridPosition));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void LinearView_WhenInstantiatedWithIndexToGridPositionWithZeroCount_ThrowArgumentException()
        {
            //arrange
            string expectedMessage = "indexToGridPosition parameter has a count of zero.";
            string value = "Value";
            Dictionary<int, Point> indexToGridPosition = new Dictionary<int, Point>();

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => new LinearView(value, indexToGridPosition));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}