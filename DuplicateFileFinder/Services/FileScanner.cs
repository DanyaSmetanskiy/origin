namespace DuplicateFileFinder.Services;

public class FileScanner : IFileScanner
{
    public IEnumerable<string> ScanFiles(string rootPath)
    {
        if (!Directory.Exists(rootPath))
            throw new DirectoryNotFoundException($"Папку не знайдено: {rootPath}");

        return Directory.EnumerateFiles(rootPath, "*", SearchOption.AllDirectories);
    }
}