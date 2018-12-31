using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;

namespace WordSearch.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IConsoleWrapper consoleWrapper = new ConsoleWrapper();
            IFileOperations fileOperations = new FileOperations();
            IWordFinder wordFinder = new WordFinder();
            ISearchOrientationManager searchOrientationManager = new SearchOrientationManager();

            var wordSearchProgram = new WordSearchProgram(consoleWrapper, fileOperations, wordFinder, searchOrientationManager);

            wordSearchProgram.ProgramLoop("puzzles");
        }

        
            
    }
}
