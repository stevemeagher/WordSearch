using System;
using Xunit;
using WordSearch.WordSearchLib;
using WordSearch.Tests.Common;

namespace WordSearch.Tests
{
    public class TestUtilitiesUnitTests
    {
        private TestUtilities _testUtilities;

        public TestUtilitiesUnitTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void StringToGrid_WellFormedStringInput_Creates2dStringArray()
        {
            //arrange
            string source = "ABC|DEF|GHI";
            string[,] expeceted = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            //act
            string[,] actual = _testUtilities.StringToGrid(source);

            //assert
            Assert.Equal(expeceted, actual);
        }

        [Fact]
        public void StringToGrid_NoRowDividers_GeneratesArgumentException()
        {
            //arrange
            string source = "ABCDEFGHI";
            string expectedMessage = "source parameter not in correct format: no row separator characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => _testUtilities.StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void StringToGrid_EmptyInputParameter_GeneratesArgumentException()
        {
            //arrange
            string source = "";
            string expectedMessage = "source parameter contains no characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => _testUtilities.StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);
        }
        
        [Fact]
        public void StringToGrid_InputParameterWouldProduceJaggedArray_GeneratesArgumentException()
        {
            //arrange
            string source = "ABC|DE|FGHI";
            string expectedMessage = "source parameter not in correct format: rows must contain the same number of characters.";

            //act & assert
            var exception = Assert.Throws<ArgumentException>(() => _testUtilities.StringToGrid(source));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}