using Microsoft.AspNetCore.Mvc;

namespace SmartCarWashTest.CRUD.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}