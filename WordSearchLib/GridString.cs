using System.Collections.Generic;
using System.Drawing;

namespace WordSearch.WordSearchLib
{
    public class LinearView
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