// 1) create a program that will take name from user and greet the user

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.Write($"Enter your name : ");
        string name = Console.ReadLine() ?? "";
        if(string.IsNullOrEmpty(name))
            Console.WriteLine("\nEnter a vaid name.");
        else
            Console.WriteLine($"Good Morning!! {name}");
    }
}
