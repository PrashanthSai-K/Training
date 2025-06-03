using System.Text;
using System.Threading.Tasks;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Repository;
using Microsoft.EntityFrameworkCore;

namespace ClinicTest;

public class Tests
{
    private ClinicDBContext _clinicDBContext;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicDBContext>().UseInMemoryDatabase("TestDb").Options;
        _clinicDBContext = new ClinicDBContext(options);
    }

    [Test]
    public async Task Test1()
    {
        //arrange 
        var Email = "test@gmail.com";
        var Password = Encoding.UTF8.GetBytes("test1293");
        var Role = "Doctor";
        var HashKey = Guid.NewGuid().ToByteArray();

        var user = new User()
        {
            UserName = Email,
            Password = Password,
            Role = Role,
            HashKey = HashKey
        };
        var Doctor = new Doctor()
        {
            Name = "Test",
            Email = Email,
            YearsOfExperience = 10
        };
        IRepository<int, Doctor> _repository = new DoctorRepository(_clinicDBContext);

        //action
        _clinicDBContext.Add(user);
        await _clinicDBContext.SaveChangesAsync();

        var result = await _repository.Create(Doctor);
        
        //assert
        Assert.That(result, Is.Not.Null, "Doctor not added");
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [TearDown]
    public void CleanUp()
    {
        _clinicDBContext?.Dispose();
    }
}
