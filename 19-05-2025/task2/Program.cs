// 2) Take 2 numbers from user and print the largest

public class findLargest 
{
    public static int FindLargest(int a, int b)
    {
        if(a > b)
            return a;
        else
            return b;
    }
    public static void Main(string[] args)
    {
        Console.Write($"Enter your number 1 : ");
        int num1 = Int32.Parse(Console.ReadLine());
        Console.Write($"Enter your number 2 : ");
        int num2 = Int32.Parse(Console.ReadLine());
        Console.WriteLine($"Largest of two : {FindLargest(num1, num2)}");
    }

}