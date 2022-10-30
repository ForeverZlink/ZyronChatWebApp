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
        public bool SaveMessagesChatBetweenTwoUsers(string UserToSendAMessageIdentification,string message)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                string usernameActualUser = User.Identity.Name;
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == usernameActualUser );
                var userToSend = this.Context.Users.FirstOrDefault(x => x.UserName == UserToSendAMessageIdentification);
                if (userToSend != null || user!=null)
                {
                    SearchChat:
                        var Chat = this.Context.ChatMessages.FirstOrDefault(
                            x => x.IdUserReceiver == user.Id && x.IdUserSender == userToSend.Id ||
                                x.IdUserSender == user.Id && x.IdUserReceiver == userToSend.Id
                            );
                    
                        if (Chat != null)
                        {
                            DateTime DatetimeTodaySended = DateTime.UtcNow.Date;
                            TimeSpan TimeMessageSended = DateTime.UtcNow.TimeOfDay;
                            var MessageSaved = new Messages() {Sender=user.UserName ,Message = message, TimeSended = TimeMessageSended, DateSended = DatetimeTodaySended };
                        
                            Chat.MessagesList.Add(MessageSaved);
                            this.Context.Update(Chat);
                            this.Context.SaveChanges();


                            return true;
                        }
                        else
                        {
                            this.CreateNewChat(userToSend.Id);
                            goto SearchChat;
                        }
                }
            }
            return false;
        }
    }
}