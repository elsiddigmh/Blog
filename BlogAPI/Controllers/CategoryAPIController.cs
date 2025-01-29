using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{

    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryAPIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
