using Microsoft.AspNetCore.Mvc;

namespace HealthHub_API.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
