using System;

namespace Factory;

public class TextFileHandler : IFileHandler
{
    private string FilePath;

    public TextFileHandler()
    {
        FilePath = "./sample.txt";
    }

    public void ReadFromFile()
    {
        try
        {
            Console.WriteLine("\n----Data Reading from a Text file----\n");

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

    public void WriteToFile(string[] data)
    {
        try
        {
            File.WriteAllLines(FilePath, data);
            Console.WriteLine("----Data written to a Text file----\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }
    }
}
