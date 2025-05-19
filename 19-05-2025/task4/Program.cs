// 4) Take username and password from user. Check if user name is "Admin" and password is "pass" if yes then print success message.

public class Login
{
    public static bool CheckCredentials(string username, string password)
    {
        if (username == "Admin" && password == "pass")
            return true;
        return false;
    }

    public static void Main(string[] args)
    {
        int counter = 0;
        while (counter++ < 3)
        {
            Console.WriteLine($"Attempt {counter}");

            Console.Write("Enter username : ");
            string username = Console.ReadLine();
            Console.Write("Enter password : ");
            string password = Console.ReadLine();
            if (CheckCredentials(username, password))
            {
                Console.WriteLine("Login successfull!!");
                return;
            }

            if (counter == 3)
            {
                Console.WriteLine("Invalid attempts for 3 times. Exiting....");
            }
        }
    }

}