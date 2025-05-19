/*
10) write a program that accepts a 9-element array representing a Sudoku row.

*/
public class Check
{
    public static bool ValidateRow(int[] arr)
    {
        HashSet<int> set = new HashSet<int>();
        for(int i=0;i<arr.Length;i++)
        {
            if(!set.Contains(arr[i]) && arr[i] > 0 && arr[i] < 10)
            {
                set.Add(arr[i]);
            }
        }
        if(set.Count() == 9)
            return true;
        return false;
    }
    public static int GetNumber(string promt)
    {
        int num;
        Console.Write(promt);
        while(!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Enter a valid number.");
        
        return num;
    }
    public static void Main(string[] args)
    {
        int[] arr = new int[9];

        for(int i=0;i<9;i++)
        {
            arr[i] = GetNumber($"Enter number {i+1} : ");
        }

        if(ValidateRow(arr))
            Console.WriteLine("Valid Row");
        else
            Console.WriteLine("Not a valid row");

    }
}