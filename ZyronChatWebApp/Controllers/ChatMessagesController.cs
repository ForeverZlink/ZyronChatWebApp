using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ChatMessagesController : Controller
    {
        
        public UserContext Context { get; set; }
        public ChatMessagesLogic ChatMessagesLogic { get; set; }
        
        public ILogger<ChatMessagesController> logger { get; set; }
        public ChatMessagesController(UserContext dbcontext, ILogger<ChatMessagesController> logger)
        {
            this.Context = dbcontext;
            this.ChatMessagesLogic = new ChatMessagesLogic(dbcontext);
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChatMenu(string IdPublicUserToTalk)
        {
            //This action is to take all messages among
            //two users and return to a view. 
            //In this view, will be the chat, with all
            //history of messages of the users.
            this.logger.LogInformation("Start ChatMenu view");
            if (IdPublicUserToTalk == null)
            {
                this.logger.LogError("UserToTalkUsername its null");
                return NotFound();
            }

            this.logger.LogInformation("Getting username of user caller of method");
            string IdUserCallerPrivate = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            string IdUserCallerPublic = this.Context.UserPublic.FirstOrDefault(x => x.IdPrivate == IdUserCallerPrivate).IdPublic;

            if (IdUserCallerPrivate != null && IdUserCallerPublic!= null)
            {
                this.logger.LogCritical("Getting messages");
                var messages = this.ChatMessagesLogic.GetMessagesOfAmongTwoUsers(IdUserCallerPublic, IdPublicUserToTalk);
                this.logger.LogInformation("End of search messages");

                this.logger.LogInformation("Verifying messages");
                if (messages == null) {
                    this.logger.LogError("Could not find any message ");
                    return null;

                }
                this.logger.LogInformation("Starting to setting ViewBag propertys");

                this.logger.LogInformation("Setting First Viewbag property ");
                this.ViewBag.UserToSend = IdPublicUserToTalk;

                this.logger.LogInformation("Setting Second ViewBag propertys");
                this.ViewBag.AllMessages = messages;

                this.logger.LogInformation("Returning the view");
                return View();


            }
            this.logger.LogError("Not was possible get de username of user");
            return NotFound();
            

            
        }
        
        
        
    }
}