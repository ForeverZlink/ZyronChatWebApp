using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers.Account
{
    public class UserController : Controller
    {
        public UserContext Context { get; set; }
        public UserManager<User> UserManagement { get; set; }
        public UserController(UserContext dbContext,UserManager<User> usermanager)
        {
            Context= dbContext;
            UserManagement= usermanager;    
        }
        public async Task<IActionResult> RegisterUser(User NewUserInfo)
        {
            if (ModelState.IsValid)
            {
                this.Context.Add(NewUserInfo);
                this.Context.SaveChanges();
                
                var result = await this.UserManagement.CreateAsync(NewUserInfo);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
        {
                    return View();
                }

            }
            
            return null;
        }
    }
}
