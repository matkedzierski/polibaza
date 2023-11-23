using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PoliBaza.Pages
{
	[Authorize]
	public class ViewMediaModel : PageModel
	{
		private readonly ILogger<LibraryModel> _logger;

		public ViewMediaModel(ILogger<LibraryModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
		}
	}
}