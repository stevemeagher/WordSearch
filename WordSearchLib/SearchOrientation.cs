using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace WordSearch.WordSearchLib
{
    public class SearchOrientation : ISearchOrientation
    {
        private readonly GridToLinearStrategy _gridToLinearStrategy;
        private readonly LinearView _linearView;

        public SearchOrientation(GridToLinearStrategy gridToLinearStrategy)
        {
            if (gridToLinearStrategy is null) throw new ArgumentException("gridToLinearStrategy parameter is null.");

            _gridToLinearStrategy = gridToLinearStrategy;

            _linearView = _gridToLinearStrategy.GridToLinear();
        }

        public bool IsSearchTargetFound(string searchTarget)
        {
            return String.IsNullOrEmpty(searchTarget) || _linearView is null || _linearView.Value is null ? false : _linearView.Value.IndexOf(searchTarget.ToUpper()) != -1;
        }

        public PointList GetCoordinatesOfSearchTarget(string searchTarget)
        {
            if (String.IsNullOrEmpty(searchTarget)) return null;

            int targetIndex = _linearView.Value.IndexOf(searchTarget.ToUpper());

            //reduce targetIndex by the number of |'s (row or column boundary indicators) found up to the position of the target string
            var boundaryIndicatorCount = _linearView.Value.Substring(0, targetIndex).Count(o => o == '|');
            targetIndex = targetIndex - boundaryIndicatorCount;

            PointList coordinates = new PointList();

            for (int i = targetIndex; i < targetIndex + searchTarget.Length; i++)
            {
                //if last coordinate then don't add a comma
                string comma = i == targetIndex + searchTarget.Length - 1 ? "" : ",";

                coordinates.Add(new Point(_linearView.IndexToGridPosition[i].X, _linearView.IndexToGridPosition[i].Y));
            }

            return coordinates;
        }
    }
}