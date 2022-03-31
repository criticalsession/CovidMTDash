using Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CovidMTDash.Pages
{
    public class IndexModel : PageModel
    {
        public string data { get; set; }
        private IWebHostEnvironment env;

        public IndexModel(IWebHostEnvironment _env)
        {
            env = _env;
            data = "";
        }

        public void OnGet()
        {
            var result = DataFetcher.Get(env.WebRootPath);
            if (result != null) data = result.TotalCases.ToString();
            else data = "No data";
        }
    }
}