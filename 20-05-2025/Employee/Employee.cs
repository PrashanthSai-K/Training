namespace Employee;

public class Employee : IComparable<Employee>
{
    int id, age;

    string name;

    double salary;

    public Employee()

    {

    }

    public Employee(int id, int age, string name, double salary)

    {

        this.id = id;

        this.age = age;

        this.name = name;

        this.salary = salary;

    }


    public int CompareTo(Employee other)
    {
        return this.Salary.CompareTo(other.salary);
    }

        public static int GetInputAge(string promt)
    {
        int num;
        Console.Write(promt);
        while (!int.TryParse(Console.ReadLine(), out num) || num < 18)
            Console.WriteLine("Please enter a valid age.");

        return num;
    }

    public static double GetInputSalary(string promt)
    {
        double num;
        Console.Write(promt);
        while (!double.TryParse(Console.ReadLine(), out num) || num < 0)
            Console.WriteLine("Please enter a valid Salary.");

        return num;
    }
    public static string GetInputString(string promt)
    {
        string UserInput;
        while (true)
        {
            Console.Write(promt);
            UserInput = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrEmpty(UserInput))
            {
                Console.WriteLine("Please Enter a valid input.");
            }
            else
            {
                return UserInput;
            }
        }
    }


    public void TakeEmployeeDetailsFromUser(bool edit = false)
    {

        if(!edit)
        {
            Console.WriteLine("Please enter the employee ID");

            id = Convert.ToInt32(Console.ReadLine());
        }

        name = GetInputString("Please enter the employee name : ");
        
        age = GetInputAge("Please enter the employee age : ");

        salary = GetInputSalary("Please enter the employee salary : ");

    }

    public override string ToString()

    {
        return "Employee ID : " + id + "\nName : " + name + "\nAge : " + age + "\nSalary : " + salary;
    }

    public int Id { get => id; set => id = value; }

    public int Age { get => age; set => age = value; }

    public string Name { get => name; set => name = value; }

    public double Salary { get => salary; set => salary = value; }

}
