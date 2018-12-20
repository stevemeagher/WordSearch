using System;
using Xunit;
using WordSearch.WordSearchLib;
using System.Collections.Generic;

namespace Tests
{
    public class TestUtilities
    {
        /// <summary>Converts formatted string to 2d string array </summary>
        /// <para>StringToGrid converts a specifically formatted source string into a two-dimensional string array.  
        /// This method is used to produce string arrays for testing and enables the formatted strings to be passed 
        /// as InLineData attribute parameters.
        /// </para>
        public static string[,] StringToGrid(string source)
        {
            if (string.IsNullOrEmpty(source)) throw new ArgumentException("source parameter contains no characters.");
            if (!source.Contains("|")) throw new ArgumentException("source parameter not in correct format: no row separator characters.");

            string[] rows = source.Split('|');
            int rowCount = rows.Length;
            int columnCount = rows[0].Length;

            foreach (var row in rows)
            {
                if (row.Length != columnCount) throw new ArgumentException("source parameter not in correct format: rows must contain the same number of characters.");
            }

            string[,] grid = new string[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    grid[i,j] = rows[i].Substring(j,1);
                }
            }
            return grid;
        }

        public static List<ISearchOrientation> GetSearchOrientations(string[,] grid)
        {
            return new List<ISearchOrientation>() 
            {
                new SearchOrientation(new GridToLinearLeftRightStrategy(grid)),
                new SearchOrientation(new GridToLinearRightLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopBottomStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomTopStrategy(grid)),
                new SearchOrientation(new GridToLinearTopLeftBottomRightStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomRightTopLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearTopRightBottomLeftStrategy(grid)),
                new SearchOrientation(new GridToLinearBottomLeftTopRightStrategy(grid))
            };
        }
    }
}