namespace WordSearch.FileLib
{
    public interface IFileOperations
    {
        string[] ReadLines(string filePath);
        string ApplicationBasePath(string baseApplicationDirectory);
        string[] GetDirectoryContents(string directoryPath);
        string GetFileNameFromPath(string filePath);
        bool DirectoryExists(string directoryPath);
    }
}