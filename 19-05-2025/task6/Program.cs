//6) Count the Frequency of Each Element

public class Frequency
{
    
    public static int FindFrequency(int n, int[] arr)
    {
        int count = 0;
        for(int i=0;i<arr.Length;i++)
        {
            if(arr[i] == n)
                count++;
        }
        return count;
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
        for(int i=00;i<n;i++)
        {
            arr[i] = GetInputNumber($"Enter number {i+1}");
        }

        HashSet<int> printed = new HashSet<int>();

        for(int i=0;i<n;i++)
        {   
            if(!printed.Contains(arr[i]))
            {
                Console.WriteLine($"{arr[i]} occurs {FindFrequency(arr[i], arr)} times");
                printed.Add(arr[i]);
            }
        }
    }
}