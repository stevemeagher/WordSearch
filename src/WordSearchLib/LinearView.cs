using System;
using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class LinearView : ILinearView
    {
        public LinearView(string value, Dictionary<int, Point> indexToGridPosition)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("value parameter is empty or null.");
            if (indexToGridPosition == null) throw new ArgumentException("indexToGridPosition parameter is null.");
            if (indexToGridPosition.Count == 0) throw new ArgumentException("indexToGridPosition parameter has a count of zero.");

            Value = value;
            IndexToGridPosition = indexToGridPosition;

            ReversedValue = ReverseValue(value);
            ReversedIndexToGridPosition = ReverseIndexToGridPosition(indexToGridPosition);
        }
        
        public string Value { get; set; }
        public Dictionary<int, Point> IndexToGridPosition { get; set; }

        public string ReversedValue { get ; private set;}

        public Dictionary<int, Point> ReversedIndexToGridPosition { get; private set;}

        private string ReverseValue(string source)
        {
            char[] charArray = source.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private Dictionary<int, Point> ReverseIndexToGridPosition(Dictionary<int, Point> source)
        {
            Dictionary<int, Point> reverseIndexToPos = new Dictionary<int, Point>();

            int count = IndexToGridPosition.Count;

            for (int i = 0; i < count; i++)
            {
                reverseIndexToPos.Add(count - i - 1, source[i]);
            }

            return reverseIndexToPos;
        }
    }
}