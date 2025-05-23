using System;
using AbstractFactory.Handlers;
using AbstractFactory.Interfaces;

namespace AbstractFactory.Factory;

public class TextFileFactory : IFileFactory
{
    public IReader CreateReader()
    {
        return new TextFileReader();
    }

    public IWriter CreateWriter()
    {
        return new TextFileWriter();
    }
}
