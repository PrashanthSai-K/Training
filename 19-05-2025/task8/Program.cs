// 8) Given two integer arrays, merge them into a single array.

public class Merge
{
    public static void MergeTwoArray(int[] arr1, int[] arr2, int[] merge)
    {
        for(int i=0;i<arr1.Length;i++)
        {
            merge[i] = arr1[i];
        }

        for(int i=0;i<arr2.Length;i++)
        {
            merge[arr1.Length + i] = arr2[i];
        }

    }
    public static int GetInputNumber(string promt)
    {
        int num;
        Console.Write($"{promt} : ");
        while (!int.TryParse(Console.ReadLine(), out num))
            Console.WriteLine("Please enter a valid number.");

        return num;
    }

    public static void Main(string[] args)
    {

        int n1 = GetInputNumber("Please enter length of the first array");

        int[] arr1 = new int[n1];
        for (int i = 0; i < n1; i++)
        {
            arr1[i] = GetInputNumber($"Enter number {i + 1}");
        }

        int n2 = GetInputNumber("Please enter length of the second array");

        int[] arr2 = new int[n2];
        for (int i = 0; i < n2; i++)
        {
            arr2[i] = GetInputNumber($"Enter number {i + 1}");
        }

        int[] merge = new int[n1+n2];

        MergeTwoArray(arr1, arr2, merge);

        System.Console.Write($"Merged Array : {string.Join(", ", merge)}");
    }
}