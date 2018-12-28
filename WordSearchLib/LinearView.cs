using System;
using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class LinearView : ILinearView
    {
        public LinearView(string value, Dictionary<int, Point> indexToGridPosition)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("value parameter cannot be empty or null.");
            if (indexToGridPosition == null) throw new ArgumentException("indexToGridPosition parameter cannot be null.");
            if (indexToGridPosition.Count == 0) throw new ArgumentException("indexToGridPosition parameter has a count of zero.");

            Value = value;
            IndexToGridPosition = indexToGridPosition;
        }
        
        public string Value { get; set; }
        public Dictionary<int, Point> IndexToGridPosition { get; set; }
    }
}