using IdentityRazorDemo.Data;
using IdentityRazorDemo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(op =>
{
    
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    options.UseSqlServer(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// AddIdentity 里面有执行配置
//builder.Services.AddAuthentication(op =>
//{
//    op.DefaultScheme = IdentityConstants.ApplicationScheme;
//    op.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//}).AddIdentityCookies(op => { });

// 修改后会直接修改 ApplicationDbContextModelSnapshot 
// 但是 Add-Migration 不会有新字段，所以要手动添加。
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;


    // 用户注册配置
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

    // 注册密码配置
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 4;

    // 登录配置
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    //options.Cookie.Name = "YourAppCookieName";
    //options.Cookie.HttpOnly = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    //options.LoginPath = "/Identity/Account/Login";
    //// ReturnUrlParameter requires 
    ////using Microsoft.AspNetCore.Authentication.Cookies;
    //options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    //options.SlidingExpiration = true;
});

builder.Services.Configure<PasswordHasherOptions>(option =>
{
    option.IterationCount = 12000;
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
