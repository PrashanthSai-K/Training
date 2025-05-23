using System;
using Proxy.Interfaces;
using Proxy.Models;

namespace Proxy;

public class ProxyFile : IFile
{
    private IFile _file;
    public ProxyFile(IFile file)
    {
        _file = file;
    }
    public void Read()
    {
        User user = new User().GetUser();

        if (user.Role == "Admin")
            _file.Read();
        else
            Console.WriteLine("You don't have permission to read this file");
    }

}
