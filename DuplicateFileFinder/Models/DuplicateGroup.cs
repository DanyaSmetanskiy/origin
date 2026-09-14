namespace DuplicateFileFinder.Models;

public class DuplicateGroup
{
    public string Hash { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public List<string> FilePaths { get; set; } = new();
}