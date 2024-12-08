using GymManagement.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>() // Ensure roles are included
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


builder.Services.AddTransient<IEmailSender, NoOpEmailSender>();  // No-Op Email Sender

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed roles and admin user AFTER the app has run
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRolesAndAdminUser(services);
}

app.Run();

static async Task CreateRolesAndAdminUser(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin" ,"Receptionist"};
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminUser = new ApplicationUser
    {
        UserName = "admin@admin.com",
        Email = "admin@admin.com",
        EmailConfirmed = true
    };

    string adminPassword = "Admin@123";
    var user = await userManager.FindByEmailAsync("admin@admin.com");

    if (user == null)
    {
        var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
        if (createAdminUser.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// No-Op Email Sender implementation
public class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Log or do nothing
        return Task.CompletedTask;
    }
}

