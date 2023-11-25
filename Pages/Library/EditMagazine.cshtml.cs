﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages.Library
{
    [Authorize(Roles = RoleNames.ADMIN)]
    public class EditMagazineModel : PageModel
    {
        private readonly ILogger<LibraryModel> _logger;
        private readonly LibraryService _lib;
        public LibraryItem.Magazine Item { get; set; } = new();
        public string StatusMessage { get; set; }

        public EditMagazineModel(ILogger<LibraryModel> logger, LibraryService lib)
        {
            _logger = logger;
            _lib = lib;
        }

        public async Task<IActionResult> OnGet(string? id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var item = await _lib.Get(Guid.Parse(id));
                if (item is LibraryItem.Magazine magazine)
                {
                    Item = magazine;
                }
                else
                {
                    //return RedirectToPage("/Library");                    
                }   
            }
            else
            {
                Item = new LibraryItem.Magazine();
            }

            return Page();
        }
     
        public async Task<IActionResult> OnPost(IFormCollection form, IFormFile? file)
        {
            Item = await _lib.MapToItem(form, file) as LibraryItem.Magazine ?? new LibraryItem.Magazine();
            if (!TryValidateModel(Item)) return Page();
            await _lib.Update(Item);
            StatusMessage = "Pomyślnie zapisano.";
            return Page();
        }

        public string? GetPhotoBase64()
        {
            return Item.Photo != null ? Convert.ToBase64String(Item.Photo) : null;
        }
    }
}