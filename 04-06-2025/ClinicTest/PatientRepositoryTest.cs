using System;
using System.Text;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Repository;
using Microsoft.EntityFrameworkCore;

namespace ClinicTest;

public class PatientRepositoryTest
{
    private ClinicDBContext _clinicDBContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicDBContext>().UseInMemoryDatabase(databaseName: "TestDbPatientRepo").Options;
        _clinicDBContext = new ClinicDBContext(options);
    }

    [Test]
    public async Task AddPatient()
    {
        //arrange 
        var Email = "patient@gmail.com";
        var Password = Encoding.UTF8.GetBytes("test1293");
        var Role = "Patient";
        var HashKey = Guid.NewGuid().ToByteArray();

        var user = new User()
        {
            UserName = Email,
            Password = Password,
            Role = Role,
            HashKey = HashKey
        };
        var patient = new Patient()
        {
            Name = "patient1",
            Email = Email,
            Age = 10,
            Phone = "1234567890"
        };
        IRepository<int, Patient> _repository = new PatientRepository(_clinicDBContext);

        //action
        _clinicDBContext.Add(user);
        await _clinicDBContext.SaveChangesAsync();

        var result = await _repository.Create(patient);

        //assert
        Assert.That(result, Is.Not.Null, "Patient not added");
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [TestCase(1)]
    public async Task UpdatePatient(int id)
    {
        //Arrange 
        IRepository<int, Patient> _repository = new PatientRepository(_clinicDBContext);

        //Action
        var patient = new Patient()
        {
            Id = id,
            Name = "patient2",
            Email = "patient2@gmail.com",
            Age = 20,
            Phone = "0987654321"
        };

        var result = await _repository.Update(id, patient);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Email, Is.EqualTo("patient2@gmail.com"));
    }


    [Test]
    public async Task GetPatientById()
    {
        var email = "patient1@example.com";
        var patient = new Patient { Name = "Test", Email = email, Age = 20, Phone = "9999999999" };
        IRepository<int, Patient> _repository = new PatientRepository(_clinicDBContext);

        var createdPatient = await _repository.Create(patient);

        // Act
        var result = await _repository.GetById(createdPatient.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(createdPatient.Id));
    }

    [Test]
    public async Task GetPatients()
    {
        //Arrange 
        IRepository<int, Patient> _repository = new PatientRepository(_clinicDBContext);

        //Action
        var result = await _repository.GetAll();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    // [TestCase(1)]
    // public async Task DeletePatient(int id)
    // {
    //     //Arrange 
    //     IRepository<int, Patient> _repository = new PatientRepository(_clinicDBContext);

    //     //Action
    //     var result = _repository.Delete(id);

    //     //Assert
    //     Assert.That(result, Is.Not.Null);
    //     Assert.That(result.Id, Is.EqualTo(id));
    // }


    [TearDown]
    public void CleanUp()
    {
        _clinicDBContext?.Dispose();
    }

}
