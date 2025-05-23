using System;

namespace AbstractFactory.Interfaces;

public interface IFileFactory
{
    IReader CreateReader();
    IWriter CreateWriter();
}
