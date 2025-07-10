using ComandLib;
namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _dirPath;
    public DirectorySizeCommand(string dirPath)
    {
        _dirPath = dirPath;
    }
    public void Execute()
    {
        if (!Directory.Exists(_dirPath))
        {
            if (string.IsNullOrWhiteSpace(_dirPath))
                throw new ArgumentException("Путь к каталогу не может быть пустым.", nameof(_dirPath));
            Console.WriteLine($"Ошибка: Каталог не найден — {_dirPath}");
            return;
        }
        
        var directoryInfo = new DirectoryInfo(_dirPath);
        long totalSize = GetDirectorySize(directoryInfo);

        Console.WriteLine($"Общий размер каталога '{_dirPath}': {totalSize}");
    }
    public long GetDirectorySize(DirectoryInfo directoryInfo)
    {
        long fileTotalSize = directoryInfo
            .GetFiles()
            .Sum(file => file.Length);

        long dirTotalSize = directoryInfo
            .GetDirectories()
            .Sum(subDir => GetDirectorySize(subDir));

        return fileTotalSize + dirTotalSize;
    }

}

public class FindFilesCommand : ICommand
{
    private readonly string _dirPath;
    private readonly string _mask;
    public FindFilesCommand(string dirPath, string mask)
    {
        if (string.IsNullOrWhiteSpace(dirPath))
            throw new ArgumentException("Путь к каталогу не может быть пустым.", nameof(dirPath));
        if (string.IsNullOrWhiteSpace(mask))
            throw new ArgumentException("Маска не может быть пустой.", nameof(dirPath));

        _dirPath = dirPath;
        _mask = mask;
    }

    public void Execute()
    {
        try
        {
            var files = Directory.GetFiles(_dirPath, _mask, SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                Console.WriteLine($"Файлы по маске '{_mask}' не найдены в каталоге '{_dirPath}'.");
                return;
            }

            Console.WriteLine($"Найденные файлы по маске '{_mask}':");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске файлов: {ex.Message}");
        }
    }

    public int HowManyFilesFitTheMask()
    {
        var files = Directory.GetFiles(_dirPath, _mask, SearchOption.AllDirectories);
        return files.Count();
    }

}