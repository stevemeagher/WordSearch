using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;

namespace WordSearch.Tests.Common
{
    public class TestUtilities
    {
        public static string APPLICATION_DIRECTORY = "WordSearch";

        /// <summary>Converts formatted string to 2d string array </summary>
        /// <para>StringToGrid converts a specifically formatted source string into a two-dimensional string array.  
        /// This method is used to produce string arrays for testing and enables the formatted strings to be passed 
        /// as InLineData attribute parameters.
        /// </para>
        public string[,] StringToGrid(string source)
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

        public bool CreateEmptyDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            Directory.CreateDirectory(directoryPath);

            return Directory.Exists(directoryPath);
        }
    }
}