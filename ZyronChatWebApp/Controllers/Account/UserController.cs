using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Models;

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
