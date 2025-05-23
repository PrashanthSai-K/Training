using System;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Handlers;

public class TextFileReader : IReader
{
    public void ReadFromFile(string FilePath)
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

}
