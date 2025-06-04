using System;
using ClinicManagement.Context;
using ClinicManagement.Controllers;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClinicTest;

public class DoctorControllerTest
{
    [Test]
    public async Task CreateDoctor()
    {
        //Arrange
        var doctor = new Doctor()
        {
            Name = "sampledoc",
            Email = "sample@gmail.com",
            YearsOfExperience = 10
        };

        Mock<IDoctorService> doctorServiceMock = new Mock<IDoctorService>();
        doctorServiceMock.Setup(ds => ds.AddDoctor(It.IsAny<DoctorAddRequestDto>()))
                         .ReturnsAsync(doctor);

        DoctorController _controller = new DoctorController(doctorServiceMock.Object);

        //Action
        var result = await _controller.CreateDoctor(new DoctorAddRequestDto()
        {
            Name = doctor.Name,
            Email = doctor.Email,
            YearsOfExperience = doctor.YearsOfExperience,
            Password = "12345"
        });
        var okresult = result.Result as OkObjectResult;
        var createdDoctor = okresult.Value as Doctor;

        //Assert
        Assert.That(createdDoctor, Is.Not.Null);
        Assert.That(createdDoctor.Email, Is.EqualTo("sample@gmail.com"));
    }

    [TestCase("General")]
    public async Task GetDoctorBySpeciality(string speciality)
    {
        //Arrange
        var doctor = new DoctorsBySpecialityDto()
        {
            Dname = "sampledoc",
            Yoe = 10
        };

        Mock<IDoctorService> doctorServiceMock = new Mock<IDoctorService>();
        doctorServiceMock.Setup(ds => ds.GetDoctorsBySpeciality(It.IsAny<string>()))
                         .ReturnsAsync(new List<DoctorsBySpecialityDto>
                         {
                            doctor
                         });
        DoctorController _controller = new DoctorController(doctorServiceMock.Object);

        //Action
        var result = await _controller.GetDoctorBySpeciality(speciality);
        var okresult = result.Result as OkObjectResult;
        var doctors = okresult?.Value as List<DoctorsBySpecialityDto>;

        //Assert
        Assert.That(doctors, Is.Not.Null);
        Assert.That(doctors.Count(), Is.EqualTo(1));

    }

}
