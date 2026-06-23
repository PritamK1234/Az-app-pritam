using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Az_app_pritam.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }
        public void OnGet()
        {
            ViewData["Greetings"] = _config["Greetings"];

        }
    }
}
