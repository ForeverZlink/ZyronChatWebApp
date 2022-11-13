using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Logics
{
    public class ChatMessagesLogic
    {
        private UserContext Context;
        public ChatMessagesLogic(UserContext userContext)
        {
            this.Context = userContext;
        }
        public string SearchUserIdWithUsername(string Username)
        {
            var user = this.Context.Users.FirstOrDefault(x => x.UserName == Username);
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return null;
            }
        }
        public string SaveMessagesChatBetweenTwoUsers(string usernameActualUser, string UserToSendAMessageIdentificationUsername, string message)
        {
            //Save the message between two users

            var user = this.Context.Users.FirstOrDefault(x => x.UserName == usernameActualUser);
            var userToSend = this.Context.Users.FirstOrDefault(x => x.UserName == UserToSendAMessageIdentificationUsername);
            if (userToSend != null && user != null)
            {
            SearchChat:
                var Chat = this.Context.ChatMessages.FirstOrDefault(
                    x => x.IdUserReceiver == user.Id && x.IdUserSender == userToSend.Id ||
                        x.IdUserSender == user.Id && x.IdUserReceiver == userToSend.Id
                    );

                if (Chat != null)
                {

                    string id = Guid.NewGuid().ToString();
                    DateTime DatetimeTodaySended = DateTime.UtcNow.Date;
                    TimeSpan TimeMessageSended = DateTime.UtcNow.TimeOfDay;
                    var MessageSaved = new Messages() { Id = id, Sender = user.UserName, Message = message, ChatMessagesId = Chat.Id, TimeSended = TimeMessageSended, DateSended = DatetimeTodaySended };


                    this.Context.Messages.Add(MessageSaved);
                    this.Context.SaveChanges();


                    return userToSend.Id;
                }
                else
                {
                    this.CreateNewChat(user.UserName,userToSend.Id);
                    goto SearchChat;
                }
            }
            return null;


        }

        public Messages[]  GetMessagesOfAmongTwoUsers(string usernameUserCaller, string UserToTalkUsername)
        {
            //This action is to take all messages among
            //two users and return to a view. 
            //In this view, will be the chat, with all
            //history of messages of the users.


            var UserCaller = this.Context.Users.FirstOrDefault(x => x.UserName == usernameUserCaller);
            var UserToTalk = this.Context.Users.FirstOrDefault(x => x.UserName == UserToTalkUsername);
            if (UserCaller != null && UserToTalk != null)
            {
                var chatmessages = this.Context.ChatMessages.Where(
                        x => x.IdUserReceiver == UserCaller.Id && x.IdUserSender == UserToTalk.Id ||
                            x.IdUserSender == UserCaller.Id && x.IdUserReceiver == UserToTalk.Id
                        ).Include(x => x.MessagesList).FirstOrDefault();

                var UserToSend = this.Context.Users.FirstOrDefault(x => x.Id == UserToTalk.Id);
                
                if (chatmessages != null)
                {
                    var AllMessages = chatmessages.MessagesList
                        .OrderBy(x => x.DateSended).OrderBy(x => x.TimeSended).ToArray();

                    return AllMessages;

                    
                }return null;
                
            }return null;

           
        }

        public bool CreateNewChat(string UsernameOfUserSender,string IdUserToReceiveMessages)
        {
            if (IdUserToReceiveMessages != null)
            {
                string usernameActualUser = UsernameOfUserSender;
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
                    if (chatmessages == null)
                    {
                        string id = Guid.NewGuid().ToString();
                        var chat = new ChatMessages() { Id = id, IdUserSender = UserLogged.Id, IdUserReceiver = IdUserToReceiveMessages };
                        this.Context.Add(chat);
                         this.Context.SaveChangesAsync();

                        return true;
                    }return false;

                }return false;
              

            }return false;
            
        }
    }
}
