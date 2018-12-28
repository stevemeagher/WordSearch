using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public interface ILinearView
    {
        string Value { get; set; }
        Dictionary<int, Point> IndexToGridPosition { get; set; }
    }
}