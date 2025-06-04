using System.Text;
using ClinicManagement.Auth;
using ClinicManagement.Context;
using ClinicManagement.Interfaces;
using ClinicManagement.Misc.OtherFunctionalities;
using ClinicManagement.Models;
using ClinicManagement.Repository;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

builder.Logging.AddLog4Net();

#region Repositoy
builder.Services.AddScoped<IRepository<int, Patient>, PatientRepository>();
builder.Services.AddScoped<IRepository<int, Doctor>, DoctorRepository>();
builder.Services.AddScoped<IRepository<string, User>, UserRepository>();
builder.Services.AddScoped<IRepository<int, Appointment>, AppointmentRepository>();
builder.Services.AddScoped<IRepository<int, Speciality>, SpecialityRepository>();
builder.Services.AddScoped<IRepository<int, DoctorSpeciality>, DoctorSpecialityRepository>();
builder.Services.AddScoped<IOtherContextFunctionalities, OtherContextFunctionalities>();
#endregion

#region Service
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ISpecialityService, SpecialityService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
#endregion

#region AuthenticationFilter
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                });

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(opts =>
{
    opts.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    opts.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    opts.CallbackPath = "/signin-google";
});
#endregion

#region AuthorizationPolicy
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("ExperiencedDoctor", policy =>
    {
        policy.Requirements.Add(new MinimumExperienceRequirement(3));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, MinimumExperienceHandler>();
#endregion

#region misc
builder.Services.AddAutoMapper(typeof(User));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();