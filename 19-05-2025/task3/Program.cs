//3) Take 2 numbers from user, check the operation user wants to perform (+,-,*,/). Do the operation and print the result

public class Calculate
{
    public static void Calc(int a, int b, string op)
    {
        switch (op)
        {
            case "+":
                Console.WriteLine($"Sum of two : {a + b}");
                break;
            case "-":
                Console.WriteLine($"Sub of two : {a - b}");
                break;
            case "*":
                Console.WriteLine($"Product of two : {a * b}");
                break;
            case "/":
                Console.WriteLine($"Division of two : {a / b}");
                break;
            default:
                Console.WriteLine($"Enter a valid operation");
                break;
        }
    }
    public static void Main(string[] args)
    {
        Console.Write($"Enter your number 1 : ");
        int num1 = Int32.Parse(Console.ReadLine());
        Console.Write($"Enter your number 2 : ");
        int num2 = Int32.Parse(Console.ReadLine());
        Console.Write($"Enter any operation (+, -, *, /) : ");
        string op = Console.ReadLine();
        Calc(num1, num2, op);
    }

}