﻿using System;
using System.IO;

namespace WordSearch.FileLib
{
    public class FileOperations
    {
        private string _applicationBasePath = "";
        private string _baseApplicationDirectory = "";

        public string[] ReadLines(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllLines(filePath);
            }
            else
            {
                return new string[0];
            }
        }

        public string ApplicationBasePath(string baseApplicationDirectory)
        {
            if (_applicationBasePath == "" || baseApplicationDirectory != _baseApplicationDirectory)
            {
                _baseApplicationDirectory = baseApplicationDirectory;
                _applicationBasePath = GetApplicationBasePath(baseApplicationDirectory);
            }

            return _applicationBasePath;
        }

        private string GetApplicationBasePath(string baseApplicationDirectory)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            var indexOfBaseName = path.IndexOf(baseApplicationDirectory);

            if (indexOfBaseName == -1)
            {
                throw new ApplicationException($"Applictaion path does not include {baseApplicationDirectory}");
            }

            return path.Substring(0,indexOfBaseName + baseApplicationDirectory.Length);
        }

        public string[] GetDirectoryContents(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            Array.Sort(filePaths);
            return filePaths;
        }
    }
}
