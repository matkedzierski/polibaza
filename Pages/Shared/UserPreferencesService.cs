using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PoliBaza.Data;

namespace PoliBaza.Pages.Shared;

public class UserPreferencesService
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UserPreferencesService> _logger;

    public UserPreferencesService(
        ApplicationDbContext dbContext, 
        UserManager<IdentityUser> userManager, 
        ILogger<UserPreferencesService> logger, 
        IMemoryCache cache)
    {
        _db = dbContext;
        _userManager = userManager;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<UserPreferences> GetUserPreferences(ClaimsPrincipal principal)
    {
        var prefs = GetCachedPrefs(principal);
        if (prefs != null) return prefs;
        var user = await _userManager.GetUserAsync(principal);
        prefs = await _db.UserPreferences.FirstOrDefaultAsync(p => p.User == user);
        if (prefs != null)
        {
            StoreCachedPrefs(principal, prefs);
            return prefs;
        }
        prefs = new UserPreferences { Theme = UserPreferences.ThemeMode.Light, User = user };
        await _db.AddAsync(prefs);
        await _db.SaveChangesAsync();
        StoreCachedPrefs(principal, prefs);
        return prefs;
    }

    private UserPreferences? GetCachedPrefs(IPrincipal principal)
    {
        var isCached = _cache.TryGetValue(principal.Identity?.Name ?? "any", out UserPreferences value);
        _logger.LogDebug("isCached: {isCached}, value: {value}", isCached, value);
        return isCached ? value : null;
    }
    
    public void StoreCachedPrefs(ClaimsPrincipal principal, UserPreferences prefs)
    {
        _cache.Set(principal.Identity?.Name ?? "any", prefs);
        _logger.LogDebug("Stored prefs: {prefs}", prefs);
    }
}