using System;
using Proxy.Interfaces;

namespace Proxy;

public class Files : IFile
{
    private string FilePath = "./sample.txt";
    public void Read()
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

    public void ReadMetadata()
    {
        try
        {
            FileInfo fileInfo = new FileInfo(FilePath);
            Console.WriteLine("-----Metadata of the file------");
            Console.WriteLine($"File Path : {fileInfo.FullName}\nFile Name: {fileInfo.Name}\nFile Type : {fileInfo.Extension}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error : {e.Message}");
        }
    }
}
