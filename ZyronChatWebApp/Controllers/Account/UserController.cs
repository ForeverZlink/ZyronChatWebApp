using Microsoft.AspNetCore.Mvc;

namespace ZyronChatWebApp.Controllers.Account
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
