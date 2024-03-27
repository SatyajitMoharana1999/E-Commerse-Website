using Microsoft.AspNetCore.Mvc;

namespace E_Commerse_Website.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
