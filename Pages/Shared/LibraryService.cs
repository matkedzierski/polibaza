using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PoliBaza.Data;

namespace PoliBaza.Pages.Shared;

public class LibraryService
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UserPreferencesService> _logger;

    public LibraryService(
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

    public IQueryable<LibraryItem> GetAll()
    {
        return _db.LibraryItems;
    }
}