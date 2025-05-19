/*
12) Write a program that:

Takes a message string as input (only lowercase letters, no spaces or symbols).

Encrypts it by shifting each character forward by 3 places in the alphabet.

Decrypts it back to the original message by shifting backward by 3.

*/

using System.Text;

public class Encrypt
{
    public static string EncryptMessage(string str)
    {
        StringBuilder strB = new StringBuilder();

        for(int i=0;i<str.Length;i++)
        {
            strB.Append((char)(((str[i] - 'a' + 3) % 26 ) + 'a'));
        }
        return strB.ToString();
    }
        public static string DecryptMessage(string str)
    {
        StringBuilder strB = new StringBuilder();

        for(int i=0;i<str.Length;i++)
        {
            strB.Append((char)(((str[i] - 'a' - 3 + 26) % 26) + 'a'));
        }
        return strB.ToString();
    }

    public static string GetString(string promt)
    {
        Console.WriteLine(promt);
        string a;
        while(true)
        {
            a = (Console.ReadLine() ?? "").Trim();
            if(string.IsNullOrEmpty(a))
            {
                Console.WriteLine("Enter a valid string");
                continue;
            }
            bool valid = true;
            for(int i=0;i<a.Length;i++)
            {
                if(a[i] < 'a' || a[i] > 'z')
                {
                    valid = false;
                    break;
                }
            }
            if(valid)
                break;
        }
        return a;
    }
    public static void Main(string[] args)
    {
        string str = GetString("Please enter a string");
        string enc = EncryptMessage(str);
        string dec = DecryptMessage(enc);

        Console.WriteLine($"Encoded : {enc}, Decoded : {dec}");
    }
}