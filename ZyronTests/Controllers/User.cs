using Microsoft.AspNetCore.Identity;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Controllers.Account;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ZyronTests.Controllers
{
    public class User
    {
        static public UserManager<UserModelCustom> UserManagerInstance{ get; set; }
        static public SignInManager<UserModelCustom> SignInManager { get; set; }
        static public UserContext context = new DatabaseConstructorTesting().CreateContext();

        public string SucessReturnControllerActionRedirection = "RedirectToAction";

        [Fact]
        public void RegisterUser()
        {

        }
    }
}