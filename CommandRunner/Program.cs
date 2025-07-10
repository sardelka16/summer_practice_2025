using System.Reflection;
using ComandLib;
namespace CommandRunner;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var assembly = Assembly.LoadFrom("../FileSystemCommands/bin/Debug/net9.0/FileSystemCommands.dll");
            ICommand directorySizeCommand = (ICommand)Activator.CreateInstance(
                assembly.GetType("FileSystemCommands.DirectorySizeCommand"), "..");
            ICommand findFilesCommand = (ICommand)Activator.CreateInstance(
                assembly.GetType("FileSystemCommands.FindFilesCommand"), "..", "*.cs");

            directorySizeCommand.Execute();
            findFilesCommand.Execute();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

