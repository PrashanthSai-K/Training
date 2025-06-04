using System;
using System.Text;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Repository;
using Microsoft.EntityFrameworkCore;

namespace ClinicTest;

public class DoctorRepositoryTest
{
    private ClinicDBContext _clinicDBContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicDBContext>().UseInMemoryDatabase(databaseName : "TestDbDoctorRepo").Options;
        _clinicDBContext = new ClinicDBContext(options);
    }

    [Test]
    public async Task AddDoctor()
    {
        //arrange 
        var Email = "test1@gmail.com";
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

    [TestCase(1)]
    public async Task UpdateDoctor(int id)
    {
        //Arrange 
        IRepository<int, Doctor> _repository = new DoctorRepository(_clinicDBContext);

        //Action
        var Doctor = new Doctor()
        {
            Id = id,
            Name = "Test",
            Email = "test2@gmail.com",
            YearsOfExperience = 8
        };

        var result = await _repository.Update(id, Doctor);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Email, Is.EqualTo("test2@gmail.com"));
    }


    [Test]
    public async Task GetDoctorById()
    {
        //Arrange 
        var email = "doctor1@example.com";
        var doctor = new Doctor { Name = "Doc", Email = email, YearsOfExperience=10 };
        IRepository<int, Doctor> _repository = new DoctorRepository(_clinicDBContext);

        var createdDoctor = await _repository.Create(doctor);

        //Action
        var result = await _repository.GetById(createdDoctor.Id);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(createdDoctor.Id));
    }

    [Test]
    public async Task GetDoctors()
    {
        //Arrange 
        IRepository<int, Doctor> _repository = new DoctorRepository(_clinicDBContext);

        //Action
        var result = await _repository.GetAll();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    // [TestCase(1)]
    // public async Task DeleteDoctor(int id)
    // {
    //     //Arrange 
    //     IRepository<int, Doctor> _repository = new DoctorRepository(_clinicDBContext);

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
