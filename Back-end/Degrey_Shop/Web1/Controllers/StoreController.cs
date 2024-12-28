using Microsoft.AspNetCore.Mvc;

namespace Web1.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
