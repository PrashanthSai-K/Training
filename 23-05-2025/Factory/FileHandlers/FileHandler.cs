using System;

namespace Factory;

public abstract class FileHandler
{
    public abstract IFileHandler CreateFileHandler();
}
