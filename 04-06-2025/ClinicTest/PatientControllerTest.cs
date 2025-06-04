using System;
using ClinicManagement.Controllers;
using ClinicManagement.Interfaces;
using ClinicManagement.Models;
using ClinicManagement.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClinicTest;

public class PatientControllerTest
{
    [Test]
    public async Task CreatePatients()
    {
        //Arrage
        var patient = new Patient()
        {
            Name = "samplepatient",
            Email = "sample@gmail.com",
            Age = 10,
            Phone = "1234567890"
        };

        Mock<IPatientService> patientServiceMock = new Mock<IPatientService>();
        patientServiceMock.Setup(ps => ps.CreatePatient(It.IsAny<PatientAddRequestDto>()))
                         .ReturnsAsync(patient);

        PatientController _controller = new PatientController(patientServiceMock.Object);

        //Action
        var result = await _controller.CreatePatients(new PatientAddRequestDto()
        {
            Name = "samplepatient",
            Email = "sample@gmail.com",
            Age = 10,
            Phone = "1234567890",
            Password = "12345"
        });
        var okresult = result.Result as OkObjectResult;
        var createdPatient = okresult.Value as Patient;

        //Assert
        Assert.That(createdPatient, Is.Not.Null);
        Assert.That(createdPatient.Email, Is.EqualTo("sample@gmail.com"));
    }

    [Test]
    public async Task GetPatients()
    {
        //Arrange
        var patient = new Patient()
        {
            Name = "samplepatient",
            Email = "sample@gmail.com",
            Age = 10,
            Phone = "1234567890"
        };

        Mock<IPatientService> patientServiceMock = new Mock<IPatientService>();
        patientServiceMock.Setup(ps => ps.GetPatients())
                         .ReturnsAsync(new List<Patient>
                         {
                            patient
                         });
        PatientController _controller = new PatientController(patientServiceMock.Object);

        //Action
        var result = await _controller.GetPatients();
        var okresult = result.Result as OkObjectResult;
        var patients = okresult?.Value as List<Patient>;

        //Assert
        Assert.That(patient, Is.Not.Null);
        Assert.That(patients.Count(), Is.EqualTo(1));

    }

}
