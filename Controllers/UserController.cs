using Microsoft.AspNetCore.Mvc;

namespace HealthHub_API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
