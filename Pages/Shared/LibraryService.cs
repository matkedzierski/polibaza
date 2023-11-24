using Microsoft.AspNetCore.Identity;
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

    public IQueryable<LibraryItem?> GetAll()
    {
        return _db.LibraryItems;
    }

    public async Task AddAll(IEnumerable<LibraryItem?> items)
    {
        await _db.LibraryItems.AddRangeAsync(items);
        await _db.SaveChangesAsync();
    }

    public async Task Add(LibraryItem? libraryItem)
    {
        await _db.LibraryItems.AddAsync(libraryItem);
        await _db.SaveChangesAsync();
    }

    public async Task<LibraryItem?> Get(Guid id)
    {
        return (await _db.LibraryItems.FindAsync(id)) ?? null;
    }
}