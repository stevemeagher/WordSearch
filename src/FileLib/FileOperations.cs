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

        public string ApplicationBasePath(string baseApplicationDirectory, string fullApplicationPath = "")
        {
            if (_applicationBasePath == "" || baseApplicationDirectory != _baseApplicationDirectory)
            {
                _baseApplicationDirectory = baseApplicationDirectory;
                _applicationBasePath = GetApplicationBasePath(baseApplicationDirectory, fullApplicationPath);
            }

            return _applicationBasePath;
        }

        private string GetApplicationBasePath(string baseApplicationDirectory, string fullApplicationPath)
        {
            string path = String.IsNullOrEmpty(fullApplicationPath) ? Directory.GetCurrentDirectory() : fullApplicationPath;

            int indexOfBaseName = -1;
            int lastIndexOfBaseName = -1;
            
            do
            {
                lastIndexOfBaseName = indexOfBaseName;
                indexOfBaseName = path.IndexOf(baseApplicationDirectory, indexOfBaseName + 1);
            } while (indexOfBaseName != -1);


            if (lastIndexOfBaseName == -1)
            {
                throw new ApplicationException($"Application path does not include {baseApplicationDirectory}");
            }

            int indexOfNextSlash = path.IndexOf("/", lastIndexOfBaseName);

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
