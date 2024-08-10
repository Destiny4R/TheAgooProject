using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddRazorPages();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(j => {
    j.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
{
    //Accont Lockout configuration
    Options.Lockout.MaxFailedAccessAttempts = 3;
    //Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(35800);
    // SETTING UP YOUR CUSTOM PASSWORD REQUIREMENTS
    Options.Password.RequiredLength = 0;
    Options.Password.RequiredUniqueChars = 0;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireDigit = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.User.AllowedUserNameCharacters = "/abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    //Email confirmation configuration
    //Options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(a =>
{
    a.LoginPath = $"/Account/Login";
    a.LogoutPath = $"/Account/Logout";
    a.AccessDeniedPath = $"/Account/AccessDenied";
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
