using Delegates.Models;

internal class Sample
{
    List<Employee> employees = new List<Employee>()
    {
        new Employee(101,30, "John Doe",  50000),
        new Employee(102, 25,"Jane Smith",  60000),
        new Employee(103,35, "Sam Brown",  70000)
    };

    public delegate void MyDelegate<T>(T num1, T num2);

    public static void Add(int a, int b)
    {
        Console.WriteLine($"The Sum of {a} and {b} is {a + b}");
    }
    public static void Sub(int a, int b)
    {
        Console.WriteLine($"The Difference of {a} and {b} is {a + b}");
    }

    public static void Mul(int a, int b)
    {
        Console.WriteLine($"The Product of {a} and {b} is {a + b}");
    }
    public static void Div(int a, int b)
    {
        Console.WriteLine($"The Quotient of {a} and {b} is {a + b}");
    }

    public static void FindEmployee()
    {
        int EmpId = 102;
        Predicate<Employee> predicate = e => e.Id == EmpId;
        Employee? emp = new Sample().employees.Find(predicate);
        Console.WriteLine("\n ------------Find Employee--------------");
        Console.WriteLine(emp?.ToString() ?? "Employee not found");
    }

    public static void SortEmployee()
    {
        var sorted = new Sample().employees.OrderBy(e => e.Name);
        Console.WriteLine("\n ------------Sorted List--------------");
        Console.WriteLine(string.Join("\n", sorted));
    }

    public static void Main(string[] ars)
    {
        new Sample();
        //Deletgate with generic type
        MyDelegate<int> del = new MyDelegate<int>(Add);
        del += Sub;
        del += Mul;
        del += Div;
        del(10, 20);

        //Action - pre-defined delegate with generic type
        //Lambda expression = anonymous functions
        Action<int, int> ActionDel = (int a, int b) => Console.WriteLine($"The Remainder of {a} and {b} is {a % b}");
        ActionDel(20, 10);

        //Predicate pass function as arguments
        FindEmployee();
        SortEmployee();

        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

        int i = 0;

        var query = numbers.Select(number => ++i).ToList();

        foreach (var value in query)
        {
            Console.WriteLine($"v = {value}, i = {i}");
        }

    }
}