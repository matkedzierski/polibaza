using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages
{
    [Authorize(Roles = RoleNames.ADMIN)]
    public class AdminModel : PageModel
    {
        private readonly ILogger<LibraryModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserPreferencesService _prefsService;
        public readonly IQueryable<IdentityUser> Users;
        [TempData] public string StatusMessage { get; set; } = "";

        public AdminModel(ILogger<LibraryModel> logger,
            UserPreferencesService prefsService,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _prefsService = prefsService;
            Users = _userManager.Users;
        }

        public async Task<IActionResult> OnPostDelete(IFormCollection data)
        {
            var userId = data["userId"];
            var user = await _userManager.FindByIdAsync(userId);
            await _prefsService.DeleteUserPreferences(user);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.DeleteAsync(user);
            StatusMessage = $"Użytkownik {user.Email} został usunięty.";
            return Page();
        }


        public async Task<IActionResult> OnPostPromote(IFormCollection data)
        {
            var userId = data["userId"];
            var user = await _userManager.FindByIdAsync(userId);
            
            if (await _userManager.IsInRoleAsync(user, RoleNames.USER))
                await _userManager.RemoveFromRoleAsync(user, RoleNames.USER);
            await _userManager.AddToRoleAsync(user, RoleNames.ADMIN);
            
            StatusMessage = $"Użytkownik {user.Email} został awansowany na administratora.";
            return Page();
        }

        public async Task<IActionResult> OnPostDemote(IFormCollection data)
        {
            var userId = data["userId"];
            var user = await _userManager.FindByIdAsync(userId);
            
            if (await _userManager.IsInRoleAsync(user, RoleNames.ADMIN))
                await _userManager.RemoveFromRoleAsync(user, RoleNames.ADMIN);
            await _userManager.AddToRoleAsync(user, RoleNames.USER);

            StatusMessage = $"Użytkownik {user.Email} został zdegradowany do roli Użytkownika.";
            return Page();
        }
    }
}