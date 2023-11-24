using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.AddTransient<ColorService>();
builder.Services.AddTransient<StartupOperations>();
builder.Services.AddTransient<UserPreferencesService>();
builder.Services.AddTransient<LibraryService>();

var app = builder.Build();

// Prepare default roles
var scope = app.Services.CreateScope();
var startup = scope.ServiceProvider.GetService<StartupOperations>();
if (startup != null)
{
    await startup.CreateRoles();
    await startup.CreateUser();
    await startup.SeedData();
    
}
    

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