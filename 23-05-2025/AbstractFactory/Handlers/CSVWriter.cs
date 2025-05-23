using System;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Handlers;

public class CSVFileWriter : IWriter
{
    public void WriteToFile(string FilePath, string[] data)
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
