using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Az_app_pritam.Pages
{
    public class DeploymentSlotsModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public string EnvironmentName { get; set; } = string.Empty;

        public string MachineName { get; set; } = string.Empty;

        public string SlotName { get; set; } = string.Empty;

        public string Version { get; set; } = "Version 1";

        public DeploymentSlotsModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
            EnvironmentName = _environment.EnvironmentName;

            MachineName = Environment.MachineName;

            SlotName =
                Environment.GetEnvironmentVariable("WEBSITE_SLOT_NAME")
                ?? "Production";
        }
    }
}