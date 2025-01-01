using Virtue.UserService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building the app.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Load connection string
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Using Connection String: {connectionString}");

// Add DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add controllers, Swagger, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();
