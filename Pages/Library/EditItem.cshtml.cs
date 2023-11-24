using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages.Library
{
    [Authorize]
    public class EditItemModel : PageModel
    {
        private readonly ILogger<LibraryModel> _logger;
        private readonly LibraryService _lib;
        public LibraryItem Item { get; set; }

        public EditItemModel(ILogger<LibraryModel> logger, LibraryService lib)
        {
            _logger = logger;
            _lib = lib;
        }
        
        public async Task OnGet(string id)
        {
            var item = await _lib.Get(Guid.Parse(id));
            if (item != null)
            {
                Item = item;
            }
        }
    }
}