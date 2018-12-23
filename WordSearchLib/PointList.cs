using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace WordSearch.WordSearchLib
{
    public class PointList : List<Point>
    {
        private string _noPointsMessage = "";

        public PointList():base()
        {
        }

        public PointList(string noPointsMessage):base()
        {
            _noPointsMessage = noPointsMessage;
        }

        // public string ToString(string nullListMessage)
        // {
        //     if (this == null || this.Count() == 0)
        //         return nullListMessage;
        //     else
        //         return ToString();
        // }


        public override string ToString()
        {
            if (this == null || this.Count() == 0)
                return _noPointsMessage;

            StringBuilder coordinates = new StringBuilder();
            var lastPoint = this.Last();
            foreach (var point in this)
            {
                string comma = point == lastPoint ? "" : ",";

                coordinates.Append($"({point.X},{point.Y}){comma}");
            }

            return coordinates.ToString();
        }
    }
}