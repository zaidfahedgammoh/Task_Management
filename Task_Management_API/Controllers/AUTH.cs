using Microsoft.AspNetCore.Mvc;

namespace Task_Management_API.Controllers
{
    public class AUTH : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
