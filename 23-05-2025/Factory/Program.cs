using Factory;
using Factory.FactoryCreator;

public class FactoryDesign
{
    public static void Main(string[] args)
    {
        IFileHandler textFileHandler = new TextFileHandlerCreator().CreateFileHandler();
        textFileHandler.WriteToFile(["Hii", "I am sai"]);
        textFileHandler.ReadFromFile();

        IFileHandler csvFileHandler = new CSVFileHandlerCreator().CreateFileHandler();
        csvFileHandler.WriteToFile(["1, sai, 10,000", "2, hari, 20,000"]);
        csvFileHandler.ReadFromFile();
    }
}