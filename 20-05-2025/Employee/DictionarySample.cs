using System;

namespace Employee;

public class DictionarySample
{
    Dictionary<string, double> products = new Dictionary<string, double>();

    public void AddProducts()
    {
       products.Add("Laptop", 34000);
       products.Add("Mobile", 15000);
       products.Add("Earphones", 3000);
       products.Add("Tablet", 10000);
       products.Add("Airpods", 12000); 
    }

    public void ViewProducts()
    {
        foreach(var pair in products)
        {
            Console.WriteLine($"Product Name : {pair.Key}, Product Price : {pair.Value}");
        }
    }

    public void FindProduct(string name)
    {
        if(products.ContainsKey(name))
        {
            products.TryGetValue(name, out double price);            
            Console.WriteLine($"Product {name} is priced at {price}");
        }else
        {
            Console.WriteLine($"Product {name} not found");
        }
    }

    public void Start()
    {
        AddProducts();
        ViewProducts();
        Console.Write("Enter product name : ");
        string name = Console.ReadLine();
        FindProduct(name);
    }
}
