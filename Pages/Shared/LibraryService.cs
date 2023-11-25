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

    public IQueryable<LibraryItem> GetAll()
    {
        return _db.LibraryItems;
    }

    public async Task AddAll(IEnumerable<LibraryItem> items)
    {
        await _db.LibraryItems.AddRangeAsync(items);
        await _db.SaveChangesAsync();
    }

    public async Task Add(LibraryItem libraryItem)
    {
        await _db.LibraryItems.AddAsync(libraryItem);
        await _db.SaveChangesAsync();
    }


    public async Task Delete(string id)
    {
        var item = await Get(Guid.Parse(id));
        if (item != null)
        {
            _db.LibraryItems.Remove(item);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<LibraryItem?> Update(LibraryItem? libraryItem)
    {
        var item = _db.LibraryItems.Update(libraryItem).Entity;
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task<LibraryItem?> Get(Guid id)
    {
        return (await _db.LibraryItems.FindAsync(id)) ?? null;
    }

    public async Task<LibraryItem?> MapToItem(IFormCollection form, IFormFile? formFile)
    {
        LibraryItem? item;
        if (string.IsNullOrEmpty(form["id"]) || Guid.Parse(form["id"]) == Guid.Empty)
        {
            item = Enum.Parse<LibraryItem.Type>(form["Item.ItemType"]) switch
            {
                LibraryItem.Type.BOOK => new LibraryItem.Book(),
                LibraryItem.Type.MAGAZINE => new LibraryItem.Magazine(),
                LibraryItem.Type.MULTIMEDIA => new LibraryItem.Multimedia(),
                _ => null
            };
        }
        else
        {
            item = await Get(Guid.Parse(form["id"]));
        }
        
        if (item == null)
        {
            return null;
        }
        item.Title = form["Item.Title"];
        item.Author = form["Item.Author"];
        item.Publisher = form["Item.Publisher"];
        item.Description = form["Item.Description"];
        item.Tags = form["Item.Tags"];
        item.Contents = form["Item.Contents"];
        
        if (formFile != null)
        {
            var filePath = Path.GetTempFileName();
            await using (var stream = File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            item.Photo = await File.ReadAllBytesAsync(filePath);
        }

        switch (item)
        {
            case LibraryItem.Book book:
                book.IsbnNumber = form["Item.IsbnNumber"];
                book.PageCount = string.IsNullOrEmpty(form["Item.PageCount"]) ? null : int.Parse(form["Item.PageCount"]);
                break;
            case LibraryItem.Magazine magazine:
                magazine.IsbnNumber = form["Item.IsbnNumber"];
                magazine.PageCount = string.IsNullOrEmpty(form["Item.PageCount"]) ? null : int.Parse(form["Item.PageCount"]);
                break;
            case LibraryItem.Multimedia multimedia:
                multimedia.Duration = string.IsNullOrEmpty(form["Item.Duration"]) ? null : TimeSpan.Parse(form["Item.Duration"]);
                multimedia.Publisher = form["Item.Publisher"];
                break;
        }

        return item;
    }
}