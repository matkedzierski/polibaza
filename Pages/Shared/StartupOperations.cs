using Microsoft.AspNetCore.Identity;

namespace PoliBaza.Pages.Shared;

public class StartupOperations
{
    private readonly ILogger<StartupOperations> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly LibraryService _lib;
    public StartupOperations(RoleManager<IdentityRole> roleManager, 
        ILogger<StartupOperations> logger,
        LibraryService libService,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
        _lib = libService;
    }
    
    public async Task CreateUser()
    {
        var name = DefaultAdminData.UserName;
        
        var user = await _userManager.FindByNameAsync(name);
        if (user != null)
        {
            _logger.LogInformation("User {name} already exists", name);
            await AddUserToRole(user);
        }
        else
        {
            var newUser = new IdentityUser(name)
            {
                Email = name,
                EmailConfirmed = true,
                PhoneNumber = "111222333"
            };
        
            await _userManager.CreateAsync(newUser);
            _logger.LogInformation("User {name} created successfully", name);
            user = newUser;
        }

        const string pass = DefaultAdminData.Password;
        var hash = new PasswordHasher<IdentityUser>().HashPassword(user, pass);
        user.PasswordHash = hash;
        await _userManager.UpdateAsync(user);
        _logger.LogInformation("Admin password reverted to default");
        
        await AddUserToRole(user);
    }

    private async Task AddUserToRole(IdentityUser user)
    {
        if (await _userManager.IsInRoleAsync(user, RoleNames.ADMIN))
        {
            _logger.LogInformation("User {name} is already in role {role}", user.Email, RoleNames.ADMIN);
            return;
        }
        await _userManager.AddToRoleAsync(user, RoleNames.ADMIN);
    }

    public async Task CreateRoles()
    {
        foreach (var role in RoleNames.GetAll())
        {
            await CreateRole(role);
        }
    }

    private async Task CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            _logger.LogInformation("Role {roleName} created", roleName);
        }
        else
        {
            _logger.LogInformation("Role {roleName} already exists", roleName);
        }
    }

    /*public async Task SeedData()
    {
        _db
    }   */
    
}