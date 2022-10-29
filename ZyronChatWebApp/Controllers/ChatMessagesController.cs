using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ChatMessagesController : Controller
    {
        private readonly ILogger<ChatMessagesController> _logger;
        public UserContext Context { get; set; }

        public SignInManager<UserModelCustom> SignInManager { get; set; }
        public UserManager<UserModelCustom> UserManager { get; set; }
        public ChatMessagesController(ILogger<ChatMessagesController> logger, UserContext dbContext,
            SignInManager<UserModelCustom> SignInManages, UserManager<UserModelCustom> usermanager)
        {
            _logger = logger;
            this.Context = dbContext;
            this.UserManager = usermanager;
            this.SignInManager = SignInManages; 

            
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChatWithASpecificUser(string UserToSendAMessageIdentification)
        {
            return View();
        }
    }
}