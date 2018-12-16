using System;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class WordSearchLibTests
    {
        [Fact]
        public void ConvertGridToStringLeftToRight()
        {
            //arrange
            string[,] grid = {
                {"A","B","C"},
                {"D","E","F"},
                {"G","H","I"}
            };

            string expected = "ABCDEFGHI";

            //act
            var gridToString = new GridToString();
            string actual = gridToString.Convert(grid);

            //assert
            Assert.True(expected == actual);

        }
    }
}
