using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PoliBaza.Data;
using PoliBaza.Pages.Shared;

namespace PoliBaza.Pages
{
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

		public void OnGet()
		{
			Items = _lib.GetAll();
		}
	}
}