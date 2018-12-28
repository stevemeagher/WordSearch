using System;
using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class LinearView : ILinearView
    {
        public LinearView(string value, Dictionary<int, Point> indexToGridPosition)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("The value parameter cannot be empty or null.");

            Value = value;
            IndexToGridPosition = indexToGridPosition;
        }
        
        public string Value { get; set; }
        public Dictionary<int, Point> IndexToGridPosition { get; set; }
    }
}