using Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CovidMTDash.Pages
{
    public class IndexModel : PageModel
    {
        public ProcessedCovidData? data { get; set; }
        private IWebHostEnvironment env;

        public IndexModel(IWebHostEnvironment _env)
        {
            env = _env;
            data = new ProcessedCovidData();
        }

        public void OnGet()
        {
            //data = DataFetcher.Get(env.WebRootPath);
            data = null;
        }
    }
}