using Azure.Storage.Blobs;
using FileHandling.Context;
using FileHandling.Interface;
using FileHandling.Interfaces;
using FileHandling.Models;
using FileHandling.Repository;
using FileHandling.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FileHandlingContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration["Azure:StorageString"];
    return new BlobServiceClient(connectionString);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IRepository<int, UploadedFile>, UploadFilesRepository>();

builder.Services.AddScoped<IFileHandler, FileHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.MapControllers();

app.Run();
