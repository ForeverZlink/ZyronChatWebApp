using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers.Account
{
    public class UserController : Controller
    {
        public UserContext Context { get; set; }
        public SignInManager<UserModelCustom> SignInManager { get; set; }
        public UserManager<UserModelCustom> UserManagement { get; set; }
        public UserController(UserContext dbContext, SignInManager<UserModelCustom> SignInManages, UserManager<UserModelCustom> usermanager)
        {
            Context= dbContext;
            UserManagement= usermanager;
            SignInManager = SignInManages;
        }

     
        public async Task<IActionResult> LoginUser(string username)
        {
            if (username == null)
            {
                return View();
            }

            var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
            
            if (user!= null) {
                await this.SignInManager.SignInAsync(user,true);
                return RedirectToAction("Index");
            
            }
            return View();

        }
        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string Name, string Password, string Email)
        {
               
            var User = new UserModelCustom { UserName=Name, Email=Email };

            
            var result = await this.UserManagement.CreateAsync(User, Password);

            if (result.Succeeded)
            {
                string MessageOfSucess = "New user create with sucess";
                await this.SignInManager.SignInAsync(User, true);

                return RedirectToAction("Index", "DashManagement",MessageOfSucess);
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
