using Microsoft.AspNetCore.Mvc;

namespace HealthHub_API.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
