using System;

namespace Proxy.Models;

public class User
{
    public string? UserName { get; set; }
    public string? Role { get; set; }

    public User GetUser()
    {
        User user = new User();
        Console.Write("Enter Username : ");
        user.UserName = Console.ReadLine() ?? "";
        Console.Write("Enter User Role : ");
        user.Role = Console.ReadLine() ?? "";
        return user;
    }
}
