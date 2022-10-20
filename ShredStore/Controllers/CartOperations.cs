using Microsoft.AspNetCore.Mvc;

namespace ShredStore.Controllers
{
    public class CartOperations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
