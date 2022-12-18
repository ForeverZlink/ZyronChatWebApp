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
        public string SaveMessagesChatBetweenTwoUsers(string IdPublicUserCaller, string IdPublicUserToSend, string message)
        {
            //Save the message between two users

            var user = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserCaller);
            var userToSend = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserToSend);
            if (userToSend != null && user != null)
            {
            SearchChat:
                var Chat = this.Context.ChatMessages.FirstOrDefault(
                    x => x.IdUserReceiver == user.IdPublic && x.IdUserSender == userToSend.IdPublic ||
                        x.IdUserSender == user.IdPublic && x.IdUserReceiver == userToSend.IdPublic
                    );

                if (Chat != null)
                {

                    string id = Guid.NewGuid().ToString();
                    DateTime DatetimeTodaySended = DateTime.UtcNow;
                    TimeSpan TimeMessageSended = DateTime.UtcNow.TimeOfDay;
                    var MessageSaved = new Messages() { Id = id, Sender = user.Username, Message = message, ChatMessagesId = Chat.Id, TimeSended = TimeMessageSended, DateSended = DatetimeTodaySended };


                    this.Context.Messages.Add(MessageSaved);
                    this.Context.SaveChanges();


                    return userToSend.IdPublic;
                }
                else
                {
                    this.CreateNewChat(user.IdPublic,userToSend.IdPublic);
                    goto SearchChat;
                }
            }
            return null;


        }

        public Messages[]  GetMessagesOfAmongTwoUsers(string IdPublicUserCaller, string IdPublicUserTalk)
        {
            //This action is to take all messages among
            //two users and return to a view. 
            //In this view, will be the chat, with all
            //history of messages of the users.

            if (IdPublicUserCaller != null && IdPublicUserTalk != null) {

                var UserCaller = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserCaller);
                var UserToTalk = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserTalk);
                if (UserCaller != null && UserToTalk != null)
                {
                    var chatmessages = this.Context.ChatMessages.Where(
                            x => x.IdUserReceiver == UserCaller.IdPublic && x.IdUserSender == UserToTalk.IdPublic ||
                                x.IdUserSender == UserCaller.IdPublic && x.IdUserReceiver == UserToTalk.IdPublic
                            ).Include(x => x.MessagesList).FirstOrDefault();

                    var UserToSend = this.Context.Users.FirstOrDefault(x => x.Id == UserToTalk.IdPublic);

                    if (chatmessages != null)
                    {
                        var AllMessages = chatmessages.MessagesList
                            .OrderByDescending(x => x.DateSended).ToArray();

                        return AllMessages;


                    }
                    return null;

                }
                return null;

            }return null;
            

            

           
        }

        
        public List<ChatMessages> OrderChatsByRecentMessages(string IdPublicUserCaller)
        {
            //This method has the purpose of order the list of contact of a user 
            // but Taking as a principal factor the messages recents among the users 
            //What really matter here its the is how recent the message exchange 
            // The messages of user caller are too count

            if(IdPublicUserCaller == null)
            {
                return null;
            }

            var ChatMessagesObjects = this.Context.ChatMessages
                                        .Include("MessagesList").Include("UserSender").Include("UserReceiver")
                                        .Where(x => x.IdUserSender == IdPublicUserCaller || x.IdUserReceiver == IdPublicUserCaller).Where(x=>x.MessagesList.Count>0)
                                        .ToList();

            //The first Object is with the more recent message
            var ChatsOrderly = new List<ChatMessages>();

            //Ordering the Message in each ChatObject
            foreach (var  Chat in ChatMessagesObjects)
            {
                 
                Chat.MessagesList = Chat.MessagesList.OrderByDescending(x => x.DateSended).ToList();

            }

            
            //Starting a ordering the Chats (of the more recent to the more older)
            foreach (var ChatToOrder in ChatMessagesObjects)
            {
                var MostRecentMessageOfTheChatToOrder = ChatToOrder.MessagesList.FirstOrDefault();
               
                if (ChatsOrderly.Count == 0)
                {
                    ChatsOrderly.Add(ChatToOrder);
                    continue;
                }

                else {
                    var MessageMoreRecentInListOrderned = ChatsOrderly.FirstOrDefault().MessagesList.FirstOrDefault();
                    var MessageMoreRecentOfLastObjectInListOrderned = ChatsOrderly.LastOrDefault().MessagesList.FirstOrDefault();

                    if (MostRecentMessageOfTheChatToOrder.DateSended.CompareTo(MessageMoreRecentInListOrderned.DateSended) > 0)
                    {
                        ChatsOrderly.Insert(0, ChatToOrder);

                    }
                    else if (ChatToOrder.MessagesList.FirstOrDefault().DateSended.CompareTo(MessageMoreRecentOfLastObjectInListOrderned.DateSended) < 0)
                    {
                        ChatsOrderly.Add(ChatToOrder);
                    }
                    else
                    {
                        foreach (var Chat in ChatsOrderly)
                        {
                            var MessageMostRecentOfThisChat = Chat.MessagesList.FirstOrDefault();

                            if (MostRecentMessageOfTheChatToOrder.DateSended.CompareTo(MessageMostRecentOfThisChat.DateSended) > 0)
                            {
                                int IndexChat = ChatMessagesObjects.ToList().IndexOf(Chat);
                                ChatsOrderly.Insert(IndexChat, ChatToOrder);
                                break;
                            }
                        }
                    }



                }
                

            }

            return ChatsOrderly;
            

        }
        public bool CreateNewChat(string IdPublicUserSender,string IdPublicUserToReceiveMessages)
        {
            if (IdPublicUserToReceiveMessages != null && IdPublicUserSender != null)
            {
                
                var UserLogged = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserSender);
                var UserToSend = this.Context.UserPublic.FirstOrDefault(x => x.IdPublic == IdPublicUserSender);
                if (UserLogged != null && UserToSend !=null)
                {
                    //Search if a object ChatMessages already exists among the both user
                    var chatmessages = this.Context.ChatMessages.FirstOrDefault(
                        x => x.IdUserReceiver == UserLogged.IdPublic && x.IdUserSender == IdPublicUserToReceiveMessages ||
                            x.IdUserSender == UserLogged.IdPublic && x.IdUserReceiver == IdPublicUserToReceiveMessages
                        );
                    //If null, it means that the user being added has not yet added the user performing this action.
                    //So the user who is now adding is the first to open the connection and the program will not need
                    //to create a new object for that.
                    if (chatmessages == null)
                    {
                        string id = Guid.NewGuid().ToString();
                        var chat = new ChatMessages() { Id = id, IdUserSender = UserLogged.IdPublic, IdUserReceiver = IdPublicUserToReceiveMessages };
                        this.Context.Add(chat);
                         this.Context.SaveChanges();

                        return true;
                    }return false;

                }return false;
              

            }return false;
            
        }
    }
}
