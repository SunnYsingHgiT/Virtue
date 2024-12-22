using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Google.Apis.Auth;
using Virtue.Web.Models;
using Virtue.Web.Services;
using Virtue.Web.Helper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Firebase settings and FirebaseAuthService
FirestoreHelper.SetEnvironmentVariable();

//var firebaseSettings = builder.Configuration.GetSection("FirebaseSettings").Get<FirebaseSettings>();
//builder.Services.AddSingleton(new FirebaseAuthService(firebaseSettings));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/Error"); // Updated to point to AuthController
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure the authentication and authorization middleware
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
