using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages.Library
{
    [Authorize]
    public class ViewItemModel : PageModel
    {
        private readonly ILogger<LibraryModel> _logger;
        private readonly LibraryService _lib;
        public LibraryItem Item { get; set; }

        public ViewItemModel(ILogger<LibraryModel> logger, LibraryService lib)
        {
            _logger = logger;
            _lib = lib;
        }
        
        public async Task<IActionResult> OnGet(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Page();
            }
            var item = await _lib.Get(Guid.Parse(id));
            if (item != null)
            {
                Item = item;
            }

            return Page();
        }
        
        public bool IsBook()
        {
            return Item is LibraryItem.Book;
        }
        public bool IsMagazine()
        {
            return Item is LibraryItem.Magazine;
        }
        public bool IsMultimedia()
        {
            return Item is LibraryItem.Multimedia;
        }

        public string? GetPhoto()
        {
            return Item.Photo != null ? Convert.ToBase64String(Item.Photo) : null;
        }
    }
}