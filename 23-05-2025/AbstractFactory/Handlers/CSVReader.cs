using System;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Handlers;

public class CSVFileReader : IReader
{
    public void ReadFromFile(string FilePath)
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

}
