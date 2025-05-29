using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Misc.OtherFunctionalities;
using ClinicManagement.Models;
using ClinicManagement.Repository;
using ClinicManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });

builder.Services.AddDbContext<ClinicDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IRepository<int, Patient>, PatientRepository>();
builder.Services.AddScoped<IRepository<int, Doctor>, DoctorRepository>();
builder.Services.AddScoped<IRepository<int, Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepository<int, Speciality>, SpecialityRepository>();
builder.Services.AddScoped<IRepository<int, DoctorSpeciality>, DoctorSpecialityRepository>();
builder.Services.AddScoped<IOtherContextFunctionalities, OtherContextFunctionalities>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorServiceWithTransaction>();
// builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ISpecialityService, SpecialityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
