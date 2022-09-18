using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers.Account
{
    public class UserController : Controller
    {
        public UserContext Context { get; set; }
        public UserManager<IdentityUser> UserManagement { get; set; }
        public UserController(UserContext dbContext,UserManager<IdentityUser> usermanager)
        {
            Context= dbContext;
            UserManagement= usermanager;    
        }

       
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string Name, string Password, string Email)
        {
            
            var User = new IdentityUser { UserName=Name, Email=Email };

            
            var result = await this.UserManagement.CreateAsync(User, Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                List<string> ListOfErrors = new List<string>();
                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError(string.Empty, error.Description);
                   
                    ListOfErrors.Add(error.Description);
                    
                }
                return View("Index",ListOfErrors);
                
            }
            

          
        }
    }
}
