using System.Text;

namespace WordSearch.WordSearchLib
{
    public class WordFinder
    {
        public string GetCoordinatesOfSearchTarget(string[,] grid, string searchTarget)
        {
            var gridToLinear = new GridToLinear();

            var linearView = gridToLinear.ConvertToLeftToRight(grid);

            int targetIndex = linearView.Value.IndexOf(searchTarget.ToUpper());

            if (targetIndex != -1)
            {
                StringBuilder coordinates = new StringBuilder();

                for (int i = targetIndex; i < targetIndex + searchTarget.Length; i++)
                {
                    //if last coordinate then don't add a comma
                    string comma = i == targetIndex + searchTarget.Length - 1 ? "" : ",";

                    coordinates.Append($"({linearView.IndexToGridPosition[i].X},{linearView.IndexToGridPosition[i].Y}){comma}");
                }

                return coordinates.ToString();
            }

            return "";
        }
    }
}