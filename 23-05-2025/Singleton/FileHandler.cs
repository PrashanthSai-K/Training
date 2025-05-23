using System;
using System.IO;


namespace Singleton;

public sealed class FileHandler
{
    private string FilePath;
    private FileHandler()
    {
        FilePath = "./sample.txt";
    }

    private static readonly Lazy<FileHandler> _fileHandler = new Lazy<FileHandler>(() => new FileHandler());

    public static FileHandler GetFileHandler()
    {
        return _fileHandler.Value;
    }

    public void WriteToFile(string[] data)
    {
        try
        {
            File.WriteAllLines(FilePath, data);
            Console.WriteLine("----Data written to a file----\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }
    }

    public void AppendToFile(string[] data)
    {
        try
        {
            File.AppendAllLines(FilePath, data);
            Console.WriteLine("----Data appending to a file----\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }
    }


    public void ReadFromFile()
    {
        try
        {
            Console.WriteLine("\n----Data Reading from a file----\n");

            string[] lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("\n----Data Read Completed----\n");

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }

    }

}
