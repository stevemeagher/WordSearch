using System;
using System.IO;

namespace WordSearch.FileLib
{
    public class FileOperations : IFileOperations
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
            string path = Directory.GetCurrentDirectory();

            var indexOfBaseName = path.IndexOf(baseApplicationDirectory);

            if (indexOfBaseName == -1)
            {
                throw new ApplicationException($"Applictaion path does not include {baseApplicationDirectory}");
            }

            int indexOfNextSlash = path.IndexOf("/", indexOfBaseName);

            return path.Substring(0,indexOfNextSlash);
        }

        public string[] GetDirectoryContents(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            Array.Sort(filePaths);
            return filePaths;
        }

        public string GetFileNameFromPath(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
    }
}
