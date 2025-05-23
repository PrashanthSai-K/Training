using System;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Handlers;

public class TextFileWriter : IWriter
{
    public void WriteToFile(string FilePath, string[] data)
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
