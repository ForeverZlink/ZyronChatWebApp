using Microsoft.AspNetCore.Mvc;

namespace ZyronChatWebApp.Controllers.Account
{
    public class UserController : Controller
    {
        public async Task<IActionResult> RegisterUser()
        {
            return View();
        }
    }
}
