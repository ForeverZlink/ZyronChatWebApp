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

     
        public async Task<IActionResult> LoginUser(string username,string password)
        {
            List<string> errors = new List<string>();
            if (username == null || password==null )
            {
                return View();
            }

            var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
            
            if (user!= null) {
                var result = await this.SignInManager.PasswordSignInAsync(user, password,true,false);
                if (result.Succeeded == true)
                {

                    return RedirectToAction("Index", "DashManagement");
                }
                else
                {
                    errors.Add("Não foi possível verificar sua autenticidade. Verifique seus dados e tente novamente");
                    return View(errors);
                }
    
                
            
            }
            errors.Add("Não existe nenhum usuário com esse nome");
            return View(errors);

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

                ViewBag.UserCreatedWithSucess = true;
                return View("Index") ;
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
