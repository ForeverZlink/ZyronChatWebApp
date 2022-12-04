

using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;


namespace ZyronTests.ModelTesting
{
    public class ChatMessagesLogicTesting
    {
        static public DatabaseConstructorTesting fixture = new DatabaseConstructorTesting();
        static UserContext context = fixture.CreateContext();
        public ChatMessagesLogic chatmessages = new ChatMessagesLogic(context);

        //Definition of user 
        string Email = "carlos@gmail.com";
        

        //Definition of user receiver
        
        string EmailReceiver = "Zelda@gmail.com";
        

        
        [Fact]
        public void GetMessagesOfAmongTwoUsers_All_Ok()
        {
            string Id = Guid.NewGuid().ToString();
            string IdPublic = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();


            string IdReceiver = Guid.NewGuid().ToString();
            string IdPublicReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserPublic UserSender = new UserPublic() {IdPrivate= Id,Username = Username, IdPublic= IdPublic };
            UserPublic UserReceiver = new UserPublic() {IdPrivate=IdReceiver, Username=UsernameReceiver,IdPublic = IdPublicReceiver };

            context.UserPublic.AddRange(UserSender,UserReceiver);
            

            context.SaveChanges();

            //Creating new Chat (object to relate users and the messages) 
            var ChatCreatedWithSucess = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver);


            //Messages defition
            string Message = "Hello world";

            //Saving the messages
            var SavedWithSucess = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic, Message);
            Assert.NotNull(SavedWithSucess);

            //Getting the messages
            var MessageList = this.chatmessages.GetMessagesOfAmongTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic);
            //Verify if Messagelist not is null (case be, the method has failed)
            Assert.NotNull(MessageList);

            //Verifying if the message saved in SaveMessagesChatBetweenTwoUsers is same  message returned trough GetMessagesOfAmongTwoUsers
            //This manner its for verify if GetMessagesOfAmongTwoUsers return the corrects messages saved among the users.

            Assert.Contains(Message, MessageList.FirstOrDefault().Message);
            
        }
        [Fact]
        public void GetMessagesOfAmongTwoUsers_Both_Users_Dont_Exists_in_Database()
        {
            string usernameFake = "dont exist";
            string username2Fake = "dont exist";
            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(usernameFake,username2Fake);
            Assert.Null(messages);
        }


        [Fact]
        public void GetMessagesOfAmongTwoUsers_All_Arguments_Are_Null()
        {

            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(null, null);
            Assert.Null(messages);
        }
        [Fact]
        public void GetMessagesOfAmongTwoUsers_Dont_Exist_a_ChatMessage_Object_Related_With_The_Both_Users()
        {
            string Id = Guid.NewGuid().ToString();
            string IdPublic = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();


            string IdPublicReceiver = Guid.NewGuid().ToString();
            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserPublic UserSender = new UserPublic() { IdPrivate = Id, Username = Username, IdPublic = IdPublic };
            UserPublic UserReceiver = new UserPublic() { IdPrivate = IdReceiver, Username = UsernameReceiver, IdPublic = IdPublicReceiver };

            UserModelCustom UserSenderSystem = new UserModelCustom() { Id = Id, UserName = Username, };
            UserModelCustom UserReceiverSystem = new UserModelCustom() { Id = IdReceiver, UserName = UsernameReceiver, };

            //Saving in database 
            context.AddRange(UserSender, UserReceiver,UserSenderSystem,UserReceiverSystem);
            context.SaveChanges();

            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic);
            Assert.Null(messages);
        }
        [Fact]
        public void Create_New_Chat_All_Ok()
        {
            string Id = Guid.NewGuid().ToString();
            string IdPublic = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();


            string IdPublicReceiver = Guid.NewGuid().ToString();
            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users

            UserPublic UserSender = new UserPublic() { IdPrivate = Id, Username = Username, IdPublic = IdPublic };
            UserPublic UserReceiver = new UserPublic() { IdPrivate = IdReceiver, Username = UsernameReceiver, IdPublic = IdPublicReceiver };

            UserModelCustom UserSenderSystem = new UserModelCustom() { Id = Id, UserName = Username, };
            UserModelCustom UserReceiverSystem = new UserModelCustom() { Id = IdReceiver, UserName = UsernameReceiver, };
            //Saving in database 
            context.AddRange(UserSender, UserReceiver, UserSenderSystem, UserReceiverSystem);
            context.SaveChanges();

            var ChatCreatedWithSucess = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver);
            
            Assert.True(ChatCreatedWithSucess);
            
           
            
           
        }
        [Fact]
        public void OrderListOfContactsByRecentMessages_All_Ok() {
            string Id = Guid.NewGuid().ToString();
            string IdPublic = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();
            UserPublic UserSender = new UserPublic() {IdPrivate=Id, Username = Username, IdPublic = IdPublic };

            //First chat
            string IdReceiver = Guid.NewGuid().ToString();
            string IdPublicReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();

            UserPublic UserReceiver = new UserPublic() {IdPrivate=IdReceiver, Username = UsernameReceiver, IdPublic = IdPublicReceiver };
           

            //Second Chat
            string IdReceiver2 = Guid.NewGuid().ToString();
            string UsernameReceiver2 = Guid.NewGuid().ToString();
            string IdPublicReceiver2 = Guid.NewGuid().ToString();
            UserPublic UserReceiver2 = new UserPublic() {IdPrivate=IdPublicReceiver2, Username = UsernameReceiver2,IdPublic = IdPublicReceiver2 };
            

            //Third Chat
            string IdReceiver3 = Guid.NewGuid().ToString();
            string UsernameReceiver3 = Guid.NewGuid().ToString();
            string IdPublicReceiver3 = Guid.NewGuid().ToString();
            UserPublic UserReceiver3 = new UserPublic() {IdPrivate=IdReceiver3, Username = UsernameReceiver3, IdPublic = IdPublicReceiver3 };

            UserModelCustom UserSenderSystem = new UserModelCustom() { Id = Id, UserName = Username, };
            UserModelCustom UserReceiverSystem = new UserModelCustom() { Id = IdReceiver, UserName = UsernameReceiver };
            UserModelCustom UserReceiverSystem2 = new UserModelCustom() { Id = IdReceiver2, UserName = UsernameReceiver2 };
            UserModelCustom UserReceiverSystem3 = new UserModelCustom() { Id = IdReceiver3, UserName = UsernameReceiver3, };
            //Saving all users in database (its necessary save before of create a new Chat)
            context.AddRange(UserSender, UserReceiver, UserReceiver2,UserReceiver3,
                               UserSenderSystem,UserReceiverSystem,UserReceiverSystem2,UserReceiverSystem3);
            context.SaveChanges();

            //Creating the Chat object 
            var ChatCreatedWithSucess = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver);
            var ChatCreatedWithSucess2 = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver2);
            var ChatCreatedWithSucess3 = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver3);

            //Messages of UserReceiver
            string MessageOne = "More older Message";
            string MessageTwo = "More recent Message";


            //Saving messages with UserReceiver
            var SavedWithSucess = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic, MessageOne);
            var SavedWithSucess2 = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic, MessageTwo);

            //Verifying the results
            Assert.Equal(SavedWithSucess,UserReceiver.IdPublic);
            Assert.Equal(SavedWithSucess2, UserReceiver.IdPublic);

           
            //Saving Messages with UserReceiver2
            var SavedWithSucessSecondChat = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver2.IdPublic, MessageOne);
            var SavedWithSucessSecondChat2 = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver2.IdPublic, MessageTwo);

            //Verifying the results
            Assert.Equal(SavedWithSucessSecondChat, UserReceiver2.IdPrivate);
            Assert.Equal(SavedWithSucessSecondChat2, UserReceiver2.IdPrivate);

            //Saving Messages with UserReceiver2
            var SavedWithSucessThirdChat = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver3.IdPublic, MessageOne);
            var SavedWithSucessThirdChat2 = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver3.IdPublic, MessageTwo);


            
            //Verifying the results
            Assert.Equal(SavedWithSucessThirdChat, UserReceiver3.IdPublic);
            Assert.Equal(SavedWithSucessThirdChat2, UserReceiver3.IdPublic);


            string MessageMoreRecent = "Message more recent of all";
            this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver.IdPublic, MessageMoreRecent);
            this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.IdPublic, UserReceiver3.IdPublic, MessageMoreRecent);

            //Ordering the chats
            var result = this.chatmessages.OrderChatsByRecentMessages(UserSender.IdPublic);

            
            Assert.NotNull(result);


        }
        [Fact]
        public void Create_New_Chat_But_All_Arguments_Are_null() {
            var NoSucess=this.chatmessages.CreateNewChat(null, null);
            Assert.False(NoSucess);
        }

        [Fact]
        public void Create_New_Chat_User_Receiver_Not_Found_With_The_Id()
        {
            string IdDontExistInDatabase = "2323232332";
            
            string Username = Guid.NewGuid().ToString();

            
            var NoSucessFalseWanted = this.chatmessages.CreateNewChat(Username, IdDontExistInDatabase);
            Assert.False(NoSucessFalseWanted);

        }

        [Fact]
        public void Create_New_Chat_Already_Exists_a_Entity_With_The_Id_of_Both_Users()
        {
            //This verify how is the response of program when its tried 
            //create a object of the type ChatMessages when already exist 
            // a entity with the both users related.
            string Id = Guid.NewGuid().ToString();
            string IdPublic = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();


            string IdPublicReceiver = Guid.NewGuid().ToString();
            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserPublic UserSender = new UserPublic() { IdPrivate = Id, Username = Username, IdPublic = IdPublic };
            UserPublic UserReceiver = new UserPublic() { IdPrivate = IdReceiver, Username = UsernameReceiver, IdPublic = IdPublicReceiver };
            //Saving in database 
            context.AddRange(UserSender, UserReceiver);
            context.SaveChanges();
            //Creating the register if dont exist in database 
            var SucessTrueExpected = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver);
            Assert.True(SucessTrueExpected);

            //Trying create a object, but he already exists in database the combination.
            var NoSucessFalseWanted = this.chatmessages.CreateNewChat(IdPublic, IdPublicReceiver);
            Assert.False(NoSucessFalseWanted);

        }
    }
    
}