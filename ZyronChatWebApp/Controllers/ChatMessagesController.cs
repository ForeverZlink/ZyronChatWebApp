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

        
        public ChatMessagesController(UserContext dbcontext)
        {
            this.Context = dbcontext;
            this.ChatMessagesLogic = new ChatMessagesLogic(dbcontext);
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChatMenu(string IdUserToTalk)
        {
            //This action is to take all messages among
            //two users and return to a view. 
            //In this view, will be the chat, with all
            //history of messages of the users.


            var UserCaller = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (UserCaller != null)
            {
                var chatmessages = this.Context.ChatMessages.Where(
                        x => x.IdUserReceiver == UserCaller.Id && x.IdUserSender == IdUserToTalk ||
                            x.IdUserSender == UserCaller.Id && x.IdUserReceiver == IdUserToTalk
                        ).Include(x=>x.MessagesList).FirstOrDefault();
                if (chatmessages != null)
                {
                    var MessagesOfUserCaller = chatmessages.MessagesList.Where(x => x.Sender == UserCaller.Id)
                        .OrderBy(x=>x.DateSended).OrderBy(x=>x.TimeSended).ToArray();
                    var MessagesOfAnotherUserToTalk = chatmessages.MessagesList.Where(x => x.Sender == IdUserToTalk)
                        .OrderBy(x => x.DateSended).OrderBy(x => x.TimeSended).ToArray();

                    ViewBag.MessagesOfUser = MessagesOfUserCaller;
                    ViewBag.MessagesOfUserToTalk = MessagesOfAnotherUserToTalk;
                    return View();
                }
                return View();
            }

            return View();
        }
        
        public async Task<IActionResult> CreateNewChat(string IdUserToReceiveMessages)
        {
            if (IdUserToReceiveMessages != null)
            {
                string usernameActualUser = User.Identity.Name; 
                var UserLogged = this.Context.Users.FirstOrDefault(x => x.UserName == usernameActualUser);
               
                if (UserLogged != null)
                {
                    //Search if a object ChatMessages already exists among the both user
                    var chatmessages = this.Context.ChatMessages.FirstOrDefault(
                        x => x.IdUserReceiver == UserLogged.Id && x.IdUserSender == IdUserToReceiveMessages ||
                            x.IdUserSender == UserLogged.Id && x.IdUserReceiver == IdUserToReceiveMessages
                        );
                    //If null, it means that the user being added has not yet added the user performing this action.
                    //So the user who is now adding is the first to open the connection and the program will not need
                    //to create a new object for that.
                    if (chatmessages==null) {
                        string id = Guid.NewGuid().ToString();
                        var chat = new ChatMessages() {Id=id, IdUserSender = UserLogged.Id, IdUserReceiver = IdUserToReceiveMessages };
                        this.Context.Add(chat);
                        await this.Context.SaveChangesAsync();

                    }

                    return Ok();

                }
                return NotFound();
                
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