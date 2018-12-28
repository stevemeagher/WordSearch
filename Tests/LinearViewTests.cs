using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xunit;
using WordSearch.WordSearchLib;

namespace Tests
{
    public class LinearViewTests
    {
        private TestUtilities _testUtilities;

        public LinearViewTests()
        {
            _testUtilities = new TestUtilities();
        }

        [Fact]
        public void LinearView_WhenInstantiatedWithValidParameterValues_CanAccessPropertyValues()
        {
            //arrange
            string expectedValue = "Value";
            Dictionary<int, Point> expecetdIndexToGridPosition = new Dictionary<int, Point>();
            expecetdIndexToGridPosition.Add(1, new Point(1,1));

            //act
            ILinearView linearView = new LinearView(expectedValue, expecetdIndexToGridPosition);

            //assert
            Assert.Equal(expectedValue, linearView.Value);
            Assert.Equal(expecetdIndexToGridPosition, linearView.IndexToGridPosition);
        }

        
    }
}