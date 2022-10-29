using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ChatMessagesController : Controller
    {
        
        public UserContext Context { get; set; }

        
        

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateNewChat(string IdUserToReceiveMessages)
        {
            if (IdUserToReceiveMessages != null)
            {
                var UserLogged = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (UserLogged != null)
                {
                    var chat = new ChatMessages() { IdUserSender = UserLogged.Id, IdUserReceiver = IdUserToReceiveMessages };
                    this.Context.Add(chat);
                    await this.Context.SaveChangesAsync();

                }
            
        }
            return NotFound();
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