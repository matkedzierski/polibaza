using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PoliBaza.Pages
{
	[Authorize]
	public class AdminModel : PageModel
	{
		private readonly ILogger<LibraryModel> _logger;

		public AdminModel(ILogger<LibraryModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
		}
	}
}