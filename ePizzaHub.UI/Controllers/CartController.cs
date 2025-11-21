using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
