using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages;

[Authorize]
public class LibraryModel : PageModel
{
	private readonly ILogger<LibraryModel> _logger;
	private readonly LibraryService _lib;
	public IEnumerable<LibraryItem?> Items { get; set; }

	public LibraryModel(ILogger<LibraryModel> logger, LibraryService lib)
	{
		_logger = logger;
		_lib = lib;
	}

	public async Task<IActionResult> OnPostEdit(string id)
	{
		var item = await _lib.Get(Guid.Parse(id));
		return item?.ItemType switch
		{
			LibraryItem.Type.BOOK => RedirectToPage("/Library/EditBook", new { id }),
			LibraryItem.Type.MAGAZINE => RedirectToPage("/Library/EditMagazine", new { id }),
			LibraryItem.Type.MULTIMEDIA => RedirectToPage("/Library/EditMultimedia", new { id }),
			_ => OnGet()
		};
	}
	public  IActionResult OnPostCreate(LibraryItem.Type type)
	{
		return type switch
		{
			LibraryItem.Type.BOOK => RedirectToPage("/Library/EditBook"),
			LibraryItem.Type.MAGAZINE => RedirectToPage("/Library/EditMagazine"),
			LibraryItem.Type.MULTIMEDIA => RedirectToPage("/Library/EditMultimedia"),
			_ => OnGet()
		};
	}

	public IActionResult OnPostView(string? id)
	{
		return RedirectToPage("/Library/ViewItem", new { id });
	}

	public IActionResult OnPostSearch(string search)
	{
		Items = _lib.GetAll().Where(it => !string.IsNullOrEmpty(it.Title) && it.Title.ToLower().Contains(search.ToLower()));
		return Page();
	}
	public async Task<IActionResult> OnPostDelete(string? id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			await _lib.Delete(id);
		}

		return OnGet();
	}
	public IActionResult OnPost()
	{
		return OnGet();
	}
	public IActionResult OnGet()
	{
		Items = _lib.GetAll();
		return Page();
	}
}