using Application.Interfaces;
using Application.Services.Account.Commands.RegisterUser;
using Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

builder.Services.AddScoped<RegisterUserValidation>();


builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();


// Add Policy Roles
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Enum.GetName(BaseRole.Admin), policy => policy.RequireRole(Enum.GetName(BaseRole.Admin)));
    config.AddPolicy(Enum.GetName(BaseRole.Customer), policy => policy.RequireRole(Enum.GetName(BaseRole.Customer)));
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.AccessDeniedPath = "/Error";
    option.LogoutPath = "/LogOut";
    option.LoginPath = "/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
