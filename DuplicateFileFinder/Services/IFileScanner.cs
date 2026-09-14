namespace DuplicateFileFinder.Services;

public interface IFileScanner
{
    IEnumerable<string> ScanFiles(string rootPath);
}