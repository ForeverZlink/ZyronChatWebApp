using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            Context = dbContext;
            UserManagement = usermanager;
            SignInManager = SignInManages;
        }

        public async Task<IActionResult> LogoutUser()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
            if (user != null)
            {
                await this.SignInManager.SignOutAsync();
                return RedirectToAction("Index", "DashManagement");
            }
            return View();
        }
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            List<string> errors = new List<string>();
            if (username == null || password == null)
            {
                ViewBag.LoginWithSucess = false;

                return View();
            }

            var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);

            if (user != null)
            {
                var result = await this.SignInManager.PasswordSignInAsync(user, password, true, false);
                if (result.Succeeded == true)
                {
                    ViewBag.LoginWithSucess = true;
                    return RedirectToAction("Index", "DashManagement");
                }
                else
                {
                    ViewBag.LoginWithSucess = false;
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


        public bool RegisterUserPublicVisibility(string IdUser, string Username, string Email)
        {

            //Creates a new UserPublic object. 
            //This object has the purpose of the create relationship with another 
            //models without that any data to be hacked.
            if (IdUser==null || Username==null || Email== null)
            {
                throw new ArgumentException("Arguments passed are null");
            }
            string IdPublic = Guid.NewGuid().ToString();
            var NewUserPublic = new UserPublic() { IdPublic = IdPublic, IdPrivate = IdUser,Username=Username };
            if (NewUserPublic != null)
            {
                var result = this.Context.UserPublic.Add(NewUserPublic);
                if (result==null) {
                    throw new NullReferenceException("Null object ");
                }
                var saving = this.Context.SaveChanges();
                return true;

            }
            return false;
            
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string Name, string Password, string Email)
        {

            var User = new UserModelCustom { UserName = Name, Email = Email };
            

            var result = await this.UserManagement.CreateAsync(User, Password);

            
            if (result.Succeeded)
            {
                var ResultUserPublicCreation = this.RegisterUserPublicVisibility(User.Id, Name, Email);
                if (ResultUserPublicCreation == null)
                {
                    return NotFound();
                }
                //New list now its required, because to create a relationship among UserModelCustom and UserScheduleListOfContacts its necessary
                // of UserScheduleListOfContacts receive the User.id for identification.
                var List = new UserScheduleListOfContacts() { UserId = User.Id };
                this.Context.Add(List);
                this.Context.SaveChanges();

                ViewBag.UserCreatedWithSucess = true;
                return View("Index");
            }
            else
            {
                ViewBag.UserCreatedWithSucess = false;
                List<string> ListOfErrors = new List<string>();
                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError(string.Empty, error.Description);

                    ListOfErrors.Add(error.Description);

                }
                return View("Index", ListOfErrors);

            }


 
        }
    }
}
