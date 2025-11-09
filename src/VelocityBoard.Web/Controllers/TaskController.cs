using Microsoft.AspNetCore.Mvc;

namespace VelocityBoard.Web.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
