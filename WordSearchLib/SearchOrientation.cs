using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace WordSearch.WordSearchLib
{
    public class SearchOrientation : ISearchOrientation
    {
        private readonly GridToLinearStrategy _gridToLinearStrategy;
        private readonly LinearView _linearView;

        public SearchOrientation(GridToLinearStrategy gridToLinearStrategy)
        {
            if (gridToLinearStrategy is null) throw new ArgumentException("gridToLinearStragtey parameter is null.");

            _gridToLinearStrategy = gridToLinearStrategy;

            _linearView = _gridToLinearStrategy.GridToLinear();
        }

        public bool IsSearchTargetFound(string searchTarget)
        {
            return String.IsNullOrEmpty(searchTarget) ? false : _linearView.Value.IndexOf(searchTarget.ToUpper()) != -1;
        }

        public string GetCoordinatesOfSearchTarget(string searchTarget)
        {
            if (String.IsNullOrEmpty(searchTarget)) return "";

            int targetIndex = _linearView.Value.IndexOf(searchTarget.ToUpper());

            StringBuilder coordinates = new StringBuilder();

            for (int i = targetIndex; i < targetIndex + searchTarget.Length; i++)
            {
                //if last coordinate then don't add a comma
                string comma = i == targetIndex + searchTarget.Length - 1 ? "" : ",";

                coordinates.Append($"({_linearView.IndexToGridPosition[i].X},{_linearView.IndexToGridPosition[i].Y}){comma}");
            }

            return coordinates.ToString();
        }
    }
}