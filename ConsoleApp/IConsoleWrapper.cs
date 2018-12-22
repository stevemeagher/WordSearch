using System;

namespace WordSearch.ConsoleApp
{
    public interface IConsoleWrapper
    {
        void WriteLine();
        void WriteLine(string output);
        void Write(string output);
        string ReadLine();
        ConsoleKeyInfo ReadKey();
        ConsoleColor BackgroundColor {get; set;}
        ConsoleColor ForegroundColor {get; set;}
    }
}