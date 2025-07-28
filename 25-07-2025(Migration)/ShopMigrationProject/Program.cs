
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Controller
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });
#endregion

builder.Services.AddDbContext<ChienVHShopDBEntities>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorsService, ColorService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IOrderService, OrdersService>();
builder.Services.AddScoped<IProductsService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();


// builder.Services.AddScoped<IChatService, FaqChatService>();

builder.Services.AddAutoMapper(typeof(Category));
builder.Services.AddAutoMapper(typeof(Color));
builder.Services.AddAutoMapper(typeof(Order));
builder.Services.AddAutoMapper(typeof(Product));
builder.Services.AddAutoMapper(typeof(Model));
builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddAutoMapper(typeof(News));


#region CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:53579")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

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
