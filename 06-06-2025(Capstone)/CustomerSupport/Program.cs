
using System.Text;
using CustomerSupport.Context;
using CustomerSupport.Interfaces;
using CustomerSupport.MessageHub;
using CustomerSupport.Models;
using CustomerSupport.Repositories;
using CustomerSupport.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Chat API", Version = "v1" });
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

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"] ?? ""))
                    };
                });
#endregion


#region CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

#region  Services
builder.Services.AddTransient<IAgentService, AgentService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IHashingService, HashingService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IChatMessageService, ChatMessageService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
#endregion

#region Repositories
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<int, Agent>, AgentRepository>();
builder.Services.AddTransient<IRepository<int, Customer>, CustomerRepository>();
builder.Services.AddTransient<IRepository<int, Chat>, ChatRepository>();
builder.Services.AddTransient<IRepository<int, ChatMessage>, ChatMessagesRepository>();
builder.Services.AddTransient<IRepository<string, Image>, ImageRepository>();
builder.Services.AddTransient<IRepository<int, AuditLog>, AuditLogRepository>();
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddAutoMapper(typeof(Agent));
builder.Services.AddAutoMapper(typeof(Customer));
builder.Services.AddAutoMapper(typeof(Chat));
builder.Services.AddAutoMapper(typeof(ChatMessage));
#endregion

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<ChatHub>("/chathub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.MapControllers();

app.Run();


/*

Endpoints :

post - /api/v1/auth/login
post - api/v1/auth/refresh

post   - /api/v1/agent/register
put    - /api/v1/agent/{id}
delete - /api/v1/agent/{id}
get    - /api/v1/agent?pagination&filter

post   - /api/v1/customer/register
put    - /api/v1/customer/{id}
delete - /api/v1/customer/{id}
get    - /api/v1/customer?pagination&filter

post   - /api/v1/chat/create
delete - /api/v1/chat/{id}
get    - /api/v1/chat?pagination&flter

post   - /api/v1/chat/{chatId}/message/create
put    - /api/v1/chat/{chatId}/message/{id}
delete - /api/v1/chat/{chatId}/message/{id}
get    - /api/v1/chat/{chatId}/message?pagination&filter

post - /api/v1/chat/{chatId}/image/upload
get  - /api/v1/chat/{chatId}/image/{id}/dowmload //individual
get  - /api/v1/chat/{chatId}/image //list

post   - /api/v1/auditlog/create
delete - /api/v1/auditlog/{id}
get    - /api/v1/auditlog?pagination&filter

*/