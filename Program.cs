using IdentityApp.Models;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ProductDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:AppDataConnection"]);
});

builder.Services.AddHttpsRedirection(opts =>
{
    opts.HttpsPort = 44350;
});

builder.Services.AddDbContext<IdentityDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"],
        opts => opts.MigrationsAssembly("IdentityApp"));
});

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<IdentityDbContext>();

var emailSenderConfig = builder.Configuration.GetSection("EmailSenderConfiguration").Get<EmailSenderConfiguration>();

builder.Services.AddSingleton(emailSenderConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
