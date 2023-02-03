using IdentityApp;
using IdentityApp.Models;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");

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

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddScoped<TokenUrlEncoderService>();
builder.Services.AddScoped<IdentityEmailService>();

builder.Services.AddAuthentication()
    .AddFacebook(opts =>
    {
        opts.AppId = builder.Configuration["Facebook:AppId"];
        opts.AppSecret = builder.Configuration["Facebook:AppSecret"];
    })
    .AddGoogle(opts =>
    {
        opts.ClientId = builder.Configuration["Google:ClientId"];
        opts.ClientSecret = builder.Configuration["Google:ClientSecret"];
    })
    .AddTwitter(opts =>
    {
        opts.ConsumerKey = builder.Configuration["Twitter:ApiKey"];
        opts.ConsumerSecret = builder.Configuration["Twitter:ApiSecret"];
    });

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Identity/SignIn";
    opts.LogoutPath = "/Identity/SignOut";
    opts.AccessDeniedPath = "/Identity/Forbidden";
});

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

app.SeedUserStoreForDashboard();

app.Run();
