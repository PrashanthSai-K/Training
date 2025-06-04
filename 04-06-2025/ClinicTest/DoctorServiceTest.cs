using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Mappers;
using ClinicManagement.Misc.OtherFunctionalities;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using ClinicManagement.Repository;
using ClinicManagement.Services;
using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClinicTest;

public class DoctorServiceTest
{
    private ClinicDBContext _clinicDBContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicDBContext>().UseInMemoryDatabase(databaseName : "TestDbDoctorService").Options;
        _clinicDBContext = new ClinicDBContext(options);
    }

    [TestCase("General")]
    public async Task GetDoctosBySpeciality(string speciality)
    {
        //Arrange
        Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_clinicDBContext);
        Mock<SpecialityRepository> specialityRepositoryMock = new(_clinicDBContext);
        Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_clinicDBContext);
        Mock<UserRepository> userRepositoryMock = new(_clinicDBContext);
        Mock<OtherContextFunctionalities> otherContextFunctionalitiesMock = new(_clinicDBContext);
        Mock<EncryptionService> encryptionServiceMock = new();
        Mock<IMapper> mapperMock = new();

        otherContextFunctionalitiesMock.Setup(ocf => ocf.GetDoctorsBySpeciality(It.IsAny<string>()))
                                   .ReturnsAsync(new List<DoctorsBySpecialityDto>
                                   {
                                    new DoctorsBySpecialityDto()
                                    {
                                        Dname = "test",
                                        Yoe = 3,
                                        Id = 1
                                    }
                                   });

        var doctotService = new DoctorService(
            userRepositoryMock.Object, doctorRepositoryMock.Object,
            specialityRepositoryMock.Object, doctorSpecialityRepositoryMock.Object,
            otherContextFunctionalitiesMock.Object, mapperMock.Object, encryptionServiceMock.Object
        );

        //Action
        var result = await doctotService.GetDoctorsBySpeciality(speciality);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [TestCase(1)]
    public async Task GetDoctorById(int id)
    {
        //Arrange
        Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_clinicDBContext);
        Mock<SpecialityRepository> specialityRepositoryMock = new(_clinicDBContext);
        Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_clinicDBContext);
        Mock<UserRepository> userRepositoryMock = new(_clinicDBContext);
        Mock<OtherContextFunctionalities> otherContextFunctionalitiesMock = new(_clinicDBContext);
        Mock<EncryptionService> encryptionServiceMock = new();
        Mock<IMapper> mapperMock = new();

        var doctor = new Doctor()
        {
            Id = id,
            Name = "sai",
            Email = "sample@gmail.com",
            YearsOfExperience = 3
        };

        doctorRepositoryMock.Setup(dr => dr.GetById(It.IsAny<int>()))
                            .ReturnsAsync(doctor);

        var doctotService = new DoctorService(
            userRepositoryMock.Object, doctorRepositoryMock.Object,
            specialityRepositoryMock.Object, doctorSpecialityRepositoryMock.Object,
            otherContextFunctionalitiesMock.Object, mapperMock.Object, encryptionServiceMock.Object
        );

        var result = await doctotService.GetById(id);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task AddDoctor()
    {
        // Arrange
        var userRepository = new UserRepository(_clinicDBContext);
        var doctorRepository = new DoctorRepository(_clinicDBContext);
        var specialityRepository = new SpecialityRepository(_clinicDBContext);
        var doctorSpecialityRepository = new DoctorSpecialityRepository(_clinicDBContext);

        var encryptionServiceMock = new Mock<IEncryptionService>();
        encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                             .ReturnsAsync(new EncryptModel
                             {
                                 EncryptedData = Encoding.UTF8.GetBytes("encrypted-pass"),
                                 HashKey = Guid.NewGuid().ToByteArray()
                             });

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<DoctorAddRequestDto, User>(It.IsAny<DoctorAddRequestDto>()))
                  .Returns(new User { UserName = "sai@gmail.com", Role = "Doctor", });


        var doctotService = new DoctorService(
            userRepository, doctorRepository,
            specialityRepository, doctorSpecialityRepository,
            new OtherContextFunctionalities(_clinicDBContext), mapperMock.Object, encryptionServiceMock.Object
        );

        var doctorDto = new DoctorAddRequestDto
        {
            Email = "sai@gmail.com",
            Password = "123",
            Name = "sai",
            YearsOfExperience = 3,
        };

        // Act
        var result = await doctotService.AddDoctor(doctorDto);

        // Assert
        Assert.That(result,  Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("sai"));
        Assert.That(result.Email, Is.EqualTo("sai@gmail.com"));
    }


    [TearDown]
    public void CleanUp()
    {
        _clinicDBContext?.Dispose();
    }
}
