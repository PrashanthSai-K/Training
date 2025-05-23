using Proxy;
using Proxy.Interfaces;

public class ProxyDesign
{
    public static void Main(string[] args)
    {
        IFile file = new Files();
        IFile proxyFile = new ProxyFile(file);

        proxyFile.Read();
    }
}