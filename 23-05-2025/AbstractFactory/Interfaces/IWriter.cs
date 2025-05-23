using System;

namespace AbstractFactory.Interfaces;

public interface IWriter
{
    void WriteToFile(string FilePath, string[] data);
}
