using System.Text;

namespace WordSearch.WordSearchLib
{
    public class WordFinder
    {
        private enum LinearViewDirection
        {
            LeftToRight,
            RightToLeft
        }

        public string GetCoordinatesOfSearchTarget(string[,] grid, string searchTarget)
        {
            var gridToLinear = new GridToLinear();

            var linearViewLeftToRight = gridToLinear.ConvertToLeftToRight(grid);
            var linearViewRightToLeft = gridToLinear.ConvertToRightToLeft(grid);

            var linearViewDirection = LinearViewDirection.LeftToRight;
            int targetIndex = linearViewLeftToRight.Value.IndexOf(searchTarget.ToUpper());

            if (targetIndex == -1)
            {
                linearViewDirection = LinearViewDirection.RightToLeft;;
                targetIndex = linearViewRightToLeft.Value.IndexOf(searchTarget.ToUpper());
            }

            if (targetIndex != -1)
            {
                StringBuilder coordinates = new StringBuilder();

                for (int i = targetIndex; i < targetIndex + searchTarget.Length; i++)
                {
                    //if last coordinate then don't add a comma
                    string comma = i == targetIndex + searchTarget.Length - 1 ? "" : ",";
                    switch (linearViewDirection)
                    {
                        case LinearViewDirection.LeftToRight:
                            coordinates.Append($"({linearViewLeftToRight.IndexToGridPosition[i].X},{linearViewLeftToRight.IndexToGridPosition[i].Y}){comma}");
                            break;
                        case LinearViewDirection.RightToLeft:
                            coordinates.Append($"({linearViewRightToLeft.IndexToGridPosition[i].X},{linearViewRightToLeft.IndexToGridPosition[i].Y}){comma}");
                            break;
                            
                    }
                }

                return coordinates.ToString();
            }

            return "";
        }
    }
}