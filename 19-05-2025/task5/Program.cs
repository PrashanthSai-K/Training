// 5) Take 10 numbers from user and print the number of numbers that are divisible by 7	

public class IsDivisible
{
    public static void IsDivisibleBySeven(int[] arr)
    {
        Console.Write("Numbers divisible by 7 are :");
        for (int i = 0; i < 7; i++)
        {
            if (arr[i] % 7 == 0)
                Console.Write($" {arr[i]} ");
        }
    }

    public static int GetInputNumber()
    {
        int num;

        while(!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");
        
        return num;
    }
    public static void Main(String[] args)
    {
        int[] arr = new int[7];

        for (int i = 0; i < 7; i++)
        {
            Console.Write($"Please Enter number {i + 1} : ");
            arr[i] = GetInputNumber();
        }
        IsDivisibleBySeven(arr);
    }

}