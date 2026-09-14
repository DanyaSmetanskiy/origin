using System.Security.Cryptography;
using DuplicateFileFinder.Models;

namespace DuplicateFileFinder.Services;

public class DuplicateFinder : IDuplicateFinder
{
    public List<DuplicateGroup> FindDuplicates(IEnumerable<string> filePaths)
    {
        // Крок 1: групуємо за розміром файлу (швидко, без читання вмісту).
        // Файли різного розміру точно не можуть бути дублікатами.
        var groupedBySize = filePaths
            .Select(path => new FileInfo(path))
            .Where(fi => fi.Exists)
            .GroupBy(fi => fi.Length)
            .Where(g => g.Count() > 1); // групи з 1 файлом одразу відкидаємо

        var result = new List<DuplicateGroup>();

        // Крок 2: всередині кожної групи рахуємо хеш вмісту (SHA-256)
        // і групуємо за хешем. Файл читається лише один раз.
        foreach (var sizeGroup in groupedBySize)
        {
            var hashGroups = new Dictionary<string, List<string>>();

            foreach (var fileInfo in sizeGroup)
            {
                string hash = ComputeFileHash(fileInfo.FullName);

                if (!hashGroups.TryGetValue(hash, out var list))
                {
                    list = new List<string>();
                    hashGroups[hash] = list;
                }
                list.Add(fileInfo.FullName);
            }

            foreach (var kvp in hashGroups.Where(g => g.Value.Count > 1))
            {
                result.Add(new DuplicateGroup
                {
                    Hash = kvp.Key,
                    FileSize = sizeGroup.Key,
                    FilePaths = kvp.Value
                });
            }
        }

        return result;
    }

    private static string ComputeFileHash(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = sha256.ComputeHash(stream);
        return Convert.ToHexString(hashBytes);
    }
}