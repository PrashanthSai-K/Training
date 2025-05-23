using System;

namespace Factory.FactoryCreator;

public class TextFileHandlerCreator : FileHandler
{
    public override IFileHandler CreateFileHandler()
    {
        return new TextFileHandler();
    }
}
