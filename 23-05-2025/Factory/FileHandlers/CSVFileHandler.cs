using System;

namespace Factory;

public class CSVFileHandler : IFileHandler
{
    private string FilePath;

    public CSVFileHandler()
    {
        FilePath = "./sample.csv";
    }
    public void ReadFromFile()
    {
        try
        {
            Console.WriteLine("\n----Data Reading from a CSV file----\n");

            var lines = File.ReadAllLines(FilePath).Skip(1);
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
            string[] header = ["Id, Name, Salary"];
            string[] AllData = header.Concat(data).ToArray();
            File.WriteAllLines(FilePath, AllData);
            Console.WriteLine("----Data written to a CSV file----\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }
    }
}
