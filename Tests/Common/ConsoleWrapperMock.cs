using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Xunit;
using Moq;
using WordSearch.ConsoleApp;
using WordSearch.FileLib;
using WordSearch.WordSearchLib;
using System.Linq;

namespace WordSearch.Tests.Common
{
    public class ConsoleWrapperMock : ConsoleWrapper
    {
        private bool _writeClearCommand;

        public ConsoleWrapperMock(bool writeClearCommand = false)
        {
            _writeClearCommand = writeClearCommand;
            ReadKeyChars = null;
        }

        public override ConsoleColor BackgroundColor
        { 
            get 
            {
                return Console.BackgroundColor;
            }
            set
            {
                Write($"<bg:{value.ToString()}>");
                Console.BackgroundColor = value;
            }
        }

        public override ConsoleColor ForegroundColor
        { 
            get 
            {
                return Console.ForegroundColor;
            }
            set
            {
                Write($"<fg:{value.ToString()}>");
                Console.ForegroundColor = value;
            }
        }

        public string ReadLineResult {get; set;}
        public List<string> ReadLineResults {get; set;}
        public char ReadKeyChar {get; set;}
        public List<char> ReadKeyChars {get; set;}

        public override string ReadLine()
        {
            if (ReadLineResults == null)
            {
                WriteLine(ReadLineResult);
                return ReadLineResult;
            }
            else if (ReadLineResults.Count > 0)
            {
                string line = ReadLineResults.First();
                ReadLineResults.RemoveAt(0);
                
                WriteLine(line);
                return line;
            }

            return "";
        }

        public override ConsoleKeyInfo ReadKey()
        {
            if (ReadKeyChars == null)
            {
                Write(ReadKeyChar.ToString());
                ConsoleKey key = new ConsoleKey();
                return new ConsoleKeyInfo(ReadKeyChar, key, false, false, false);
            }
            else if (ReadKeyChars.Count > 0)
            {
                char keyChar = ReadKeyChars.First();
                ReadKeyChars.RemoveAt(0);
                
                Write(keyChar.ToString());
                ConsoleKey key = new ConsoleKey();
                return new ConsoleKeyInfo(keyChar, key, false, false, false);
            }

            return new ConsoleKeyInfo(ReadKeyChar, new ConsoleKey(), false, false, false);
        }

        public override void Clear()
        {
            if (_writeClearCommand)
            {
                Write("<clear>");
            }    
            base.Clear();
        }
    }
}