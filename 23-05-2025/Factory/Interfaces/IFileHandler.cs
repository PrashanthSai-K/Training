using System;

namespace Factory;

public interface IFileHandler
{
    void ReadFromFile();
    void WriteToFile(string[] data);
}
