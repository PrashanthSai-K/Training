/*
11) In the question ten extend it to validate a sudoku game. 
Validate all 9 rows (use int[,] board = new int[9,9])
*/

public class Check
{
    public static bool ValidateRow(int[] row)
    {
        HashSet<int> set = new HashSet<int>();
        for (int i = 0; i < row.Length; i++)
        {
            if (!set.Contains(row[i]) && row[i] > 0 && row[i] < 10)
            {
                set.Add(row[i]);
            }
        }
        if (set.Count() == 9)
            return true;
        return false;
    }
    public static bool ValidateColumn(int[,] col, int colIndex)
    {
        HashSet<int> set = new HashSet<int>();
        for (int i = 0; i < 9; i++)
        {
            int num = col[i, colIndex];
            if (num < 1 || num > 9 || !set.Add(num))
                return false;
        }
        return true;
    }
    public static bool ValidateBox(int[,] box, int startRow, int startCol)
    {
        HashSet<int> set = new HashSet<int>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int num = box[startRow + i, startCol + j];
                if (num < 1 || num > 9 || !set.Add(num))
                    return false;
            }
        }
        return true;
    }
    public static bool ValidateSudoku(int[,] arr)
    {
        // Validate Rows
        for (int i = 0; i < 9; i++)
        {
            int[] row = new int[9];

            for (int j = 0; j < 9; j++)
            {
                row[j] = arr[i, j];
            }
            if (!ValidateRow(row))
                return false;
        }
        //Validate Columns
        for (int j = 0; j < 9; j++)
        {
            if (!ValidateColumn(arr, j))
                return false;
        }
        //Validate Square
        for (int i = 0; i < 9; i += 3)
        {
            for (int j = 0; j < 9; j += 3)
            {
                if (!ValidateBox(arr, i, j))
                    return false;
            }
        }

        return true;
    }
    public static int[] GetRowInput(string promt)
    {
        int[] row = new int[9]; ;
        Console.Write(promt);
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Enter a valid input.");
                continue;
            }
            string[] numbers = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (numbers.Length != 9)
            {
                Console.WriteLine("Enter a valid input.");
                continue;
            }
            bool valid = true;
            for (int i = 0; i < 9; i++)
            {
                if (int.TryParse(numbers[i], out int number) && number >= 1 && number <= 9)
                    row[i] = number;
                else
                {
                    Console.WriteLine("Enter a valid input.");
                    valid = false;
                }
            }
            if (valid)
                return row;
        }
    }
    public static void Main(string[] args)
    {
        int[,] arr = new int[9, 9];

        for (int i = 0; i < 9; i++)
        {
            int[] row = GetRowInput($"Enter row {i + 1} : ");
            for (int j = 0; j < 9; j++)
            {
                arr[i, j] = row[j];
            }
        }

        if (ValidateSudoku(arr))
            Console.WriteLine("Valid Sudoku");
        else
            Console.WriteLine("Not a Valid Sudoku");
    }
}


/*
    7 9 2 1 5 4 3 8 6 
    6 4 3 8 2 7 1 5 9
    8 5 1 3 9 6 7 2 4
    2 6 5 9 7 3 8 4 1
    4 8 9 5 6 1 2 7 3
    3 1 7 4 8 2 9 6 5
    1 3 6 7 4 8 5 9 2
    9 7 4 2 1 5 6 3 8
    5 2 8 6 3 9 4 1 7

*/