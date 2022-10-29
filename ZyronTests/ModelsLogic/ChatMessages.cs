

using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronTests.ModelTesting
{
    public class ChatMessagesLogicModel
    {
        static public UserContext context = new DatabaseConstructorTesting().CreateContext();

        [Fact]
        public void Create_New_Object()
        {
            //Definition of user sender
            string Username = "Carls1os";
            string Email = "carlos@gmail.com";
            string Id = "1";
            var UserSender = new UserModelCustom() { UserName = Username, Email = Email,Id=Id };
            

            //Definition of user receiver
            string UsernameReceiver = "KSjkafs";
            string EmailReceiver = "Zelda@gmail.com";
            string IdReceiver = "22";
            var UserReceiver = new UserModelCustom() { UserName = UsernameReceiver, Email = EmailReceiver, Id = IdReceiver };
            //Creation of users
            context.Users.AddRange(UserSender,UserReceiver);
            context.SaveChanges();

            //Verify if the users was  created in database 
            var UserSenderDatabase = context.Users.FirstOrDefault(x => x.UserName == Username);
            Assert.NotNull(UserSenderDatabase);
            var UserReceiverDatabase = context.Users.FirstOrDefault(x => x.UserName == UsernameReceiver);
            Assert.NotNull(UserReceiverDatabase);

            //Definition of ChatMessages
            string IdChatMessage = "2";
            var Chat = new ChatMessages() {Id=IdChatMessage,IdUserSender= UserSenderDatabase.Id ,IdUserReceiver=UserReceiverDatabase.Id};

            //Cration of ChatMessage in database 
            context.Add(Chat);
            context.SaveChanges();

            //Verify if ChatMessage object was created in database as expected
            var ChatFromDatabase = context.ChatMessages.FirstOrDefault(x => x.Id == Chat.Id);
            Assert.NotNull(ChatFromDatabase);

            //Verify if ChatFromDatabase can acess values of UserSender and UserReceiver fields
            //(remember just id of UserSender and UserReceiver was atributed for the ChatMessages object.
            //Of this manner, this verification was the purpose of verify the sucess of relationship and
            // if its possible access another table related

            Assert.Equal(UserReceiverDatabase.UserName, ChatFromDatabase.UserReceiver.UserName);
            Assert.Equal(UserSenderDatabase.UserName, ChatFromDatabase.UserSender.UserName);
            
           
        }
    }
    
}