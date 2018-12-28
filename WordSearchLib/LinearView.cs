using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class LinearView : ILinearView
    {
        public LinearView(string value, Dictionary<int, Point> indexToGridPosition)
        {
            Value = value;
            IndexToGridPosition = indexToGridPosition;
        }
        
        public string Value { get; set; }
        public Dictionary<int, Point> IndexToGridPosition { get; set; }
    }
}