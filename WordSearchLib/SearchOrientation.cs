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
        private readonly ILinearView _linearView;

        public SearchOrientation(GridToLinearStrategy gridToLinearStrategy)
        {
            if (gridToLinearStrategy is null) throw new ArgumentException("gridToLinearStrategy parameter is null.");

            _gridToLinearStrategy = gridToLinearStrategy;

            _linearView = _gridToLinearStrategy.GridToLinear();
        }

        public SearchOrientation(ILinearView linearView)
        {
            _linearView = linearView;
        }

        public ILinearView LinearView => _linearView;

        public bool IsSearchTargetFound(string searchTarget)
        {
            if (String.IsNullOrEmpty(searchTarget) || _linearView is null || _linearView.Value is null) return false;
            
            return _linearView.Value.IndexOf(searchTarget.ToUpper()) != -1 || _linearView.ReversedValue.IndexOf(searchTarget.ToUpper()) != -1;
        }

        public PointList GetCoordinatesOfSearchTarget(string searchTarget)
        {
            if (String.IsNullOrEmpty(searchTarget)) return null;

            string source;
            Dictionary<int, Point> indexToGridPosition;

            int targetIndex = _linearView.Value.IndexOf(searchTarget.ToUpper());

            if (targetIndex == -1)
            {
                targetIndex = _linearView.ReversedValue.IndexOf(searchTarget.ToUpper());
                indexToGridPosition = _linearView.ReversedIndexToGridPosition;
                source = _linearView.ReversedValue;
            }
            else
            {
                indexToGridPosition = _linearView.IndexToGridPosition;
                source = _linearView.Value;
            }

            //reduce targetIndex by the number of |'s (row or column boundary indicators) found up to the position of the target string
            var boundaryIndicatorCount = source.Substring(0, targetIndex).Count(o => o == '|');
            targetIndex = targetIndex - boundaryIndicatorCount;

            PointList coordinates = new PointList();

            for (int i = targetIndex; i < targetIndex + searchTarget.Length; i++)
            {
                coordinates.Add(new Point(indexToGridPosition[i].X, indexToGridPosition[i].Y));
            }

            return coordinates;
        }
    }
}