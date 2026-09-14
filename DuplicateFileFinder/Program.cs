using DuplicateFileFinder.Services;

if (args.Length == 0)
{
    Console.WriteLine("Використання: dotnet run -- <шлях_до_папки>");
    return;
}

string path = args[0];

IFileScanner scanner = new FileScanner();
IDuplicateFinder finder = new DuplicateFinder();

try
{
    Console.WriteLine($"Сканування папки: {path}");
    var files = scanner.ScanFiles(path);

    var duplicates = finder.FindDuplicates(files);

    if (duplicates.Count == 0)
    {
        Console.WriteLine("Дублікатів не знайдено.");
        return;
    }

    Console.WriteLine($"\nЗнайдено {duplicates.Count} груп(и) дублікатів:\n");

    int groupNumber = 1;
    foreach (var group in duplicates)
    {
        Console.WriteLine($"Група {groupNumber} (розмір: {group.FileSize} байт, hash: {group.Hash[..12]}...):");
        foreach (var file in group.FilePaths)
        {
            Console.WriteLine($"  - {file}");
        }
        Console.WriteLine();
        groupNumber++;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Помилка: {ex.Message}");
}