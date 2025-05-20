
using Employee;

class main
{
    public static void Main(string[] args)
    {
        //Easy mode tasks
        EmployeePromotion promotion = new EmployeePromotion();
        promotion.Start();

        //Medium mode tasks
        Medium medium = new Medium();
        medium.Start();

        //Hard mode tasks
        Hard hard = new Hard(medium);
        hard.Start();

        //Dictionary tasks
        DictionarySample dict = new DictionarySample();
        dict.Start();
    }
}
