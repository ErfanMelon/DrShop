using Application;
using Application.Interfaces;
using Common;
using Dr_Shop.Models.Filters;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(PageExceptionFilter),-1);
});


string connectionString = builder.Configuration["ConnectionString"];
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// Database Services
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

// Implement MediatR DesignPattern
builder.Services.AddMediatR(typeof(MediatorStartup).Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Auth";
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Error/403";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Enum.GetName(BaseRole.Admin), policy => policy.RequireRole(Enum.GetName(BaseRole.Admin)));
    config.AddPolicy(Enum.GetName(BaseRole.Customer), policy => policy.RequireRole(Enum.GetName(BaseRole.Customer)));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
