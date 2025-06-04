using System;
using System.Text;
using AutoMapper;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Migrations;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using ClinicManagement.Repository;
using ClinicManagement.Services;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClinicTest;

public class PatientServiceTest
{
    private ClinicDBContext _clinicDBContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicDBContext>().UseInMemoryDatabase(databaseName : "TestDbPatientService").Options;
        _clinicDBContext = new ClinicDBContext(options);
    }

    [Test]
    public async Task CreatePatient()
    {
        //Arrange
        var patientRepository = new PatientRepository(_clinicDBContext);
        var userRepository = new UserRepository(_clinicDBContext);
        var mapperMock = new Mock<IMapper>();
        var encryptionServiceMock = new Mock<IEncryptionService>();

        encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                             .ReturnsAsync(new EncryptModel
                             {
                                 EncryptedData = Encoding.UTF8.GetBytes("encrypted-pass"),
                                 HashKey = Guid.NewGuid().ToByteArray()
                             });

        mapperMock.Setup(m => m.Map<PatientAddRequestDto, User>(It.IsAny<PatientAddRequestDto>()))
                  .Returns(new User { UserName = "hari@gmail.com", Role = "Patient", });

        var patient = new PatientAddRequestDto()
        {
            Email = "hari@gmail.com",
            Password = "123",
            Name = "hari",
            Age = 20,
            Phone = "12345678909"
        };

        var patientService = new PatientService(patientRepository, userRepository, mapperMock.Object, encryptionServiceMock.Object);

        //Action
        var result = await patientService.CreatePatient(patient);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Email, Is.EqualTo("hari@gmail.com"));
    }

    [Test]
    public async Task GetPatients()
    {
        //Arrange
        var patientRepository = new PatientRepository(_clinicDBContext);
        var userRepository = new UserRepository(_clinicDBContext);
        var mapperMock = new Mock<IMapper>();
        var encryptionServiceMock = new Mock<IEncryptionService>();

        var patientService = new PatientService(patientRepository, userRepository, mapperMock.Object, encryptionServiceMock.Object);

        //Action
        var result = await patientService.GetPatients();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [TestCase(1)]
    public async Task GetPatientById(int id)
    {
        //Arrange
        var patientRepository = new PatientRepository(_clinicDBContext);
        var userRepository = new UserRepository(_clinicDBContext);
        var mapperMock = new Mock<IMapper>();
        var encryptionServiceMock = new Mock<IEncryptionService>();

        var patientService = new PatientService(patientRepository, userRepository, mapperMock.Object, encryptionServiceMock.Object);

        //Action
        var result = await patientService.GetById(id);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [TestCase(2)]
    public async Task DeletePatient(int id)
    {
        //Arrange
        var patientRepository = new PatientRepository(_clinicDBContext);
        var userRepository = new UserRepository(_clinicDBContext);
        var mapperMock = new Mock<IMapper>();
        var encryptionServiceMock = new Mock<IEncryptionService>();

        var patientService = new PatientService(patientRepository, userRepository, mapperMock.Object, encryptionServiceMock.Object);

        //Assert
        Assert.ThrowsAsync<Exception>(()=>  patientService.DeletePatient(id));
    }

    [TearDown]
    public void CleanUp()
    {
        _clinicDBContext?.Dispose();
    }
}
