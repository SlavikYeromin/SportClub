using Microsoft.AspNetCore.Mvc;

namespace SportClubFinal.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
