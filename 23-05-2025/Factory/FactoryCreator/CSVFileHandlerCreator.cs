using System;

namespace Factory.FactoryCreator;

public class CSVFileHandlerCreator : FileHandler
{
    public override IFileHandler CreateFileHandler()
    {
        return new CSVFileHandler();
    }
}
