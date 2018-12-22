using System;
using System.IO;

namespace WordSearch.ConsoleApp
{
    public class WordSearchProgram
    {
        public void WriteGridToConsole(string[,] grid)
        {
            var columnsCount = grid.GetLength(1);
            var rowsCount = grid.GetLength(0);

            for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
            {
                for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
                {
                    string letter = grid[rowNumber, columnNumber];

                    Console.Write(letter);
                    
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}