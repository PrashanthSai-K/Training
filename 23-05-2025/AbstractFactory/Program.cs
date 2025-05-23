using AbstractFactory.Factory;
using AbstractFactory.Interfaces;

public class AbstractFactoryDesign
{
    public static void Main(string[] args)
    {
        IFileFactory textFileFactory = new TextFileFactory();
        IWriter textFileWriter = textFileFactory.CreateWriter();
        IReader textFileReader = textFileFactory.CreateReader();
        textFileWriter.WriteToFile("./sample.txt", ["Hi", "I am sai"]);
        textFileReader.ReadFromFile("./sample.txt");

        IFileFactory csvFileFactory = new CSVFileFactory();
        IWriter csvFileWriter = csvFileFactory.CreateWriter();
        IReader csvFileReader = csvFileFactory.CreateReader();
        csvFileWriter.WriteToFile("./sample.csv", ["1, sai, 10,000", "2, hari, 20,000"]);
        csvFileReader.ReadFromFile("./sample.csv");

    }   
}