using Microsoft.AspNetCore.Mvc;

namespace SmartCarWashTest.Sale.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}



