using System;

namespace WordSearch.ConsoleApp
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public virtual ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
        public virtual ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string output)
        {
            Console.Write(output);
        }

        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}