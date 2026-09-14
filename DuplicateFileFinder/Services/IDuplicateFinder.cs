using DuplicateFileFinder.Models;

namespace DuplicateFileFinder.Services;

public interface IDuplicateFinder
{
    List<DuplicateGroup> FindDuplicates(IEnumerable<string> filePaths);
}