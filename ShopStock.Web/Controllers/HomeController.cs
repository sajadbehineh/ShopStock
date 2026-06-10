using Microsoft.AspNetCore.Mvc;

namespace ShopStock.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("About-Us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("Contact-Us")]
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
