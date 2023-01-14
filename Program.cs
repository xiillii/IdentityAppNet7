using IdentityApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProductDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:AppDataConnection"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
