using Singleton;

public class Filehandler
{
    public static void Main(string[] args)
    {
        FileHandler fileHandler1 = FileHandler.GetFileHandler();

        fileHandler1.WriteToFile(["hi", "i am sai"]);
        fileHandler1.ReadFromFile();


        FileHandler fileHandler2 = FileHandler.GetFileHandler();

        fileHandler2.AppendToFile(["i have completed engineering"]);
        fileHandler2.ReadFromFile();
    }
}