using System.Security.Claims;
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
        var user = await _userManager.GetUserAsync(principal);
        return await GetUserPreferences(user);
    }
    public async Task<UserPreferences> GetUserPreferences(IdentityUser? user)
    {
        if (user == null) return UserPreferences.DEFAULT;
        var prefs = GetCachedPrefs(user.Id);
        if (prefs != null) return prefs;
        
        prefs = await _db.UserPreferences.FirstOrDefaultAsync(p => p.User == user);
        if (prefs != null)
        {
            StoreCachedPrefs(user.Id, prefs);
            return prefs;
        }
        prefs = new UserPreferences { Theme = UserPreferences.ThemeMode.Light, User = user };
        await _db.AddAsync(prefs);
        await _db.SaveChangesAsync();
        StoreCachedPrefs(user.Id, prefs);
        return prefs;
    }

    private UserPreferences? GetCachedPrefs(string key)
    {
        var isCached = _cache.TryGetValue(key, out UserPreferences value);
        _logger.LogDebug("isCached: {isCached}, value: {value}", isCached, value);
        return isCached ? value : null;
    }
    
    public void StoreCachedPrefs(string key, UserPreferences? prefs)
    {
        _cache.Set(key, prefs, TimeSpan.FromHours(1));
        _logger.LogDebug("Stored prefs: {prefs}", prefs);
    }

    public async Task DeleteUserPreferences(IdentityUser user)
    {
        var existingPrefs = _db.UserPreferences.FirstOrDefault(p => p.User == user);
        if (existingPrefs != null)
        {
            _db.UserPreferences.Remove(existingPrefs);
            await _db.SaveChangesAsync();
        }
        StoreCachedPrefs(user.Id, null);
    }
}