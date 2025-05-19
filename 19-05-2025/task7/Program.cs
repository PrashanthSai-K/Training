// 7) create a program to rotate the array to the left by one position.

public class Rotate
{   
    public static void RotateLeftByOne(int[] arr)
    {
        if(arr == null || arr.Length <= 1)
            return;
        int first = arr[0];

        for(int i=0;i<arr.Length-1;i++)
        {
            arr[i] = arr[i+1];
        }
        arr[arr.Length-1] = first;
    }

    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write($"{promt} : ");
        while(!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");
        
        return num;
    }



    public static void Main(string[] args)
    {
        int n = GetInputNumber("Please enter length of array");

        int[] arr = new int[n];
        for(int i=0;i<n;i++)
        {
            arr[i] = GetInputNumber($"Enter number {i+1}");
        }

        RotateLeftByOne(arr);

        Console.WriteLine("Rotated Array : ");
        foreach(int num in arr)
        {
            Console.Write($" {num} ");
        }

    }
}