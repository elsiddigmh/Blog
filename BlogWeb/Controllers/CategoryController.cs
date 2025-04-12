using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }
    }
}
