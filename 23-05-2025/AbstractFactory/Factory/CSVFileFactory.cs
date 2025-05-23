using System;
using AbstractFactory.Handlers;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Factory;

public class CSVFileFactory : IFileFactory
{
    public IReader CreateReader()
    {
        return new CSVFileReader();
    }

    public IWriter CreateWriter()
    {
        return new CSVFileWriter();
    }
}
